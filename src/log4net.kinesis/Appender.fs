namespace log4net.Appender

open System
open System.Collections.Generic
open System.IO
open System.Threading

open Amazon.Kinesis.Model

open log4net.Appender
open log4net.Core

type Agent<'T> = MailboxProcessor<'T>

[<AutoOpen>]
module internal Model =
    type LogEvent =
        {
            LoggerName          : string
            Level               : string
            Timestamp           : DateTime
            ThreadName          : string
            CallerInfo          : string
            Message             : string
            ExceptionMessage    : string
            StackTrace          : string
        }

        member this.ToJson() = 
            sprintf "{\"LoggerName\":\"%s\",\"Level\":\"%s\",\"Timestamp\":\"%s\",\"ThreadName\":\"%s\",\"CallerInfo\":\"%s\",\"Message\":\"%s\",\"ExceptionMessage\":\"%s\",\"StackTrace\":\"%s\"}" 
                    this.LoggerName 
                    this.Level 
                    (this.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fffffff"))
                    this.ThreadName
                    this.CallerInfo
                    this.Message
                    this.ExceptionMessage
                    this.StackTrace

type KinesisAppender () as this =
    inherit AppenderSkeleton()

    static let kinesis = Amazon.AWSClientFactory.CreateAmazonKinesisClient()

    let genWorker _ = 
        let agent = Agent<LogEvent>.Start(fun inbox ->
            async {
                while true do
                    let! evt = inbox.Receive()

                    let payload = evt.ToJson() |> System.Text.Encoding.UTF8.GetBytes
                    use stream  = new MemoryStream(payload)
                    let req = new PutRecordRequest(StreamName   = this.StreamName,
                                                   PartitionKey = Guid.NewGuid().ToString(),
                                                   Data         = stream)
                    do! kinesis.PutRecordAsync(req) |> Async.AwaitTask |> Async.Ignore
            })
        agent.Error.Add(fun exn -> 
            Console.WriteLine(exn)) // swallow exceptions so to stop agents from be coming useless after exception..

        agent
    
    let mutable workers : Agent<LogEvent>[] = [||]
    
    let initCount = ref 0
    let init () = 
        // make sure we only initialize the workers array once
        if Interlocked.CompareExchange(initCount, 1, 0) = 0 then
            workers <- { 1..this.LevelOfConcurrency } |> Seq.map genWorker |> Seq.toArray

    let workerIdx = ref 0
    let send evt = 
        if workers.Length = 0 then init()

        let idx = Interlocked.Increment(workerIdx)
        if idx < workers.Length 
        then workers.[idx].Post evt
        else let idx' = idx % workers.Length
             Interlocked.CompareExchange(workerIdx, idx', idx) |> ignore
             workers.[idx'].Post evt

    member val StreamName = "" with get, set
    member val LevelOfConcurrency = 10 with get, set

    override this.Append(loggingEvent : LoggingEvent) = 
        let exnMessage, stackTrace = 
            match loggingEvent.ExceptionObject with
            | null -> String.Empty, String.Empty
            | exn  -> exn.Message, exn.StackTrace

        let evt = { 
                    LoggerName          = loggingEvent.LoggerName
                    Level               = loggingEvent.Level.Name
                    Timestamp           = loggingEvent.TimeStamp
                    ThreadName          = loggingEvent.ThreadName
                    CallerInfo          = loggingEvent.LocationInformation.FullInfo
                    Message             = loggingEvent.RenderedMessage
                    ExceptionMessage    = exnMessage
                    StackTrace          = stackTrace
                  }
        send evt
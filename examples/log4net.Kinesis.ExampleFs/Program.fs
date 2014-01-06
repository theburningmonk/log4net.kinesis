// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

open log4net

[<EntryPoint>]
let main argv = 
    let logger = LogManager.GetLogger("Test")

    let computation = 
        async {
            while true do
                logger.Debug("Debug")
                logger.Info("Info")
                logger.Error("Errors")
                do! Async.Sleep(1)
        }

    Async.Start(computation)

    printfn "Press any key to stop..."
    let _ = System.Console.ReadKey()
    0 // return an integer exit code

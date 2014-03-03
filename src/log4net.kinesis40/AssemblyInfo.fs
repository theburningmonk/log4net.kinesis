namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("log4net.Kinesis")>]
[<assembly: AssemblyProductAttribute("log4net.Kinesis")>]
[<assembly: AssemblyDescriptionAttribute("log4net appender for logging into an Amazon Kinesis stream")>]
[<assembly: AssemblyVersionAttribute("0.1.1")>]
[<assembly: AssemblyFileVersionAttribute("0.1.1")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "0.1.1"

namespace FSharp.Service1

open System
open System.Collections.Generic
open System.Linq
open System.ServiceProcess
open System.Text


module Program =

    [<EntryPoint>]
    let Main(args) = 
        // Define your services
        let myService = new MyService()

        // Start the services
        let servicesToRun = [| myService :> ServiceBase |]
        ServiceBase.Run(servicesToRun)

        // main entry point return
        0


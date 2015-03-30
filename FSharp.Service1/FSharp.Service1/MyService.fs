﻿namespace FSharp.Service1

open System;
open System.Diagnostics;
open System.Linq;
open System.ServiceProcess;
open System.Text;

type public MyService() as service =
    inherit ServiceBase()
   
    // TODO define your service variables
    let eventLog = new EventLog();
    
    // TODO initialize your service
    let initService = 
        service.ServiceName <- "FSharp.Service1" 

        // Define the Event Log
        let eventSource = "FSharp.Service1"
        if not (EventLog.SourceExists(eventSource)) then
            EventLog.CreateEventSource(eventSource, "Application");

        eventLog.Source <- eventSource;
        eventLog.Log <- "Application";

    do
        initService

    // TODO define your service operations
    override service.OnStart(args:string[]) =
        base.OnStart(args)
        eventLog.WriteEntry("Service Started")

    override service.OnStop() =
        base.OnStop()
        eventLog.WriteEntry("Service Ended")


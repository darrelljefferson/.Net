@echo off
rem installutil="C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\installutil.exe"
set installutil="C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\installutil.exe"
set projectDir=C:\Users\djefferson\Projects\FSharp.Service1\FSharp.Service1

rem Install the new service
%installutil% "%projectDir%\bin\Debug\FSharp.Service1.exe"

rem Start the service
NET START FSharp.Service1.MyService

pause

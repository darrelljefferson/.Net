@echo off
rem installutil="C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\installutil.exe"
set installutil="C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\installutil.exe"
set projectDir=C:\Users\djefferson\Projects\FSharp.Service1\FSharp.Service1

rem Start the service
NET STOP  FSharp.Service1.MyService

rem Uninstall the service
%installutil% /u "%projectDir%\bin\Debug\FSharp.Service1.exe"

pause

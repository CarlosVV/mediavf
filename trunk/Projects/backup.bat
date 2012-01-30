@echo off
For /f "tokens=2-4 delims=/ " %%a in ('date /t') do (set mydate=%%c%%a%%b)
For /f "tokens=1-2 delims=/:" %%a in ("%TIME%") do (set mytime=%%a%%b)
xcopy /e /y /r "C:\Users\Evan\Projects\Eclipse" "E:\Projects\Current\Eclipse" >> C:\Users\Evan\Projects\BackupLogs\%mydate%%mytime%.txt
xcopy /e /y /r "C:\Users\Evan\Projects\SQLServer" "E:\Projects\Current\SQLServer" >> C:\Users\Evan\Projects\BackupLogs\%mydate%%mytime%.txt
xcopy /e /y /r "C:\Users\Evan\Projects\Visual Studio" "E:\Projects\Current\Visual Studio" >> C:\Users\Evan\Projects\BackupLogs\%mydate%%mytime%.txt
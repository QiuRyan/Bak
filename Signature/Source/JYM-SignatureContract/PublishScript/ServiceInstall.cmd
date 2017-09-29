@echo off
@echo off 
set servicename=SignContractService
set FrameworkPath=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319
set ServicePath=%~dp0JYM-SignatureContract.exe
 
echo %ServicePath%

if exist "%FrameworkPath%" goto netOld 
:DispError 
echo 您的机器上没有安装 .net Framework 4.0,安装即将终止.
echo 您的机器上没有安装 .net Framework 4.0,安装即将终止  >InstallService.log
goto LastEnd 
:netOld 
cd %FrameworkPath%
echo 您的机器上安装了相应的.net Framework 4.0,可以安装本服务. 
echo 您的机器上安装了相应的.net Framework 4.0,可以安装本服务 >InstallService.log
echo.
echo. >>InstallService.log

%FrameworkPath%\installutil.exe %ServicePath% /LogToConsole=True
echo -----------------------------
echo         服务安装成功
echo -----------------------------
pause
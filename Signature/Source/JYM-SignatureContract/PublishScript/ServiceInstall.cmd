@echo off
@echo off 
set servicename=SignContractService
set FrameworkPath=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319
set ServicePath=%~dp0JYM-SignatureContract.exe
 
echo %ServicePath%

if exist "%FrameworkPath%" goto netOld 
:DispError 
echo ���Ļ�����û�а�װ .net Framework 4.0,��װ������ֹ.
echo ���Ļ�����û�а�װ .net Framework 4.0,��װ������ֹ  >InstallService.log
goto LastEnd 
:netOld 
cd %FrameworkPath%
echo ���Ļ����ϰ�װ����Ӧ��.net Framework 4.0,���԰�װ������. 
echo ���Ļ����ϰ�װ����Ӧ��.net Framework 4.0,���԰�װ������ >InstallService.log
echo.
echo. >>InstallService.log

%FrameworkPath%\installutil.exe %ServicePath% /LogToConsole=True
echo -----------------------------
echo         ����װ�ɹ�
echo -----------------------------
pause
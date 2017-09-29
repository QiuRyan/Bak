@echo off
net stop SignContractService
set FrameworkPath=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319
set ServicePath=%~dp0\JYM-SignatureContract.exe 


%FrameworkPath%\installutil.exe /u  %ServicePath%


echo -----------------------------
echo         服务卸载成功
echo -----------------------------
pause
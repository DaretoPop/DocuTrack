@echo off

timeout /t 5 /nobreak >nul
del "%~dp0DocuTrack.exe" /f /q
del "%~dp0DocuTrack.pdb" /f /q
rmdir "%~dp0Data" /s /q
exit /b


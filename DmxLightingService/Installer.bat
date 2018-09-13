
@ECHO OFF

REM The following directory is for .NET 2.0
set DOTNETFX2=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319
set PATH=%PATH%;%DOTNETFX2%

echo Installing WindowsService...
echo ---------------------------------------------------
%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\InstallUtil /u DmxLightingService.exe
%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\InstallUtil /i DmxLightingService.exe
echo ---------------------------------------------------
echo Done.
pause
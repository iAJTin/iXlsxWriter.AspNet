@ECHO OFF
CLS

rmdir ..\documentation /s /q

xmldocmd ..\src\lib\bin\Debug\net461\iXlsxWriter.AspNet.dll ..\documentation
         
PAUSE

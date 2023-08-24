@ECHO OFF
CLS

..\src\.nuget\nuget Pack iXlsxWriter.AspNet.1.0.1.nuspec -NoDefaultExcludes -NoPackageAnalysis -OutputDirectory ..\deployment\nuget 

pause


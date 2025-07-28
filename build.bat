@echo off
set config=Debug
set destdir=.\Build\
echo Ready to copy the %config% assemblies to the %destdir% directory...

set msbuild_exe="C:\Windows\Microsoft.NET\Framework64\v4.0.30319\msbuild.exe"
%msbuild_exe% /t:Build /p:Configuration=%config% DbExplorer.sln
if ERRORLEVEL 1 goto Error

md %destdir%
rem copy bin\%config%\*.dll %destdir%
copy bin\%config%\DbExplorer.exe %destdir%
copy DbExplorer32\bin\%config%\DbExplorer32.exe %destdir%
copy Components\Rational.DB.dll %destdir%
copy Components\DbSchemaTools.dll %destdir%
copy Components\DbCodeGen.dll %destdir%
copy Components\Westwind.RazorHosting.dll %destdir%
copy Components\System.Web.Razor.dll %destdir%
copy Sample.config %destdir%
copy LICENSE %destdir%
copy NOTICE %destdir%
md %destdir%\CodeGen
copy CodeGen\*.* %destdir%\CodeGen\

echo Done.
pause

goto End
:Error
echo Error!
pause
:End

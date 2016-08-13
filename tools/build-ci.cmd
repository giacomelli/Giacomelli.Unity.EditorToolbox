echo Building Giacomelli.Unity.EditorToolbox.dll...

mkdir C:\projects\Giacomelli.Unity.EditorToolbox\build
cd C:\projects\Giacomelli.Unity.EditorToolbox\src\Giacomelli.Unity.EditorToolbox\bin\Release

echo Calling ILRepack...
C:\projects\Giacomelli.Unity.EditorToolbox\tools\ILRepack.exe /target:library /targetplatform:"v4,C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5" /out:C:\projects\Giacomelli.Unity.EditorToolbox\build\Giacomelli.Unity.EditorToolbox.dll /wildcards Giacomelli.Unity.*.dll
echo ILRepack finished.

cd C:\projects\Giacomelli.Unity.EditorToolbox

echo done!

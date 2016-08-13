echo Building Giacomelli.Unity.EditorToolbox.dll...

mkdir C:\projects\Giacomelli-Unity-EditorToolbox\build
cd C:\projects\Giacomelli-Unity-EditorToolbox\src\Giacomelli.Unity.EditorToolbox\bin\Release

echo Calling ILRepack...
C:\projects\Giacomelli-Unity-EditorToolbox\tools\ILRepack.exe /target:library /out:C:\projects\Giacomelli-Unity-EditorToolbox\build\Giacomelli.Unity.EditorToolbox.dll /wildcards Giacomelli.Unity.*.dll
echo ILRepack finished.

cd C:\projects\Giacomelli-Unity-EditorToolbox

echo done!

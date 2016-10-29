mkdir ../build

cd ../src/Giacomelli.Unity.EditorToolbox/bin/Release

mono ../../../../tools/ILRepack.exe /target:library /out:../../../../build/Giacomelli.Unity.EditorToolbox.dll /wildcards Giacomelli.Unity.*.dll

cd ../../../../tools

mkdir ../build

cd ../src/Giacomelli.Unity.EditorToolbox/bin/Release

/Library/Frameworks/Mono.framework/Versions/4.4.2/bin/mono ../../../../tools/ILRepack.exe /target:library /out:../../../../build/Giacomelli.Unity.EditorToolbox.dll /wildcards Giacomelli.Unity.*.dll

cd ../../../../tools


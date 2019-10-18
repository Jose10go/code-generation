#load "RoseLibGenerator.csx.cs"
#r "CodeGen.Generators.dll"

var sc = new RoseLibScriptContainer(Context);
await sc.OutputProjectStructure();
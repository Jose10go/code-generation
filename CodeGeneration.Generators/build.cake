///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Debug");
var projectBasePath = Argument("target", "");
var buildDir = Directory(projectBasePath);

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(ctx =>
{
	// Executed BEFORE the first task.
	Information("Running tasks...");
});

Teardown(ctx =>
{
	// Executed AFTER the last task.
	Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

Task("Copy Generator DLL to CommandCore")
.Does(() => {
	Information("Copying CommandBuilderGenerator.cs (from T4 template) to Codege.Core project");
	var files = GetFiles("./bin/" + configuration + "/*.dll");
	CopyFiles(files, "../CodeGen.Core/Commands/CommandBuilders");
});

RunTarget(target);
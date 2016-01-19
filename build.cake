//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target        = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

// Other variables
var Version       = System.IO.File.ReadAllText("VERSION").Trim();

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory("./build");
});

Task("Build")
    .IsDependentOn("Clean")
    .Does(() =>
{
    MSBuild("./src/NuBot.sln", 
        settings => settings.SetConfiguration(configuration));
});

Task("Package")
    .IsDependentOn("Build")
    .Does(() =>
{
    var binaries = Directory("./src")
                 + Directory("NuBot")
                 + Directory("bin")
                 + Directory(configuration);

    var settings = new NuGetPackSettings
    {
        Id = "NuBot",
        Version = Version,
        Authors = new [] { "Viktor Elofsson" },
        Description = "A robotic butler for your organization.",
        Title = "NuBot",
        Files = new []
        {
            new NuSpecContent { Source = "NuBot.dll", Target = "lib/net45" }
        },
        BasePath = binaries,
        OutputDirectory = "build"
    };

    NuGetPack(settings);
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Build")
    .IsDependentOn("Package");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var artifactsDirectory = MakeAbsolute(Directory("./artifacts"));


Task("Clear-Nuget-Packages")
.Does(() =>
{
    Information(artifactsDirectory);
    var packages = GetFiles($"{artifactsDirectory}/*.nupkg");
    DeleteFiles(packages);
});

Task("Build")
.IsDependentOn("Clear-Nuget-Packages")
.Does(() =>
{
    foreach(var project in GetFiles("./src/**/*.csproj"))
    {
        DotNetCoreBuild(
            project.GetDirectory().FullPath, 
            new DotNetCoreBuildSettings()
            {
                Configuration = configuration
            });
    }
});

Task("Test")
.IsDependentOn("Build")
.Does(() =>
{
    foreach(var project in GetFiles("./tests/**/*.csproj"))
    {
        DotNetCoreTest(
            project.GetDirectory().FullPath,
            new DotNetCoreTestSettings()
            {
                Configuration = configuration
            });
    }
});

Task("Create-Nuget-Package")
.IsDependentOn("Test")
.Does(() =>
{
    foreach (var project in GetFiles("./src/**/*.csproj"))
    {
        DotNetCorePack(
            project.GetDirectory().FullPath,
            new DotNetCorePackSettings()
            {
                Configuration = configuration,
                OutputDirectory = artifactsDirectory
            });
    }
});

Task("Push-Nuget-Package")
.IsDependentOn("Create-Nuget-Package")
.Does(() =>
{
    var apiKey = EnvironmentVariable("apiKey") ?? "apiKey";
    foreach (var package in GetFiles($"{artifactsDirectory}/*.nupkg"))
    {
        try{
            NuGetPush(package, 
                new NuGetPushSettings {
                    Source = "https://www.nuget.org/api/v2/package",
                    ApiKey = apiKey
            });
        }catch(System.Exception ex){
            Information(ex.Message);
        }
        
    }
});

/// Aqui devem ser definidas as tasks

Task("Default")
  .IsDependentOn("Push-Nuget-Package");

RunTarget(target);
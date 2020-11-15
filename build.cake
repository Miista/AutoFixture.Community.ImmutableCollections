var target = Argument("target", "Default");

var solutionFile = "./src/AutoFixture.Community.ImmutableCollections.sln";
var packages = new List<Package>
{
  new Package
  {
    Project = "AutoFixture.Community.ImmutableCollections",
    Targets = new []{"netstandard2.0"}
  }
};

class Package
{
  public string Project { get; set; }
  public string[] Targets { get; set; }
}

Task("Build")
  .Does(() =>
{
  var buildSettings = new DotNetCoreBuildSettings
  {
    Configuration = "Release",
    Verbosity = DotNetCoreVerbosity.Minimal
  };
  
  DotNetCoreBuild(solutionFile, buildSettings);

});

Task("Test")
  .Does(() =>
{
  var settings = new DotNetCoreTestSettings
  {
    Verbosity = DotNetCoreVerbosity.Minimal
  };

  DotNetCoreTest(solutionFile, settings);
})
;

Task("Default")
  .IsDependentOn("Build")
  .IsDependentOn("Test")
;

Task("Pack")
  .IsDependentOn("Build")
  .IsDependentOn("Test")
  .Does(() =>
{
  foreach (var package in packages)
  {
    Pack(package.Project, package.Targets);
  }
})
;

RunTarget(target);

public void Pack(string projectName, string[] targets) 
{
  var buildSettings = new DotNetCoreMSBuildSettings()
    .WithProperty("NuspecFile", $"../../nuget/{projectName}.nuspec")
    .WithProperty("NuspecBasePath", "bin/Release");
  var settings = new DotNetCorePackSettings
  {
    MSBuildSettings = buildSettings,
    Verbosity = DotNetCoreVerbosity.Minimal,
    Configuration = "Release",
    IncludeSource = true,
    IncludeSymbols = true,
    OutputDirectory = "./nuget"
  };
  
  DotNetCorePack($"./src/{projectName}/{projectName}.csproj", settings);
}

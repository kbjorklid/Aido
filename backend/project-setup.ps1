<#
.SYNOPSIS
    Creates a new .NET solution based on the Clean Architecture pattern.

.DESCRIPTION
    This script automates the setup of a multi-project .NET solution, including source projects,
    test projects, project references, and necessary NuGet packages. It allows customization
    of the solution name, output directory, and .NET framework version.

.PARAMETER SolutionName
    The name of the solution and the base name for the projects. This is a required parameter.

.PARAMETER SolutionDir
    The root directory where the solution and project folders will be created. Defaults to the current directory.

.PARAMETER DotnetVersion
    The target .NET framework moniker for the projects (e.g., net8.0, net9.0). Defaults to 'net9.0'.

.EXAMPLE
    # Create a solution named "MyAwesomeApp" in the current directory using .NET 9
    .\setup-solution.ps1 -SolutionName "MyAwesomeApp"

.EXAMPLE
    # Create a solution named "FeedbackSorter" in a specific directory using .NET 8
    .\setup-solution.ps1 -SolutionName "FeedbackSorter" -SolutionDir "C:\dev\projects" -DotnetVersion "net8.0"
#>
param(
    [Parameter(Mandatory=$true)]
    [string]$SolutionName,

    [string]$SolutionDir = ".",

    [string]$DotnetVersion = "net9.0"
)

# --- Initial Setup ---
$srcDir = "src"
$testsDir = "tests"

# Create the solution directory if it doesn't exist
Write-Host "Solution will be created in: $(Resolve-Path $SolutionDir)"
if (-not (Test-Path $SolutionDir)) {
    New-Item -ItemType Directory -Path $SolutionDir
}

# Create the solution file
Write-Host "Creating solution file: $SolutionName.sln"
dotnet new sln -n $SolutionName -o $SolutionDir

# Create the source and tests directories
$fullSrcDir = Join-Path $SolutionDir $srcDir
$fullTestsDir = Join-Path $SolutionDir $testsDir

Write-Host "Creating source directory: $fullSrcDir"
if (-not (Test-Path $fullSrcDir)) {
    New-Item -ItemType Directory -Path $fullSrcDir
}

Write-Host "Creating tests directory: $fullTestsDir"
if (-not (Test-Path $fullTestsDir)) {
    New-Item -ItemType Directory -Path $fullTestsDir
}

# --- Create C# Source Projects ---
Write-Host "Creating C# projects for .NET version: $DotnetVersion"

# Core, Application, and Infrastructure projects (Class Libraries)
$classlibProjects = @(
    "$SolutionName.SharedKernel",
    "$SolutionName.Core",
    "$SolutionName.Application",
    "$SolutionName.Infrastructure"
)

foreach ($projectName in $classlibProjects) {
    $projectPath = Join-Path $fullSrcDir $projectName
    Write-Host "Creating class library: $projectName"
    dotnet new classlib -n $projectName -o $projectPath -f $DotnetVersion
    dotnet sln "$SolutionDir/$SolutionName.sln" add "$projectPath/$projectName.csproj"
}

# Presentation project (Web API)
$presentationProjectName = "$SolutionName.Presentation"
$presentationProjectPath = Join-Path $fullSrcDir $presentationProjectName
Write-Host "Creating Web API project: $presentationProjectName"
dotnet new webapi -n $presentationProjectName -o $presentationProjectPath -f $DotnetVersion
dotnet sln "$SolutionDir/$SolutionName.sln" add "$presentationProjectPath/$presentationProjectName.csproj"


# --- Create Test Projects ---
$testProjects = @(
    "$SolutionName.Core.UnitTests",
    "$SolutionName.Application.UnitTests",
    "$SolutionName.Infrastructure.UnitTests",
    "$SolutionName.Presentation.UnitTests"
)

Write-Host "Creating test projects:"
foreach ($testProjectName in $testProjects) {
    $testProjectPath = Join-Path $fullTestsDir $testProjectName
    Write-Host "Creating test project: $testProjectName"
    dotnet new xunit -n $testProjectName -o $testProjectPath -f $DotnetVersion
    dotnet sln "$SolutionDir/$SolutionName.sln" add "$testProjectPath/$testProjectName.csproj"
}

# --- Add Project References ---
Write-Host "Adding project references..."

$coreProject = "$fullSrcDir/$SolutionName.Core/$SolutionName.Core.csproj"
$sharedKernelProject = "$fullSrcDir/$SolutionName.SharedKernel/$SolutionName.SharedKernel.csproj"
$appProject = "$fullSrcDir/$SolutionName.Application/$SolutionName.Application.csproj"
$infraProject = "$fullSrcDir/$SolutionName.Infrastructure/$SolutionName.Infrastructure.csproj"
$presProject = "$fullSrcDir/$SolutionName.Presentation/$SolutionName.Presentation.csproj"

# Core -> SharedKernel
Write-Host "  $SolutionName.Core -> $SolutionName.SharedKernel"
dotnet add $coreProject reference $sharedKernelProject

# Application -> Core
Write-Host "  $SolutionName.Application -> $SolutionName.Core"
dotnet add $appProject reference $coreProject

# Infrastructure -> Application
Write-Host "  $SolutionName.Infrastructure -> $SolutionName.Application"
dotnet add $infraProject reference $appProject

# Presentation -> Application, Infrastructure
Write-Host "  $SolutionName.Presentation -> $SolutionName.Application"
dotnet add $presProject reference $appProject
Write-Host "  $SolutionName.Presentation -> $SolutionName.Infrastructure"
dotnet add $presProject reference $infraProject


# --- Add Test Project References ---
Write-Host "Adding test project references..."

dotnet add "$fullTestsDir/$SolutionName.Core.UnitTests/$SolutionName.Core.UnitTests.csproj" reference $coreProject
dotnet add "$fullTestsDir/$SolutionName.Application.UnitTests/$SolutionName.Application.UnitTests.csproj" reference $appProject
dotnet add "$fullTestsDir/$SolutionName.Infrastructure.UnitTests/$SolutionName.Infrastructure.UnitTests.csproj" reference $infraProject
dotnet add "$fullTestsDir/$SolutionName.Presentation.UnitTests/$SolutionName.Presentation.UnitTests.csproj" reference $presProject

# --- Add NuGet Packages to Test Projects ---
Write-Host "Adding NuGet packages (xunit, NSubstitute) to test projects..."

foreach ($testProjectName in $testProjects) {
    $testProjectPath = Join-Path $fullTestsDir $testProjectName
    $testProjectFile = "$testProjectPath/$testProjectName.csproj"
    dotnet add $testProjectFile package NSubstitute
    # xunit is already included by the `dotnet new xunit` template
}

Write-Host "✅ Solution setup complete!"
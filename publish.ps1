param (
    [string]$Configuration = "Release"
)

$ErrorActionPreference = "Stop"

$projectName = "UTTT"
$projectFile = "UTTT.Gui\UTTT.Gui.csproj"
$publishBaseDir = "publish"

# Clean old publish directory
if (Test-Path $publishBaseDir) {
    Remove-Item -Recurse -Force $publishBaseDir
}
New-Item -ItemType Directory -Force -Path $publishBaseDir | Out-Null

$runtimes = @("win-x64", "linux-x64", "osx-x64", "osx-arm64")

foreach ($rid in $runtimes) {
    Write-Host "Publishing for $rid..." -ForegroundColor Cyan
    
    $outDir = Join-Path "publish_temp" $rid
    if (Test-Path $outDir) { Remove-Item -Recurse -Force $outDir }
    
    # Run publish
    $publishArgs = @("publish", $projectFile, "-c", $Configuration, "-r", $rid, "--self-contained", "true", "-p:PublishSingleFile=true", "-o", $outDir)
    $process = Start-Process -FilePath "dotnet" -ArgumentList $publishArgs -Wait -NoNewWindow -PassThru
    
    if ($process.ExitCode -ne 0) {
        Write-Error "Publish failed for RID $rid"
        exit $process.ExitCode
    }
    
    $zipPath = Join-Path $publishBaseDir "$projectName-$rid.zip"
    Write-Host "Creating archive $zipPath..." -ForegroundColor Green
    
    Compress-Archive -Path "$outDir\*" -DestinationPath $zipPath -Force
}

# Cleanup temp
if (Test-Path "publish_temp") {
    Remove-Item -Recurse -Force "publish_temp"
}

Write-Host "Publishing complete. Packaged artifacts are in '$publishBaseDir'." -ForegroundColor Cyan

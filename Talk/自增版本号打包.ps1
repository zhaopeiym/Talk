$LibraryName = "Talk"
[xml]$versionXml = Get-Content ./version.props
$Version = $versionXml.Project.PropertyGroup.VersionPrefix.split("\.")
$Version[3] = ($version[3] -as [int]) + 1 
$Version = $Version -join "."
$versionXml.Project.PropertyGroup.VersionPrefix = $Version
$versionSuffix = $versionXml.Project.PropertyGroup.VersionSuffix
$versionXml.Save("$PSScriptRoot/version.props")

Write-Host '升级依赖包...'
dotnet add package Talk.Extensions

# 打包
Write-Host '开始打包...'
dotnet pack /p:Version=$Version$versionSuffix -c Release

Write-Host '开始上传...'
nuget push ./bin/Release/$LibraryName.$Version$versionSuffix.nupkg -ApiKey oy2irwyzh6oprjroljwav5aospph6tuepbjibnvdcwmhq4 -Source https://api.nuget.org/v3/index.json

Write-Host '输入空格退出...' -NoNewline
$null = [Console]::ReadKey('?') #等待输入按键
##
# Automates building and packaging an Electron application with bundled .NET Core components.
# Handles actions like dependency restoration, bundling utilities/APIs, package options, and Electron configuration.
# Provides command-line arguments for customization and offers usage guidance.
##

[CmdletBinding(PositionalBinding=$false)]
Param(
  [switch]$help,
  [switch]$xrestore,
  [switch]$bundleUtil,
  [switch]$bundleApi,
  [switch]$noPackage,
  [switch]$development,
  [ValidateSet("x64", "arm64")][Alias('a')][String]$electronArch = "x64",
  [Parameter(ValueFromRemainingArguments=$true)][String[]]$properties
)

# get the directory of the powershell script
$source_dir_full=$PSScriptRoot

# updatable (users could change)
$electron_api_resource_path="../client/src/Resources/net_iot_api"
$electron_util_resource_path="../client/src/Resources/net_iot_util"
$electron_src_path='../client/src'
$electron_out_path='../client/out'
$dotnet_api_src_path='../net_iot/net_iot_api'
$dotnet_api_proj_to_publish='net_iot_api.csproj'
$dotnet_util_src_path='../net_iot/net_iot_util'
$dotnet_util_proj_to_publish='net_iot_util.csproj'
$bundleAll = $true
$package = $true
$configuration = 'Release'

# update vars based on user input
if($bundleUtil -or $bundleApi){
  $bundleAll = $false
}

if($noPackage){
  $package = $false
}

if($development){
  $configuration = 'Debug'
}

function Print-Usage() {
  Write-Host "Common settings:"
  Write-Host "  -help                     Print help and exit (short: -h)"
  Write-Host ""

  Write-Host "Actions:"
  Write-Host "  -xrestore                 Restore dependencies for dotnet projects (short: -re)"
  Write-Host "  -bundleUtil               Bundles the dotnet cli utility into the electron client"
  Write-Host "  -bundleApi                Bundles the dotnet api into the electron client"
  Write-Host "  -noPackage                Don't build and package the electron app"
  Write-Host "  -development              Builds the app for electron development (short: -d)"
  Write-Host "  -electronArch             Specifies the electron architecture. One of [x64|arm64] (short: -a)"
  Write-Host ""

  Write-Host "Command line arguments not listed above are passed through to dotnet build."
}


function prompt($1){
  Write-Host "**************************************************************************"
  Write-Host "$1"
  Write-Host "**************************************************************************"
}

function publishApiToElectron(){
  prompt "Building Dotnet Api for Electron"

  if (Test-Path $dotnet_api_src_path/bin) {
    Remove-Item -Recurse -Force -Path $dotnet_api_src_path/bin
  }

  if(Test-Path $dotnet_api_src_path/obj){
    Remove-Item -Recurse -Force -Path $dotnet_api_src_path/obj
  }

  dotnet publish $dotnet_api_src_path/$dotnet_api_proj_to_publish `
  -c $configuration `
  -p:PublishSingleFile=$true `
  -p:PublishTrimmed=$false `
  -p:DebugType=None `
  -p:DebugSymbols=$false `
  -p:Restore=$xrestore `
  --self-contained `
  @properties

  Copy-Item -Recurse -force -Path $dotnet_api_src_path/bin/$configuration/net8.0/*/publish/* -Destination $electron_api_resource_path

  Write-Host "Done!"
}

function publishUtilToElectron(){
  prompt "Building Dotnet Util for Electron"

  if (Test-Path $dotnet_util_src_path/bin) {
    Remove-Item -Recurse -Force -Path $dotnet_util_src_path/bin
  }

  if(Test-Path $dotnet_util_src_path/obj){
    Remove-Item -Recurse -Force -Path $dotnet_util_src_path/obj
  }

  dotnet publish $dotnet_util_src_path/$dotnet_util_proj_to_publish `
  -c $configuration `
  -p:PublishSingleFile=$true `
  -p:PublishTrimmed=$false `
  -p:DebugType=None `
  -p:DebugSymbols=$false `
  -p:Restore=$xrestore `
  --self-contained `
  @properties

  Copy-Item -Recurse -force -Path $dotnet_util_src_path/bin/$configuration/net8.0/*/publish/* -Destination $electron_util_resource_path

  Write-Host "Done!"
}

function packageElectronApp(){
  prompt "Packaging Electron App"

  if (Test-Path $electron_out_path) {
    Remove-Item -Recurse -Force -Path $electron_out_path
  }

  cd $electron_src_path

  # install deps
  npm ci

  # run the electron make command
  npm run make -- --arch="$electronArch"
}

function startElectronApp(){
  prompt "Starting Electron App"

  if (Test-Path $electron_out_path) {
    Remove-Item -Recurse -Force -Path $electron_out_path
  }

  cd $electron_src_path

  # install deps
  npm ci

  # run the electron start command
  npm start
}

function main(){
  if($help){
    Print-Usage
    exit 0
  }

  # copy external resources to electron
  if($bundleAll){
    publishApiToElectron
    publishUtilToElectron
  } else {
    if($bundleUtil){
      publishUtilToElectron
    }

    if($bundleApi){
      publishApiToElectron
    }
  }

  # build electron app
  if($development){
    startElectronApp
  } elseif($package){
    packageElectronApp
  }
}

try {
  # run main function
  main
  exit 0
}
catch {
  Write-Host $_.ScriptStackTrace
  exit 1
}

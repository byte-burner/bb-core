#!/usr/bin/env bash

##
# Automates building and packaging an Electron application with bundled .NET Core components.
# Handles actions like dependency restoration, bundling utilities/APIs, package options, and Electron configuration.
# Provides command-line arguments for customization and offers usage guidance.
##

# Stop script if unbound variable found (use ${var:-} if intentional)
set -u


# Stop script if command returns non-zero exit code.
# Prevents hidden errors caused by missing error code propagation.
set -e


usage()
{
  echo "Common settings:"
  echo "  --help                     Print help and exit (short: -h|-?)"
  echo ""

  echo "Actions:"
  echo "  --restore                  Restore dependencies for dotnet projects (short: -re)"
  echo "  --bundle-util              Bundles the dotnet cli utility into the electron client"
  echo "  --bundle-api               Bundles the dotnet api into the electron client"
  echo "  --no-package               Don't build and package the electron app"
  echo "  --development              Builds the app for electron development (short: -d)"
  echo "  --electron-arch            Specifies the electron architecture. One of [x64|arm64] (short: -a)"
  echo ""

  echo "Command line arguments not listed above are passed through to dotnet build."
}


# get the directory of the bash script
source="${BASH_SOURCE[0]}"
source_dir="$(dirname $source)"
source_dir_full=$(cd $source_dir && pwd)


# updatable (users could change)
electron_api_resource_path="../client/src/Resources/net_iot_api"
electron_util_resource_path="../client/src/Resources/net_iot_util"
electron_src_path='../client/src'
electron_out_path='../client/out'
dotnet_api_src_path='../net_iot/net_iot_api'
dotnet_util_src_path='../net_iot/net_iot_util'

restore=true
bundle_all=true
bundle_api=false
bundle_util=false
package=true
development=false
configuration='Release'
electron_arch='x64'


properties=''
while [[ $# > 0 ]]; do
  opt="$(echo "${1/#--/-}" | tr "[:upper:]" "[:lower:]")"
  case "$opt" in
    -help|-h|-'?')
      usage
      exit 0
      ;;
    -restore|-re)
      restore=true
      ;;
    -bundle-util)
      bundle_util=true
      bundle_all=false
      ;;
    -bundle-api)
      bundle_api=true
      bundle_all=false
      ;;
    -no-package)
      package=false
      ;;
    -development|-d)
      development=true
      configuration='Debug'
      ;;
    -electron-arch|-a)
      electron_arch=$2
      shift
      ;;
    *)
      properties="$properties $1"
      ;;
  esac

  shift
done


function prompt(){
  echo -e "\n**************************************************************************"
  echo -e "$1"
  echo -e "**************************************************************************\n"
}

function publishApiToElectron(){
  prompt "Building Dotnet Api for Electron"

  rm -rf $dotnet_api_src_path/bin $dotnet_api_src_path/obj

  dotnet publish $dotnet_api_src_path/net_iot_api.csproj \
  -c $configuration \
  -p:PublishSingleFile=true \
  -p:PublishTrimmed=false \
  -p:DebugType=None \
  -p:DebugSymbols=false \
  -p:Restore=$restore \
  --self-contained \
  $properties

  cp -r $dotnet_api_src_path/bin/$configuration/net8.0/*/publish/* $electron_api_resource_path

  echo "Done!"
}

function publishUtilToElectron(){
  prompt "Building Dotnet Util for Electron"

  rm -rf $dotnet_util_src_path/bin $dotnet_util_src_path/obj

  dotnet publish $dotnet_util_src_path/net_iot_util.csproj \
  -c $configuration \
  -p:PublishSingleFile=true \
  -p:PublishTrimmed=false \
  -p:DebugType=None \
  -p:DebugSymbols=false \
  -p:Restore=$restore \
  --self-contained \
  $properties

  cp -r $dotnet_util_src_path/bin/$configuration/net8.0/*/publish/* $electron_util_resource_path

  echo "Done!"
}


function packageElectronApp(){
  prompt "Packaging Electron App"

  rm -rf $electron_out_path #remove out dir if exists
  cd $electron_src_path

  # install deps
  npm ci

  # run the electron make command
  npm run make -- --arch="$electron_arch"
}

function startElectronApp(){
  prompt "Starting Electron App"

  rm -rf $electron_out_path #remove out dir if exists
  cd $electron_src_path

  # install deps
  npm ci

  # run the electron start command
  npm start
}


function main(){
  # copy external resources to electron
  if [ $bundle_all = true ]; then
    publishApiToElectron
    publishUtilToElectron
  else
    if [ $bundle_util = true ]; then
      publishUtilToElectron
    fi

    if [ $bundle_api = true ]; then
      publishApiToElectron
    fi
  fi

  # build electron app
  if [ $development = true ]; then
    startElectronApp
  elif [ $package = true ]; then
    packageElectronApp
  fi
}


# run main function
main


#!/bin/bash
@echo on

function do_publish {
  arch="$1"
  echo "Building for $arch"
  dotnet publish -c Release -r "$arch" --self-contained true
  echo "Copying outputs"
  mkdir -p YetAnotherSnakeGame-"$arch"
  cp -r src/SnakeGame.DesktopGL/bin/Release/net8.0/"$arch"/publish/* YetAnotherSnakeGame-"$arch"
  echo "Zipping outputs"
  zip -r YetAnotherSnakeGame-"$arch" YetAnotherSnakeGame-"$arch" -9
  echo "Cleaning up"
  rm -rf YetAnotherSnakeGame-"$arch"
}

do_publish "linux-x64"
do_publish "win-x64"
do_publish "osx-x64"

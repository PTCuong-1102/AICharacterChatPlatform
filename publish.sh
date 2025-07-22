#!/bin/bash

echo "=== Publishing AI Character Chat Platform ==="

# Clean previous builds
echo "Cleaning previous builds..."
dotnet clean

# Restore packages
echo "Restoring packages..."
dotnet restore

# Build solution
echo "Building solution..."
dotnet build --configuration Release

# Publish for different platforms
echo "Publishing for Windows x64..."
dotnet publish AICharacterChat.UI/AICharacterChat.UI.csproj -c Release -r win-x64 --self-contained true -o ./publish/win-x64

echo "Publishing for Linux x64..."
dotnet publish AICharacterChat.UI/AICharacterChat.UI.csproj -c Release -r linux-x64 --self-contained true -o ./publish/linux-x64

echo "Publishing for macOS x64..."
dotnet publish AICharacterChat.UI/AICharacterChat.UI.csproj -c Release -r osx-x64 --self-contained true -o ./publish/osx-x64

echo "Publishing for macOS ARM64..."
dotnet publish AICharacterChat.UI/AICharacterChat.UI.csproj -c Release -r osx-arm64 --self-contained true -o ./publish/osx-arm64

echo "=== Publishing completed ==="
echo "Published files are in ./publish/ directory"


@echo off
dotnet new sln -n Game2D
dotnet new classlib -n Game2D
erase "Game2D\Class1.cs"
dotnet new console -n SandboxTK
xcopy "vendor\bin\scripts\Program.cs" "SandboxTK\Program.cs" /Y
dotnet sln Game2D.sln add SandboxTK/SandboxTK.csproj
dotnet sln Game2D.sln add Game2D/Game2D.csproj
dotnet add SandboxTK/SandboxTK.csproj reference Game2D/Game2D.csproj
vendor\bin\scripts\setup-windows.py
dotnet add Game2D/Game2D.csproj package OpenTK
dotnet add Game2D/Game2D.csproj package StbImageSharp
pause
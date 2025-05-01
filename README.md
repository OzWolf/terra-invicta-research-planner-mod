# Terra Invicta - Research Planner Mod

[![License](https://img.shields.io/badge/License-Apache_2.0-blue.svg)](https://opensource.org/licenses/Apache-2.0)

This is the source code for a Unity Mod Manager mod for the game Terra Invicta.  This mod is [published via Steam](https://steamcommunity.com/sharedfiles/filedetails/?id=3293065056).

## Free Use

The source code for this project is open for free use by whoever wants to use it.  Fork it into your own project, copy fragments as needed.  Permission is not needed from this author to use.

This claim is _only_ for the source code of this project and does not cover the source code of the Terra Invicta game, Unity Mod Manager, or any of the project dependencies.

## Setting Up The Project

_Note: This setup assumes you have already installed [Unity Mod Manager](https://www.nexusmods.com/site/mods/21), which is needed to use this project in Terra Invicta._

This project's target framework is **.NET Standard 2.1**.  This ensures the most stable compatibility with the game engine.  Attempts to target .NET 7 or .NET 8 have resulted in consistent crashes.

1. Define a new environment variable on your computer (account-level should work) of `TerraInvictaInstallPath` and set the value to the base directory of your Terra Invicta install.
2. Load the solution in Visual Studio 2022.
3. Verify the assemblies are all mapped (the environment variable should allow the project to find the required DLLs from Terra Invicta, Unity and Unity Mod Manager).
4. If the project has not retrieved the **ModKit** package via nuget, manually retrieve it yourself.

### Unity Resources

The `research_planner` file is a Unity resource file and would require Unity tools to edit.

### Building

If you attempt to build this project, it has a built-in target to automatically attempt to copy the mod DLL and the supporting `ModKit.dll` file to the mod folder in the Terra Invicta install location.

## UI Library

[@cabarius](https://github.com/cabarius) has created an awesome UI library that makes making UIs in Unity Mod Manager a lot easier as part of their [ToyBox](https://github.com/cabarius/ToyBox) project for Pathfinder.

## Credits

A call out to the following tools and libraries for making this mod possible:

+ [Unity Mod Manager](https://www.nexusmods.com/site/mods/21)
+ [ModKit from ToyBox](https://github.com/cabarius/ToyBox)

# Development and Release Strategy
# Source Code

The basic source code is available via public <a href="https://github.com/hauptstadtbytes/BYTES-DotNet" target="_blank">Github Repository</a>. 

Beside the _res_ directory - containing additional resources like icons or a copy of the license file, all codes are residing in the _src_ folder. The Visual Studio Solution file is ment to bootstrap the various sub-projects:

- _BYTES.NET_ is the core library, holding all data types and API definitions
- _BYTES.NET.Test_ holds the unit tests for _BYTES.NET_
- a various number of extension projects (e.g. _BYTES.NET.MS_), intended to demonstrate how to extend the framework using the API

After cloning the repository, you should be able to build the project locally, whereby the binaries should be available in the _\\src\\BYTES.NET\\bin_ directory and it's subfolders.

# Versioning and Deployment

The library is versioned, following the <a href="https://semver.org/" target="_blank">Semantic Versioning Scheme</a>. Pre-Release versions will be made public as NuGet packages via Github, being <a href="https://docs.microsoft.com/de-de/nuget/create-packages/prerelease-packages" target="_blank">labeled with a postfix</a> (like e.g. _-alpha1_). 

Official releases will be made public <a href="https://www.nuget.org/packages/BYTES.NET/" target="_blank">via nuget.org</a>.

# Design Guidelines

Although being prepared for targetting the .NET core ecosystem, the main development is currently done having .NET 4.6 and 4.8 in mind. 

New features shall be implemented being as cross-platform compatible and backward-compatibe as possible. OS-specific features will be deployed in dedicated packages.

The number of Microsoft and 3rd-party dependencies shall be reduced to an absolute minimum.
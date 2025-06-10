# Reqres User Integration Library

## Overview
A .NET class library that fetches and caches user data from the public [reqres.in](https://reqres.in) API using HttpClient and Clean Architecture principles.

## Project Structure
- `ReqresUserLibrary/`: Core class library
- `ReqresUserLibrary.Tests/`: Unit tests using xUnit and Moq
- `ReqresUserConsoleApp/`: Console app demonstrating usage

## How to Run
Prerequites
- Dotnet Sdk 8.0

```bash cmd commands

dotnet build
dotnet test ./ReqresUserLibrary.Tests
dotnet run --project ReqresUserConsoleApp

<h1 align="center" style="display: block; font-size: 2.5em; font-weight: bold">BoleroSocialLogins</h1>

Table of Contents
============
<!--ts-->
* [Introduction](#introduction)
* [Tools and Libraries Used](#tools-and-libraries-used)
* [Getting Started](#getting-started)
  * [Prerequisites](#prerequisites)
  * [Building and Running Locally](#building-and-running-locally)
<!--te-->
---

Introduction📄
============

This is a very basic implementation of using the ASP.NET OAuth2 authentication libraries to allow users to log in with Google and Microsoft in an F# Bolero project. The example code uses a "user info" object to send the user's claims and other data from the server side to the client side.

Tools and Libraries Used🛠
============

* [.NET](https://dotnet.microsoft.com/en-us/)
* [ASP.NET](https://dotnet.microsoft.com/en-us/apps/aspnet)
* [F#](https://learn.microsoft.com/en-us/dotnet/fsharp/what-is-fsharp)
* [F# Bolero](https://fsbolero.io/)
* [Paket](https://fsprojects.github.io/Paket/)
* [FusionTasks](https://github.com/kekyo/FSharp.Control.FusionTasks)

Getting Started📍
============

### Prerequisites🔍
This project contains the code for setting up Google and Microsoft logins, so you'll need to create app registrations for both to get your client id and client secrets respectively.

* [Google Cloud Instructions](https://support.google.com/cloud/answer/6158849)
* [Microsoft Azure Instructions](https://learn.microsoft.com/en-us/entra/identity-platform/quickstart-register-app)

> 💡 If you would rather not implement both, you can remove the registration of the one you don't want from the `ConfigureServices` method in the server project's `Startup.fs`, and then remove associated button from the `view` function in the client project's `Main.fs` file.

### Building and Running Locally🏗
1. Clone the repository
2. Run `dotnet tool restore` in the project root, then run `dotnet paket install` to install dependencies.
3. Add your Google/Microsoft client id and secret to the server project's User Secrets.
4. Build and run the project, and click on the buttons to log in to a provider.
5. You should see your account's name and internal GUID on the home page.

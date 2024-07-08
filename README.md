<h1 align="center" style="display: block; font-size: 2.5em; font-weight: bold">BoleroSocialLogins</h1>

## Introduction 📌

This is a very basic implementation of using the ASP.NET OAuth2 authentication libraries to allow users to log in with Google and Microsoft in an F# Bolero project. The example code uses a "user info" object to send the user's claims and other data from the server side to the client side.

## Tools and Libraries Used 📌

* .NET
* ASP.NET
* F#
* [F# Bolero](https://fsbolero.io/)
* [Paket](https://fsprojects.github.io/Paket/)
* [FusionTasks](https://github.com/kekyo/FSharp.Control.FusionTasks)

## Getting Started 📌 

### Prerequisites
This project contains the code for setting up Google and Microsoft logins, so you'll need to create app registrations for both to get your client id and client secrets respectively.

* [Google Cloud Instructions](https://support.google.com/cloud/answer/6158849)
* [Microsoft Azure Instructions](https://learn.microsoft.com/en-us/entra/identity-platform/quickstart-register-app)

> 💡 If you would rather not implement both, you can remove the registration of the one you don't want from the `ConfigureServices` method in the server project's `Startup.fs`, and then remove associated button from the `view` function in the client project's `Main.fs` file.

### Running locally
1. Clone the repository
2. Run `dotnet tool restore` in the project root, then run `dotnet paket install` to install dependencies.
3. Add your Google/Microsoft client id and secret to the server project's User Secrets.
4. Start the project, and click on the buttons to log in to a provider.

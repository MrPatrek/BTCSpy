# BTCSpy
"Bitcoin spy". For the input, you specify *the type of your desired operation* (buy/sell BTC) and *the amount of BTC for your chosen operation*. For the output, you get a list of best possible orders to execute against different crypto-exchanges in terms of price depending on your input operation type.

The application is built on the top of .NET 8 (ASP.NET Core 8).

Solution consists of **two main independent programs**: `BTCSpyConsoleApp` (console-mode application) and `BTCSpy` (Web API), as well as `BTCSpy`'s dependencies (class libraries) and xUnit test project. Both perform the same task, only the "UI" differs.

## How to run it?
- Option #1: native console-mode application and/or Web API:
  - Import the project and restore the NuGet packages. Then launch either `BTCSpyConsoleApp` or `BTCSpy`.
- Option #2: [Dockerized application](https://hub.docker.com/r/mrpatrek/btcspy) (Web API only):
  - Keep in mind that this option requires an (explicitly installed) SSL certificate, which you can generate with the following set of commands (may not work if applied via `PowerShell`, try `Command Prompt` instead):
    - `dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\btcspy.pfx -p awesomepass`
    - `dotnet dev-certs https --trust`
    - In case commands above cause failure, try `dotnet dev-certs https --clean` and repeat the commands above.
  - Import the project and run:
    - `docker-compose up --build` to (build and) launch the API;
    - `docker-compose down` to shut down the API.

## How to test it?
Use [this Postman collection](https://github.com/MrPatrek/BTCSpy/blob/main/BTCSpy.postman_collection.json).

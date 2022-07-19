# light-chess


### Useful commands:
* `dotnet test` for running tests
* `dotnet run --project Application` for running Program.cs
* `dotnet build` for building all projects
* `dotnet format` for code refactoring 
* `dotnet format` for code refactoring 
* `dotnet sln add Bot/Bot.csproj` for including Bot project into solution file. 

### How to run the bot on lichess:

1) Run `dotnet run --project Application` for building the current project
2) Clone https://github.com/ShailChoksi/lichess-bot and follow instructions to set up an environment and create `config.yml`
3) Cut and paste all files from `./Application/bin/Debug/net6.0/` (current project) to `./engines/` (lichess-bot project)
4) Set `engine.name` to `"Application"` in `config.yml`
5) Run `python3 lichess-bot.py` (lichess-bot project)

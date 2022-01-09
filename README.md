# leaderboard

A site and API to handle best times on different tracks and games. 

## To run

- Pull project
- Restore project 
- Get a connectionString to a MongoDB database. You can create a free one here: https://mongodb.com
- Add ConnectionString and other settings to  appsettings.Development.json (this file isn't tracked in git, and safe to have in project)
- Create an application for Discord API. Add a redirect URI matching the one launched in project. 
- Run Server project.

### Example appsettings.Development.json file
Here are example JSON for Server project. These are expected to be there, and an error will occur if the value doesn't exist.

I will create a better guide to set ut project at some point

``` json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "mongoSandbox": "INSERT YOUR DATABASE CONNECTIONSRING HERE"
  },
  "JWT": {
    "key": "GENERATE A KEY. NEEDS TO BE >= 128 CHARACTERS",
    "iss": "Kim",
    "aud": "Kim"
  },
  "Discord": {
    "clientId": "YOUR DISCORD APP ID",
    "clientSecret": "YOUR DISCORD CLIENT SECRET",
    "url": "https://discord.com/api",
    "redirectUri": "https://localhost:7099" //EXAMPLE
  }
}

```

# Getting Started

[Go back](../index.md)

## Install botgf

The BotGF project is still under development, to build it clone the repository into any folder.

`git clone https://github.com/Nik300/BotGeneralFramework`

Now build the project using the [dotnet sdk](https://dotnet.microsoft.com/en-us/download)

`dotnet build --configuration Release ./BotGeneralFramework/BotGeneralFramework.CLI`

Optionally you can add `BotGeneralFramework.CLI/bin/Release/.net7.0` to your PATH variable to be able to reference the `botgf` command anywhere

## Initialize a new project

To initialize a new project make sure that the folder where you'd like the project to be initialized doesn't exist yet and input the following command into the console

`botgf --init hello-world`

The command will prompt you some basic information about the bot (name, author, license, description and version)

This will create a folder called `hello-world` in the current directory with the following file structure

```
hello-world
|--> bot.js
|--> botconfig.json
|--> types.d.ts 
```

### Contents of bot.js

This is the main script that will be used as entry for the bot project.
The script has three global variables set up by the framework:
 - **app**: It is the instance of the app that will manage all the bot, console and app events;
 - **config**: This contains an object copy of the botconfig.json file;
 - **options**: This object contains the runtime options chosen when starting the project via command line.

After the initialization of the project this file should look like this:
```javascript
/// <reference path="./types.d.ts" />
app.on("ready", (ctx, next) => {
  console.log("Bot ready!");
  return next();
});
```

Go to the [app section](../pages/app.md#events) to find out more about app events

### Contents of botconfig.json

This config file contains the information given at the time of initialization of the project and some additional options that will be used by the bot during runtime.

After the initialization of the project this file should look like this:
```json
{
  "platforms": {},
  "bot": {
    "version": "1.0.0",
    "name": "testold",
    "author": "n",
    "description": "",
    "license": "MIT",
    "repository": ""
  },
  "options": {
    "exampleOption": "exampleValue"
  }
}
```

#### Initialize the telegram platform

To initialize the telegram platform edit the `platforms` property like this:
```json
{
  "platforms": {
    "telegram": {
      "access": {
        "token": "YOUR:TOKEN"
      }
    }
  }
}
```

### Contents of types.d.ts

This file contains all the type definitions and variable declarations for the bot.
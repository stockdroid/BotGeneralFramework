% BotGeneralFramework(1) botgf 1.0
% Nicola Leone Ciardi
% April 2023

# NAME
botgf - run and build bot frameworks with ease

# SYNOPSIS
**botgf** [**OPTIONS**] [**PROJECT**]

# DESCRIPTION
**botgf** helps community developers build their bots cross-platform and with very low effort. Use **botgf --init** to initialize a new project

# OPTIONS
**-h**, **--help**
: Shows the help message

**-ver**, **--version**
: Shows the version of the BotGeneralFramework running on this machine

**-v**, **--verbose**
: Enables the verbose flag, which allows log messages to be displayed

**-p**, **--project** *PROJECT*
: Sets the project to run

**-s**, **--script** *FILE*
: Sets the script to run

**-c**, **--config** *FILE*
: Sets the config file

**--init**
: Initializes a new BotGeneralFramework project

**-i**, **--info**
: Shows some info about the project

**-t**, **--time**
: Sets the time flag, which prints the elapsed time after the completion of each task by the bot

# EXAMPLES
**botgf path/to/project | botgf -p path/to/project | botgf --project path/to/project**
**botgf path/to/script.js | botgf -s path/to/script.js | botgf --script path/to/script.js**
**botgf -c path/to/config.json path/to/project**
**botgf -v path/to/project | botgf --verb path/to/project**
**botgf -t path/to/project | botgf --time path/to/project**
**botgf -ver | botgf --version**
**botgf --init path**

# EXIT VALUES
**0**
: Success

**-1**
: General Error

# COPYRIGHT
Copyright ©️ Foooball SRL. All Rights Reserved.

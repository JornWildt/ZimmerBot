# Rejseplanen ZimmerBot add-on

This project illustrates how to integrate the Rejseplanen API into ZimmerBot as an add-on.
"Rejseplanen" is a danish online travel planner for bus and train travel in Denmark.

## How to use
Link the project into your own bot-runner and load the "rejseplanen.zbot" file into your bot.
See for instance the ZimmerBot.Console program for how it is done.

The ZimmerBot framework automatically scans the included assemblies for assemblies named
"*.ZimmerBot.AddOn", loads these and invoke their main application entry point.
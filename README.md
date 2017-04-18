# ZimmerBot
ZimmerBot is an experiment with chat bots (with a German twist in the name ...).
It can function as an "Eliza" clone (see the ZimmerBotEliza project) or perhaps
as a personal assistant (see the ZimmerBot.Console project).

The program is given to the public domain "AS IS", so have fun and do not blame
the author for any problems you may run into.

License: MIT (see the file "LICENSE").

## Trying it out

Load the solution, set startup project as either ZimmerBot.Console or ZimmerBotEliza, build, run and start chatting. 
The Eliza clone should be the easiest to get something funny out of.

## Credits

The ZimmerBot language is heavily inspired by ChatScript https://github.com/bwilcox-1234/ChatScript

ZimmerBot itself builds on a lot of NuGet packages from various sources:

- CuttingEdge.Conditions
- Ramone
- YaccLexTools
- log4net
- Quartz.net
- Antlr4.StringTemplate
- NUnit
- NHunspell

## Spelling corrections

ZimmerBot does spelling corrections and stemming using NHunspell. For this to work you need dictionary files to
match the chosen language:

* Download "xx_XX.dic" and "xx_XX.aff" files from where ever the language files are maintained. Save the files in the language file directory.
  * Danish files can be found here: http://www.stavekontrolden.dk/
* Set language with app-setting "ZimmerBot.Language" to your bot's language.
* Set language file directory with app-setting "ZimmerBot.LanguageDirectory".
* Spelling correction can be disabled with "ZimmerBot.EnableSpellingCorrections".
* Stemming can be disabled with "ZimmerBot.EnableStemming".

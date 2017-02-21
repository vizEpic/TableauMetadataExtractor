## Description
Metadata Extractor for Tableau Server repository

## Development Set Up
Install Visual Studio (preferably 2013 version and above).
In Visual Studio go to Tools/NuGet Package Manager/Package Manager Console. 
Run following command to install dependencies.

```sh
$ Update-Package -reinstall
```
Make a copy of App.Example.config and rename App.Example.config to App.config.
Updated credentials in the App.config.


## Installation File
To create set up file. Switch from Debug to Release. Right click on TableauDataExtractorSetUp and Build.


## Disclaimer
This software is supplied AS-IS as an open-source self-service community tool under the MIT license.

This is not supported by Tableau.  It has been designed to work with Tableau Server 10.1 and may work with 9.3 forward.  I just know that it broke when we upgraded from 9.2 to 10.1 and think its because the internal tables changed keys to accommodate revision history.

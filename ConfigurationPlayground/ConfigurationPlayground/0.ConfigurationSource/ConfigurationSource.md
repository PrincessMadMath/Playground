How to implement custom Source Provider

Interesting details:
- It's the source provider that is responsible for the configuration change detection, the source are not reload everytime the IConfiguration is queried (i.e: when building options)
- Since dotnet 6 and usage of ConfigurationManager, the data is immediately loaded (step in Configuration.Add in program.cs) - https://andrewlock.net/exploring-dotnet-6-part-1-looking-inside-configurationmanager-in-dotnet-6/ 
- Help understand the concept of sentinel key with Azure App Configuration


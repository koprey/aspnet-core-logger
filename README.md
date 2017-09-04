
# aspnet-core-logger
Extension to ASP.NET Core Microsoft Extension Logging


## Add in appsetting.json

```
"Logging": {
    "IncludeScopes": "true",
    "LogLevel": {
      "Default": "Debug",
      "System": "Information",
      "Microsoft": "Information"
    },
    "SqlServer": {
      "ConnectionString": "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\\Data\\sampleapp-db.mdf;Integrated Security=True;Connect Timeout=30"
    }
  }
```

## Add in configuration logging

```
.AddSqlServer(loggingConfiguration);
```


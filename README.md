# CloudEncryption
Encryption Based Cloud Storage

## Requirements (Nuggets)
```cs
System.Data.SQLite
GleamTech.FileUltimate
Microsoft.AspNetCore.Http
using Microsoft.JSInterop
Microsoft.AspNetCore.Session
Microsoft.AspNetCore.WebApi.Client (Recommended)
```

## Personalize
When changing a variable value, also change every occurence
```cs
# modifiable string
var string = "MyString";
...
```

## Database Manager
```batch
  ..
  |__ CloudEncryption\
  |      |__ Database    <------
                |__ DatabaseManager.cs    <------
  |      |__ ... 
  |__ ...
```

## Encryption Manager
```batch
  ..
  |__ CloudEncryption\
  |      |__ Encryption    <------
                |__ EncryptionHandler.cs    <------
  |      |__ ... 
  |__ ...
```


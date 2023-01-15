# CloudEncryption
Encryption Based Cloud Storage

![home page](https://imgur.com/9DL3E9C.png)
![login page](https://i.imgur.com/NLHo1B8.png)
![index page](https://i.imgur.com/nbeRcvh.png)

## Requirements (Nuggets)
```cs
System.Data.SQLite
GleamTech.FileUltimate
Microsoft.AspNetCore.Http
Microsoft.JSInterop
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


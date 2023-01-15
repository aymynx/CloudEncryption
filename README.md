# CloudEncryption
Encryption Based Cloud Storage

<img src="https://i.imgur.com/NLHo1B8.png" \>
<img src="https://i.imgur.com/nbeRcvh.png" \>

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


# Getting started

[![continuous](https://github.com/seayxu/NullModemEmulator/actions/workflows/continuous.yml/badge.svg?branch=main)](https://github.com/seayxu/NullModemEmulator/actions/workflows/continuous.yml)

# Package Version
|Version|NuGet|MyGet|
|---|---|---|
|Stable|[![NuGet](https://img.shields.io/nuget/v/Hsu.NullModemEmulator?style=flat-square)](https://www.nuget.org/packages/Hsu.NullModemEmulator) | [![MyGet](https://img.shields.io/myget/godsharp/v/Hsu.NullModemEmulator?style=flat-square&label=myget)](https://www.myget.org/feed/godsharp/package/nuget/Hsu.NullModemEmulator) |
|Preview|[![NuGet](https://img.shields.io/nuget/vpre/Hsu.NullModemEmulator?style=flat-square)](https://www.nuget.org/packages/Hsu.NullModemEmulator) | [![MyGet](https://img.shields.io/myget/godsharp/vpre/Hsu.NullModemEmulator?style=flat-square&label=myget)](https://www.myget.org/feed/godsharp/package/nuget/Hsu.NullModemEmulator) |

## Install com0com
[com0com home page](https://sourceforge.net/projects/com0com/)
>Windows 10+ need update driver with sign
> - Window Update
> - Setting->Windows Update->Check for Update
> - Setting->Windows Update->Advanced Options->Optional Update
> - Vyacheslav Frolov - CNCPorts - 10/10/2015 12:00:00 AM - 3.0.0.0

## Install Nuget Package
- Package Manager
  ```bash
  Install-Package Hsu.NullModemEmulator
  ```
- .NET CLI
  ```bash
  dotnet add package Hsu.NullModemEmulator
  ```

- PackageReference
  ```csharp
  <PackageReference Include="Hsu.NullModemEmulator" Version="2022.206.16.0" />
  ```

## Instance Object

```csharp
NullModemEmulatorManager manager = new();
```

## List Virtual Ports
```csharp
var list = await manager.ListAsync();
foreach (var item in list)
{
    Console.WriteLine(item);
    Console.WriteLine(item.A);
    Console.WriteLine(item.B);
}
```

## Add a Pair Virtual Port
```csharp
var ret = await manager.AddPairAsync(
    new PortBuilder()
    .PortName("COM1")
    .EmulateBaudRate(true)
    ,
    new PortBuilder()
    .PortName("COM2")
    .EmulateBaudRate(true)
);
```

## Change Virtual Port Paramters
```csharp
var ret = await manager.ChangeAsync(
    1,
    PortBusOrder.A,
    new PortBuilder()
    .PortName("COM11")
    .EmulateBaudRate(false)
);
```

## Remove a Pair Virtual Port
```csharp
var ret = await manager.RemoveAsync(1);
```

## Remove All Virtual Ports
```csharp
var ret = await manager.RemoveAllAsync();
```

## List Friendly Name
```csharp
var list = await manager.ListFriendlyNameAsync();
foreach (var item in list)
{
    Console.WriteLine(item.Bus.FriendlyName);
    Console.WriteLine(item.A.FriendlyName);
    Console.WriteLine(item.B.FriendlyName);
}
```
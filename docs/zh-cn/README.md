# 快速入门

[![continuous](https://github.com/seayxu/NullModemEmulator/actions/workflows/continuous.yml/badge.svg?branch=main)](https://github.com/seayxu/NullModemEmulator/actions/workflows/continuous.yml)

## 安装 Nuget 包
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

## 实例化对象

```csharp
NullModemEmulatorManager manager = new();
```

## 列出所有虚拟串口对
```csharp
var list = await manager.ListAsync();
foreach (var item in list)
{
    Console.WriteLine(item);
    Console.WriteLine(item.A);
    Console.WriteLine(item.B);
}
```

## 添加一个虚拟串口对
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

## 改变虚拟串口参数
```csharp
var ret = await manager.ChangeAsync(
    1,
    PortBusOrder.A,
    new PortBuilder()
    .PortName("COM11")
    .EmulateBaudRate(false)
);
```

## 移除一个虚拟串口对
```csharp
var ret = await manager.RemoveAsync(1);
```

## 移除所有虚拟串口对
```csharp
var ret = await manager.RemoveAllAsync();
```

## 列出虚拟串口的友好名称
```csharp
var list = await manager.ListFriendlyNameAsync();
foreach (var item in list)
{
    Console.WriteLine(item.Bus.FriendlyName);
    Console.WriteLine(item.A.FriendlyName);
    Console.WriteLine(item.B.FriendlyName);
}
```
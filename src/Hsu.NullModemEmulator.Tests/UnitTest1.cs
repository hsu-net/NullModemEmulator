using System.IO.Ports;

namespace Hsu.NullModemEmulator.Tests;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void AllPortsTest()
    {
        var ports = SerialPort.GetPortNames();
        foreach (var item in ports)
        {
            Console.WriteLine(item);
        }
    }

    [TestMethod]
    public async Task HelpAsyncTest()
    {
        NullModemEmulatorManager manager = new();
        var list = await manager.HelpAsync();
        foreach (var item in list)
        {
            Console.WriteLine(item);
        }
    }

    [TestMethod]
    public async Task ListAsyncTest()
    {
        NullModemEmulatorManager manager = new();
        var list = await manager.ListAsync();
        foreach (var item in list)
        {
            Console.WriteLine(item);
            Console.WriteLine(item.A);
            Console.WriteLine(item.B);
        }
    }

    [TestMethod]
    public async Task ListFriendlyNameAsyncTest()
    {
        NullModemEmulatorManager manager = new();
        var list = await manager.ListFriendlyNameAsync();
        foreach (var item in list)
        {
            Console.WriteLine(item.Bus.FriendlyName);
            Console.WriteLine(item.A.FriendlyName);
            Console.WriteLine(item.B.FriendlyName);
        }
    }

    [TestMethod]
    public async Task UpdateFriendlyNameAsyncTest()
    {
        NullModemEmulatorManager manager = new();
        await manager.UpdateFriendlyNameAsync();
    }

    [TestMethod]
    public async Task AddPairAsyncTest()
    {
        NullModemEmulatorManager manager = new();
        var ret = await manager.AddPairAsync(
            new PortBuilder()
            .PortName("COM7")
            .EmulateBaudRate(true)
            ,
            new PortBuilder()
            .PortName("COM8")
            .EmulateBaudRate(true)
        );
        Assert.IsTrue(ret);
    }

    [TestMethod]
    public async Task ChangeAsyncTest()
    {
        NullModemEmulatorManager manager = new();
var ret = await manager.ChangeAsync(
    1,
    PortBusOrder.A,
    new PortBuilder()
    .PortName("COM7")
    .EmulateBaudRate(true)
);
        Assert.IsTrue(ret);
    }

    [TestMethod]
    public async Task RemoveAsyncTest()
    {
        NullModemEmulatorManager manager = new();
        await manager.ListAsync();
        var ret = await manager.RemoveAsync(1);
        Assert.IsTrue(ret);
    }
}
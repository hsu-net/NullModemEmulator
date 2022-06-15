using CliWrap;
using CliWrap.Buffered;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Hsu.NullModemEmulator;

/// <summary>
/// The virtual port manager
/// <br/>
/// 虚拟串口管理器
/// </summary>
public class NullModemEmulatorManager
{
    private string _path { get; set; }
    private string _setupc { get; set; }

    private ConcurrentDictionary<int, PortBus> _pairs;

    /// <summary>
    /// The pair of ports.
    /// <br/>
    /// 虚拟串口对
    /// </summary>
    public IReadOnlyDictionary<int, PortBus> Pairs => _pairs;

    /// <summary>
    /// Initialize the NullModemEmulatorManager
    /// <br/>
    /// 初始化 com0com 管理
    /// </summary>
    public NullModemEmulatorManager()
    {
        _pairs = new();
        Initial();
    }

    /// <summary>
    /// Initialize the NullModemEmulatorManager
    /// <br/>
    /// 初始化 com0com 管理
    /// </summary>
    /// <param name="path">
    /// The root directory of com0com.exe
    /// <br/>
    /// com0com.exe 安装目录
    /// </param>
    public NullModemEmulatorManager(string path)
    {
        _pairs = new();
        SetRoot(path);
    }

    /// <summary>
    /// Initialize the root path of the NullModemEmulator
    /// <br/>
    /// 初始化,默认取注册表中安装路径
    /// </summary>
    public void Initial()
    {
        SetRoot(RegistryHelper.GetInstallPath());
    }

    private void SetRoot(string path)
    {
        if (!Directory.Exists(path)) throw new DirectoryNotFoundException();
        _path = path ?? throw new FileNotFoundException("the install path of com0com not found.");
        var tmp = Path.Combine(_path, "setupc.exe");
        if (!File.Exists(_setupc) == false) throw new FileNotFoundException("the path of setupc.exe for com0com not found.");
        _setupc = tmp;
    }

    /// <summary>
    /// Custom directory of com0com.
    /// <br/>
    /// 自定义 com0com 根目录
    /// </summary>
    /// <param name="path"></param>
    public void CustomRoot(string path)
    {
        SetRoot(path);
    }

    /// <summary>
    /// Create a command.
    /// </summary>
    /// <param name="values">The arguments</param>
    /// <returns></returns>
    private Command GetCommand(params string[] values)
    {
        return Cli.Wrap(_setupc)
            .WithWorkingDirectory(_path)
            .WithArguments(args => args
                .Add($"--output \"{Path.Combine(Directory.GetCurrentDirectory(), "nme.log")}\"")
                .Add("--silent")
                .Add(values)
            );
    }

    /// <summary>
    /// Print help information.
    /// <br/>
    /// 打印帮助信息
    /// </summary>
    /// <returns></returns>
    public async Task<string> HelpAsync()
    {
        var result = await GetCommand("help").ExecuteBufferedAsync();
        return result.StandardOutput;
    }

    /// <summary>
    /// List all the virtual ports.
    /// <br/>
    /// 获取全部虚拟串口对
    /// </summary>
    /// <returns>
    /// The array of virtual ports pair.
    /// <br/>
    /// 返回串口对数组
    /// </returns>
    public async Task<PortBus[]> ListAsync()
    {
        var result = await GetCommand("--detail-prms list").ExecuteBufferedAsync();
        var lines = result.StandardOutput.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        _pairs.Clear();
        foreach (var item in lines)
        {
            //CNCA0 PortName=COM1,EmuBR=no,EmuOverrun=no,PlugInMode=no,ExclusiveMode=no,HiddenMode=no,AllDataBits=no,cts=rrts,dsr=rdtr,dcd=rdtr,ri=!on,EmuNoise=0,AddRTTO=0,AddRITO=0
            if (string.IsNullOrWhiteSpace(item)) continue;

            PortDescriptior descriptior = new();
            int index = 0;
            var segments = item.Split(',');

            var tmp = segments[index++].Trim().Split('=');
            descriptior.DeviceName = tmp[0].Split(' ')[0];
            descriptior.BusId = int.Parse(descriptior.DeviceName.Substring(4));
            descriptior.BusOrder = descriptior.DeviceName.Substring(3, 1) == "A" ? PortBusOrder.A : PortBusOrder.B;
            descriptior.PortName = tmp[1];

            tmp = segments[index++].Trim().Split('=');
            descriptior.EmulateBaudRate = tmp[1] == "yes";

            tmp = segments[index++].Trim().Split('=');
            descriptior.EmulateOverrun = tmp[1] == "yes";

            tmp = segments[index++].Trim().Split('=');
            descriptior.PlugInMode = tmp[1] == "yes";

            tmp = segments[index++].Trim().Split('=');
            descriptior.ExclusiveMode = tmp[1] == "yes";

            tmp = segments[index++].Trim().Split('=');
            descriptior.HideMode = tmp[1] == "yes";

            tmp = segments[index++].Trim().Split('=');
            descriptior.AllDataBits = tmp[1] == "yes";

            if (!_pairs.TryGetValue(descriptior.BusId, out var pair) || pair == null)
            {
                pair = new(descriptior.BusId);
                _pairs.TryAdd(descriptior.BusId, pair);
            }

            switch (descriptior.BusOrder)
            {
                case PortBusOrder.A:
                    pair.A = descriptior;
                    break;

                case PortBusOrder.B:
                    pair.B = descriptior;
                    break;
            }
        }

        return _pairs.Values.ToArray();
    }

    /// <summary>
    /// List friendly names of all the virtual ports.
    /// <br/>
    /// 获取全部虚拟串口对的友好名称
    /// </summary>
    /// <returns>
    /// The friendly names array of virtual ports pair.
    /// <br/>
    /// 串口对友好名称数组
    /// </returns>
    public async Task<PortBusDescriptior[]> ListFriendlyNameAsync()
    {
        try
        {
            var result = await GetCommand("--detail-prms listfnames").ExecuteBufferedAsync();
            var lines = result.StandardOutput.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            List<PortBusDescriptior> descriptiors = new(lines.Length);

            foreach (var item in lines)
            {
                /*
                    CNCBUS0 FriendlyName="com0com - bus for serial port pair emulator 0 (COM1 <-> COM2)"
                    CNCA0 FriendlyName="com0com - serial port emulator CNCA0 (COM1)"
                    CNCB0 FriendlyName="com0com - serial port emulator CNCB0 (COM2)"
                 */
                if (string.IsNullOrWhiteSpace(item)) continue;

                FriendlyNameDescriptior descriptior = new();
                var segments = item.Split('=');
                int index = 0;
                var tmp = segments[index++].Trim().Split(' ');
                descriptior.Name = tmp[0];
                if (descriptior.Name.StartsWith("CNCBUS", StringComparison.OrdinalIgnoreCase))
                {
                    descriptior.Type = DeviceType.Bus;
                }
                else
                {
                    descriptior.Type = DeviceType.Port;
                    descriptior.Order = descriptior.Name.StartsWith("CNCA", StringComparison.OrdinalIgnoreCase) ? PortBusOrder.A : PortBusOrder.B;
                }

                descriptior.Id = int.Parse(tmp[0].Replace("CNCBUS", "").Replace("CNCA", "").Replace("CNCB", ""));

                descriptior.FriendlyName = segments[index].Trim();

                PortBusDescriptior bus;
                if (!descriptiors.Exists(x => x.Id == descriptior.Id))
                {
                    bus = new PortBusDescriptior(descriptior.Id);
                    descriptiors.Add(bus);
                }
                else
                {
                    bus = descriptiors.First(x => x.Id == descriptior.Id);
                }

                switch (descriptior.Type)
                {
                    case DeviceType.Bus:
                        bus.Bus = descriptior;
                        break;

                    case DeviceType.Port:
                        switch (descriptior.Order)
                        {
                            case PortBusOrder.A:
                                bus.A = descriptior;
                                break;

                            case PortBusOrder.B:
                                bus.B = descriptior;
                                break;
                        }
                        break;
                }
            }

            return descriptiors.ToArray();
        }
        finally
        {
            await ListAsync();
        }
    }

    /// <summary>
    /// Update the friendly name of the virtual port.
    /// <br/>
    /// 更新虚拟串口的友好名称
    /// </summary>
    /// <returns></returns>
    public async Task UpdateFriendlyNameAsync()
    {
        /*
        .\setupc.exe updatefnames
        CNCBUS0 FriendlyName="com0com - bus for serial port pair emulator 0 (COM1 <-> COM2)"
        CNCA0 FriendlyName="com0com - serial port emulator CNCA0 (COM1)"
        CNCB0 FriendlyName="com0com - serial port emulator CNCB0 (COM2)"
        CNCBUS5 FriendlyName="com0com - bus for serial port pair emulator 5 (COM5 <-> COM6)"
        CNCA5 FriendlyName="com0com - serial port emulator CNCA5 (COM5)"
        CNCB5 FriendlyName="com0com - serial port emulator CNCB5 (COM6)"
        */
        await GetCommand($"updatefnames").ExecuteBufferedAsync();
    }

    /// <summary>
    /// List busy port name.
    /// <br/>
    /// 获取正在使用的虚拟串口名称
    /// </summary>
    /// <param name="filter">
    /// The filter for port name
    /// <br/>
    /// 串口名称过滤器
    /// </param>
    /// <param name="prefix">
    /// The prefix of filter
    /// <br/>
    /// 名称过滤器前缀
    /// </param>
    /// <returns></returns>
    public async Task<string[]> BusyNamesAsync(int? filter = null, string prefix = "COM")
    {
        var result = await GetCommand($"--detail-prms busynames {prefix}{(!filter.HasValue ? "*?" : filter.Value)}").ExecuteBufferedAsync();
        var lines = result.StandardOutput.Split(new char[] { '\n', ' ' }, StringSplitOptions.RemoveEmptyEntries);
        return lines;
    }

    /// <summary>
    /// Add a virtual port pair.
    /// <br/>
    /// 添加一个虚拟串口对
    /// </summary>
    /// <param name="a">
    /// The first port of the pair.
    /// <br/>
    /// 第一个串口
    /// </param>
    /// <param name="b">
    /// The second port of the pair.
    /// <br/>
    /// 第二个串口
    /// </param>
    /// <param name="id">
    /// The id of the pair,default is -1,automatically assigned by the system.
    /// <br/>
    /// 指定串口对的id，默认为-1，由系统自动分配
    /// </param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task<bool> AddPairAsync(PortBuilder a, PortBuilder b, int id = -1)
    {
        /*
         * --detail-prms
         CNCA1 PortName=COM3,EmuBR=no,EmuOverrun=no,PlugInMode=no,ExclusiveMode=no,HiddenMode=no,AllDataBits=no,cts=rrts,dsr=rdtr,dcd=rdtr,ri=!on,EmuNoise=0,AddRTTO=0,AddRITO=0
         CNCB1 PortName=COM4,EmuBR=no,EmuOverrun=no,PlugInMode=no,ExclusiveMode=no,HiddenMode=no,AllDataBits=no,cts=rrts,dsr=rdtr,dcd=rdtr,ri=!on,EmuNoise=0,AddRTTO=0,AddRITO=0
         ComDB: COM3 - logged as "in use"
         ComDB: COM4 - logged as "in use"

        CNCA2 PortName=COM5
        CNCB2 PortName=COM6
        ComDB: COM5 - logged as "in use"
        ComDB: COM6 - logged as "in use"

        DIALOG: {
            The port name COM3 is already used for other device \Device\com0com11.
        } ... ERROR

        Install not completed!

        */

        try
        {
            if (id >= 0 && _pairs.ContainsKey(id)) throw new ArgumentException("the id of pair existed.", nameof(id));
            var result = await GetCommand($"install{(id < 0 ? "" : $" {id}")} {a} {b}").ExecuteBufferedAsync();
            await UpdateFriendlyNameAsync();
            return !result.StandardOutput.Contains("DIALOG");
        }
        finally
        {
            await ListAsync();
        }
    }

    /// <summary>
    /// To change a virtual port parameters.
    /// <br/>
    /// 改变虚拟串口的参数
    /// </summary>
    /// <param name="id">
    /// The value of the pair id.
    /// <br/>
    /// 串口对标识
    /// </param>
    /// <param name="order">
    /// The order of the port.
    /// <br/>
    /// 串口的顺序
    /// </param>
    /// <param name="builder">
    /// The parameters will changed.
    /// <br/>
    /// 要改变的参数
    /// </param>
    /// <returns></returns>
    public async Task<bool> ChangeAsync(int id, PortBusOrder order, PortBuilder builder)
    {
        /*
        change CNCA0 EmuBR=yes,EmuOverrun=yes

        CNCA0 PortName=COM1,EmuBR=yes,EmuOverrun=yes,cts=rrts,ri=!on
        CNCB0 PortName=COM2,EmuBR=yes

        change CNCA0 EmuBR=no,EmuOverrun=ye1s

               CNCA0 PortName=COM1,EmuBR=yes,EmuOverrun=yes,cts=rrts,ri=!on
        Invalid value 'ye1s'
               CNCB0 PortName=COM2,EmuBR=yes
         */
        try
        {
            var result = await GetCommand($"change CNC{order}{id} {builder}").ExecuteBufferedAsync();
            return !result.StandardOutput.Contains("Invalid");
        }
        finally
        {
            await ListAsync();
        }
    }

    /// <summary>
    /// Remove a virtual port pair.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private async Task<bool> RemoveInternalAsync(int id)
    {
        /*
        Disabled  root\com0com \Device\00000278
        Removed  root\com0com \Device\00000278
        Removed CNCA2 com0com\port
        Removed CNCB2 com0com\port
        ComDB: COM5 - released
        ComDB: COM6 - released
        */

        if (!_pairs.Any(x => x.Key == id))
        {
            throw new ArgumentOutOfRangeException(nameof(id), $"the pair of {id} is not exist.");
        }
        var _cmd = GetCommand($"remove {id}");
        Console.WriteLine(_cmd);

        var result = await _cmd.ExecuteBufferedAsync();
        var lines = result.StandardOutput.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        var ret = lines.Count(x => x.StartsWith("Removed")) == 3;
        return ret;
    }

    /// <summary>
    /// Remove a virtual port pair.
    /// <br/>
    /// 移除一个虚拟串口对
    /// </summary>
    /// <param name="id">
    /// The value of the pair id.
    /// <br/>
    /// 虚拟串口对的标识
    /// </param>
    /// <returns></returns>
    public async Task<bool> RemoveAsync(int id)
    {
        try
        {
            return await RemoveInternalAsync(id);
        }
        finally
        {
            await ListAsync();
        }
    }

    /// <summary>
    /// Remove all virtual port pair.
    /// <br/>
    /// 移除全部虚拟串口对
    /// </summary>
    /// <returns></returns>
    public async Task<bool> RemoveAllAsync()
    {
        try
        {
            bool ret = true;
            foreach (var item in _pairs)
            {
                ret &= await RemoveInternalAsync(item.Key);
            }
            return ret;
        }
        finally
        {
            await ListAsync();
        }
    }
}
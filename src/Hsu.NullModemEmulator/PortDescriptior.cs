using System.Runtime.Serialization;

namespace Hsu.NullModemEmulator;

/*
CNCA0 PortName=COM1,
EmuBR=no,
EmuOverrun=no|yes,
PlugInMode=no|yes,
ExclusiveMode=no|yes,
HiddenMode=no|yes,
AllDataBits=no|yes,
cts=rrts|!rrts,
dsr=rdtr|!rdtr,
dcd=rdtr|!rdtr,
ri=!on|on,
EmuNoise=0,
AddRTTO=0,
AddRITO=0

*/
/// <summary>
/// The descriptior of virtual port
/// <br/>
/// 虚拟串口描述符
/// </summary>
public record PortDescriptior
{
    /// <summary>
    /// Pair identity
    /// <br/>
    /// 虚拟串口对总线标识
    /// </summary>
    public int BusId { get; set; }
    /// <summary>
    /// The order of port
    /// <br/>
    /// 虚拟串口顺序
    /// </summary>
    public PortBusOrder BusOrder { get; set; }
    /// <summary>
    /// Device name,the actual name of the virtual serial port
    /// <br/>
    /// 设备名称，虚拟串口实际名称
    /// </summary>
    public string DeviceName { get; set; }
    /// <summary>
    /// Port Name
    /// <br/>
    /// 串口名称
    /// </summary>
    [DataMember(Name = "PortName")]
    public string PortName { get; set; }
    /// <summary>
    /// Enable/disable baud rate emulation in the direction to the paired port(disabled by default)
    /// <br/>
    /// 启用/禁用到配对端口方向的波特率仿真（默认禁用）
    /// </summary>
    [DataMember(Name = "EmuBR")]
    public bool EmulateBaudRate { get; set; }
    /// <summary>
    /// Enable/disable buffer overrun (disabled by default)
    /// <br/>
    /// 启用/禁用缓冲区溢出（默认禁用）
    /// </summary>
    [DataMember(Name = "EmuOverrun")]
    public bool EmulateOverrun { get; set; }
    /// <summary>
    /// Enable/disable plug-in mode, the plug-in mode port is hidden and can't be open if the paired port is not open(disabled by default)
    /// <br/>
    /// 启用/禁用插件模式，插件模式端口是隐藏的，如果配对端口未打开则无法打开（默认禁用）
    /// </summary>
    [DataMember(Name = "PlugInMode")]
    public bool PlugInMode { get; set; }
    /// <summary>
    /// Enable/disable exclusive mode, the exclusive mode port is hidden if it is open(disabled by default)
    /// <br/>
    /// 启用/禁用独占模式,独占模式端口如果打开则隐藏（默认禁用）
    /// </summary>
    [DataMember(Name = "ExclusiveMode")]
    public bool ExclusiveMode { get; set; }
    /// <summary>
    /// Enable/disable hidden mode, the hidden mode port is hidden as it is possible for port enumerators (disabled by default)
    /// 启用/禁用隐藏模式
    /// </summary>
    [DataMember(Name = "HiddenMode")]
    public bool HideMode { get; set; }
    /// <summary>
    /// Enable/disable all data bits transfer disregard
    /// <br/>
    /// 启用/禁用忽略所有数据位传输
    /// </summary>
    [DataMember(Name = "AllDataBits")]
    public bool AllDataBits { get; set; }
    /// <summary>
    /// Clear to Send
    /// <br/>
    /// 清除发送
    /// </summary>
    [DataMember(Name = "cts")]
    public bool Cts { get; set; }
    /// <summary>
    /// Data set Ready
    /// <br/>
    /// 数据准备好
    /// </summary>
    [DataMember(Name = "dsr")]
    public bool Dsr { get; set; }
    /// <summary>
    /// Data Carrier Detect
    /// <br/>
    /// 数据载波检测
    /// </summary>
    [DataMember(Name = "dcd")]
    public bool Dcd { get; set; }
    /// <summary>
    /// Ring indicator
    /// <br/>
    /// 振铃指示
    /// </summary>
    [DataMember(Name = "ri")]
    public bool RingIndicator { get; set; }

    /// <summary>
    /// Emulate Noise：Probability in range 0-0.99999999 of error per character frame in the direction to the paired port(0 by default)
    /// <br/>
    /// 模拟噪声：到配对端口的方向上每个字符帧的错误概率在 0-0.99999999 范围内（默认为 0）
    /// </summary>
    [DataMember(Name = "EmuNoise")]
    public float EmulateNoise { get; set; }

    /// <summary>
    /// add [n] milliseconds to the total time-out period for read operations(0 by default)
    /// <br/>
    /// 将 [n] 毫秒添加到读取操作的总超时时间（默认为 0）
    /// </summary>
    [DataMember(Name = "AddRTTO")]
    public int AddRTTO { get; set; } = 0;

    /// <summary>
    /// add [n] milliseconds to the maximum time allowed to elapse between the arrival of two characters for read operations(0 by default)
    /// <br/>
    /// 将 [n] 毫秒添加到两个字符到达读取操作之间允许经过的最长时间（默认为 0）
    /// </summary>
    [DataMember(Name = "AddRITO")]
    public int AddRITO { get; set; } = 0;
}
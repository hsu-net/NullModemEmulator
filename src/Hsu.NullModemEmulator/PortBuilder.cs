using System.Collections.Concurrent;
using System.Linq;
using System.Text;

namespace Hsu.NullModemEmulator;

/// <summary>
/// The virtual port builder
/// <br/>
/// 虚拟串口构建器
/// </summary>
public class PortBuilder
{
    private ConcurrentDictionary<string, string> _arguments { get; set; }

    /// <summary>
    /// The virtual port builder
    /// <br/>
    /// 虚拟串口构建器
    /// </summary>
    public PortBuilder()
    {
        _arguments = new();
    }

    /// <summary>
    /// Reset arguments
    /// <br/>
    /// 重置参数
    /// </summary>
    public void Reset()
    {
        _arguments.Clear();
    }

    /// <summary>
    /// Set port name
    /// <br/>
    /// 设置串口号
    /// </summary>
    /// <param name="portName">
    /// The port name
    /// <br/>
    /// 串口号
    /// </param>
    /// <returns></returns>
    public PortBuilder PortName(string portName)
    {
        return WithArgument("PortName", portName);
    }

    /// <summary>
    /// Enable/disable baud rate emulation in the direction to the paired port(disabled by default)
    /// <br/>
    /// 启用/禁用到配对端口方向的波特率仿真（默认禁用）
    /// </summary>
    /// <param name="enable">
    /// Enable or not
    /// <br/>
    /// 启用/禁用
    /// </param>
    /// <returns></returns>
    public PortBuilder EmulateBaudRate(bool enable)
    {
        return WithArgument("EmuBR", enable ? "yes" : "no");
    }

    /// <summary>
    /// Enable/disable buffer overrun (disabled by default)
    /// <br/>
    /// 启用/禁用缓冲区溢出（默认禁用）
    /// </summary>
    /// <param name="enable">
    /// Enable or not
    /// <br/>
    /// 启用/禁用
    /// </param>
    /// <returns></returns>
    public PortBuilder EmulateOverrun(bool enable)
    {
        return WithArgument("EmuOverrun", enable ? "yes" : "no");
    }

    /// <summary>
    /// Enable/disable plug-in mode, the plug-in mode port is hidden and can't be open if the paired port is not open(disabled by default)
    /// <br/>
    /// 启用/禁用插件模式，插件模式端口是隐藏的，如果配对端口未打开则无法打开（默认禁用）
    /// </summary>
    /// <param name="enable">
    /// Enable or not
    /// <br/>
    /// 启用/禁用
    /// </param>
    /// <returns></returns>
    public PortBuilder PlugInMode(bool enable)
    {
        return WithArgument("PlugInMode", enable ? "yes" : "no");
    }

    /// <summary>
    /// Enable/disable exclusive mode, the exclusive mode port is hidden if it is open(disabled by default)
    /// <br/>
    /// 启用/禁用独占模式,独占模式端口如果打开则隐藏（默认禁用）
    /// </summary>
    /// <param name="enable">
    /// Enable or not
    /// <br/>
    /// 启用/禁用
    /// </param>
    /// <returns></returns>

    public PortBuilder ExclusiveMode(bool enable)
    {
        return WithArgument("ExclusiveMode", enable ? "yes" : "no");
    }

    /// <summary>
    /// Enable/disable hidden mode, the hidden mode port is hidden as it is possible for port enumerators (disabled by default)
    /// 启用/禁用隐藏模式
    /// </summary>
    /// <param name="enable">
    /// Enable or not
    /// <br/>
    /// 启用/禁用
    /// </param>
    /// <returns></returns>
    public PortBuilder HiddenMode(bool enable)
    {
        return WithArgument("HiddenMode", enable ? "yes" : "no");
    }

    /// <summary>
    /// Enable/disable all data bits transfer disregard
    /// <br/>
    /// 启用/禁用忽略所有数据位传输
    /// </summary>
    /// <param name="enable">
    /// Enable or not
    /// <br/>
    /// 启用/禁用
    /// </param>
    /// <returns></returns>
    public PortBuilder AllDataBits(bool enable)
    {
        return WithArgument("AllDataBits", enable ? "yes" : "no");
    }

    //public PortBuilder Cts(bool enable)
    //{
    //    return WithArgument("cts", enable ? "rrts" : "!rrts");
    //}

    //public PortBuilder Dsr(bool enable)
    //{
    //    return WithArgument("dsr", enable ? "rdtr" : "!rdtr");
    //}

    //public PortBuilder Dcd(bool enable)
    //{
    //    return WithArgument("dcd", enable ? "rdtr" : "!rdtr");
    //}

    /// <summary>
    /// Ring indicator
    /// <br/>
    /// 振铃指示
    /// </summary>
    /// <param name="enable">
    /// Enable or not
    /// <br/>
    /// 启用/禁用
    /// </param>
    /// <returns></returns>
    public PortBuilder RingIndicator(bool enable)
    {
        return WithArgument("ri", enable ? "on" : "!on");
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    private PortBuilder WithArgument(string key, string value)
    {
        _arguments.AddOrUpdate(key, value, (o, n) => value);
        return this;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        var builder = new StringBuilder();
        int i = 0;
        var keys = _arguments.Keys.ToArray();
        builder.Append($"{keys[i]}={_arguments[keys[i]]}");

        while (i < _arguments.Count - 1)
        {
            i++;
            builder.Append($",{keys[i]}={_arguments[keys[i]]}");
        }

        return builder.ToString();
    }
}
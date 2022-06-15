namespace Hsu.NullModemEmulator;

/// <summary>
/// The friendly name descriptor for bus and port
/// <br/>
/// 虚拟串口对和虚拟串口的友好名称描述符
/// </summary>
public record struct FriendlyNameDescriptior
{
    /// <summary>
    /// Identity
    /// <br/>
    /// 标识
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Name
    /// <br/>
    /// 名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Friendly Name
    /// <br/>
    /// 友好名称
    /// </summary>
    public string FriendlyName { get; set; }
    /// <summary>
    /// Device type
    /// <br/>
    /// 设备类型
    /// </summary>
    public DeviceType Type { get; set; }
    /// <summary>
    /// Device order
    /// <br/>
    /// 虚拟串口顺序
    /// </summary>
    public PortBusOrder? Order { get; set; }
}
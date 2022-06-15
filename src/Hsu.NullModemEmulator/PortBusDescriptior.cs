namespace Hsu.NullModemEmulator;

/// <summary>
/// The descriptior of virtual port bus
/// <br/>
/// 虚拟串口对描述符
/// </summary>
public record PortBusDescriptior
{
    /// <summary>
    /// Identity of virtual port bus
    /// <br/>
    /// 虚拟串口对标识
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// The descriptior friendly name of bus
    /// <br/>
    /// 虚拟串口对友好名称描述符
    /// </summary>
    public FriendlyNameDescriptior Bus { get; set; }
    /// <summary>
    /// The descriptior friendly name of first virtual port
    /// <br/>
    /// A虚拟串口友好名称描述符
    /// </summary>
    public FriendlyNameDescriptior A { get; set; }
    /// <summary>
    /// The descriptior friendly name of second virtual port
    /// <br/>
    /// B虚拟串口友好名称描述符
    /// </summary>
    public FriendlyNameDescriptior B { get; set; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="id">
    /// Identity of virtual port bus
    /// <br/>
    /// 虚拟串口对标识
    /// </param>
    public PortBusDescriptior(int id)
    {
        Id = id;
    }
}
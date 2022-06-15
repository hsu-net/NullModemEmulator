namespace Hsu.NullModemEmulator;

/// <summary>
/// The pair ports bus
/// <br/>
/// 成对虚拟串口
/// </summary>
public record PortBus
{
    /// <summary>
    /// Identity of pair port
    /// <br/>
    /// 成对虚拟串口标识
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Name:CNCBUS[n]
    /// <br/>
    /// 名称:CNCBUS[n]
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// the first port of pair
    /// <br/>
    /// 第一个虚拟串口
    /// </summary>
    public PortDescriptior A { get; set; }
    /// <summary>
    /// the second port of pair
    /// <br/>
    /// 第二个虚拟串口
    /// </summary>
    public PortDescriptior B { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="id">
    /// Identity of pair port
    /// <br/>
    /// 成对虚拟串口标识
    /// </param>
    public PortBus(int id)
    {
        Id = id;
        Name = $"CNCBUS{id}";
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="a">
    /// the first port of pair
    /// <br/>
    /// 第一个虚拟串口
    /// </param>
    /// <param name="b">
    /// the second port of pair
    /// <br/>
    /// 第二个虚拟串口
    /// </param>
    public PortBus(PortDescriptior a, PortDescriptior b)
    {
        A = a;
        B = b;
    }
}
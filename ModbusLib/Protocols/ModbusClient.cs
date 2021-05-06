// Decompiled with JetBrains decompiler
// Type: ModbusLib.Protocols.ModbusClient
// Assembly: ModbusLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3455EC4F-205D-4D22-B187-B5EFF11CDE1C
// Assembly location: C:\Program Files (x86)\Farrellton Solar\Modbus Master\ModbusLib.dll

namespace ModbusLib.Protocols
{
  public class ModbusClient : IProtocol
  {
    public ModbusClient(IProtocolCodec codec) => this.Codec = codec;

    public IProtocolCodec Codec { get; private set; }

    public event ModbusCommand.OutgoingData OutgoingData;

    public event ModbusCommand.IncommingData IncommingData;

    public void OnOutgoingData(byte[] data)
    {
      if (this.OutgoingData == null)
        return;
      this.OutgoingData(data);
    }

    public void OnIncommingData(byte[] data)
    {
      if (this.IncommingData == null)
        return;
      this.IncommingData(data);
    }

    public byte Address { get; set; }

    public CommResponse ExecuteGeneric(ICommClient port, ModbusCommand command)
    {
      ClientCommData data = new ClientCommData((IProtocol) this);
      data.UserData = (object) command;
      this.Codec.ClientEncode((CommDataBase) data);
      CommResponse commResponse = port.Query(data);
      if (data.OutgoingData != null)
        this.OnOutgoingData(data.OutgoingData.ToArray());
      if (data.IncomingData != null)
        this.OnIncommingData(data.IncomingData.ToArray());
      return commResponse;
    }

    public void ExecuteAsync(ICommClient port, ModbusCommand command)
    {
      ClientCommData data = new ClientCommData((IProtocol) this);
      data.UserData = (object) command;
      this.Codec.ClientEncode((CommDataBase) data);
      port.QueryAsync(data);
    }
  }
}

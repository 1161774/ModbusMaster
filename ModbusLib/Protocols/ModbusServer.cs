// Decompiled with JetBrains decompiler
// Type: ModbusLib.Protocols.ModbusServer
// Assembly: ModbusLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3455EC4F-205D-4D22-B187-B5EFF11CDE1C
// Assembly location: C:\Program Files (x86)\Farrellton Solar\Modbus Master\ModbusLib.dll

namespace ModbusLib.Protocols
{
  public class ModbusServer : IProtocol
  {
    public ModbusServer(IProtocolCodec codec) => this.Codec = codec;

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
  }
}

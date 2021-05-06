// Decompiled with JetBrains decompiler
// Type: ModbusLib.Protocols.IProtocol
// Assembly: ModbusLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3455EC4F-205D-4D22-B187-B5EFF11CDE1C
// Assembly location: C:\Program Files (x86)\Farrellton Solar\Modbus Master\ModbusLib.dll

namespace ModbusLib.Protocols
{
  public interface IProtocol
  {
    IProtocolCodec Codec { get; }

    event ModbusCommand.OutgoingData OutgoingData;

    event ModbusCommand.IncommingData IncommingData;

    void OnOutgoingData(byte[] data);

    void OnIncommingData(byte[] data);
  }
}

// Decompiled with JetBrains decompiler
// Type: ModbusLib.Protocols.IProtocolCodec
// Assembly: ModbusLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3455EC4F-205D-4D22-B187-B5EFF11CDE1C
// Assembly location: C:\Program Files (x86)\Farrellton Solar\Modbus Master\ModbusLib.dll

namespace ModbusLib.Protocols
{
  public interface IProtocolCodec
  {
    void ClientEncode(CommDataBase data);

    CommResponse ClientDecode(CommDataBase data);

    void ServerEncode(CommDataBase data);

    CommResponse ServerDecode(CommDataBase data);
  }
}

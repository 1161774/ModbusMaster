// Decompiled with JetBrains decompiler
// Type: ModbusLib.Protocols.ModbusCommandCodec
// Assembly: ModbusLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3455EC4F-205D-4D22-B187-B5EFF11CDE1C
// Assembly location: C:\Program Files (x86)\Farrellton Solar\Modbus Master\ModbusLib.dll

namespace ModbusLib.Protocols
{
  public class ModbusCommandCodec
  {
    public virtual void ClientEncode(ModbusCommand command, ByteArrayWriter body)
    {
    }

    public virtual void ClientDecode(ModbusCommand command, ByteArrayReader body)
    {
    }

    public virtual void ServerEncode(ModbusCommand command, ByteArrayWriter body)
    {
    }

    public virtual void ServerDecode(ModbusCommand command, ByteArrayReader body)
    {
    }
  }
}

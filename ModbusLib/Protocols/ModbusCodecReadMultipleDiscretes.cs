// Decompiled with JetBrains decompiler
// Type: ModbusLib.Protocols.ModbusCodecReadMultipleDiscretes
// Assembly: ModbusLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3455EC4F-205D-4D22-B187-B5EFF11CDE1C
// Assembly location: C:\Program Files (x86)\Farrellton Solar\Modbus Master\ModbusLib.dll

namespace ModbusLib.Protocols
{
  public class ModbusCodecReadMultipleDiscretes : ModbusCommandCodec
  {
    public override void ClientEncode(ModbusCommand command, ByteArrayWriter body) => ModbusCodecBase.PushRequestHeader(command, body);

    public override void ClientDecode(ModbusCommand command, ByteArrayReader body) => ModbusCodecBase.PopDiscretes(command, body);

    public override void ServerEncode(ModbusCommand command, ByteArrayWriter body) => ModbusCodecBase.PushDiscretes(command, body);

    public override void ServerDecode(ModbusCommand command, ByteArrayReader body)
    {
      ModbusCodecBase.PopRequestHeader(command, body);
      command.Data = new ushort[command.Count];
      command.QueryTotalLength += 2;
    }
  }
}

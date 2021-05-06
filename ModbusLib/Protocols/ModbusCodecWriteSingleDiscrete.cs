// Decompiled with JetBrains decompiler
// Type: ModbusLib.Protocols.ModbusCodecWriteSingleDiscrete
// Assembly: ModbusLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3455EC4F-205D-4D22-B187-B5EFF11CDE1C
// Assembly location: C:\Program Files (x86)\Farrellton Solar\Modbus Master\ModbusLib.dll

namespace ModbusLib.Protocols
{
  public class ModbusCodecWriteSingleDiscrete : ModbusCommandCodec
  {
    public override void ClientEncode(ModbusCommand command, ByteArrayWriter body)
    {
      body.WriteUInt16BE((ushort) command.Offset);
      int num = command.Data[0] != (ushort) 0 ? 65280 : 0;
      body.WriteInt16BE((short) num);
    }

    public override void ClientDecode(ModbusCommand command, ByteArrayReader body)
    {
    }

    public override void ServerEncode(ModbusCommand command, ByteArrayWriter body) => ModbusCodecBase.PushRequestHeader(command, body);

    public override void ServerDecode(ModbusCommand command, ByteArrayReader body)
    {
      command.Offset = (int) body.ReadUInt16BE();
      command.Count = 1;
      command.QueryTotalLength += 2;
      command.Data = new ushort[1];
      command.Data[0] = body.ReadUInt16BE() != (ushort) 0 ? ushort.MaxValue : (ushort) 0;
    }
  }
}

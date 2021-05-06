// Decompiled with JetBrains decompiler
// Type: ModbusLib.Protocols.ModbusCodecBase
// Assembly: ModbusLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3455EC4F-205D-4D22-B187-B5EFF11CDE1C
// Assembly location: C:\Program Files (x86)\Farrellton Solar\Modbus Master\ModbusLib.dll

namespace ModbusLib.Protocols
{
  public class ModbusCodecBase
  {
    public static readonly ModbusCommandCodec[] CommandCodecs = new ModbusCommandCodec[24];

    static ModbusCodecBase()
    {
      ModbusCodecBase.CommandCodecs[3] = (ModbusCommandCodec) new ModbusCodecReadMultipleRegisters();
      ModbusCodecBase.CommandCodecs[16] = (ModbusCommandCodec) new ModbusCodecWriteMultipleRegisters();
      ModbusCodecBase.CommandCodecs[1] = (ModbusCommandCodec) new ModbusCodecReadMultipleDiscretes();
      ModbusCodecBase.CommandCodecs[2] = (ModbusCommandCodec) new ModbusCodecReadMultipleDiscretes();
      ModbusCodecBase.CommandCodecs[4] = (ModbusCommandCodec) new ModbusCodecReadMultipleRegisters();
      ModbusCodecBase.CommandCodecs[5] = (ModbusCommandCodec) new ModbusCodecWriteSingleDiscrete();
      ModbusCodecBase.CommandCodecs[6] = (ModbusCommandCodec) new ModbusCodecWriteSingleRegister();
      ModbusCodecBase.CommandCodecs[15] = (ModbusCommandCodec) new ModbusCodecForceMultipleCoils();
    }

    internal static void PushRequestHeader(ModbusCommand command, ByteArrayWriter body)
    {
      body.WriteUInt16BE((ushort) command.Offset);
      body.WriteInt16BE((short) command.Count);
    }

    internal static void PopRequestHeader(ModbusCommand command, ByteArrayReader body)
    {
      command.Offset = (int) body.ReadUInt16BE();
      command.Count = (int) body.ReadInt16BE();
    }

    internal static void PushDiscretes(ModbusCommand command, ByteArrayWriter body)
    {
      int count = command.Count;
      body.WriteByte((byte) ((count + 7) / 8));
      for (int index = 0; index < count; ++index)
      {
        byte num1 = (byte) ((uint) command.Data[index] >> 8);
        byte num2 = (byte) ((uint) command.Data[index] & (uint) byte.MaxValue);
        body.WriteByte(num1);
        body.WriteByte(num2);
      }
    }

    internal static void PopDiscretes(ModbusCommand command, ByteArrayReader body)
    {
      byte num1 = body.ReadByte();
      int count = command.Count;
      command.Data = new ushort[count];
      command.QueryTotalLength += (int) num1 + 1;
      byte num2;
      byte num3;
      for (int index = 0; !body.EndOfBuffer && command.Count > index; command.Data[index++] = (ushort) ((uint) num2 << 8 | (uint) num3))
      {
        num2 = body.CanRead(1) ? body.ReadByte() : (byte) 0;
        num3 = body.CanRead(1) ? body.ReadByte() : (byte) 0;
      }
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: ModbusLib.Protocols.ModbusRtuCodec
// Assembly: ModbusLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3455EC4F-205D-4D22-B187-B5EFF11CDE1C
// Assembly location: C:\Program Files (x86)\Farrellton Solar\Modbus Master\ModbusLib.dll

namespace ModbusLib.Protocols
{
  public class ModbusRtuCodec : ModbusCodecBase, IProtocolCodec
  {
    void IProtocolCodec.ClientEncode(CommDataBase data)
    {
      ModbusClient ownerProtocol = (ModbusClient) data.OwnerProtocol;
      ModbusCommand userData = (ModbusCommand) data.UserData;
      byte functionCode = userData.FunctionCode;
      ByteArrayWriter byteArrayWriter1 = new ByteArrayWriter();
      ModbusCodecBase.CommandCodecs[(int) functionCode]?.ClientEncode(userData, byteArrayWriter1);
      ByteArrayWriter byteArrayWriter2 = new ByteArrayWriter();
      byteArrayWriter2.WriteByte(ownerProtocol.Address);
      byteArrayWriter2.WriteByte(functionCode);
      byteArrayWriter2.WriteBytes(byteArrayWriter1);
      ushort num = ByteArrayHelpers.CalcCRC16(byteArrayWriter2.ToArray(), 0, byteArrayWriter2.Length);
      byteArrayWriter2.WriteInt16LE((short) num);
      data.OutgoingData = byteArrayWriter2.ToReader();
    }

    CommResponse IProtocolCodec.ClientDecode(CommDataBase data)
    {
      ModbusClient ownerProtocol = (ModbusClient) data.OwnerProtocol;
      ModbusCommand userData = (ModbusCommand) data.UserData;
      ByteArrayReader incomingData = data.IncomingData;
      int count = incomingData.Length - 4;
      if (count >= 0 && (int) incomingData.ReadByte() == (int) ownerProtocol.Address)
      {
        byte num1 = incomingData.ReadByte();
        ByteArrayReader body = new ByteArrayReader(incomingData.ReadBytes(count));
        ushort num2 = ByteArrayHelpers.CalcCRC16(incomingData.ToArray(), 0, incomingData.Length - 2);
        if ((int) incomingData.ReadInt16LE() == (int) (short) num2 && ((int) num1 & (int) sbyte.MaxValue) == (int) userData.FunctionCode)
        {
          if (num1 <= (byte) 127)
          {
            ModbusCodecBase.CommandCodecs[(int) num1]?.ClientDecode(userData, body);
            return new CommResponse(data, 3);
          }
          if (incomingData.CanRead(1))
            userData.ExceptionCode = incomingData.ReadByte();
          return new CommResponse(data, 2);
        }
      }
      return new CommResponse(data, 0);
    }

    void IProtocolCodec.ServerEncode(CommDataBase data)
    {
      ModbusServer ownerProtocol = (ModbusServer) data.OwnerProtocol;
      ModbusCommand userData = (ModbusCommand) data.UserData;
      byte functionCode = userData.FunctionCode;
      ByteArrayWriter byteArrayWriter1 = new ByteArrayWriter();
      ModbusCodecBase.CommandCodecs[(int) functionCode]?.ServerEncode(userData, byteArrayWriter1);
      if (userData.ExceptionCode == (byte) 0)
      {
        int length = byteArrayWriter1.Length;
      }
      ByteArrayWriter byteArrayWriter2 = new ByteArrayWriter();
      byteArrayWriter2.WriteByte(ownerProtocol.Address);
      if (userData.ExceptionCode == (byte) 0)
      {
        byteArrayWriter2.WriteByte(userData.FunctionCode);
        byteArrayWriter2.WriteBytes(byteArrayWriter1);
      }
      else
      {
        byteArrayWriter2.WriteByte((byte) ((uint) userData.FunctionCode | 128U));
        byteArrayWriter2.WriteByte(userData.ExceptionCode);
      }
      ushort num = ByteArrayHelpers.CalcCRC16(byteArrayWriter2.ToArray(), 0, byteArrayWriter2.Length);
      byteArrayWriter2.WriteInt16LE((short) num);
      data.OutgoingData = byteArrayWriter2.ToReader();
    }

    CommResponse IProtocolCodec.ServerDecode(CommDataBase data)
    {
      ModbusServer ownerProtocol = (ModbusServer) data.OwnerProtocol;
      ByteArrayReader incomingData = data.IncomingData;
      int length = incomingData.Length;
      if (length < 4)
        return new CommResponse(data, 0);
      if ((int) incomingData.ReadByte() == (int) ownerProtocol.Address)
      {
        byte fc = incomingData.ReadByte();
        if ((int) fc < ModbusCodecBase.CommandCodecs.Length)
        {
          ModbusCommand command = new ModbusCommand(fc);
          data.UserData = (object) command;
          command.QueryTotalLength = 6;
          ModbusCommandCodec commandCodec = ModbusCodecBase.CommandCodecs[(int) fc];
          ByteArrayReader body = new ByteArrayReader(incomingData.ReadBytes(length - 4));
          commandCodec.ServerDecode(command, body);
          ushort num = ByteArrayHelpers.CalcCRC16(incomingData.ToArray(), 0, command.QueryTotalLength - 2);
          if ((int) ByteArrayHelpers.ReadInt16LE(((IByteArray) incomingData).Data, command.QueryTotalLength - 2) == (int) (short) num)
            return new CommResponse(data, 3);
        }
      }
      return new CommResponse(data, 1);
    }
  }
}

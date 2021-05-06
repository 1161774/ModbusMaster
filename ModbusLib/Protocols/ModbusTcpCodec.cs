// Decompiled with JetBrains decompiler
// Type: ModbusLib.Protocols.ModbusTcpCodec
// Assembly: ModbusLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3455EC4F-205D-4D22-B187-B5EFF11CDE1C
// Assembly location: C:\Program Files (x86)\Farrellton Solar\Modbus Master\ModbusLib.dll

using System;

namespace ModbusLib.Protocols
{
  public class ModbusTcpCodec : ModbusCodecBase, IProtocolCodec
  {
    void IProtocolCodec.ClientEncode(CommDataBase data)
    {
      ModbusClient ownerProtocol = (ModbusClient) data.OwnerProtocol;
      ModbusCommand userData = (ModbusCommand) data.UserData;
      byte functionCode = userData.FunctionCode;
      ByteArrayWriter byteArrayWriter1 = new ByteArrayWriter();
      ModbusCodecBase.CommandCodecs[(int) functionCode]?.ClientEncode(userData, byteArrayWriter1);
      int num = 2 + byteArrayWriter1.Length;
      ByteArrayWriter byteArrayWriter2 = new ByteArrayWriter();
      byteArrayWriter2.WriteUInt16BE((ushort) userData.TransId);
      byteArrayWriter2.WriteInt16BE((short) 0);
      byteArrayWriter2.WriteInt16BE((short) num);
      byteArrayWriter2.WriteByte(ownerProtocol.Address);
      byteArrayWriter2.WriteByte(functionCode);
      byteArrayWriter2.WriteBytes(byteArrayWriter1);
      data.OutgoingData = byteArrayWriter2.ToReader();
    }

    CommResponse IProtocolCodec.ClientDecode(CommDataBase data)
    {
      ModbusClient ownerProtocol = (ModbusClient) data.OwnerProtocol;
      ModbusCommand userData = (ModbusCommand) data.UserData;
      ByteArrayReader incomingData = data.IncomingData;
      if (incomingData.Length >= 6 && (int) incomingData.ReadUInt16BE() == (int) (ushort) userData.TransId && incomingData.ReadInt16BE() == (short) 0)
      {
        short num1 = incomingData.ReadInt16BE();
        if (incomingData.Length >= (int) num1 + 6 && (int) incomingData.ReadByte() == (int) ownerProtocol.Address)
        {
          byte num2 = incomingData.ReadByte();
          if (((int) num2 & (int) sbyte.MaxValue) == (int) userData.FunctionCode)
          {
            if (num2 <= (byte) 127)
            {
              ByteArrayReader body = new ByteArrayReader(incomingData.ReadToEnd());
              ModbusCodecBase.CommandCodecs[(int) num2]?.ClientDecode(userData, body);
              return new CommResponse(data, 3);
            }
            userData.ExceptionCode = incomingData.ReadByte();
            return new CommResponse(data, 2);
          }
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
      int num = userData.ExceptionCode == (byte) 0 ? 2 + byteArrayWriter1.Length : 3;
      ByteArrayWriter byteArrayWriter2 = new ByteArrayWriter();
      byteArrayWriter2.WriteUInt16BE((ushort) userData.TransId);
      byteArrayWriter2.WriteInt16BE((short) 0);
      byteArrayWriter2.WriteInt16BE((short) num);
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
      data.OutgoingData = byteArrayWriter2.ToReader();
    }

    CommResponse IProtocolCodec.ServerDecode(CommDataBase data)
    {
      ModbusServer ownerProtocol = (ModbusServer) data.OwnerProtocol;
      ByteArrayReader incomingData = data.IncomingData;
      if (incomingData.Length >= 6)
      {
        ushort num1 = incomingData.ReadUInt16BE();
        if (incomingData.ReadInt16BE() == (short) 0)
        {
          short num2 = incomingData.ReadInt16BE();
          if (incomingData.Length >= (int) num2 + 6)
          {
            if ((int) incomingData.ReadByte() == (int) ownerProtocol.Address)
            {
              byte fc = incomingData.ReadByte();
              if ((int) fc >= ModbusCodecBase.CommandCodecs.Length)
                throw new ApplicationException("Unknown function code");
              ModbusCommand command = new ModbusCommand(fc);
              data.UserData = (object) command;
              command.TransId = (int) num1;
              ByteArrayReader body = new ByteArrayReader(incomingData.ReadToEnd());
              ModbusCodecBase.CommandCodecs[(int) fc]?.ServerDecode(command, body);
              return new CommResponse(data, 3);
            }
          }
          else
            goto label_10;
        }
        return new CommResponse(data, 1);
      }
label_10:
      return new CommResponse(data, 0);
    }
  }
}

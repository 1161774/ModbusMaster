// Decompiled with JetBrains decompiler
// Type: ModbusLib.Protocols.ModbusCommand
// Assembly: ModbusLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3455EC4F-205D-4D22-B187-B5EFF11CDE1C
// Assembly location: C:\Program Files (x86)\Farrellton Solar\Modbus Master\ModbusLib.dll

namespace ModbusLib.Protocols
{
  public class ModbusCommand
  {
    public const byte FuncReadMultipleRegisters = 3;
    public const byte FuncWriteMultipleRegisters = 16;
    public const byte FuncReadCoils = 1;
    public const byte FuncReadInputDiscretes = 2;
    public const byte FuncReadInputRegisters = 4;
    public const byte FuncWriteCoil = 5;
    public const byte FuncWriteSingleRegister = 6;
    public const byte FuncReadExceptionStatus = 7;
    public const byte ExceptionConnectionLost = 254;
    public const byte SendFail = 100;
    public const byte ExceptionOffset = 128;
    public const byte FuncForceMultipleCoils = 15;
    public const byte ErrorIllegalFunction = 1;
    public const byte ErrorIllegalDataAddress = 2;
    public const byte ErrorIllegalDataValue = 3;
    public const byte ErrorIllegalResponseLength = 4;
    public const byte ErrorAcknowledge = 5;
    public const byte ErrorSlaveDeviceBusy = 6;
    public const byte ErrorNegativeAcknowledge = 7;
    public const byte ErrorMemoryParity = 8;
    public readonly byte FunctionCode;
    public int TransId;
    internal int QueryTotalLength;

    public ModbusCommand(byte fc) => this.FunctionCode = fc;

    public int Offset { get; set; }

    public int Count { get; set; }

    public ushort[] Data { get; set; }

    public byte ExceptionCode { get; set; }

    public delegate void OutgoingData(byte[] data);

    public delegate void IncommingData(byte[] data);
  }
}

// Decompiled with JetBrains decompiler
// Type: ModbusLib.CommResponse
// Assembly: ModbusLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3455EC4F-205D-4D22-B187-B5EFF11CDE1C
// Assembly location: C:\Program Files (x86)\Farrellton Solar\Modbus Master\ModbusLib.dll

namespace ModbusLib
{
  public class CommResponse
  {
    public const int Unknown = 0;
    public const int Ignore = 1;
    public const int Critical = 2;
    public const int Ack = 3;
    public readonly CommDataBase Data;
    public readonly int Status;

    public CommResponse(CommDataBase data, int status)
    {
      this.Data = data;
      this.Status = status;
    }
  }
}

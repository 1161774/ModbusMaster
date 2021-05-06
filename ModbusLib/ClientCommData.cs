// Decompiled with JetBrains decompiler
// Type: ModbusLib.ClientCommData
// Assembly: ModbusLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3455EC4F-205D-4D22-B187-B5EFF11CDE1C
// Assembly location: C:\Program Files (x86)\Farrellton Solar\Modbus Master\ModbusLib.dll

using ModbusLib.Protocols;

namespace ModbusLib
{
  public class ClientCommData : CommDataBase
  {
    public int Timeout = 1000;
    public int Retries = 3;

    public ClientCommData(IProtocol protocol)
      : base(protocol)
    {
    }
  }
}

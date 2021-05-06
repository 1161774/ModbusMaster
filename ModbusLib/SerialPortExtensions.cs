// Decompiled with JetBrains decompiler
// Type: ModbusLib.SerialPortExtensions
// Assembly: ModbusLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3455EC4F-205D-4D22-B187-B5EFF11CDE1C
// Assembly location: C:\Program Files (x86)\Farrellton Solar\Modbus Master\ModbusLib.dll

using ModbusLib.Protocols;
using System.IO.Ports;

namespace ModbusLib
{
  public static class SerialPortExtensions
  {
    public static ICommClient GetClient(this SerialPort port) => (ICommClient) new SerialPortClient(port);

    public static ICommServer GetListener(this SerialPort port, IProtocol protocol) => (ICommServer) new SerialPortServer(port, protocol);
  }
}

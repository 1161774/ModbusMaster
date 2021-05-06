// Decompiled with JetBrains decompiler
// Type: ModbusLib.SocketExtensions
// Assembly: ModbusLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3455EC4F-205D-4D22-B187-B5EFF11CDE1C
// Assembly location: C:\Program Files (x86)\Farrellton Solar\Modbus Master\ModbusLib.dll

using ModbusLib.Protocols;
using System.Net.Sockets;

namespace ModbusLib
{
  public static class SocketExtensions
  {
    public static ICommClient GetClient(this Socket port) => (ICommClient) new IpClient(port);

    public static ICommServer GetTcpListener(this Socket port, IProtocol protocol) => (ICommServer) new TcpServer(port.Accept(), protocol);

    public static ICommServer GetUdpListener(this Socket port, IProtocol protocol) => (ICommServer) new UdpServer(port, protocol);
  }
}

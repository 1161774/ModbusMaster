// Decompiled with JetBrains decompiler
// Type: ModbusLib.UdpServer
// Assembly: ModbusLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3455EC4F-205D-4D22-B187-B5EFF11CDE1C
// Assembly location: C:\Program Files (x86)\Farrellton Solar\Modbus Master\ModbusLib.dll

using ModbusLib.Protocols;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ModbusLib
{
  internal class UdpServer : IpServer
  {
    public UdpServer(Socket port, IProtocol protocol)
      : base(port, protocol)
    {
    }

    protected override void Worker()
    {
      while (!this._closing)
      {
        int available = this.Port.Available;
        if (available > 0)
        {
          byte[] numArray = new byte[available];
          EndPoint remoteEP = (EndPoint) new IPEndPoint(IPAddress.Any, 0);
          this.Port.ReceiveFrom(numArray, ref remoteEP);
          this.Protocol.OnIncommingData(numArray);
          ServerCommData serverCommData = new ServerCommData(this.Protocol);
          serverCommData.IncomingData = new ByteArrayReader(numArray);
          ServerCommData data = serverCommData;
          if (this.Protocol.Codec.ServerDecode((CommDataBase) data).Status == 3)
          {
            this.OnServeCommand(data);
            this.Protocol.Codec.ServerEncode((CommDataBase) data);
            byte[] array = data.OutgoingData.ToArray();
            this.Port.SendTo(array, remoteEP);
            this.Protocol.OnOutgoingData(array);
          }
        }
        Thread.Sleep(100);
      }
    }
  }
}

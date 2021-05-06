// Decompiled with JetBrains decompiler
// Type: ModbusLib.TcpServer
// Assembly: ModbusLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3455EC4F-205D-4D22-B187-B5EFF11CDE1C
// Assembly location: C:\Program Files (x86)\Farrellton Solar\Modbus Master\ModbusLib.dll

using ModbusLib.Protocols;
using System.Net.Sockets;
using System.Threading;

namespace ModbusLib
{
  internal class TcpServer : IpServer
  {
    private const int CacheSize = 300;
    internal int IdleTimeout = 60;

    public TcpServer(Socket port, IProtocol protocol)
      : base(port, protocol)
    {
    }

    protected override void Worker()
    {
      int counter = this.IdleTimeout;
      using (new Timer((TimerCallback) (_ => --counter), (object) null, 1000, 1000))
      {
        ByteArrayWriter byteArrayWriter = (ByteArrayWriter) null;
        byte[] numArray = new byte[300];
        while (!this._closing)
        {
          if (counter > 0)
          {
            int num = this.Port.Available;
            if (num > 0)
            {
              if (num > 300)
                num = 300;
              this.Port.Receive(numArray, num, SocketFlags.None);
              this.Protocol.OnIncommingData(numArray);
              if (byteArrayWriter == null)
                byteArrayWriter = new ByteArrayWriter();
              byteArrayWriter.WriteBytes(numArray, 0, num);
              ServerCommData data = new ServerCommData(this.Protocol);
              data.IncomingData = byteArrayWriter.ToReader();
              switch (this.Protocol.Codec.ServerDecode((CommDataBase) data).Status)
              {
                case 1:
                  byteArrayWriter = (ByteArrayWriter) null;
                  break;
                case 3:
                  this.OnServeCommand(data);
                  this.Protocol.Codec.ServerEncode((CommDataBase) data);
                  if (data.OutgoingData != null)
                  {
                    byte[] array = data.OutgoingData.ToArray();
                    this.Port.Send(array);
                    this.Protocol.OnOutgoingData(array);
                  }
                  counter = this.IdleTimeout;
                  byteArrayWriter = (ByteArrayWriter) null;
                  break;
              }
            }
            Thread.Sleep(100);
          }
          else
            break;
        }
      }
      this.Port.Close();
    }
  }
}

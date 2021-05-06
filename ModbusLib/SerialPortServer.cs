// Decompiled with JetBrains decompiler
// Type: ModbusLib.SerialPortServer
// Assembly: ModbusLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3455EC4F-205D-4D22-B187-B5EFF11CDE1C
// Assembly location: C:\Program Files (x86)\Farrellton Solar\Modbus Master\ModbusLib.dll

using ModbusLib.Protocols;
using System.IO.Ports;
using System.Threading;

namespace ModbusLib
{
  internal class SerialPortServer : ICommServer
  {
    public readonly SerialPort Port;
    public readonly IProtocol Protocol;
    private Thread _thread;
    protected bool _closing;

    public SerialPortServer(SerialPort port, IProtocol protocol)
    {
      this.Port = port;
      this.Protocol = protocol;
    }

    public void Start()
    {
      this._thread = new Thread(new ThreadStart(this.Worker));
      this._thread.Start();
    }

    private void Worker()
    {
      ByteArrayWriter byteArrayWriter = (ByteArrayWriter) null;
      while (!this._closing)
      {
        int bytesToRead = this.Port.BytesToRead;
        if (bytesToRead > 0)
        {
          byte[] numArray = new byte[bytesToRead];
          this.Port.Read(numArray, 0, bytesToRead);
          this.Protocol.OnIncommingData(numArray);
          if (byteArrayWriter == null)
            byteArrayWriter = new ByteArrayWriter();
          byteArrayWriter.WriteBytes(numArray, 0, bytesToRead);
          ServerCommData data = new ServerCommData(this.Protocol);
          data.IncomingData = byteArrayWriter.ToReader();
          CommResponse commResponse = this.Protocol.Codec.ServerDecode((CommDataBase) data);
          if (commResponse.Status == 3)
          {
            this.OnServeCommand(data);
            this.Protocol.Codec.ServerEncode((CommDataBase) data);
            byte[] array = data.OutgoingData.ToArray();
            this.Port.Write(array, 0, array.Length);
            this.Protocol.OnOutgoingData(array);
            byteArrayWriter = (ByteArrayWriter) null;
          }
          else if (commResponse.Status == 1)
            byteArrayWriter = (ByteArrayWriter) null;
        }
        Thread.Sleep(100);
      }
    }

    public void Abort()
    {
      this._closing = true;
      if (this._thread == null || !this._thread.IsAlive)
        return;
      this._thread.Join();
    }

    public event ServeCommandHandler ServeCommand;

    protected virtual void OnServeCommand(ServerCommData data)
    {
      ServeCommandHandler serveCommand = this.ServeCommand;
      if (serveCommand == null)
        return;
      serveCommand((object) this, new ServeCommandEventArgs(data));
    }
  }
}

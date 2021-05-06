// Decompiled with JetBrains decompiler
// Type: ModbusLib.IpServer
// Assembly: ModbusLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3455EC4F-205D-4D22-B187-B5EFF11CDE1C
// Assembly location: C:\Program Files (x86)\Farrellton Solar\Modbus Master\ModbusLib.dll

using ModbusLib.Protocols;
using System.Net.Sockets;
using System.Threading;

namespace ModbusLib
{
  internal abstract class IpServer : ICommServer
  {
    public readonly Socket Port;
    public readonly IProtocol Protocol;
    private Thread _thread;
    protected bool _closing;

    protected IpServer(Socket port, IProtocol protocol)
    {
      this.Port = port;
      this.Protocol = protocol;
    }

    public void Start()
    {
      this._thread = new Thread(new ThreadStart(this.Worker));
      this._thread.Start();
    }

    protected abstract void Worker();

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

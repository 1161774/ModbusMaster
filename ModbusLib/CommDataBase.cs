// Decompiled with JetBrains decompiler
// Type: ModbusLib.CommDataBase
// Assembly: ModbusLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3455EC4F-205D-4D22-B187-B5EFF11CDE1C
// Assembly location: C:\Program Files (x86)\Farrellton Solar\Modbus Master\ModbusLib.dll

using ModbusLib.Protocols;

namespace ModbusLib
{
  public class CommDataBase
  {
    protected CommDataBase(IProtocol protocol) => this.OwnerProtocol = protocol;

    public IProtocol OwnerProtocol { get; private set; }

    public object UserData { get; internal set; }

    public ByteArrayReader OutgoingData { get; internal set; }

    public ByteArrayReader IncomingData { get; internal set; }
  }
}

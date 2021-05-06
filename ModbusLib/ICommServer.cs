// Decompiled with JetBrains decompiler
// Type: ModbusLib.ICommServer
// Assembly: ModbusLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3455EC4F-205D-4D22-B187-B5EFF11CDE1C
// Assembly location: C:\Program Files (x86)\Farrellton Solar\Modbus Master\ModbusLib.dll

namespace ModbusLib
{
  public interface ICommServer
  {
    void Start();

    void Abort();

    event ServeCommandHandler ServeCommand;
  }
}

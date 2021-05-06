// Decompiled with JetBrains decompiler
// Type: ModbusLib.ServeCommandEventArgs
// Assembly: ModbusLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3455EC4F-205D-4D22-B187-B5EFF11CDE1C
// Assembly location: C:\Program Files (x86)\Farrellton Solar\Modbus Master\ModbusLib.dll

using System;

namespace ModbusLib
{
  public class ServeCommandEventArgs : EventArgs
  {
    public ServeCommandEventArgs(ServerCommData data) => this.Data = data;

    public ServerCommData Data { get; private set; }
  }
}

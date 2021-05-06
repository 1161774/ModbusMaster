// Decompiled with JetBrains decompiler
// Type: ModbusMaster.Program
// Assembly: ModbusMaster, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 37E26B38-B86A-41D8-BB81-49883CB2BCAB
// Assembly location: C:\Program Files (x86)\Farrellton Solar\Modbus Master\ModbusMaster.exe

using System;
using System.Windows.Forms;

namespace ModbusMaster
{
  internal static class Program
  {
    [STAThread]
    private static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run((Form) new MasterForm());
    }
  }
}

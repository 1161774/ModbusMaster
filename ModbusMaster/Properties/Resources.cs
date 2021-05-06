// Decompiled with JetBrains decompiler
// Type: ModbusMaster.Properties.Resources
// Assembly: ModbusMaster, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 37E26B38-B86A-41D8-BB81-49883CB2BCAB
// Assembly location: C:\Program Files (x86)\Farrellton Solar\Modbus Master\ModbusMaster.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace ModbusMaster.Properties
{
  [DebuggerNonUserCode]
  [CompilerGenerated]
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (object.ReferenceEquals((object) ModbusMaster.Properties.Resources.resourceMan, (object) null))
          ModbusMaster.Properties.Resources.resourceMan = new ResourceManager("ModbusMaster.Properties.Resources", typeof (ModbusMaster.Properties.Resources).Assembly);
        return ModbusMaster.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => ModbusMaster.Properties.Resources.resourceCulture;
      set => ModbusMaster.Properties.Resources.resourceCulture = value;
    }
  }
}

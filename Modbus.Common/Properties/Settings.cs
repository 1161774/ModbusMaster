// Decompiled with JetBrains decompiler
// Type: Modbus.Common.Properties.Settings
// Assembly: Modbus.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4D2ECFBB-7EFF-4853-BA31-64D66B5217FA
// Assembly location: C:\Program Files (x86)\Farrellton Solar\Modbus Master\Modbus.Common.dll

using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.IO.Ports;
using System.Runtime.CompilerServices;

namespace Modbus.Common.Properties
{
  [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
  [CompilerGenerated]
  public sealed class Settings : ApplicationSettingsBase
  {
    private static Settings defaultInstance = (Settings) SettingsBase.Synchronized((SettingsBase) new Settings());

    public static Settings Default => Settings.defaultInstance;

    [DebuggerNonUserCode]
    [DefaultSettingValue("TCP")]
    [UserScopedSetting]
    public string CommunicationMode
    {
      get => (string) this[nameof (CommunicationMode)];
      set => this[nameof (CommunicationMode)] = (object) value;
    }

    [DebuggerNonUserCode]
    [DefaultSettingValue("127.0.0.1")]
    [UserScopedSetting]
    public string IPAddress
    {
      get => (string) this[nameof (IPAddress)];
      set => this[nameof (IPAddress)] = (object) value;
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("502")]
    public int TCPPort
    {
      get => (int) this[nameof (TCPPort)];
      set => this[nameof (TCPPort)] = (object) value;
    }

    [DebuggerNonUserCode]
    [UserScopedSetting]
    [DefaultSettingValue("COM1")]
    public string PortName
    {
      get => (string) this[nameof (PortName)];
      set => this[nameof (PortName)] = (object) value;
    }

    [DebuggerNonUserCode]
    [DefaultSettingValue("9600")]
    [UserScopedSetting]
    public int Baud
    {
      get => (int) this[nameof (Baud)];
      set => this[nameof (Baud)] = (object) value;
    }

    [DefaultSettingValue("None")]
    [UserScopedSetting]
    [DebuggerNonUserCode]
    public Parity Parity
    {
      get => (Parity) this[nameof (Parity)];
      set => this[nameof (Parity)] = (object) value;
    }

    [DebuggerNonUserCode]
    [UserScopedSetting]
    [DefaultSettingValue("0")]
    public ushort StartAddress
    {
      get => (ushort) this[nameof (StartAddress)];
      set => this[nameof (StartAddress)] = (object) value;
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("64")]
    public ushort DataLength
    {
      get => (ushort) this[nameof (DataLength)];
      set => this[nameof (DataLength)] = (object) value;
    }

    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    [UserScopedSetting]
    public byte SlaveId
    {
      get => (byte) this[nameof (SlaveId)];
      set => this[nameof (SlaveId)] = (object) value;
    }

    [UserScopedSetting]
    [DefaultSettingValue("1")]
    [DebuggerNonUserCode]
    public int SlaveDelay
    {
      get => (int) this[nameof (SlaveDelay)];
      set => this[nameof (SlaveDelay)] = (object) value;
    }

    [DebuggerNonUserCode]
    [DefaultSettingValue("8")]
    [UserScopedSetting]
    public int DataBits
    {
      get => (int) this[nameof (DataBits)];
      set => this[nameof (DataBits)] = (object) value;
    }

    [DebuggerNonUserCode]
    [DefaultSettingValue("One")]
    [UserScopedSetting]
    public StopBits StopBits
    {
      get => (StopBits) this[nameof (StopBits)];
      set => this[nameof (StopBits)] = (object) value;
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("Integer")]
    public string DisplayFormat
    {
      get => (string) this[nameof (DisplayFormat)];
      set => this[nameof (DisplayFormat)] = (object) value;
    }

    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    [UserScopedSetting]
    public byte Function
    {
      get => (byte) this[nameof (Function)];
      set => this[nameof (Function)] = (object) value;
    }
  }
}

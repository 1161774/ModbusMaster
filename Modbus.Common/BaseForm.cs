// Decompiled with JetBrains decompiler
// Type: Modbus.Common.BaseForm
// Assembly: Modbus.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4D2ECFBB-7EFF-4853-BA31-64D66B5217FA
// Assembly location: C:\Program Files (x86)\Farrellton Solar\Modbus Master\Modbus.Common.dll

using Modbus.Common.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace Modbus.Common
{
  public class BaseForm : Form
  {
    private DisplayFormat _displayFormat = DisplayFormat.Integer;
    private CommunicationMode _communicationMode;
    protected Socket _socket;
    protected readonly ushort[] _registerData = new ushort[65600];
    protected int _displayCtrlCount;
    private bool _logPaused;
    private ushort _startAddress;
    private ushort _dataLength;
    protected IContainer components;
    protected GroupBox groupBox4;
    protected ListBox listBoxCommLog;
    protected Label label7;
    protected TextBox textBoxSlaveID;
    protected GroupBox groupBox3;
    protected RadioButton radioButtonLED;
    protected RadioButton radioButtonInteger;
    protected RadioButton radioButtonHex;
    protected RadioButton radioButtonBinary;
    protected Button buttonClear;
    protected Button buttonImport;
    protected Button buttonExport;
    protected GroupBox grpStart;
    protected GroupBox groupBoxRTU;
    protected ComboBox comboBoxSerialPorts;
    protected Label label4;
    protected Label label5;
    protected GroupBox groupBoxMode;
    protected RadioButton radioButtonUDP;
    protected RadioButton radioButtonTCP;
    protected GroupBox groupBoxTCP;
    protected Label label6;
    protected TextBox textBoxPort;
    protected Label label1;
    protected TextBox textBoxSlaveDelay;
    protected ComboBox comboBoxBaudRate;
    protected RadioButton radioButtonRTU;
    protected OpenFileDialog openFileDialog;
    protected SaveFileDialog saveFileDialog;
    protected Label label8;
    protected TextBox txtIP;
    protected ComboBox comboBoxStopBits;
    protected Label label10;
    protected ComboBox comboBoxDataBits;
    protected Label label9;
    protected ComboBox comboBoxParity;
    protected Label labelParity;
    protected TabControl tabControl1;
    protected TabPage tabPage1;
    protected TabPage tabPage2;
    protected DataTab dataTab1;
    protected DataTab dataTab2;
    protected GroupBox grpExchange;
    protected Button buttonPauseLog;

    public BaseForm() => this.InitializeComponent();

    private void BaseFormLoading(object sender, EventArgs e)
    {
      this.comboBoxBaudRate.SelectedIndex = 4;
      this.FillRTUDropDownLists();
      this.CurrentTab.RegisterData = this._registerData;
      this.LoadUserData();
      this.CurrentTab.DisplayFormat = this.DisplayFormat;
      this.RefreshData();
    }

    private void BaseFormClosing(object sender, FormClosingEventArgs e) => this.SaveUserData();

    private void FillRTUDropDownLists()
    {
      this.comboBoxSerialPorts.Items.Clear();
      foreach (object portName in SerialPort.GetPortNames())
        this.comboBoxSerialPorts.Items.Add(portName);
      this.comboBoxSerialPorts.SelectedIndex = -1;
      this.comboBoxParity.Items.Clear();
      this.comboBoxParity.Items.Add((object) Parity.None.ToString());
      this.comboBoxParity.Items.Add((object) Parity.Odd.ToString());
      this.comboBoxParity.Items.Add((object) Parity.Even.ToString());
      this.comboBoxParity.Items.Add((object) Parity.Mark.ToString());
      this.comboBoxParity.Items.Add((object) Parity.Space.ToString());
    }

    private void LoadUserData()
    {
      CommunicationMode result1;
      if (System.Enum.TryParse<CommunicationMode>(Settings.Default.CommunicationMode, out result1))
        this.CommunicationMode = result1;
      DisplayFormat result2;
      if (System.Enum.TryParse<DisplayFormat>(Settings.Default.DisplayFormat, out result2))
        this.DisplayFormat = result2;
      IPAddress address;
      if (IPAddress.TryParse(Settings.Default.IPAddress, out address))
        this.IPAddress = address;
      this.TCPPort = Settings.Default.TCPPort;
      this.PortName = Settings.Default.PortName;
      this.Baud = Settings.Default.Baud;
      this.Parity = Settings.Default.Parity;
      this.StartAddress = Settings.Default.StartAddress;
      this.DataLength = Settings.Default.DataLength;
      this.SlaveId = Settings.Default.SlaveId;
      this.SlaveDelay = Settings.Default.SlaveDelay;
      this.DataBits = Settings.Default.DataBits;
      this.StopBits = Settings.Default.StopBits;
    }

    private void SaveUserData()
    {
      Settings.Default.CommunicationMode = this.CommunicationMode.ToString();
      Settings.Default.IPAddress = this.IPAddress.ToString();
      Settings.Default.DisplayFormat = this.DisplayFormat.ToString();
      Settings.Default.TCPPort = this.TCPPort;
      Settings.Default.PortName = this.PortName;
      Settings.Default.Baud = this.Baud;
      Settings.Default.Parity = this.Parity;
      Settings.Default.StartAddress = this.StartAddress;
      Settings.Default.DataLength = this.DataLength;
      Settings.Default.SlaveId = this.SlaveId;
      Settings.Default.SlaveDelay = this.SlaveDelay;
      Settings.Default.DataBits = this.DataBits;
      Settings.Default.StopBits = this.StopBits;
      Settings.Default.Save();
    }

    private void ButtonImportClick(object sender, EventArgs e)
    {
      this.openFileDialog.AddExtension = true;
      this.openFileDialog.DefaultExt = ".csv";
      this.openFileDialog.Multiselect = false;
      if (this.openFileDialog.ShowDialog() != DialogResult.OK)
        return;
      using (FileStream fileStream = new FileStream(this.openFileDialog.FileName, FileMode.Open, FileAccess.Read))
      {
        using (StreamReader streamReader = new StreamReader((Stream) fileStream))
        {
          string[] strArray1 = streamReader.ReadToEnd().Split(',');
          bool flag = true;
          foreach (string str in strArray1)
          {
            char[] chArray = new char[1]{ ':' };
            string[] strArray2 = str.Split(chArray);
            int index = int.Parse(strArray2[0]);
            DisplayFormat fmt;
            ushort result;
            if (strArray2[1].StartsWith("0x", StringComparison.CurrentCultureIgnoreCase))
            {
              fmt = DisplayFormat.Hex;
              ushort.TryParse(strArray2[1].Substring(2), NumberStyles.HexNumber, (IFormatProvider) CultureInfo.CurrentCulture, out result);
            }
            else if (strArray2[1].Length > 6)
            {
              fmt = DisplayFormat.Binary;
              result = Convert.ToUInt16(strArray2[1], 2);
            }
            else
            {
              fmt = DisplayFormat.Integer;
              result = Convert.ToUInt16(strArray2[1], 10);
            }
            if (index < this._registerData.Length)
              this._registerData[index] = result;
            if (flag)
            {
              this.SetFunction(fmt);
              flag = false;
              this.StartAddress = ushort.Parse(strArray2[0]);
            }
          }
          streamReader.Close();
          this.DataLength = Convert.ToUInt16(strArray1.Length);
        }
        fileStream.Close();
      }
      this.RefreshData();
    }

    protected void SetFunction(DisplayFormat fmt)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke((Delegate) new BaseForm.SetFunctionDelegate(this.SetFunction), (object) fmt);
      }
      else
      {
        this.DisplayFormat = fmt;
        switch (fmt)
        {
          case DisplayFormat.LED:
            this.radioButtonLED.Checked = true;
            break;
          case DisplayFormat.Binary:
            this.radioButtonBinary.Checked = true;
            break;
          case DisplayFormat.Hex:
            this.radioButtonHex.Checked = true;
            break;
          case DisplayFormat.Integer:
            this.radioButtonInteger.Checked = true;
            break;
        }
      }
    }

    private void ButtonExportClick(object sender, EventArgs e)
    {
      ushort startAddress = this.StartAddress;
      ushort dataLength = this.DataLength;
      string str1 = "-";
      switch (this.DisplayFormat)
      {
        case DisplayFormat.LED:
          str1 = "_LED_";
          break;
        case DisplayFormat.Binary:
          str1 = "_Binary_";
          break;
        case DisplayFormat.Hex:
          str1 = "_HEX_";
          break;
        case DisplayFormat.Integer:
          str1 = "_Decimal_";
          break;
      }
      string str2 = "ModbusExport_" + (object) startAddress + str1 + DateTime.Now.ToString("yyyyMMddHHmm") + ".csv";
      this.saveFileDialog.AddExtension = true;
      this.saveFileDialog.DefaultExt = ".csv";
      this.saveFileDialog.FileName = str2;
      this.saveFileDialog.OverwritePrompt = true;
      if (this.saveFileDialog.ShowDialog() != DialogResult.OK)
        return;
      using (Stream stream = this.saveFileDialog.OpenFile())
      {
        using (StreamWriter streamWriter = new StreamWriter(stream))
        {
          for (int index = 0; index < (int) dataLength; ++index)
          {
            streamWriter.Write((int) startAddress++);
            streamWriter.Write(':');
            ushort num = this._registerData[(int) this.StartAddress + index];
            switch (this.DisplayFormat)
            {
              case DisplayFormat.LED:
              case DisplayFormat.Binary:
                streamWriter.Write(Convert.ToString((int) num, 2).PadLeft(16, '0'));
                break;
              case DisplayFormat.Hex:
                streamWriter.Write(string.Format("0x{0:x4}", (object) num));
                break;
              case DisplayFormat.Integer:
                streamWriter.Write(string.Format("{0}", (object) num));
                break;
            }
            if (index < (int) dataLength - 1)
              streamWriter.Write(',');
          }
          streamWriter.Flush();
          streamWriter.Close();
        }
        stream.Close();
      }
    }

    private void RadioButtonModeChanged(object sender, EventArgs e) => this.SetMode();

    protected void SetMode()
    {
      if (this.radioButtonTCP.Checked)
      {
        this._communicationMode = CommunicationMode.TCP;
        this.groupBoxTCP.Enabled = true;
        this.groupBoxRTU.Enabled = false;
      }
      if (this.radioButtonRTU.Checked)
      {
        this._communicationMode = CommunicationMode.RTU;
        this.groupBoxTCP.Enabled = false;
        this.groupBoxRTU.Enabled = true;
      }
      if (!this.radioButtonUDP.Checked)
        return;
      this._communicationMode = CommunicationMode.UDP;
      this.groupBoxTCP.Enabled = true;
      this.groupBoxRTU.Enabled = false;
    }

    private void RadioButtonDisplayFormatCheckedChanged(object sender, EventArgs e)
    {
      if (!(sender is RadioButton))
        return;
      RadioButton radioButton = (RadioButton) sender;
      if (!radioButton.Checked)
        return;
      System.Enum.TryParse<DisplayFormat>(radioButton.Tag.ToString(), true, out this._displayFormat);
      this.CurrentTab.DisplayFormat = this.DisplayFormat;
      this.RefreshData();
    }

    protected ushort StartAddress
    {
      get => this._startAddress;
      set
      {
        this.CurrentTab.StartAddress = value;
        this.tabControl1.SelectedTab.Text = value.ToString();
        this._startAddress = value;
      }
    }

    protected ushort DataLength
    {
      get => this._dataLength;
      set
      {
        this._dataLength = value;
        this.CurrentTab.DataLength = value;
      }
    }

    public bool ShowDataLength { get; set; }

    protected IPAddress IPAddress
    {
      get => IPAddress.Parse(this.txtIP.Text);
      set => this.txtIP.Text = value.ToString();
    }

    protected int TCPPort
    {
      get => int.Parse(this.textBoxPort.Text);
      set => this.textBoxPort.Text = Convert.ToString(value);
    }

    protected byte SlaveId
    {
      get => byte.Parse(this.textBoxSlaveID.Text);
      set => this.textBoxSlaveID.Text = Convert.ToString(value);
    }

    protected int SlaveDelay
    {
      get => int.Parse(this.textBoxSlaveDelay.Text);
      set => this.textBoxSlaveDelay.Text = Convert.ToString(value);
    }

    protected string PortName
    {
      get => this.comboBoxSerialPorts.Text;
      set => this.comboBoxSerialPorts.Text = value;
    }

    protected int Baud
    {
      get => int.Parse(this.comboBoxBaudRate.Text);
      set => this.comboBoxBaudRate.SelectedItem = (object) Convert.ToString(value);
    }

    protected Parity Parity
    {
      get
      {
        Parity parity = Parity.None;
        if (this.comboBoxParity.SelectedItem.Equals((object) Parity.None.ToString()))
          parity = Parity.None;
        else if (this.comboBoxParity.SelectedItem.Equals((object) Parity.Odd.ToString()))
          parity = Parity.Odd;
        else if (this.comboBoxParity.SelectedItem.Equals((object) Parity.Even.ToString()))
          parity = Parity.Even;
        else if (this.comboBoxParity.SelectedItem.Equals((object) Parity.Mark.ToString()))
          parity = Parity.Mark;
        else if (this.comboBoxParity.SelectedItem.Equals((object) Parity.Space.ToString()))
          parity = Parity.Space;
        return parity;
      }
      set => this.comboBoxParity.SelectedItem = (object) Convert.ToString((object) value);
    }

    protected int DataBits
    {
      get
      {
        int num = 0;
        switch (this.comboBoxDataBits.SelectedIndex)
        {
          case 0:
            num = 7;
            break;
          case 1:
            num = 8;
            break;
        }
        return num;
      }
      set
      {
        switch (value)
        {
          case 7:
            this.comboBoxDataBits.SelectedIndex = 0;
            break;
          case 8:
            this.comboBoxDataBits.SelectedIndex = 1;
            break;
        }
      }
    }

    protected StopBits StopBits
    {
      get
      {
        StopBits stopBits = StopBits.None;
        switch (this.comboBoxStopBits.SelectedIndex)
        {
          case 0:
            stopBits = StopBits.None;
            break;
          case 1:
            stopBits = StopBits.One;
            break;
          case 2:
            stopBits = StopBits.OnePointFive;
            break;
          case 3:
            stopBits = StopBits.Two;
            break;
        }
        return stopBits;
      }
      set
      {
        switch (value)
        {
          case StopBits.None:
            this.comboBoxStopBits.SelectedIndex = 0;
            break;
          case StopBits.One:
            this.comboBoxStopBits.SelectedIndex = 1;
            break;
          case StopBits.Two:
            this.comboBoxStopBits.SelectedIndex = 3;
            break;
          case StopBits.OnePointFive:
            this.comboBoxStopBits.SelectedIndex = 2;
            break;
        }
      }
    }

    protected DisplayFormat DisplayFormat
    {
      get => this._displayFormat;
      set
      {
        switch (value)
        {
          case DisplayFormat.LED:
            this.radioButtonLED.Checked = true;
            break;
          case DisplayFormat.Binary:
            this.radioButtonBinary.Checked = true;
            break;
          case DisplayFormat.Hex:
            this.radioButtonHex.Checked = true;
            break;
          case DisplayFormat.Integer:
            this.radioButtonInteger.Checked = true;
            break;
        }
        this._displayFormat = value;
        this.CurrentTab.DisplayFormat = this.DisplayFormat;
        this.RefreshData();
      }
    }

    protected CommunicationMode CommunicationMode
    {
      get => this._communicationMode;
      set
      {
        switch (value)
        {
          case CommunicationMode.TCP:
            this.radioButtonTCP.Checked = true;
            break;
          case CommunicationMode.UDP:
            this.radioButtonUDP.Checked = true;
            break;
          case CommunicationMode.RTU:
            this.radioButtonRTU.Checked = true;
            break;
        }
        this._communicationMode = value;
      }
    }

    protected void ButtonClearLogClick(object sender, EventArgs e) => this.listBoxCommLog.Items.Clear();

    private void buttonPauseLog_Click(object sender, EventArgs e)
    {
      this._logPaused = !this._logPaused;
      this.buttonPauseLog.Text = this._logPaused ? "Resume" : "Pause";
    }

    public void DriverIncommingData(byte[] data)
    {
      if (this._logPaused)
        return;
      StringBuilder stringBuilder = new StringBuilder(data.Length * 2);
      foreach (byte num in data)
        stringBuilder.AppendFormat("{0:x2} ", (object) num);
      this.AppendLog(string.Format("RX: {0}", (object) stringBuilder));
    }

    public void DriverOutgoingData(byte[] data)
    {
      if (this._logPaused)
        return;
      StringBuilder stringBuilder = new StringBuilder(data.Length * 2);
      foreach (byte num in data)
        stringBuilder.AppendFormat("{0:x2} ", (object) num);
      this.AppendLog(string.Format("TX: {0}", (object) stringBuilder));
    }

    protected void AppendLog(string log)
    {
      if (this._logPaused)
        return;
      if (this.InvokeRequired)
      {
        this.BeginInvoke((Delegate) new BaseForm.AppendLogDelegate(this.AppendLog), (object) log);
      }
      else
      {
        this.listBoxCommLog.Items.Add((object) (">" + DateTime.Now.ToLongTimeString() + ": " + log));
        this.listBoxCommLog.SelectedIndex = this.listBoxCommLog.Items.Count - 1;
        this.listBoxCommLog.SelectedIndex = -1;
      }
    }

    protected void ButtonDataClearClick(object sender, EventArgs e) => this.ClearRegisterData();

    protected void ClearRegisterData()
    {
      for (int index = 0; index < this._registerData.Length; ++index)
        this._registerData[index] = (ushort) 0;
      this.RefreshData();
    }

    protected DataTab CurrentTab => (DataTab) this.tabControl1.SelectedTab.Controls[0];

    public void RefreshData()
    {
      if (this.InvokeRequired)
        this.BeginInvoke((Delegate) new Action(this.RefreshData));
      else
        this.CurrentTab.RefreshData();
    }

    public void UpdateDataTable()
    {
      if (this.InvokeRequired)
        this.BeginInvoke((Delegate) new Action(this.UpdateDataTable));
      else
        this.CurrentTab.UpdateDataTable();
    }

    private void tabControl1_Selected(object sender, TabControlEventArgs e)
    {
      this.CurrentTab.RegisterData = this._registerData;
      this.CurrentTab.DisplayFormat = this.DisplayFormat;
      TabPage selectedTab = this.tabControl1.SelectedTab;
      if (selectedTab.Text.Equals("...") && this.tabControl1.TabPages.Count < 20)
      {
        DataTab dataTab = new DataTab();
        dataTab.DataLength = (ushort) 256;
        dataTab.DisplayFormat = DisplayFormat.Integer;
        dataTab.Location = new Point(3, 3);
        dataTab.Name = "dataTab" + (object) (this.tabControl1.TabPages.Count + 1);
        dataTab.RegisterData = (ushort[]) null;
        dataTab.ShowDataLength = this.ShowDataLength;
        dataTab.Size = new Size(839, 406);
        dataTab.StartAddress = (ushort) 0;
        dataTab.TabIndex = 0;
        dataTab.OnApply += new EventHandler(this.dataTab_OnApply);
        TabPage tabPage = new TabPage();
        tabPage.Controls.Add((Control) dataTab);
        tabPage.Location = new Point(4, 22);
        tabPage.Name = "tabPage" + (object) (this.tabControl1.TabPages.Count + 1);
        tabPage.Padding = new Padding(3);
        tabPage.Size = new Size(851, 411);
        tabPage.TabIndex = this.tabControl1.TabPages.Count;
        tabPage.Text = "...";
        tabPage.UseVisualStyleBackColor = true;
        this.tabControl1.Controls.Add((Control) tabPage);
      }
      ushort startAddress = this.CurrentTab.StartAddress;
      selectedTab.Text = startAddress.ToString();
      this._startAddress = startAddress;
      this._dataLength = this.CurrentTab.DataLength;
    }

    private void dataTab_OnApply(object sender, EventArgs e)
    {
      TabPage selectedTab = this.tabControl1.SelectedTab;
      ushort startAddress = this.CurrentTab.StartAddress;
      selectedTab.Text = startAddress.ToString();
      this._startAddress = startAddress;
      this._dataLength = this.CurrentTab.DataLength;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    protected void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (BaseForm));
      this.groupBox4 = new GroupBox();
      this.buttonPauseLog = new Button();
      this.listBoxCommLog = new ListBox();
      this.buttonClear = new Button();
      this.label1 = new Label();
      this.textBoxSlaveDelay = new TextBox();
      this.label7 = new Label();
      this.textBoxSlaveID = new TextBox();
      this.groupBox3 = new GroupBox();
      this.radioButtonLED = new RadioButton();
      this.radioButtonInteger = new RadioButton();
      this.radioButtonHex = new RadioButton();
      this.radioButtonBinary = new RadioButton();
      this.buttonImport = new Button();
      this.buttonExport = new Button();
      this.grpStart = new GroupBox();
      this.groupBoxRTU = new GroupBox();
      this.comboBoxStopBits = new ComboBox();
      this.label10 = new Label();
      this.comboBoxDataBits = new ComboBox();
      this.label9 = new Label();
      this.comboBoxParity = new ComboBox();
      this.labelParity = new Label();
      this.comboBoxBaudRate = new ComboBox();
      this.comboBoxSerialPorts = new ComboBox();
      this.label4 = new Label();
      this.label5 = new Label();
      this.groupBoxMode = new GroupBox();
      this.radioButtonRTU = new RadioButton();
      this.radioButtonUDP = new RadioButton();
      this.radioButtonTCP = new RadioButton();
      this.groupBoxTCP = new GroupBox();
      this.label8 = new Label();
      this.txtIP = new TextBox();
      this.label6 = new Label();
      this.textBoxPort = new TextBox();
      this.openFileDialog = new OpenFileDialog();
      this.saveFileDialog = new SaveFileDialog();
      this.tabControl1 = new TabControl();
      this.tabPage1 = new TabPage();
      this.dataTab1 = new DataTab();
      this.tabPage2 = new TabPage();
      this.dataTab2 = new DataTab();
      this.grpExchange = new GroupBox();
      this.groupBox4.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.grpStart.SuspendLayout();
      this.groupBoxRTU.SuspendLayout();
      this.groupBoxMode.SuspendLayout();
      this.groupBoxTCP.SuspendLayout();
      this.tabControl1.SuspendLayout();
      this.tabPage1.SuspendLayout();
      this.tabPage2.SuspendLayout();
      this.grpExchange.SuspendLayout();
      this.SuspendLayout();
      this.groupBox4.Controls.Add((Control) this.buttonPauseLog);
      this.groupBox4.Controls.Add((Control) this.listBoxCommLog);
      this.groupBox4.Controls.Add((Control) this.buttonClear);
      this.groupBox4.Location = new Point(7, 699);
      this.groupBox4.Name = "groupBox4";
      this.groupBox4.Size = new Size(859, 194);
      this.groupBox4.TabIndex = 20;
      this.groupBox4.TabStop = false;
      this.groupBox4.Text = "Communication Log";
      this.buttonPauseLog.Location = new Point(660, 19);
      this.buttonPauseLog.Name = "buttonPauseLog";
      this.buttonPauseLog.Size = new Size(86, 28);
      this.buttonPauseLog.TabIndex = 25;
      this.buttonPauseLog.Text = "Pause";
      this.buttonPauseLog.Click += new EventHandler(this.buttonPauseLog_Click);
      this.listBoxCommLog.BackColor = Color.Black;
      this.listBoxCommLog.ForeColor = Color.LimeGreen;
      this.listBoxCommLog.FormattingEnabled = true;
      this.listBoxCommLog.HorizontalScrollbar = true;
      this.listBoxCommLog.Location = new Point(3, 54);
      this.listBoxCommLog.Name = "listBoxCommLog";
      this.listBoxCommLog.Size = new Size(847, 134);
      this.listBoxCommLog.TabIndex = 3;
      this.buttonClear.Location = new Point(752, 19);
      this.buttonClear.Name = "buttonClear";
      this.buttonClear.Size = new Size(86, 28);
      this.buttonClear.TabIndex = 24;
      this.buttonClear.Text = "Clear";
      this.buttonClear.Click += new EventHandler(this.ButtonClearLogClick);
      this.label1.Location = new Point(28, 43);
      this.label1.Name = "label1";
      this.label1.Size = new Size(86, 14);
      this.label1.TabIndex = 30;
      this.label1.Text = "Slave delay (ms)";
      this.textBoxSlaveDelay.Location = new Point(120, 40);
      this.textBoxSlaveDelay.Name = "textBoxSlaveDelay";
      this.textBoxSlaveDelay.Size = new Size(40, 20);
      this.textBoxSlaveDelay.TabIndex = 29;
      this.textBoxSlaveDelay.Text = "1";
      this.textBoxSlaveDelay.TextAlign = HorizontalAlignment.Right;
      this.label7.Location = new Point(28, 23);
      this.label7.Name = "label7";
      this.label7.Size = new Size(74, 14);
      this.label7.TabIndex = 28;
      this.label7.Text = "Slave ID";
      this.textBoxSlaveID.Location = new Point(120, 20);
      this.textBoxSlaveID.Name = "textBoxSlaveID";
      this.textBoxSlaveID.Size = new Size(40, 20);
      this.textBoxSlaveID.TabIndex = 27;
      this.textBoxSlaveID.Text = "1";
      this.textBoxSlaveID.TextAlign = HorizontalAlignment.Right;
      this.groupBox3.Controls.Add((Control) this.radioButtonLED);
      this.groupBox3.Controls.Add((Control) this.radioButtonInteger);
      this.groupBox3.Controls.Add((Control) this.radioButtonHex);
      this.groupBox3.Controls.Add((Control) this.radioButtonBinary);
      this.groupBox3.Location = new Point(7, 144);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new Size(166, 110);
      this.groupBox3.TabIndex = 21;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Display Format";
      this.radioButtonLED.Location = new Point(13, 19);
      this.radioButtonLED.Name = "radioButtonLED";
      this.radioButtonLED.Size = new Size(67, 21);
      this.radioButtonLED.TabIndex = 1;
      this.radioButtonLED.Tag = (object) "LED";
      this.radioButtonLED.Text = "LED";
      this.radioButtonLED.Click += new EventHandler(this.RadioButtonDisplayFormatCheckedChanged);
      this.radioButtonInteger.Checked = true;
      this.radioButtonInteger.Location = new Point(13, 81);
      this.radioButtonInteger.Name = "radioButtonInteger";
      this.radioButtonInteger.Size = new Size(67, 21);
      this.radioButtonInteger.TabIndex = 2;
      this.radioButtonInteger.TabStop = true;
      this.radioButtonInteger.Tag = (object) "Integer";
      this.radioButtonInteger.Text = "Integer";
      this.radioButtonInteger.Click += new EventHandler(this.RadioButtonDisplayFormatCheckedChanged);
      this.radioButtonHex.Location = new Point(13, 61);
      this.radioButtonHex.Name = "radioButtonHex";
      this.radioButtonHex.Size = new Size(67, 20);
      this.radioButtonHex.TabIndex = 1;
      this.radioButtonHex.Tag = (object) "Hex";
      this.radioButtonHex.Text = "Hex";
      this.radioButtonHex.Click += new EventHandler(this.RadioButtonDisplayFormatCheckedChanged);
      this.radioButtonBinary.Location = new Point(13, 40);
      this.radioButtonBinary.Name = "radioButtonBinary";
      this.radioButtonBinary.Size = new Size(67, 21);
      this.radioButtonBinary.TabIndex = 0;
      this.radioButtonBinary.Tag = (object) "Binary";
      this.radioButtonBinary.Text = "Binary";
      this.radioButtonBinary.Click += new EventHandler(this.RadioButtonDisplayFormatCheckedChanged);
      this.buttonImport.Location = new Point(188, 15);
      this.buttonImport.Name = "buttonImport";
      this.buttonImport.Size = new Size(86, 28);
      this.buttonImport.TabIndex = 26;
      this.buttonImport.Text = "Import";
      this.buttonImport.Click += new EventHandler(this.ButtonImportClick);
      this.buttonExport.Location = new Point(188, 50);
      this.buttonExport.Name = "buttonExport";
      this.buttonExport.Size = new Size(86, 28);
      this.buttonExport.TabIndex = 25;
      this.buttonExport.Text = "Export";
      this.buttonExport.Click += new EventHandler(this.ButtonExportClick);
      this.grpStart.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.grpStart.Controls.Add((Control) this.groupBoxRTU);
      this.grpStart.Controls.Add((Control) this.groupBoxMode);
      this.grpStart.Controls.Add((Control) this.groupBoxTCP);
      this.grpStart.Location = new Point(7, 12);
      this.grpStart.Name = "grpStart";
      this.grpStart.Size = new Size(665, 126);
      this.grpStart.TabIndex = 18;
      this.grpStart.TabStop = false;
      this.grpStart.Text = "Communication";
      this.groupBoxRTU.Controls.Add((Control) this.comboBoxStopBits);
      this.groupBoxRTU.Controls.Add((Control) this.label10);
      this.groupBoxRTU.Controls.Add((Control) this.comboBoxDataBits);
      this.groupBoxRTU.Controls.Add((Control) this.label9);
      this.groupBoxRTU.Controls.Add((Control) this.comboBoxParity);
      this.groupBoxRTU.Controls.Add((Control) this.labelParity);
      this.groupBoxRTU.Controls.Add((Control) this.comboBoxBaudRate);
      this.groupBoxRTU.Controls.Add((Control) this.comboBoxSerialPorts);
      this.groupBoxRTU.Controls.Add((Control) this.label4);
      this.groupBoxRTU.Controls.Add((Control) this.label5);
      this.groupBoxRTU.Enabled = false;
      this.groupBoxRTU.Location = new Point(291, 13);
      this.groupBoxRTU.Name = "groupBoxRTU";
      this.groupBoxRTU.Size = new Size(377, 106);
      this.groupBoxRTU.TabIndex = 25;
      this.groupBoxRTU.TabStop = false;
      this.groupBoxRTU.Text = "RTU";
      this.comboBoxStopBits.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxStopBits.FormattingEnabled = true;
      this.comboBoxStopBits.Items.AddRange(new object[4]
      {
        (object) "None",
        (object) "1 Bit",
        (object) "1.5 Bits",
        (object) "2 Bits"
      });
      this.comboBoxStopBits.Location = new Point(280, 48);
      this.comboBoxStopBits.Name = "comboBoxStopBits";
      this.comboBoxStopBits.Size = new Size(94, 21);
      this.comboBoxStopBits.TabIndex = 27;
      this.label10.AutoSize = true;
      this.label10.Location = new Point(215, 52);
      this.label10.Name = "label10";
      this.label10.Size = new Size(58, 13);
      this.label10.TabIndex = 26;
      this.label10.Text = "Stop Bits =";
      this.comboBoxDataBits.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxDataBits.FormattingEnabled = true;
      this.comboBoxDataBits.Items.AddRange(new object[2]
      {
        (object) "7 Bits",
        (object) "8 Bits"
      });
      this.comboBoxDataBits.Location = new Point(280, 20);
      this.comboBoxDataBits.Name = "comboBoxDataBits";
      this.comboBoxDataBits.Size = new Size(94, 21);
      this.comboBoxDataBits.TabIndex = 25;
      this.label9.AutoSize = true;
      this.label9.Location = new Point(215, 24);
      this.label9.Name = "label9";
      this.label9.Size = new Size(59, 13);
      this.label9.TabIndex = 24;
      this.label9.Text = "Data Bits =";
      this.comboBoxParity.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxParity.FormattingEnabled = true;
      this.comboBoxParity.Location = new Point(80, 74);
      this.comboBoxParity.Name = "comboBoxParity";
      this.comboBoxParity.Size = new Size(94, 21);
      this.comboBoxParity.TabIndex = 23;
      this.labelParity.AutoSize = true;
      this.labelParity.Location = new Point(36, 78);
      this.labelParity.Name = "labelParity";
      this.labelParity.Size = new Size(42, 13);
      this.labelParity.TabIndex = 22;
      this.labelParity.Text = "Parity =";
      this.comboBoxBaudRate.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxBaudRate.FormattingEnabled = true;
      this.comboBoxBaudRate.Items.AddRange(new object[15]
      {
        (object) "128000",
        (object) "155200",
        (object) "57600",
        (object) "38400",
        (object) "19200",
        (object) "14400",
        (object) "9600",
        (object) "7200",
        (object) "4800",
        (object) "2400",
        (object) "1800",
        (object) "1200",
        (object) "600",
        (object) "300",
        (object) "150"
      });
      this.comboBoxBaudRate.Location = new Point(80, 47);
      this.comboBoxBaudRate.Name = "comboBoxBaudRate";
      this.comboBoxBaudRate.Size = new Size(94, 21);
      this.comboBoxBaudRate.TabIndex = 21;
      this.comboBoxSerialPorts.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxSerialPorts.FormattingEnabled = true;
      this.comboBoxSerialPorts.Location = new Point(80, 19);
      this.comboBoxSerialPorts.Name = "comboBoxSerialPorts";
      this.comboBoxSerialPorts.Size = new Size(94, 21);
      this.comboBoxSerialPorts.TabIndex = 0;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(11, 23);
      this.label4.Name = "label4";
      this.label4.Size = new Size(66, 13);
      this.label4.TabIndex = 10;
      this.label4.Text = "Port Name =";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(36, 51);
      this.label5.Name = "label5";
      this.label5.Size = new Size(41, 13);
      this.label5.TabIndex = 12;
      this.label5.Text = "Baud =";
      this.groupBoxMode.Controls.Add((Control) this.radioButtonRTU);
      this.groupBoxMode.Controls.Add((Control) this.radioButtonUDP);
      this.groupBoxMode.Controls.Add((Control) this.radioButtonTCP);
      this.groupBoxMode.Location = new Point(6, 19);
      this.groupBoxMode.Name = "groupBoxMode";
      this.groupBoxMode.Size = new Size(81, 100);
      this.groupBoxMode.TabIndex = 0;
      this.groupBoxMode.TabStop = false;
      this.groupBoxMode.Text = "Mode";
      this.radioButtonRTU.AutoSize = true;
      this.radioButtonRTU.Location = new Point(6, 59);
      this.radioButtonRTU.Name = "radioButtonRTU";
      this.radioButtonRTU.Size = new Size(48, 17);
      this.radioButtonRTU.TabIndex = 3;
      this.radioButtonRTU.Text = "RTU";
      this.radioButtonRTU.UseVisualStyleBackColor = true;
      this.radioButtonRTU.CheckedChanged += new EventHandler(this.RadioButtonModeChanged);
      this.radioButtonUDP.AutoSize = true;
      this.radioButtonUDP.Location = new Point(6, 39);
      this.radioButtonUDP.Name = "radioButtonUDP";
      this.radioButtonUDP.Size = new Size(48, 17);
      this.radioButtonUDP.TabIndex = 2;
      this.radioButtonUDP.Text = "UDP";
      this.radioButtonUDP.UseVisualStyleBackColor = true;
      this.radioButtonUDP.CheckedChanged += new EventHandler(this.RadioButtonModeChanged);
      this.radioButtonTCP.AutoSize = true;
      this.radioButtonTCP.Checked = true;
      this.radioButtonTCP.Location = new Point(6, 19);
      this.radioButtonTCP.Name = "radioButtonTCP";
      this.radioButtonTCP.Size = new Size(46, 17);
      this.radioButtonTCP.TabIndex = 1;
      this.radioButtonTCP.TabStop = true;
      this.radioButtonTCP.Text = "TCP";
      this.radioButtonTCP.UseVisualStyleBackColor = true;
      this.radioButtonTCP.CheckedChanged += new EventHandler(this.RadioButtonModeChanged);
      this.groupBoxTCP.Controls.Add((Control) this.label8);
      this.groupBoxTCP.Controls.Add((Control) this.txtIP);
      this.groupBoxTCP.Controls.Add((Control) this.label6);
      this.groupBoxTCP.Controls.Add((Control) this.textBoxPort);
      this.groupBoxTCP.Location = new Point(93, 13);
      this.groupBoxTCP.Name = "groupBoxTCP";
      this.groupBoxTCP.Size = new Size(192, 106);
      this.groupBoxTCP.TabIndex = 0;
      this.groupBoxTCP.TabStop = false;
      this.groupBoxTCP.Text = "TCP";
      this.label8.Location = new Point(9, 50);
      this.label8.Name = "label8";
      this.label8.Size = new Size(64, 14);
      this.label8.TabIndex = 11;
      this.label8.Text = "IP Address";
      this.txtIP.Location = new Point(79, 47);
      this.txtIP.Name = "txtIP";
      this.txtIP.Size = new Size(97, 20);
      this.txtIP.TabIndex = 10;
      this.txtIP.Text = "127.0.0.1";
      this.txtIP.TextAlign = HorizontalAlignment.Right;
      this.label6.Location = new Point(9, 22);
      this.label6.Name = "label6";
      this.label6.Size = new Size(64, 14);
      this.label6.TabIndex = 9;
      this.label6.Text = "Port";
      this.textBoxPort.Location = new Point(79, 19);
      this.textBoxPort.Name = "textBoxPort";
      this.textBoxPort.Size = new Size(44, 20);
      this.textBoxPort.TabIndex = 8;
      this.textBoxPort.Text = "502";
      this.textBoxPort.TextAlign = HorizontalAlignment.Right;
      this.openFileDialog.FileName = "openFileDialog1";
      this.tabControl1.Controls.Add((Control) this.tabPage1);
      this.tabControl1.Controls.Add((Control) this.tabPage2);
      this.tabControl1.Location = new Point(7, 260);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new Size(859, 437);
      this.tabControl1.TabIndex = 35;
      this.tabControl1.Selected += new TabControlEventHandler(this.tabControl1_Selected);
      this.tabPage1.Controls.Add((Control) this.dataTab1);
      this.tabPage1.Location = new Point(4, 22);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new Padding(3);
      this.tabPage1.Size = new Size(851, 411);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Text = "Address1";
      this.tabPage1.UseVisualStyleBackColor = true;
      this.dataTab1.DataLength = (ushort) 256;
      this.dataTab1.DisplayFormat = DisplayFormat.Integer;
      this.dataTab1.Location = new Point(3, 3);
      this.dataTab1.Name = "dataTab1";
      this.dataTab1.RegisterData = (ushort[]) null;
      this.dataTab1.Size = new Size(839, 406);
      this.dataTab1.StartAddress = (ushort) 4100;
      this.dataTab1.TabIndex = 0;
      this.dataTab1.OnApply += new EventHandler(this.dataTab_OnApply);
      this.tabPage2.Controls.Add((Control) this.dataTab2);
      this.tabPage2.Location = new Point(4, 22);
      this.tabPage2.Name = "tabPage2";
      this.tabPage2.Padding = new Padding(3);
      this.tabPage2.Size = new Size(851, 411);
      this.tabPage2.TabIndex = 1;
      this.tabPage2.Text = "...";
      this.tabPage2.UseVisualStyleBackColor = true;
      this.dataTab2.DataLength = (ushort) 256;
      this.dataTab2.DisplayFormat = DisplayFormat.LED;
      this.dataTab2.Location = new Point(3, 3);
      this.dataTab2.Name = "dataTab2";
      this.dataTab2.RegisterData = (ushort[]) null;
      this.dataTab2.Size = new Size(839, 406);
      this.dataTab2.StartAddress = (ushort) 4100;
      this.dataTab2.TabIndex = 0;
      this.dataTab2.OnApply += new EventHandler(this.dataTab_OnApply);
      this.grpExchange.Controls.Add((Control) this.buttonImport);
      this.grpExchange.Controls.Add((Control) this.textBoxSlaveID);
      this.grpExchange.Controls.Add((Control) this.buttonExport);
      this.grpExchange.Controls.Add((Control) this.label1);
      this.grpExchange.Controls.Add((Control) this.label7);
      this.grpExchange.Controls.Add((Control) this.textBoxSlaveDelay);
      this.grpExchange.Location = new Point(571, 144);
      this.grpExchange.Name = "grpExchange";
      this.grpExchange.Size = new Size(289, 110);
      this.grpExchange.TabIndex = 36;
      this.grpExchange.TabStop = false;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(869, 901);
      this.Controls.Add((Control) this.grpExchange);
      this.Controls.Add((Control) this.tabControl1);
      this.Controls.Add((Control) this.groupBox4);
      this.Controls.Add((Control) this.grpStart);
      this.Controls.Add((Control) this.groupBox3);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (BaseForm);
      this.Text = "Modbus Slave";
      this.FormClosing += new FormClosingEventHandler(this.BaseFormClosing);
      this.Load += new EventHandler(this.BaseFormLoading);
      this.groupBox4.ResumeLayout(false);
      this.groupBox3.ResumeLayout(false);
      this.grpStart.ResumeLayout(false);
      this.groupBoxRTU.ResumeLayout(false);
      this.groupBoxRTU.PerformLayout();
      this.groupBoxMode.ResumeLayout(false);
      this.groupBoxMode.PerformLayout();
      this.groupBoxTCP.ResumeLayout(false);
      this.groupBoxTCP.PerformLayout();
      this.tabControl1.ResumeLayout(false);
      this.tabPage1.ResumeLayout(false);
      this.tabPage2.ResumeLayout(false);
      this.grpExchange.ResumeLayout(false);
      this.grpExchange.PerformLayout();
      this.ResumeLayout(false);
    }

    public delegate void SetFunctionDelegate(DisplayFormat log);

    public delegate void AppendLogDelegate(string log);
  }
}

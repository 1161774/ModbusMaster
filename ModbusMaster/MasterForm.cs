// Decompiled with JetBrains decompiler
// Type: ModbusMaster.MasterForm
// Assembly: ModbusMaster, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 37E26B38-B86A-41D8-BB81-49883CB2BCAB
// Assembly location: C:\Program Files (x86)\Farrellton Solar\Modbus Master\ModbusMaster.exe

using Modbus.Common;
using ModbusLib;
using ModbusLib.Protocols;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace ModbusMaster
{
  public class MasterForm : BaseForm
  {
    private new IContainer components;
    private GroupBox groupBoxFunctions;
    private Button btnReadCoils;
    private Button btnReadDisInp;
    private Button btnWriteMultipleReg;
    private Button btnReadHoldReg;
    private Button btnWriteMultipleCoils;
    private Button btnReadInpReg;
    private Button btnWriteSingleReg;
    private Button btnWriteSingleCoil;
    private Button buttonDisconnect;
    private Button btnConnect;
    private int _transactionId;
    private ModbusClient _driver;
    private ICommClient _portClient;
    private SerialPort _uart;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private new void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MasterForm));
      this.groupBoxFunctions = new GroupBox();
      this.btnReadCoils = new Button();
      this.btnReadDisInp = new Button();
      this.btnWriteMultipleReg = new Button();
      this.btnReadHoldReg = new Button();
      this.btnWriteMultipleCoils = new Button();
      this.btnReadInpReg = new Button();
      this.btnWriteSingleReg = new Button();
      this.btnWriteSingleCoil = new Button();
      this.buttonDisconnect = new Button();
      this.btnConnect = new Button();
      this.groupBox4.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.grpStart.SuspendLayout();
      this.groupBoxRTU.SuspendLayout();
      this.groupBoxMode.SuspendLayout();
      this.groupBoxTCP.SuspendLayout();
      this.grpExchange.SuspendLayout();
      this.groupBoxFunctions.SuspendLayout();
      this.SuspendLayout();
      this.comboBoxSerialPorts.Items.AddRange(new object[14]
      {
        (object) "COM3",
        (object) "COM4",
        (object) "COM3",
        (object) "COM4",
        (object) "COM3",
        (object) "COM4",
        (object) "COM3",
        (object) "COM4",
        (object) "COM3",
        (object) "COM4",
        (object) "COM5",
        (object) "COM3",
        (object) "COM4",
        (object) "COM5"
      });
      this.label1.Visible = false;
      this.textBoxSlaveDelay.Visible = false;
      this.comboBoxParity.Items.AddRange(new object[30]
      {
        (object) "None",
        (object) "Odd",
        (object) "Even",
        (object) "Mark",
        (object) "Space",
        (object) "None",
        (object) "Odd",
        (object) "Even",
        (object) "Mark",
        (object) "Space",
        (object) "None",
        (object) "Odd",
        (object) "Even",
        (object) "Mark",
        (object) "Space",
        (object) "None",
        (object) "Odd",
        (object) "Even",
        (object) "Mark",
        (object) "Space",
        (object) "None",
        (object) "Odd",
        (object) "Even",
        (object) "Mark",
        (object) "Space",
        (object) "None",
        (object) "Odd",
        (object) "Even",
        (object) "Mark",
        (object) "Space"
      });
      this.groupBoxFunctions.Controls.Add((Control) this.btnReadCoils);
      this.groupBoxFunctions.Controls.Add((Control) this.btnReadDisInp);
      this.groupBoxFunctions.Controls.Add((Control) this.btnWriteMultipleReg);
      this.groupBoxFunctions.Controls.Add((Control) this.btnReadHoldReg);
      this.groupBoxFunctions.Controls.Add((Control) this.btnWriteMultipleCoils);
      this.groupBoxFunctions.Controls.Add((Control) this.btnReadInpReg);
      this.groupBoxFunctions.Controls.Add((Control) this.btnWriteSingleReg);
      this.groupBoxFunctions.Controls.Add((Control) this.btnWriteSingleCoil);
      this.groupBoxFunctions.Enabled = false;
      this.groupBoxFunctions.Location = new Point(179, 144);
      this.groupBoxFunctions.Name = "groupBoxFunctions";
      this.groupBoxFunctions.Size = new Size(339, 110);
      this.groupBoxFunctions.TabIndex = 35;
      this.groupBoxFunctions.TabStop = false;
      this.groupBoxFunctions.Text = "Functions";
      this.btnReadCoils.Location = new Point(6, 19);
      this.btnReadCoils.Name = "btnReadCoils";
      this.btnReadCoils.Size = new Size(78, 35);
      this.btnReadCoils.TabIndex = 11;
      this.btnReadCoils.Text = "Read coils";
      this.btnReadCoils.Click += new EventHandler(this.BtnReadCoilsClick);
      this.btnReadDisInp.Location = new Point(6, 60);
      this.btnReadDisInp.Name = "btnReadDisInp";
      this.btnReadDisInp.Size = new Size(78, 35);
      this.btnReadDisInp.TabIndex = 16;
      this.btnReadDisInp.Text = "Read discrete inputs";
      this.btnReadDisInp.Click += new EventHandler(this.BtnReadDisInpClick);
      this.btnWriteMultipleReg.Location = new Point(249, 60);
      this.btnWriteMultipleReg.Name = "btnWriteMultipleReg";
      this.btnWriteMultipleReg.Size = new Size(78, 35);
      this.btnWriteMultipleReg.TabIndex = 23;
      this.btnWriteMultipleReg.Text = "Write multiple register";
      this.btnWriteMultipleReg.Click += new EventHandler(this.BtnWriteMultipleRegClick);
      this.btnReadHoldReg.Location = new Point(87, 19);
      this.btnReadHoldReg.Name = "btnReadHoldReg";
      this.btnReadHoldReg.Size = new Size(78, 35);
      this.btnReadHoldReg.TabIndex = 17;
      this.btnReadHoldReg.Text = "Read holding register";
      this.btnReadHoldReg.Click += new EventHandler(this.BtnReadHoldRegClick);
      this.btnWriteMultipleCoils.Location = new Point(249, 19);
      this.btnWriteMultipleCoils.Name = "btnWriteMultipleCoils";
      this.btnWriteMultipleCoils.Size = new Size(78, 35);
      this.btnWriteMultipleCoils.TabIndex = 22;
      this.btnWriteMultipleCoils.Text = "Write multiple coils";
      this.btnWriteMultipleCoils.Click += new EventHandler(this.BtnWriteMultipleCoilsClick);
      this.btnReadInpReg.Location = new Point(87, 60);
      this.btnReadInpReg.Name = "btnReadInpReg";
      this.btnReadInpReg.Size = new Size(78, 35);
      this.btnReadInpReg.TabIndex = 18;
      this.btnReadInpReg.Text = "Read input register";
      this.btnReadInpReg.Click += new EventHandler(this.BtnReadInpRegClick);
      this.btnWriteSingleReg.Location = new Point(168, 60);
      this.btnWriteSingleReg.Name = "btnWriteSingleReg";
      this.btnWriteSingleReg.Size = new Size(78, 35);
      this.btnWriteSingleReg.TabIndex = 21;
      this.btnWriteSingleReg.Text = "Write single register";
      this.btnWriteSingleReg.Click += new EventHandler(this.BtnWriteSingleRegClick);
      this.btnWriteSingleCoil.Location = new Point(168, 19);
      this.btnWriteSingleCoil.Name = "btnWriteSingleCoil";
      this.btnWriteSingleCoil.Size = new Size(78, 35);
      this.btnWriteSingleCoil.TabIndex = 19;
      this.btnWriteSingleCoil.Text = "Write single coil";
      this.btnWriteSingleCoil.Click += new EventHandler(this.BtnWriteSingleCoilClick);
      this.buttonDisconnect.Enabled = false;
      this.buttonDisconnect.Location = new Point(759, 47);
      this.buttonDisconnect.Name = "buttonDisconnect";
      this.buttonDisconnect.Size = new Size(86, 28);
      this.buttonDisconnect.TabIndex = 37;
      this.buttonDisconnect.Text = "Disconnect";
      this.buttonDisconnect.Click += new EventHandler(this.ButtonDisconnectClick);
      this.btnConnect.Location = new Point(759, 12);
      this.btnConnect.Name = "btnConnect";
      this.btnConnect.Size = new Size(86, 28);
      this.btnConnect.TabIndex = 36;
      this.btnConnect.Text = "Connect";
      this.btnConnect.Click += new EventHandler(this.BtnConnectClick);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.ClientSize = new Size(869, 887);
      this.Controls.Add((Control) this.buttonDisconnect);
      this.Controls.Add((Control) this.btnConnect);
      this.Controls.Add((Control) this.groupBoxFunctions);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (MasterForm);
      this.ShowDataLength = true;
      this.Text = "Modbus Master";
      this.FormClosing += new FormClosingEventHandler(this.MasterFormClosing);
      this.Controls.SetChildIndex((Control) this.grpExchange, 0);
      this.Controls.SetChildIndex((Control) this.groupBox3, 0);
      this.Controls.SetChildIndex((Control) this.grpStart, 0);
      this.Controls.SetChildIndex((Control) this.groupBox4, 0);
      this.Controls.SetChildIndex((Control) this.groupBoxFunctions, 0);
      this.Controls.SetChildIndex((Control) this.btnConnect, 0);
      this.Controls.SetChildIndex((Control) this.buttonDisconnect, 0);
      this.groupBox4.ResumeLayout(false);
      this.groupBox3.ResumeLayout(false);
      this.grpStart.ResumeLayout(false);
      this.groupBoxRTU.ResumeLayout(false);
      this.groupBoxRTU.PerformLayout();
      this.groupBoxMode.ResumeLayout(false);
      this.groupBoxMode.PerformLayout();
      this.groupBoxTCP.ResumeLayout(false);
      this.groupBoxTCP.PerformLayout();
      this.grpExchange.ResumeLayout(false);
      this.grpExchange.PerformLayout();
      this.groupBoxFunctions.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public MasterForm() => this.InitializeComponent();

    private void MasterFormClosing(object sender, FormClosingEventArgs e) => this.DoDisconnect();

    private void DoDisconnect()
    {
      if (this._socket != null)
      {
        this._socket.Close();
        this._socket.Dispose();
        this._socket = (Socket) null;
      }
      if (this._uart != null)
      {
        this._uart.Close();
        this._uart.Dispose();
        this._uart = (SerialPort) null;
      }
      this._portClient = (ICommClient) null;
      this._driver = (ModbusClient) null;
    }

    private void BtnConnectClick(object sender, EventArgs e)
    {
      try
      {
        switch (this.CommunicationMode)
        {
          case CommunicationMode.TCP:
            this._socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this._socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.Debug, true);
            this._socket.SendTimeout = 2000;
            this._socket.ReceiveTimeout = 2000;
            this._socket.Connect((EndPoint) new IPEndPoint(this.IPAddress, this.TCPPort));
            this._portClient = this._socket.GetClient();
            this._driver = new ModbusClient((IProtocolCodec) new ModbusTcpCodec())
            {
              Address = this.SlaveId
            };
            this._driver.OutgoingData += new ModbusCommand.OutgoingData(((BaseForm) this).DriverOutgoingData);
            this._driver.IncommingData += new ModbusCommand.IncommingData(((BaseForm) this).DriverIncommingData);
            this.AppendLog(string.Format("Connected using TCP to {0}", (object) this._socket.RemoteEndPoint));
            break;
          case CommunicationMode.UDP:
            this._socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            this._socket.Connect((EndPoint) new IPEndPoint(this.IPAddress, this.TCPPort));
            this._portClient = this._socket.GetClient();
            this._driver = new ModbusClient((IProtocolCodec) new ModbusTcpCodec())
            {
              Address = this.SlaveId
            };
            this._driver.OutgoingData += new ModbusCommand.OutgoingData(((BaseForm) this).DriverOutgoingData);
            this._driver.IncommingData += new ModbusCommand.IncommingData(((BaseForm) this).DriverIncommingData);
            this.AppendLog(string.Format("Connected using UDP to {0}", (object) this._socket.RemoteEndPoint));
            break;
          case CommunicationMode.RTU:
            this._uart = new SerialPort(this.PortName, this.Baud, this.Parity, this.DataBits, this.StopBits);
            this._uart.Open();
            this._portClient = this._uart.GetClient();
            this._driver = new ModbusClient((IProtocolCodec) new ModbusRtuCodec())
            {
              Address = this.SlaveId
            };
            this._driver.OutgoingData += new ModbusCommand.OutgoingData(((BaseForm) this).DriverOutgoingData);
            this._driver.IncommingData += new ModbusCommand.IncommingData(((BaseForm) this).DriverIncommingData);
            this.AppendLog(string.Format("Connected using RTU to {0}", (object) this.PortName));
            break;
        }
      }
      catch (Exception ex)
      {
        this.AppendLog(ex.Message);
        return;
      }
      this.btnConnect.Enabled = false;
      this.buttonDisconnect.Enabled = true;
      this.groupBoxFunctions.Enabled = true;
      this.groupBoxTCP.Enabled = false;
      this.groupBoxRTU.Enabled = false;
      this.groupBoxMode.Enabled = false;
      this.grpExchange.Enabled = false;
    }

    private void ButtonDisconnectClick(object sender, EventArgs e)
    {
      this.DoDisconnect();
      this.btnConnect.Enabled = true;
      this.buttonDisconnect.Enabled = false;
      this.groupBoxFunctions.Enabled = false;
      this.groupBoxMode.Enabled = true;
      this.grpExchange.Enabled = true;
      this.SetMode();
      this.AppendLog("Disconnected");
    }

    private void BtnReadCoilsClick(object sender, EventArgs e) => this.ExecuteReadCommand((byte) 1);

    private void BtnReadDisInpClick(object sender, EventArgs e) => this.ExecuteReadCommand((byte) 2);

    private void BtnReadHoldRegClick(object sender, EventArgs e) => this.ExecuteReadCommand((byte) 3);

    private void BtnReadInpRegClick(object sender, EventArgs e) => this.ExecuteReadCommand((byte) 4);

    private void ExecuteReadCommand(byte function)
    {
      try
      {
        ModbusCommand command = new ModbusCommand(function)
        {
          Offset = (int) this.StartAddress,
          Count = (int) this.DataLength,
          TransId = this._transactionId++
        };
        CommResponse commResponse = this._driver.ExecuteGeneric(this._portClient, command);
        if (commResponse.Status == 3)
        {
          command.Data.CopyTo((Array) this._registerData, (int) this.StartAddress);
          this.UpdateDataTable();
          this.AppendLog(string.Format("Read succeeded: Function code:{0}.", (object) function));
        }
        else
          this.AppendLog(string.Format("Failed to execute Read: Error code:{0}", (object) commResponse.Status));
      }
      catch (Exception ex)
      {
        this.AppendLog(ex.Message);
      }
    }

    private void ExecuteWriteCommand(byte function)
    {
      try
      {
        ModbusCommand command = new ModbusCommand(function)
        {
          Offset = (int) this.StartAddress,
          Count = (int) this.DataLength,
          TransId = this._transactionId++,
          Data = new ushort[(int) this.DataLength]
        };
        for (int index1 = 0; index1 < (int) this.DataLength; ++index1)
        {
          int index2 = (int) this.StartAddress + index1;
          if (index2 <= this._registerData.Length)
            command.Data[index1] = this._registerData[index2];
          else
            break;
        }
        CommResponse commResponse = this._driver.ExecuteGeneric(this._portClient, command);
        this.AppendLog(commResponse.Status == 3 ? string.Format("Write succeeded: Function code:{0}", (object) function) : string.Format("Failed to execute Write: Error code:{0}", (object) commResponse.Status));
      }
      catch (Exception ex)
      {
        this.AppendLog(ex.Message);
      }
    }

    private void BtnWriteSingleCoilClick(object sender, EventArgs e)
    {
      try
      {
        ModbusCommand command = new ModbusCommand((byte) 5)
        {
          Offset = (int) this.StartAddress,
          Count = 1,
          TransId = this._transactionId++,
          Data = new ushort[1]
        };
        command.Data[0] = (ushort) ((uint) this._registerData[(int) this.StartAddress] & 256U);
        CommResponse commResponse = this._driver.ExecuteGeneric(this._portClient, command);
        this.AppendLog(commResponse.Status == 3 ? string.Format("Write succeeded: Function code:{0}", (object) (byte) 5) : string.Format("Failed to execute Write: Error code:{0}", (object) commResponse.Status));
      }
      catch (Exception ex)
      {
        this.AppendLog(ex.Message);
      }
    }

    private void BtnWriteSingleRegClick(object sender, EventArgs e) => this.ExecuteWriteCommand((byte) 6);

    private void BtnWriteMultipleCoilsClick(object sender, EventArgs e) => this.ExecuteWriteCommand((byte) 15);

    private void BtnWriteMultipleRegClick(object sender, EventArgs e) => this.ExecuteWriteCommand((byte) 16);

    private void ButtonReadExceptionStatusClick(object sender, EventArgs e)
    {
    }
  }
}

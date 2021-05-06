// Decompiled with JetBrains decompiler
// Type: Modbus.Common.DataTab
// Assembly: Modbus.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4D2ECFBB-7EFF-4853-BA31-64D66B5217FA
// Assembly location: C:\Program Files (x86)\Farrellton Solar\Modbus Master\Modbus.Common.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace Modbus.Common
{
  public class DataTab : UserControl
  {
    protected int _displayCtrlCount;
    private IContainer components;
    protected GroupBox groupBox1;
    protected Button buttonApply;
    protected Label labelTxtSize;
    protected TextBox txtSize;
    protected Label label11;
    protected TextBox txtStartAdress;
    protected Button buttonClear;
    protected GroupBox groupBoxData;

    public DataTab() => this.InitializeComponent();

    public ushort StartAddress
    {
      get => this.txtStartAdress.Text.IndexOf("0x", 0, this.txtStartAdress.Text.Length) == 0 ? Convert.ToUInt16(this.txtStartAdress.Text.Replace("0x", ""), 16) : Convert.ToUInt16(this.txtStartAdress.Text);
      set => this.txtStartAdress.Text = Convert.ToString(value);
    }

    public ushort DataLength
    {
      get => Convert.ToUInt16(this.txtSize.Text);
      set => this.txtSize.Text = Convert.ToString(value);
    }

    private void txtSize_TextChanged(object sender, EventArgs e)
    {
      if (this.DataLength <= (ushort) sbyte.MaxValue)
        return;
      this.DataLength = (ushort) sbyte.MaxValue;
    }

    public bool ShowDataLength
    {
      get => this.txtSize.Visible;
      set
      {
        this.txtSize.Visible = value;
        this.labelTxtSize.Visible = value;
      }
    }

    public event EventHandler OnApply;

    public ushort[] RegisterData { get; set; }

    public DisplayFormat DisplayFormat { get; set; }

    public void RefreshData()
    {
      this.groupBoxData.Controls.Clear();
      int num1 = 0;
      int x = 10;
      int y = 20;
      while (x < this.groupBoxData.Size.Width - 100)
      {
        Label label = new Label();
        this.groupBoxData.Controls.Add((Control) label);
        label.Size = new Size(40, 20);
        label.Location = new Point(x, y);
        label.Font = new Font("Calibri", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
        switch (this.DisplayFormat)
        {
          case DisplayFormat.LED:
            LedBulb ledBulb = new LedBulb();
            this.groupBoxData.Controls.Add((Control) ledBulb);
            ledBulb.Size = new Size(25, 25);
            ledBulb.Location = new Point(x + 40, y - 5);
            ledBulb.Padding = new Padding(3);
            ledBulb.Color = Color.Red;
            ledBulb.On = false;
            ledBulb.Tag = (object) num1;
            ledBulb.Click += new EventHandler(this.BulbClick);
            y = y + ledBulb.Size.Height + 10;
            label.Text = Convert.ToString(num1);
            break;
          case DisplayFormat.Binary:
            TextBox textBox1 = new TextBox();
            this.groupBoxData.Controls.Add((Control) textBox1);
            textBox1.Size = new Size(110, 20);
            textBox1.Location = new Point(x + 40, y - 2);
            textBox1.TextAlign = HorizontalAlignment.Right;
            textBox1.Tag = (object) num1;
            textBox1.Leave += new EventHandler(this.TxtDataBinaryLeave);
            textBox1.Enter += new EventHandler(this.txtData_Enter);
            textBox1.KeyPress += new KeyPressEventHandler(this.txtDataBinaryKeyPress);
            textBox1.MaxLength = 16;
            y = y + textBox1.Size.Height + 5;
            label.Text = Convert.ToString((int) this.StartAddress + num1);
            break;
          case DisplayFormat.Hex:
            TextBox textBox2 = new TextBox();
            this.groupBoxData.Controls.Add((Control) textBox2);
            textBox2.Size = new Size(55, 20);
            textBox2.Location = new Point(x + 40, y - 2);
            textBox2.TextAlign = HorizontalAlignment.Right;
            textBox2.Tag = (object) num1;
            textBox2.MaxLength = 5;
            textBox2.Leave += new EventHandler(this.TxtDataHexLeave);
            textBox2.Enter += new EventHandler(this.txtData_Enter);
            textBox2.KeyPress += new KeyPressEventHandler(this.txtDataHexKeyPress);
            y = y + textBox2.Size.Height + 5;
            label.Text = Convert.ToString((int) this.StartAddress + num1);
            break;
          case DisplayFormat.Integer:
            TextBox textBox3 = new TextBox();
            this.groupBoxData.Controls.Add((Control) textBox3);
            textBox3.Size = new Size(55, 20);
            textBox3.Location = new Point(x + 40, y - 2);
            textBox3.TextAlign = HorizontalAlignment.Right;
            textBox3.Tag = (object) num1;
            textBox3.MaxLength = 5;
            textBox3.Leave += new EventHandler(this.TxtDataLeave);
            textBox3.Enter += new EventHandler(this.txtData_Enter);
            textBox3.KeyPress += new KeyPressEventHandler(this.txtDataIntegerKeyPress);
            y = y + textBox3.Size.Height + 5;
            label.Text = Convert.ToString((int) this.StartAddress + num1);
            break;
        }
        ++num1;
        if (y > this.groupBoxData.Size.Height - 30)
        {
          int num2 = this.DisplayFormat == DisplayFormat.Binary ? 200 : 100;
          x += num2;
          y = 20;
        }
      }
      this._displayCtrlCount = num1;
      this.UpdateDataTable();
    }

    private void txtDataBinaryKeyPress(object sender, KeyPressEventArgs e)
    {
      char keyChar = e.KeyChar;
      if (keyChar == '\b' || keyChar == '0' || (keyChar == '1' || keyChar == '\b'))
        return;
      e.Handled = true;
    }

    private void txtDataHexKeyPress(object sender, KeyPressEventArgs e)
    {
      char keyChar = e.KeyChar;
      if (keyChar == '\b' || keyChar <= 'f' && keyChar >= '=' || (keyChar <= 'F' && keyChar >= 'A' || keyChar >= '0' && keyChar <= '9') || keyChar == '\b')
        return;
      e.Handled = true;
    }

    private void txtDataIntegerKeyPress(object sender, KeyPressEventArgs e) => e.Handled = !char.IsDigit(e.KeyChar) && e.KeyChar != '\b';

    private void txtData_Enter(object sender, EventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      if (string.IsNullOrEmpty(textBox.Text))
        return;
      textBox.Clear();
    }

    private void TxtDataLeave(object sender, EventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      int num = int.Parse(textBox.Tag.ToString());
      ushort result;
      if (ushort.TryParse(textBox.Text, out result))
        this.RegisterData[(int) this.StartAddress + num] = result;
      else
        textBox.Text = "0";
    }

    private void TxtDataHexLeave(object sender, EventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      int num = int.Parse(textBox.Tag.ToString());
      ushort result;
      if (ushort.TryParse(textBox.Text, NumberStyles.HexNumber, (IFormatProvider) CultureInfo.CurrentCulture, out result))
        this.RegisterData[(int) this.StartAddress + num] = result;
      else
        textBox.Text = "0x0000";
      textBox.Text = string.Format("0x{0}", (object) textBox.Text.ToLower().PadLeft(4, '0'));
    }

    private void TxtDataBinaryLeave(object sender, EventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      int num = int.Parse(textBox.Tag.ToString());
      try
      {
        this.RegisterData[(int) this.StartAddress + num] = Convert.ToUInt16(textBox.Text, 2);
        textBox.Text = textBox.Text.PadLeft(16, '0');
      }
      catch (Exception ex)
      {
        textBox.Text = "0000000000000000";
      }
    }

    private void BulbClick(object sender, EventArgs e)
    {
      LedBulb ledBulb = (LedBulb) sender;
      ledBulb.On = !ledBulb.On;
      int num1 = int.Parse(ledBulb.Tag.ToString());
      int num2 = num1 / 16;
      ushort uint16 = Convert.ToUInt16(((num1 & 8) != 0 ? 1 : 256) << (num1 & 7));
      if (ledBulb.On)
      {
        this.RegisterData[(int) this.StartAddress + num2] |= uint16;
      }
      else
      {
        ushort num3 = (ushort)~uint16;
        this.RegisterData[(int) this.StartAddress + num2] &= num3;
      }
    }

    public void UpdateDataTable()
    {
      ushort[] numArray = new ushort[this._displayCtrlCount];
      for (int index1 = 0; index1 < this._displayCtrlCount; ++index1)
      {
        int index2 = (int) this.StartAddress + index1;
        if (index2 < this.RegisterData.Length)
          numArray[index1] = this.RegisterData[index2];
        else
          break;
      }
      foreach (Control control in (ArrangedElementCollection) this.groupBoxData.Controls)
      {
        if (control is TextBox)
        {
          int int16 = (int) Convert.ToInt16(control.Tag);
          if (int16 <= numArray.GetUpperBound(0))
          {
            switch (this.DisplayFormat)
            {
              case DisplayFormat.Binary:
                control.Text = Convert.ToString((int) numArray[int16], 2).PadLeft(16, '0');
                break;
              case DisplayFormat.Hex:
                control.Text = string.Format("0x{0:x4}", (object) numArray[int16]);
                break;
              case DisplayFormat.Integer:
                control.Text = numArray[int16].ToString((IFormatProvider) CultureInfo.InvariantCulture);
                break;
            }
            control.Visible = true;
          }
          else
            control.Text = "";
        }
        else if (control is LedBulb)
        {
          LedBulb ledBulb = (LedBulb) control;
          short int16 = Convert.ToInt16(control.Tag);
          int index = (int) int16 / 16;
          bool flag = ((int) int16 & 8) != 0;
          int num = (int) int16 & 7;
          ushort uint16 = Convert.ToUInt16((flag ? 1 : 256) << num);
          ledBulb.On = ((int) uint16 & (int) numArray[index]) != 0;
        }
      }
    }

    private void buttonApply_Click(object sender, EventArgs e)
    {
      if (!(this.txtStartAdress.Text != ""))
        return;
      try
      {
        int startAddress = (int) this.StartAddress;
        if (this.OnApply != null)
          this.OnApply((object) this, new EventArgs());
        this.RefreshData();
      }
      catch (Exception ex)
      {
        this.txtStartAdress.Text = "";
      }
    }

    private void buttonClear_Click(object sender, EventArgs e)
    {
      for (int index = 0; index < this.RegisterData.Length; ++index)
        this.RegisterData[index] = (ushort) 0;
      this.RefreshData();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.groupBox1 = new GroupBox();
      this.buttonApply = new Button();
      this.labelTxtSize = new Label();
      this.txtSize = new TextBox();
      this.label11 = new Label();
      this.txtStartAdress = new TextBox();
      this.buttonClear = new Button();
      this.groupBoxData = new GroupBox();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      this.groupBox1.Controls.Add((Control) this.buttonApply);
      this.groupBox1.Controls.Add((Control) this.labelTxtSize);
      this.groupBox1.Controls.Add((Control) this.txtSize);
      this.groupBox1.Controls.Add((Control) this.label11);
      this.groupBox1.Controls.Add((Control) this.txtStartAdress);
      this.groupBox1.Controls.Add((Control) this.buttonClear);
      this.groupBox1.Controls.Add((Control) this.groupBoxData);
      this.groupBox1.Location = new Point(3, 3);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(833, 396);
      this.groupBox1.TabIndex = 33;
      this.groupBox1.TabStop = false;
      this.buttonApply.Location = new Point(642, 11);
      this.buttonApply.Name = "buttonApply";
      this.buttonApply.Size = new Size(86, 28);
      this.buttonApply.TabIndex = 36;
      this.buttonApply.Text = "Apply";
      this.buttonApply.Click += new EventHandler(this.buttonApply_Click);
      this.labelTxtSize.Location = new Point(163, 19);
      this.labelTxtSize.Name = "labelTxtSize";
      this.labelTxtSize.Size = new Size(42, 14);
      this.labelTxtSize.TabIndex = 35;
      this.labelTxtSize.Text = "Size";
      this.txtSize.Location = new Point(211, 16);
      this.txtSize.MaxLength = 3;
      this.txtSize.Name = "txtSize";
      this.txtSize.Size = new Size(40, 20);
      this.txtSize.TabIndex = 34;
      this.txtSize.Text = "64";
      this.txtSize.TextAlign = HorizontalAlignment.Right;
      this.txtSize.TextChanged += new EventHandler(this.txtSize_TextChanged);
      this.label11.Location = new Point(6, 19);
      this.label11.Name = "label11";
      this.label11.Size = new Size(74, 14);
      this.label11.TabIndex = 27;
      this.label11.Text = "Start Address";
      this.txtStartAdress.Location = new Point(86, 16);
      this.txtStartAdress.Name = "txtStartAdress";
      this.txtStartAdress.Size = new Size(54, 20);
      this.txtStartAdress.TabIndex = 26;
      this.txtStartAdress.Text = "4100";
      this.txtStartAdress.TextAlign = HorizontalAlignment.Right;
      this.buttonClear.Location = new Point(740, 11);
      this.buttonClear.Name = "buttonClear";
      this.buttonClear.Size = new Size(86, 28);
      this.buttonClear.TabIndex = 25;
      this.buttonClear.Text = "Clear";
      this.buttonClear.Click += new EventHandler(this.buttonClear_Click);
      this.groupBoxData.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupBoxData.Location = new Point(0, 44);
      this.groupBoxData.Name = "groupBoxData";
      this.groupBoxData.Size = new Size(833, 344);
      this.groupBoxData.TabIndex = 17;
      this.groupBoxData.TabStop = false;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupBox1);
      this.Name = nameof (DataTab);
      this.Size = new Size(829, 406);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}

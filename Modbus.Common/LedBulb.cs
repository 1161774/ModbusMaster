// Decompiled with JetBrains decompiler
// Type: Modbus.Common.LedBulb
// Assembly: Modbus.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4D2ECFBB-7EFF-4853-BA31-64D66B5217FA
// Assembly location: C:\Program Files (x86)\Farrellton Solar\Modbus Master\Modbus.Common.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Modbus.Common
{
  public class LedBulb : Control
  {
    private Color _color;
    private bool _on = true;

    [DefaultValue(typeof (Color), "153, 255, 54")]
    public Color Color
    {
      get => this._color;
      set
      {
        this._color = value;
        this.DarkColor = ControlPaint.Dark(this._color);
        this.DarkDarkColor = ControlPaint.DarkDark(this._color);
        this.Invalidate();
      }
    }

    public Color DarkColor { get; protected set; }

    public Color DarkDarkColor { get; protected set; }

    public bool On
    {
      get => this._on;
      set
      {
        this._on = value;
        this.Invalidate();
      }
    }

    public LedBulb()
    {
      this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
      this.Color = Color.FromArgb((int) byte.MaxValue, 153, (int) byte.MaxValue, 54);
    }

    protected override CreateParams CreateParams
    {
      get
      {
        CreateParams createParams = base.CreateParams;
        createParams.ExStyle |= 32;
        return createParams;
      }
    }

    protected override void OnMove(EventArgs e) => this.RecreateHandle();

    protected override void OnPaintBackground(PaintEventArgs e)
    {
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      Bitmap bitmap = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height);
      using (Graphics g = Graphics.FromImage((Image) bitmap))
      {
        g.SmoothingMode = SmoothingMode.HighQuality;
        this.drawControl(g);
        e.Graphics.DrawImageUnscaled((Image) bitmap, 0, 0);
      }
    }

    private void drawControl(Graphics g)
    {
      Color baseColor = this.On ? this.Color : this.DarkColor;
      Color color = this.On ? this.DarkColor : this.DarkDarkColor;
      Rectangle rectangle = new Rectangle(this.Padding.Left, this.Padding.Top, this.Width - (this.Padding.Left + this.Padding.Right), this.Height - (this.Padding.Top + this.Padding.Bottom));
      int num = rectangle.Width < rectangle.Height ? rectangle.Width : rectangle.Height;
      Rectangle rect1 = new Rectangle(rectangle.X, rectangle.Y, num, num);
      if (rect1.Width < 1)
        rect1.Width = 1;
      if (rect1.Height < 1)
        rect1.Height = 1;
      g.FillEllipse((Brush) new SolidBrush(color), rect1);
      GraphicsPath path1 = new GraphicsPath();
      path1.AddEllipse(rect1);
      g.FillEllipse((Brush) new PathGradientBrush(path1)
      {
        CenterColor = baseColor,
        SurroundColors = new Color[1]
        {
          Color.FromArgb(0, baseColor)
        }
      }, rect1);
      GraphicsPath path2 = new GraphicsPath();
      path2.AddEllipse(rect1);
      g.SetClip(path2);
      GraphicsPath graphicsPath = new GraphicsPath();
      Rectangle rect2 = new Rectangle(rect1.X - Convert.ToInt32((float) rect1.Width * 0.15f), rect1.Y - Convert.ToInt32((float) rect1.Width * 0.15f), Convert.ToInt32((float) rect1.Width * 0.8f), Convert.ToInt32((float) rect1.Height * 0.8f));
      graphicsPath.AddEllipse(rect2);
      g.FillEllipse((Brush) new PathGradientBrush(path1)
      {
        CenterColor = Color.FromArgb(180, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue),
        SurroundColors = new Color[1]
        {
          Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue)
        }
      }, rect2);
      int width = rect1.Width;
      g.SetClip(this.ClientRectangle);
      if (!this.On)
        return;
      g.DrawEllipse(new Pen(Color.FromArgb(85, Color.Black), 1f), rect1);
    }
  }
}

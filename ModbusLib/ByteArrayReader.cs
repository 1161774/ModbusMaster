// Decompiled with JetBrains decompiler
// Type: ModbusLib.ByteArrayReader
// Assembly: ModbusLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3455EC4F-205D-4D22-B187-B5EFF11CDE1C
// Assembly location: C:\Program Files (x86)\Farrellton Solar\Modbus Master\ModbusLib.dll

using System.Collections;

namespace ModbusLib
{
  public class ByteArrayReader : IEnumerable, IByteArray
  {
    private readonly byte[] _buffer;

    public ByteArrayReader(byte[] source)
    {
      this._buffer = source.ToArray();
      this.Length = this._buffer.Length;
      this.Reset();
    }

    public ByteArrayReader(byte[] source, int offset, int count)
    {
      this._buffer = source.ToArray(offset, count);
      this.Length = count;
      this.Reset();
    }

    public int Position { get; private set; }

    public int Length { get; private set; }

    public void Reset() => this.Position = -1;

    public byte[] ToArray() => this._buffer.ToArray();

    public bool EndOfBuffer => this.Position >= this.Length - 1;

    public bool CanRead(int count)
    {
      int num = this.Length - this.Position - 1;
      return count <= num;
    }

    public byte Peek() => this._buffer[this.Position];

    public byte ReadByte() => this._buffer[++this.Position];

    public bool TryReadByte(out byte value)
    {
      if (this.CanRead(1))
      {
        value = this.ReadByte();
        return true;
      }
      value = (byte) 0;
      return false;
    }

    public byte[] ReadBytes(int count)
    {
      byte[] numArray = new byte[count];
      for (int index = 0; index < count; ++index)
        numArray[index] = this._buffer[++this.Position];
      return numArray;
    }

    public byte[] ReadToEnd() => this.ReadBytes(this.Length - (this.Position + 1));

    public short ReadInt16LE()
    {
      int offset = this.Position + 1;
      this.Position += 2;
      return ByteArrayHelpers.ReadInt16LE(this._buffer, offset);
    }

    public short ReadInt16BE()
    {
      int offset = this.Position + 1;
      this.Position += 2;
      return ByteArrayHelpers.ReadInt16BE(this._buffer, offset);
    }

    public ushort ReadUInt16LE()
    {
      int offset = this.Position + 1;
      this.Position += 2;
      return ByteArrayHelpers.ReadUInt16LE(this._buffer, offset);
    }

    public ushort ReadUInt16BE()
    {
      int offset = this.Position + 1;
      this.Position += 2;
      return ByteArrayHelpers.ReadUInt16BE(this._buffer, offset);
    }

    public int ReadInt32LE()
    {
      int offset = this.Position + 1;
      this.Position += 4;
      return ByteArrayHelpers.ReadInt32LE(this._buffer, offset);
    }

    public int ReadInt32BE()
    {
      int offset = this.Position + 1;
      this.Position += 4;
      return ByteArrayHelpers.ReadInt32BE(this._buffer, offset);
    }

    IEnumerator IEnumerable.GetEnumerator() => this._buffer.GetEnumerator();

    byte[] IByteArray.Data => this._buffer;
  }
}

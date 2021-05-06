// Decompiled with JetBrains decompiler
// Type: ModbusLib.ByteArrayWriter
// Assembly: ModbusLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3455EC4F-205D-4D22-B187-B5EFF11CDE1C
// Assembly location: C:\Program Files (x86)\Farrellton Solar\Modbus Master\ModbusLib.dll

using System;
using System.Collections;

namespace ModbusLib
{
  public class ByteArrayWriter : IEnumerable, IByteArray
  {
    private const int CHUNK_SIZE = 256;
    private byte[] _buffer;
    private int _length;
    private readonly byte[] _proxy;

    public ByteArrayWriter()
    {
      this._buffer = new byte[256];
      this._proxy = new byte[8];
    }

    public ByteArrayWriter(byte[] initial)
    {
      this._length = initial.Length;
      this._buffer = new byte[this._length];
      Array.Copy((Array) initial, (Array) this._buffer, this._length);
    }

    public int Length => this._length;

    public void Reset()
    {
      this.CheckImmutable();
      this._length = 0;
    }

    public ByteArrayReader ToReader() => new ByteArrayReader(((IByteArray) this).Data);

    public byte[] ToArray() => ((IByteArray) this).Data;

    public void WriteByte(byte value)
    {
      this.Allocate(1);
      this._buffer[this._length++] = value;
    }

    public void WriteBytes(byte[] values, int offset, int count)
    {
      this.Allocate(count);
      Array.Copy((Array) values, offset, (Array) this._buffer, this._length, count);
      this._length += count;
    }

    public void WriteBytes(byte[] values) => this.WriteBytes(values, 0, values.Length);

    public void WriteBytes(ByteArrayReader reader) => this.WriteBytes(((IByteArray) reader).Data);

    public void WriteBytes(ByteArrayWriter writer) => this.WriteBytes(((IByteArray) writer).Data);

    public void WriteInt16LE(short value)
    {
      this.CheckImmutable();
      ByteArrayHelpers.WriteInt16LE(this._proxy, 0, value);
      this.WriteBytes(this._proxy, 0, 2);
    }

    public void WriteInt16BE(short value)
    {
      this.CheckImmutable();
      ByteArrayHelpers.WriteInt16BE(this._proxy, 0, value);
      this.WriteBytes(this._proxy, 0, 2);
    }

    public void WriteUInt16LE(ushort value)
    {
      this.CheckImmutable();
      ByteArrayHelpers.WriteUInt16LE(this._proxy, 0, value);
      this.WriteBytes(this._proxy, 0, 2);
    }

    public void WriteUInt16BE(ushort value)
    {
      this.CheckImmutable();
      ByteArrayHelpers.WriteUInt16BE(this._proxy, 0, value);
      this.WriteBytes(this._proxy, 0, 2);
    }

    public void WriteInt32LE(int value)
    {
      this.CheckImmutable();
      ByteArrayHelpers.WriteInt32LE(this._proxy, 0, value);
      this.WriteBytes(this._proxy, 0, 4);
    }

    public void WriteInt32BE(int value)
    {
      this.CheckImmutable();
      ByteArrayHelpers.WriteInt32BE(this._proxy, 0, value);
      this.WriteBytes(this._proxy, 0, 4);
    }

    private void Allocate(int count)
    {
      this.CheckImmutable();
      int length = this._buffer.Length;
      int num = this._length + count;
      if (num < length)
        return;
      do
      {
        length += 256;
      }
      while (length < num);
      byte[] numArray = new byte[length];
      Array.Copy((Array) this._buffer, (Array) numArray, this._buffer.Length);
      this._buffer = numArray;
    }

    private void CheckImmutable()
    {
      if (this._proxy == null)
        throw new Exception();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      for (int i = 0; i < this._length; ++i)
        yield return (object) this._buffer[i];
    }

    byte[] IByteArray.Data
    {
      get
      {
        byte[] numArray = new byte[this._length];
        Array.Copy((Array) this._buffer, (Array) numArray, this._length);
        return numArray;
      }
    }
  }
}

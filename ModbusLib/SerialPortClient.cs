// Decompiled with JetBrains decompiler
// Type: ModbusLib.SerialPortClient
// Assembly: ModbusLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3455EC4F-205D-4D22-B187-B5EFF11CDE1C
// Assembly location: C:\Program Files (x86)\Farrellton Solar\Modbus Master\ModbusLib.dll

using System;
using System.IO.Ports;
using System.Threading;

namespace ModbusLib
{
  internal class SerialPortClient : ICommClient
  {
    public readonly SerialPort Port;

    public SerialPortClient(SerialPort port) => this.Port = port;

    public int Latency { get; set; }

    public CommResponse Query(ClientCommData data)
    {
      lock (this.Port)
      {
        byte[] array = data.OutgoingData.ToArray();
        ByteArrayWriter byteArrayWriter = new ByteArrayWriter();
        byte[] numArray = new byte[256];
        int num = 0;
        for (int retries = data.Retries; num < retries; ++num)
        {
          this.Port.DiscardInBuffer();
          this.Port.DiscardOutBuffer();
          this.Port.Write(array, 0, array.Length);
          byteArrayWriter.Reset();
          Thread.Sleep(100);
          bool timeoutExpired;
          using (new Timer((TimerCallback) (_ => timeoutExpired = true), (object) null, this.Latency + data.Timeout, -1))
          {
            timeoutExpired = false;
            while (!timeoutExpired)
            {
              int count = this.Port.BytesToRead;
              if (count > 0)
              {
                if (count > 256)
                  count = 256;
                this.Port.Read(numArray, 0, count);
                byteArrayWriter.WriteBytes(numArray, 0, count);
                data.IncomingData = byteArrayWriter.ToReader();
                CommResponse commResponse = data.OwnerProtocol.Codec.ClientDecode((CommDataBase) data);
                if (commResponse.Status == 3 || commResponse.Status == 2)
                  return commResponse;
                if (commResponse.Status != 0)
                  break;
              }
              Thread.Sleep(100);
            }
          }
        }
        return new CommResponse((CommDataBase) data, 2);
      }
    }

    public void QueryAsync(ClientCommData data) => throw new NotImplementedException();
  }
}

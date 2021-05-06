// Decompiled with JetBrains decompiler
// Type: ModbusLib.IpClient
// Assembly: ModbusLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3455EC4F-205D-4D22-B187-B5EFF11CDE1C
// Assembly location: C:\Program Files (x86)\Farrellton Solar\Modbus Master\ModbusLib.dll

using System;
using System.Net.Sockets;
using System.Threading;

namespace ModbusLib
{
  public class IpClient : ICommClient
  {
    public readonly Socket Port;
    private byte[] tcpAsyClBuffer = new byte[2048];

    public IpClient(Socket port) => this.Port = port;

    public int Latency { get; set; }

    public event IpClient.ResponseData OnResponseData;

    public event IpClient.ExceptionData OnException;

    public CommResponse Query(ClientCommData data)
    {
      lock (this.Port)
      {
        byte[] array = data.OutgoingData.ToArray();
        ByteArrayWriter byteArrayWriter = new ByteArrayWriter();
        byte[] numArray = new byte[256];
        int num1 = 0;
        for (int retries = data.Retries; num1 < retries; ++num1)
        {
          this.Port.Send(array);
          byteArrayWriter.Reset();
          bool timeoutExpired;
          using (new Timer((TimerCallback) (_ => timeoutExpired = true), (object) null, this.Latency + data.Timeout, -1))
          {
            timeoutExpired = false;
            while (!timeoutExpired)
            {
              int num2 = this.Port.Available;
              if (num2 > 0)
              {
                if (num2 > 256)
                  num2 = 256;
                this.Port.Receive(numArray, num2, SocketFlags.None);
                byteArrayWriter.WriteBytes(numArray, 0, num2);
                data.IncomingData = byteArrayWriter.ToReader();
                CommResponse commResponse = data.OwnerProtocol.Codec.ClientDecode((CommDataBase) data);
                if (commResponse.Status == 3 || commResponse.Status == 2)
                  return commResponse;
                if (commResponse.Status != 0)
                  break;
              }
              Thread.Sleep(0);
            }
          }
        }
        return new CommResponse((CommDataBase) data, 2);
      }
    }

    public void QueryAsync(ClientCommData data)
    {
      lock (this.Port)
      {
        byte[] array = data.OutgoingData.ToArray();
        this.Port.BeginSend(array, 0, array.Length, SocketFlags.None, new AsyncCallback(this.OnSend), (object) null);
        this.Port.BeginReceive(this.tcpAsyClBuffer, 0, this.tcpAsyClBuffer.Length, SocketFlags.None, new AsyncCallback(this.OnReceive), (object) data);
      }
    }

    private void OnReceive(IAsyncResult result)
    {
      if (!result.IsCompleted)
        this.CallException((ushort) byte.MaxValue, byte.MaxValue, (byte) 254);
      ByteArrayWriter byteArrayWriter = new ByteArrayWriter();
      byteArrayWriter.WriteBytes(this.tcpAsyClBuffer, 0, (int) this.tcpAsyClBuffer[8] + 9);
      ClientCommData asyncState = (ClientCommData) result.AsyncState;
      asyncState.IncomingData = byteArrayWriter.ToReader();
      CommResponse response = asyncState.OwnerProtocol.Codec.ClientDecode((CommDataBase) asyncState);
      if (this.OnResponseData == null)
        return;
      this.OnResponseData(response);
    }

    private void OnSend(IAsyncResult result)
    {
      if (result.IsCompleted)
        return;
      this.CallException(ushort.MaxValue, byte.MaxValue, (byte) 100);
    }

    internal void CallException(ushort id, byte function, byte exception)
    {
      if (this.OnException == null)
        return;
      this.OnException(id, function, exception);
    }

    public delegate void ResponseData(CommResponse response);

    public delegate void ExceptionData(ushort id, byte function, byte exception);
  }
}

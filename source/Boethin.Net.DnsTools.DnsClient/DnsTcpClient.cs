/*
 * File: Boethin.Net.DnsTools.DnsClient/DnsTcpClient.cs
 *
 * This file is part of Boethin.Net.DnsTools, a DNS debugging library.
 * Copyright (C) 2013 Sebastian Boethin <sebastian@boethin.eu>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 *
 */

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Net;
using Boethin.Net.DnsTools.DnsClient.Extensions;
using System.Diagnostics;

namespace Boethin.Net.DnsTools.DnsClient
{

  /// <summary>
  /// A System.Net.Sockets.TcpClient, extended with the IDnsClient capabilities.
  /// </summary>
  public sealed class DnsTcpClient : System.Net.Sockets.TcpClient, IDnsClient, Logging.IMessageLogger
  {

    // [RFC 1035]
    // 4.2.2. TCP usage
    //
    // Messages sent over TCP connections use server port 53 (decimal).  The
    // message is prefixed with a two byte length field which gives the message
    // length, excluding the two byte length field.  This length field allows
    // the low-level processing to assemble a complete message before beginning
    // to parse it.

    public const int DefaultPort = 53;

    #region c'tor

    public DnsTcpClient()
    {
    }

    #endregion

    #region IDnsClient

    public event MessageEventHandler RequestSending;

    //public event MessageEventHandler ResponseReceived;

    // IDnsClient.LogMessageCreated
    public event Logging.LogMessageEventHandler LogMessageCreated;

    NetworkProtocol IDnsClient.NetworkProtocol
    {
      get { return NetworkProtocol.TCP; }
    }

    bool IDnsClient.Connected
    {
      get { return base.Connected; }
    }

    void IDnsClient.Connect(string address)
    {
      if (String.IsNullOrEmpty(address))
        throw new ArgumentNullException("address", "String value cannot be null or empty.");
      ((IDnsClient)this).Connect(IPAddress.Parse(address));
    }

    void IDnsClient.Connect(IPAddress address)
    {
      if (Object.ReferenceEquals(null, address))
        throw new ArgumentNullException("address");
      Connect(new IPEndPoint(address, DefaultPort));
    }

    void IDnsClient.Connect(IPEndPoint endpoint)
    {
      if (Object.ReferenceEquals(null, endpoint))
        throw new ArgumentNullException("endpoint");
      base.Connect(endpoint);
    }

    void IDnsClient.Close()
    {
      base.Close();
    }

    Response IDnsClient.LookUp(Request request)
    {
      if (object.ReferenceEquals(null, request))
        throw new ArgumentNullException("request");

      // create request datagram, prefixed with the two byte length field
      Datagram qdata = Datagram.GetPrefixed(request.Data);

      NetworkStream ns = base.GetStream();
      if (RequestSending != null)
        RequestSending(this, new MessageEventArgs(request, ns, (IPEndPoint)Client.RemoteEndPoint));

      // LogMessage
      LogRequest(qdata.Length);

      // send prefixed request message
      request.SetTimestamp();
      ns.Write(qdata, 0, qdata.Length);

      // read response
      SyncReader reader = new SyncReader(ns);
      for (reader.Start(); reader.Next(); ) ;

      Response response = Response.Create((Datagram)reader.Data, DateTime.Now);

      // LogMessage
      LogResponse(response);

      return response;
    }

    IAsyncResult IDnsClient.BeginLookUp(Request request, AsyncCallback callback, object state)
    {
      if (object.ReferenceEquals(null, request))
        throw new ArgumentNullException("request");

      Internal.AsyncResult<Response> result = new Internal.AsyncResult<Response>(callback, state);

      // create request datagram, prefixed with the two byte length field
      Datagram qdata = Datagram.GetPrefixed(request.Data);

      NetworkStream ns = base.GetStream();

      if (RequestSending != null)
        RequestSending(this, new MessageEventArgs(request, ns, (IPEndPoint)Client.RemoteEndPoint));

      // LogMessage
      LogRequest(qdata.Length);

      // asynchronously send prefixed request message
      request.SetTimestamp();
      ns.BeginWrite(qdata, 0, qdata.Length, new AsyncCallback(AsyncRequestWritten), result);

      return result;
    }

    Response IDnsClient.EndLookUp(IAsyncResult asyncResult)
    {
      return ((Internal.AsyncResult<Response>)asyncResult).EndInvoke();
    }

    #endregion

    #region private

    // Reading a DNS response from a TCP stream requires getting the two byte length prefix
    // first and then reading the designated amount of data, where the whole process may
    // require multiple read calls (even reading the first two bytes may not work at one go).

    // Abstract implementation for both synchronous and asynchronous processes.
    private abstract class TcpReader
    {

      // A multi step reading operation can be tested by setting the ReceiveBufferSize
      // property to a small value

      // first chunk size
      private const int BufferSize = 0x400;

      private readonly byte[] TempBuffer = new byte[BufferSize];
      private int TempBufferIndex = 0;

      private byte[] DataBuffer = null;
      private int DataBufferIndex = 0;

      public byte[] Data { get { return DataBuffer; } }

      public abstract void Read(byte[] buffer, int index, int length);

      public void Start()
      {
        Read(TempBuffer, 0, BufferSize);
      }

      public bool Next(int received)
      {
        if (!(received > 0))
          throw new EndOfStreamException("TCP stream ended unexpectedly.");

        if (object.ReferenceEquals(null, DataBuffer))
        {
          if (TempBufferIndex + received >= 2) // we've got the length prefix
          {
            ushort length;
            if (0 == (length = TempBuffer.AsNetworkUint16()))
              throw new Exception("A zero length prefix was received from the server.");
            DataBuffer = new byte[length];
            DataBufferIndex = TempBufferIndex + received - 2;
            if (DataBufferIndex > 0) // keep the rest of the buffer as data
              Array.Copy(TempBuffer, 2, DataBuffer, 0, DataBufferIndex);
          }
          else // received == 1
          {
            // Only 1 byte has been received.
            // You can test this rare case by setting ReceiveBufferSize to 1.
            TempBufferIndex = 1;
            Read(TempBuffer, 1, BufferSize - 1);
          }
        }
        else
        {
          // advance the data pointer
          DataBufferIndex += received;
        }

        if (DataBuffer != null)
        {
          if (DataBufferIndex < DataBuffer.Length) // not yet done with reading
          {
            Read(DataBuffer, DataBufferIndex, DataBuffer.Length - DataBufferIndex);
          }
          else // reading finsihed
          {
            return false;
          }
        }

        return true;
      }

    }

    // Synchronous TcpReader.
    private class SyncReader : TcpReader
    {
      private readonly NetworkStream Stream;
      private int Received;

      public SyncReader(NetworkStream stream)
      {
        this.Stream = stream;
      }

      public bool Next()
      {
        return Next(Received);
      }

      public override void Read(byte[] buffer, int index, int length)
      {
        Received = Stream.Read(buffer, index, length);
      }

    }

    // The state object carryied through an asynchrounous reading process.
    private class AsyncReaderState : TcpReader
    {
      private readonly DnsTcpClient Client;
      public readonly IAsyncResult Result;

      public AsyncReaderState(DnsTcpClient client, IAsyncResult result)
      {
        Client = client;
        Result = result;
      }

      public override void Read(byte[] data, int index, int length)
      {
        NetworkStream ns = Client.GetStream();
        ns.BeginRead(data, index, length, new AsyncCallback(Client.AsyncResponseRead), this);
      }

    }

    // callback for asynchronous writing
    private void AsyncRequestWritten(IAsyncResult asyncResult)
    {
      try
      {
        // Asynchronous writing to the NetworkStream works with one call, regardless 
        // of whether or not the SendBufferSize is smaller than the amount of data.
        base.GetStream().EndWrite(asyncResult);

        // start reading
        new AsyncReaderState(this, asyncResult).Start();
      }
      catch (Exception ex)
      {
        ((Internal.AsyncResult<Response>)asyncResult.AsyncState).SetAsCompleted(
          ex, asyncResult.CompletedSynchronously);
      }
    }

    // callback for asynchronous reading
    private void AsyncResponseRead(IAsyncResult asyncResult)
    {
      AsyncReaderState state = (AsyncReaderState)asyncResult.AsyncState;
      try
      {
        if (!state.Next(base.GetStream().EndRead(asyncResult)))
        {
          Response response = Response.Create((Datagram)state.Data, DateTime.Now);

          // LogMessage
          LogResponse(response);

          ((Internal.AsyncResult<Response>)state.Result.AsyncState).SetAsCompleted(
            response, asyncResult.CompletedSynchronously);
        }
      }
      catch (Exception ex)
      {
        ((Internal.AsyncResult<Response>)state.Result.AsyncState).SetAsCompleted(
          ex, asyncResult.CompletedSynchronously);
      }
    }

    private void LogRequest(int byteLength)
    {
      if (LogMessageCreated != null)
      {
        Logging.LogMessage.LogRequest(this, byteLength, (IPEndPoint)base.Client.RemoteEndPoint);
      }
    }

    private void LogResponse(Response response)
    {
      if (LogMessageCreated != null)
      {
        Logging.LogMessage.LogResponse(this, response.Data.Length, response.Header, (IPEndPoint)base.Client.RemoteEndPoint);
      }
    }

    #endregion

    #region Logging.IMessageLogger

    void Logging.IMessageLogger.LogMessageCreate(Logging.LogMessage message)
    {
      LogMessageCreated(this, new Logging.LogMessageEventArgs(message));
    }

    #endregion

    #region override

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
    }

    #endregion

  }
}

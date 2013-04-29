/*
 * File: Boethin.Net.DnsTools.DnsClient/DnsUdpClient.cs
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
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Boethin.Net.DnsTools.DnsClient
{

  /// <summary>
  /// A UDP client for sending DNS requests.
  /// </summary>
  public class DnsUdpClient : System.Net.Sockets.UdpClient, IDnsClient, Logging.IMessageLogger
  {

    // [RFC 1035]
    // 4.2.1. UDP usage
    //
    // Messages sent using UDP user server port 53 (decimal).
    //
    // Messages carried by UDP are restricted to 512 bytes (not counting the IP
    // or UDP headers). Longer messages are truncated and the TC bit is set in
    // the header.

    public const int DefaultPort = 53;

    public const int DatagramMaxLength = 512;

    #region private

    private IPEndPoint RemoteEP = null;

    #endregion

    #region c'tor

    public DnsUdpClient()
      : base()
    {
      //_Options = options;

      // UdpClient w/o default remote host
      //UdpClient = new UdpClient(AddressFamily.InterNetwork);



      //UdpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, _Options.UdpSendTimeout);
      //UdpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, _Options.UdpReceiveTimeout);
    }

    #endregion

    #region IDnsClient

    //public event MessageEventHandler RequestSending;

    //public event MessageEventHandler ResponseReceived;

    // IDnsClient.LogMessageCreated
    public event Logging.LogMessageEventHandler LogMessageCreated;

    NetworkProtocol IDnsClient.NetworkProtocol
    {
      get { return NetworkProtocol.UDP; }
    }

    bool IDnsClient.Connected
    {
      get { return RemoteEP != null; }
    }

    /// <summary>
    /// Assign an IP address with the client using the default name server port 53.
    /// </summary>
    /// <param name="address"></param>
    void IDnsClient.Connect(IPAddress address)
    {
      ((IDnsClient)this).Connect(new IPEndPoint(address, 53));
    }

    void IDnsClient.Connect(IPEndPoint endpoint)
    {
      RemoteEP = endpoint;
    }

    void IDnsClient.Close()
    {
      RemoteEP = null;
    }

    Response IDnsClient.LookUp(Request request)
    {
      if (object.ReferenceEquals(null, request))
        throw new ArgumentNullException("request");

      if (!((IDnsClient)this).Connected)
        throw new InvalidOperationException(
          "The operation requires an IP address assigned with the client (use the Connect method).");



      // send request
      Datagram qdata = request.Data;
      if (qdata.Length > DatagramMaxLength) // can actually never happen 
        throw new InvalidOperationException(String.Format(
          "Request message length exceeds {0} bytes.", DatagramMaxLength));

      // LogMessage
      LogRequest(qdata.Length);

      request.SetTimestamp();
      base.Send(qdata, qdata.Length, RemoteEP);

      // receive response
      // Blocks until a message returns on this socket from a remote host.
      IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
      Datagram rdata = (Datagram)base.Receive(ref remote);

      Response response = Response.Create(rdata, DateTime.Now);

      // LogMessage
      LogResponse(response, remote);

      return response;
    }

    IAsyncResult IDnsClient.BeginLookUp(Request request, AsyncCallback callback, object state)
    {
      Internal.AsyncResult<Response> result = new Internal.AsyncResult<Response>(callback, state);

      // send request
      Datagram qdata = request.Data;
      if (qdata.Length > DatagramMaxLength) // can actually never happen 
        throw new InvalidOperationException(String.Format(
          "Request message length exceeds {0} bytes.", DatagramMaxLength));

      // LogMessage
      LogRequest(qdata.Length);

      request.SetTimestamp();
      base.BeginSend(qdata, qdata.Length, RemoteEP, new AsyncCallback(AsyncRequestSent), result);

      return result;
    }

    Response IDnsClient.EndLookUp(IAsyncResult asyncResult)
    {
      return ((Internal.AsyncResult<Response>)asyncResult).EndInvoke();
    }

    #endregion

    #region private

    private void AsyncRequestSent(IAsyncResult asyncResult)
    {
      try
      {
        base.EndSend(asyncResult);
        base.BeginReceive(new AsyncCallback(AsyncResponseReceived), asyncResult.AsyncState);
      }
      catch (Exception ex)
      {
        ((Internal.AsyncResult<Response>)asyncResult.AsyncState).SetAsCompleted(ex, false);
      }
    }

    private void AsyncResponseReceived(IAsyncResult asyncResult)
    {
      try
      {
        IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
        Datagram dgram = (Datagram)base.EndReceive(asyncResult, ref remote);
        Response response = Response.Create(dgram, DateTime.Now);

        // LogMessage
        LogResponse(response, remote);

        ((Internal.AsyncResult<Response>)asyncResult.AsyncState).SetAsCompleted(
          response, asyncResult.CompletedSynchronously);
      }
      catch (Exception ex)
      {
        ((Internal.AsyncResult<Response>)asyncResult.AsyncState).SetAsCompleted(ex, false);
      }
    }

    private void LogRequest(int byteLength)
    {
      if (LogMessageCreated != null)
      {
        Logging.LogMessage.LogRequest(this, byteLength, RemoteEP);
      }
    }

    private void LogResponse(Response response, System.Net.IPEndPoint remote)
    {
      if (LogMessageCreated != null)
      {
        Logging.LogMessage.LogResponse(this, response.Data.Length, response.Header, remote);
      }
    }

    #endregion

    #region Logging.IMessageLogger

    void Logging.IMessageLogger.LogMessageCreate(Logging.LogMessage message)
    {
      LogMessageCreated(this, new Logging.LogMessageEventArgs(message));
    }

    #endregion

  }
}

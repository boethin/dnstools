/*
 * File: Boethin.Net.DnsTools.DnsClient/Logging/LogMessage.cs
 *
 * This file is part of Boethin.Net.DnsTools, a DNS debugging library.
 * Copyright (C) 2013 Sebastian Boethin <sebastian@boethin.eu>
 * 
 * 
 * MIT License
 * 
 * Copyright (c) 2023 Sebastian Böthin
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 *
 */

﻿using System;

namespace Boethin.Net.DnsTools.DnsClient.Logging
{

  [Serializable()]
  public class LogMessage
  {

    #region public readonly

    public readonly LogMessageState State;

    public readonly LogMessageLevel Level;

    public readonly string MessageFormat;

    public readonly object[] MessageFormatArgs;

    public string Message
    {
      get
      {
        return String.Format(MessageFormat, MessageFormatArgs);
      }
    }

    #endregion

    #region c'tor

    internal protected LogMessage(LogMessageState state, LogMessageLevel level, string message, object[] args)
    {
      this.State = state;
      this.Level = level;
      this.MessageFormat = message;
      this.MessageFormatArgs = args;
    }

    #endregion

    #region override

    public override string ToString()
    {
      return String.Format("{0}: [{1}] {2}", State.ToString(), Level.ToString(), Message);
    }

    #endregion

    #region internal static

    internal static void LogRequest(IDnsClient client, int byteLength, System.Net.IPEndPoint remote)
    {
      ((IMessageLogger)client).LogMessageCreate(LogMessageText.REQUEST.INFO.SendingRequest(
          byteLength, client.NetworkProtocol, remote));
    }

    internal static void LogResponse(IDnsClient client, int byteLength, DNS.Header header, System.Net.IPEndPoint remote)
    {
      ((IMessageLogger)client).LogMessageCreate(LogMessageText.RESPONSE.INFO.ResponseReceived(byteLength, remote));

      if (header.RCODEValue != 0)
        ((IMessageLogger)client).LogMessageCreate(LogMessageText.RESPONSE.WARN.ResponseNonZero(header.RCODEValue, header.RCODE));

      if (header.TC)
        ((IMessageLogger)client).LogMessageCreate(LogMessageText.RESPONSE.WARN.ResponseTruncated());

    }

    #endregion

  }

}

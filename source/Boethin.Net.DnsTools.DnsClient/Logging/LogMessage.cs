/*
 * File: Boethin.Net.DnsTools.DnsClient/Logging/LogMessage.cs
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

namespace Boethin.Net.DnsTools.DnsClient.Logging
{

  [Serializable()]
  public class LogMessage
  {

    #region public readonly

    public readonly LogMessageState State;

    public readonly LogMessageLevel Level;

    public readonly string Message;

    #endregion

    #region c'tor

    internal protected LogMessage(LogMessageState state, LogMessageLevel level, string message)
    {
      this.State = state;
      this.Level = level;
      this.Message = message;
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
      ((IMessageLogger)client).LogMessageCreate(LogMessageText.SendingRequest(
          byteLength, client.NetworkProtocol, remote));
    }

    internal static void LogResponse(IDnsClient client, int byteLength, DNS.Header header, System.Net.IPEndPoint remote)
    {
      ((IMessageLogger)client).LogMessageCreate(LogMessageText.ResponseReceived(byteLength, remote));

      if (header.RCODEValue != 0)
        ((IMessageLogger)client).LogMessageCreate(LogMessageText.ResponseNonZero(header.RCODEValue, header.RCODE));

      if (header.TC)
        ((IMessageLogger)client).LogMessageCreate(LogMessageText.ResponseTruncated());

    }

    #endregion

  }

}

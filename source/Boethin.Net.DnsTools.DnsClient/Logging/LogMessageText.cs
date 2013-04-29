/*
 * File: Boethin.Net.DnsTools.DnsClient/Logging/LogMessageText.cs
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
  internal static class LogMessageText
  {
    
    /// <summary>
    /// &quot;Sending {0} bytes via {1} to {2} on port {3}.&quot;
    /// </summary>
    /// <returns></returns>
    public static LogMessage SendingRequest(int bytes, NetworkProtocol proto, System.Net.IPEndPoint ep)
    {
      return new LogMessage(LogMessageState.REQUEST, LogMessageLevel.INFO, String.Format(
        Resources.LogMessages.SendingRequest, 
        bytes, proto.ToString(), ep.Address.ToString(), ep.Port));
    }

    /// <summary>
    /// &quot;Received: {0} bytes from {1} in {2}ms."
    /// </summary>
    /// <param name="bytes"></param>
    /// <param name="remote"></param>
    /// <returns></returns>
    public static LogMessage ResponseReceived(int bytes, System.Net.IPEndPoint remote)
    {
      return new LogMessage(LogMessageState.RESPONSE, LogMessageLevel.INFO, String.Format(
        Resources.LogMessages.ResponseReceived,
        bytes, remote.Address.ToString()));
    }

    /// <summary>
    /// &quot;Non-Zero RCODE received: {0} ({1}).&quot;
    /// </summary>
    /// <param name="rcodeValue"></param>
    /// <param name="rcode"></param>
    /// <returns></returns>
    public static LogMessage ResponseNonZero(byte rcodeValue, DnsClient.DNS.RCODE rcode)
    {
      return new LogMessage(LogMessageState.RESPONSE, LogMessageLevel.WARN, String.Format(
        Resources.LogMessages.ResponseNonZero, rcodeValue, rcode.ToString()));
    }

    /// <summary>
    /// &quot;Truncated response (the TC bit is set).&quot;
    /// </summary>
    /// <returns></returns>
    public static LogMessage ResponseTruncated()
    {
      return new LogMessage(LogMessageState.RESPONSE, LogMessageLevel.WARN, 
        Resources.LogMessages.ResponseTruncated);    
    }
    
  }
}

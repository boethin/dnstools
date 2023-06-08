/*
 * File: Boethin.Net.DnsTools.DnsClient/Logging/LogMessageText.cs
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

using System;
﻿
namespace Boethin.Net.DnsTools.DnsClient.Logging
{
  internal static class LogMessageText
  {

    public static class REQUEST
    {

      public static class INFO
      {

        /// <summary>
        /// &quot;Sending {0} bytes via {1} to {2} on port {3}.&quot;
        /// </summary>
        /// <returns></returns>
        public static LogMessage SendingRequest(int bytes, NetworkProtocol proto, System.Net.IPEndPoint ep)
        {
          return Create(Resources.LogMessages.SendingRequest,
            new object[] { bytes, proto, ep.Address, ep.Port });
        }

        private static LogMessage Create(string message, object[] args)
        {
          return REQUEST.Create(LogMessageLevel.INFO, message, args);
        }

      }

      private static LogMessage Create(LogMessageLevel level, string message, object[] args)
      {
        return new LogMessage(LogMessageState.REQUEST, level, message, args);
      }

    }

    public static class RESPONSE
    {

      public static class INFO
      {

        /// <summary>
        /// &quot;Received: {0} bytes from {1} in {2}ms."
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="remote"></param>
        /// <returns></returns>
        public static LogMessage ResponseReceived(int bytes, System.Net.IPEndPoint remote)
        {
          return Create(
            Resources.LogMessages.ResponseReceived,
              new object[] { bytes, remote.Address });
        }

        private static LogMessage Create(string message, object[] args)
        {
          return RESPONSE.Create(LogMessageLevel.INFO, message, args);
        }

      }

      public static class WARN
      {

        /// <summary>
        /// &quot;Non-Zero RCODE received: {0} ({1}).&quot;
        /// </summary>
        /// <param name="rcodeValue"></param>
        /// <param name="rcode"></param>
        /// <returns></returns>
        public static LogMessage ResponseNonZero(byte rcodeValue, DnsClient.DNS.RCODE rcode)
        {
          return Create(
            Resources.LogMessages.ResponseNonZero,
              new object[] { rcodeValue, rcode });
        }

        /// <summary>
        /// &quot;Truncated response (the TC bit is set).&quot;
        /// </summary>
        /// <returns></returns>
        public static LogMessage ResponseTruncated()
        {
          return Create(
            Resources.LogMessages.ResponseTruncated,
            new object[]{
            });
        }

        private static LogMessage Create(string message, object[] args)
        {
          return RESPONSE.Create(LogMessageLevel.WARN, message, args);
        }

      }

      private static LogMessage Create(LogMessageLevel level, string message, object[] args)
      {
        return new LogMessage(LogMessageState.RESPONSE, level, message, args);
      }

    }

  }
}

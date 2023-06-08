/*
 * File: Boethin.Net.DnsTools.Resolution/Extensions/IPAddressExtensions.cs
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
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Boethin.Net.DnsTools.Resolution.Extensions
{
  public static class IPAddressExtensions
  {

    /// <summary>
    /// Get the ARPA reverse domain name for an IPv4 or IPv6 address.
    /// </summary>
    /// <param name="addr"></param>
    /// <returns></returns>
    public static string GetArpaDomain(this IPAddress addr)
    {
      if (addr == null)
        throw new ArgumentNullException("addr");

      StringBuilder result = new StringBuilder();
      byte[] bytes = addr.GetAddressBytes();

      switch (addr.AddressFamily)
      {
        case AddressFamily.InterNetwork:
          // IPv4
          for (int i = bytes.Length - 1; i >= 0; i--)
          {
            if (result.Length > 0)
              result.Append('.');
            result.Append(bytes[i].ToString());
          }
          result.Append(".in-addr.arpa");
          break;

        case AddressFamily.InterNetworkV6:
          // IPv6
          for (int i = bytes.Length - 1; i >= 0; i--)
          {
            if (result.Length > 0)
              result.Append('.');
            char[] b = String.Format("{0:x2}", bytes[i]).ToCharArray();
            result.Append(b[1]);
            result.Append('.');
            result.Append(b[0]);
          }
          result.Append(".ip6.arpa");
          break;

        default:
          throw new NotSupportedException(String.Format(
            "Method not supported for addresses of type {0}.", addr.AddressFamily.ToString()));
      }

      return result.ToString();
    }

  }
}

/*
 * File: Boethin.Net.DnsTools.Resolution/Extensions/IPAddressExtensions.cs
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

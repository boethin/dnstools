/*
 * File: Boethin.Net.DnsTools.Resolution/NameServer.cs
 *
 * This file is part of Boethin.Net.DnsTools, a DNS debugging library.
 * Copyright (C) 2013 Sebastian Boethin <boethin@xn--domain.net>
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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Boethin.Net.DnsTools.Resolution
{

  [Serializable] 
  public class NameServer : NetworkHost
  {

    public readonly DnsDomain Zone;

    #region c'tor

    public NameServer(DnsDomain name)
      : base(name)
    {
      this.Zone = null;
    }

    //public NameServer(string name)
    //  : base(name)
    //{
    //  this.Zone = null;
    //}

    //public NameServer(DnsDomain name, IEnumerable<IPAddress> addresses)
    //  : base(name, addresses)
    //{
    //}

    public NameServer(DnsDomain zone, DnsDomain name)
      : base(name)
    {
      this.Zone = zone;
    }

    public NameServer(DnsDomain zone, DnsDomain name, IEnumerable<IPAddress> addresses)
      : base(name, addresses)
    {
      this.Zone = zone;
    }

    #endregion

    #region public

    /// <summary>
    /// Get the first IP address matching the version specification (or null if no address is known so far).
    /// </summary>
    /// <param name="protocolVersion">Protocol version flag.</param>
    /// <returns></returns>
    public IPAddress GetFirstAddress(IPVersion protocolVersion)
    {
      if (false == IsResolved)
        return null;

      // If both flags are set, v6 is prefered, otherwise the specified IP version is required.
      IPAddress result = null;
      if (0 != (int)(protocolVersion & IPVersion.IPv6))
        result = Addresses.Where(
          a => a.AddressFamily == AddressFamily.InterNetworkV6).FirstOrDefault();
      if (result == null)
        result = Addresses.Where(
          a => a.AddressFamily == AddressFamily.InterNetwork).FirstOrDefault();

      if (result == null)
      {
        // no address was found although the server's addresses are already known
        throw new ResolverException(String.Format(
          "An IP address matching the version specification {0} was not found for server '{1}'.",
          protocolVersion.ToString(), Name));
      }
      return result;
    }

    //internal void ApplyAnswerRecords(DnsClient.Response response)
    //{
    //  foreach (DnsClient.DNS.Records.Address a in response.AnswerRecords.Select(
    //    rr => rr as DnsClient.DNS.Records.Address).Where(a => a != null && Name.Equals(a.Base.NAME)))
    //  {
    //    Addresses.Add(a.ADDRESS);
    //  }
    //}

    internal void ApplyAddresses(IEnumerable<DnsClient.DNS.Records.Address> addresses)
    {
      if (!addresses.Any())
        throw new ArgumentException("Address list cannot be empty.");

      foreach (DnsClient.DNS.Records.Address a in addresses)
      {
        Addresses.Add(a.ADDRESS);
      }
    }

    #endregion

  }
}

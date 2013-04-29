/*
 * File: Boethin.Net.DnsTools.Resolution/NameServer.cs
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

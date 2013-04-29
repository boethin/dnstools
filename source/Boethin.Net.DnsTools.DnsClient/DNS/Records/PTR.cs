/*
 * File: Boethin.Net.DnsTools.DnsClient/DNS/Records/PTR.cs
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

namespace Boethin.Net.DnsTools.DnsClient.DNS.Records
{

  [Serializable] 
  public class PTR : RR
  {

    // RFC 1035
    // 3.3.12. PTR RDATA format

    #region RDATA

    /// <summary>
    /// A &lt;domain-name&gt; which points to some location in the
    /// domain name space.
    /// [RFC 1035]
    /// </summary>
    public string PTRDNAME { get; private set; }

    #endregion

    #region c'tor

    internal PTR(DNS.RRBase rrbase)
      : base(rrbase)
    {
    }

    #endregion

    #region override

    public override string ToString(int namePadding)
    {
      // 1.0.0.10.in-addr.arpa.  3600 IN  PTR  example.com.
      // 1.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.8.b.d.0.1.0.0.2.ip6.arpa. 3600 IN PTR test-ipv6.example.com.
      return String.Join(" ", new string[] { 
        Base.ToString(namePadding), 
        PTRDNAME
      });
    }

    internal override void ReadRDATA(Internal.ByteReader reader)
    {
      PTRDNAME = reader.ReadDomain();
    }

    protected override bool EqualsRDATA(RR rr)
    {
      return PTRDNAME.Equals(((PTR)rr).PTRDNAME);
    }

    #endregion


  }
}

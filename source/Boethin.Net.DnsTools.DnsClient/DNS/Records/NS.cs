/*
 * File: Boethin.Net.DnsTools.DnsClient/DNS/Records/NS.cs
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

namespace Boethin.Net.DnsTools.DnsClient.DNS.Records
{

  [Serializable] 
  public class NS : RR
  {

    // RFC 1035
    // 3.3.11. NS RDATA format

    #region RDATA

    /// <summary>
    /// A &lt;domain-name&gt; which specifies a host which should be
    /// authoritative for the specified class and domain.
    /// [RFC 1035]
    /// </summary>
    public string NSDNAME { get; private set; }

    #endregion

    #region c'tor

    internal NS(DNS.RRBase rrbase)
      : base(rrbase)
    {
    }

    #endregion

    #region override

    public override string ToString(int namePadding)
    {
      // example.com.  1800  IN  NS  names1.example.com.
      return String.Join(" ", new string[] { 
        Base.ToString(namePadding), 
        NSDNAME
      });
    }

    internal override void ReadRDATA(Internal.ByteReader reader)
    {
      NSDNAME = reader.ReadDomain();
    }

    protected override bool EqualsRDATA(RR rr)
    {
      return NSDNAME.Equals(((NS)rr).NSDNAME);
    }

    #endregion
    
  }
}

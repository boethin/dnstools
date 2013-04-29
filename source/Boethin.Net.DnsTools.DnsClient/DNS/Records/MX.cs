/*
 * File: Boethin.Net.DnsTools.DnsClient/DNS/Records/MX.cs
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
  public class MX : RR
  {

    // RFC 1035
    // 3.3.9. MX RDATA format

    #region RDATA

    /// <summary>
    /// A 16 bit integer which specifies the preference given to
    /// this RR among others at the same owner.  Lower values
    /// are prefered.
    /// </summary>
    public ushort PREFERENCE { get; private set; }

    /// <summary>
    /// A &lt;domain-name&gt; which specifies a host willing to act as
    /// a mail exchange for the owner name.
    /// </summary>
    public string EXCHANGE { get; private set; }

    #endregion

    #region c'tor

    internal MX(DNS.RRBase rrbase)
      : base(rrbase)
    {
    }

    #endregion

    #region override

    public override string ToString(int namePadding)
    {
      return String.Join(" ", new string[] { 
        Base.ToString(namePadding), 
        PREFERENCE.ToString(),
        EXCHANGE
      });
    }

    internal override void ReadRDATA(Internal.ByteReader reader)
    {
      PREFERENCE = reader.ReadUIn16();
      EXCHANGE = reader.ReadDomain();
    }

    protected override bool EqualsRDATA(RR rr)
    {
      return EXCHANGE.Equals(((MX)rr).EXCHANGE) &&
        PREFERENCE.Equals(((MX)rr).PREFERENCE);
    }

    #endregion

  }
}

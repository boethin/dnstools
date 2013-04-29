/*
 * File: Boethin.Net.DnsTools.DnsClient/DNS/Records/CNAME.cs
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
  public class CNAME : RR
  {

    // [RFC 1035]
    // 3.3.1. CNAME RDATA format

    // [RFC 2181}
    // 10.1.1. CNAME terminology
    //
    //    It has been traditional to refer to the label of a CNAME record as "a
    //    CNAME".  This is unfortunate, as "CNAME" is an abbreviation of
    //    "canonical name", and the label of a CNAME record is most certainly
    //    not a canonical name.  It is, however, an entrenched usage.  Care
    //    must therefore be taken to be very clear whether the label, or the
    //    value (the canonical name) of a CNAME resource record is intended.
    //    In this document, the label of a CNAME resource record will always be
    //    referred to as an alias.


    #region RDATA

    /// <summary>
    /// A &lt;domain-name&gt; which specifies the canonical or primary
    /// name for the owner. The owner name is an alias.
    /// </summary>
    public string CANONICAL { get; private set; } // member names cannot be the same as their enclosing type

    #endregion

    #region c'tor

    internal CNAME(DNS.RRBase rrbase)
      : base(rrbase)
    {
    }

    #endregion

    #region override

    public override string ToString(int namePadding)
    {
      return String.Join(" ", new string[] { 
        Base.ToString(namePadding),
        CANONICAL.ToString()
      });
    }

    internal override void ReadRDATA(Internal.ByteReader reader)
    {
      CANONICAL = reader.ReadDomain();
    }

    protected override bool EqualsRDATA(RR rr)
    {
      return CANONICAL.Equals(((CNAME)rr).CANONICAL);
    }

    #endregion


  }
}

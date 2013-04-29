/*
 * File: Boethin.Net.DnsTools.DnsClient/DNS/Records/OPT.cs
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

  /// <summary>
  /// The OPT pseudo-RR is an essential part of the Extension mechanisms for DNS (EDNS).
  /// [RFC 2671]
  /// </summary>
  [Serializable] 
  public class OPT : RR
  {

    // [RFC 2671]
    // 4 - OPT pseudo-RR
    //
    // 4.1. One OPT pseudo-RR can be added to the additional data section of
    //
    //      either a request or a response.  An OPT is called a pseudo-RR
    //      because it pertains to a particular transport level message and not
    //      to any actual DNS data.  OPT RRs shall never be cached, forwarded,
    //      or stored in or loaded from master files.  The quantity of OPT
    //      pseudo-RRs per message shall be either zero or one, but not
    //      greater.



    #region c'tor

    internal OPT(DNS.RRBase rrbase)
      : base(rrbase)
    {
    }

    #endregion

    #region override

    public override string ToString(int namePadding)
    {
      throw new NotImplementedException();
    }

    internal override void ReadRDATA(Internal.ByteReader reader)
    {
      throw new NotImplementedException();
    }

    protected override bool EqualsRDATA(RR rr)
    {
      throw new NotImplementedException();
    }

    #endregion

  }
}

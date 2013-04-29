/*
 * File: Boethin.Net.DnsTools.DnsClient/DNS/Records/Default.cs
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
  /// Default recorce record whose type is not implemented.
  /// </summary>
  [Serializable] 
  public class Default : RR
  {

    #region RDATA

    /// <summary>
    /// The variable length string of octets that describes the resource.
    /// </summary>
    public Datagram RDATA { get; private set; }

    #endregion

    #region c'tor

    internal Default(DNS.RRBase rrbase)
      : base(rrbase)
    {
    }

    #endregion

    #region override

    public override string ToString(int namePadding)
    {
      return String.Join(" ", new string[] { 
        Base.ToString(namePadding),
        RDATA.ToLengthString()
      });
    }

    internal override void ReadRDATA(Internal.ByteReader reader)
    {
      RDATA = reader.ReadBytes(Base.RDLENGTH);
    }

    protected override bool EqualsRDATA(RR rr)
    {
      return RDATA.Equals(((Default)rr).RDATA);
    }

    #endregion

  }
}

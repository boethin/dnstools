/*
 * File: Boethin.Net.DnsTools.DnsClient/DNS/Records/Address.cs
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
using System.Net;

namespace Boethin.Net.DnsTools.DnsClient.DNS.Records
{

  /// <summary>
  /// Abstract IP address record, implemented either as A or AAAA record.
  /// </summary>
  [Serializable] 
  public abstract class Address : RR
  {

    #region RDATA

    public IPAddress ADDRESS { get; protected set; }

    #endregion
    
    #region c'tor

    internal Address(DNS.RRBase rrbase)
      : base(rrbase)
    {
    }

    #endregion

    #region override

    protected override bool EqualsRDATA(RR rr)
    {
      return this.ADDRESS.Equals(((Address)rr).ADDRESS);
    }

    #endregion

    #region Equals

    /// <summary>
    /// A common string representation of the Resource Record.
    /// </summary>
    /// <param name="namePadding">Apply PadLeft to the NAME field</param>
    /// <returns></returns>
    public override string ToString(int namePadding)
    {
      // www.example.com.   3600  IN  A  172.27.171.106
      return String.Join(" ", new string[] { 
        Base.ToString(namePadding),
        ADDRESS.ToString()
      });
    }

    public override int GetHashCode()
    {
      return base.GetHashCode() ^ ADDRESS.GetHashCode();
    }

    #endregion



  }
}

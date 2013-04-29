/*
 * File: Boethin.Net.DnsTools.DnsClient/DNS/Records/SOA.cs
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
  public class SOA : RR
  {
    
    #region RDATA

    /// <summary>
    /// The &lt;domain-name&gt; of the name server that was the
    /// original or primary source of data for this zone.
    /// [RFC 1035]
    /// </summary>
    public string MNAME { get; private set; }

    /// <summary>
    /// A &lt;domain-name&gt; which specifies the mailbox of the
    /// person responsible for this zone.
    /// [RFC 1035]
    /// </summary>
    public string RNAME { get; private set; }

    /// <summary>
    /// The unsigned 32 bit version number of the original copy
    /// of the zone.  Zone transfers preserve this value.  This
    /// value wraps and should be compared using sequence space
    /// arithmetic.
    /// [RFC 1035]
    /// </summary>
    public uint SERIAL { get; private set; }

    /// <summary>
    /// A 32 bit time interval before the zone should be
    /// refreshed.
    /// [RFC 1035]
    /// <para>All times are in units of seconds.</para>
    /// </summary>
    public uint REFRESH { get; private set; }

    /// <summary>
    /// A 32 bit time interval that should elapse before a
    /// failed refresh should be retried.
    /// [RFC 1035]
    /// <para>All times are in units of seconds.</para>
    /// </summary>
    public uint RETRY { get; private set; }

    /// <summary>
    /// A 32 bit time value that specifies the upper limit on
    /// the time interval that can elapse before the zone is no
    /// longer authoritative.
    /// [RFC 1035]
    /// <para>All times are in units of seconds.</para>
    /// </summary>
    public uint EXPIRE { get; private set; }

    /// <summary>
    /// The unsigned 32 bit minimum TTL field that should be
    /// exported with any RR from this zone.
    /// [RFC 1035]
    /// <para>All times are in units of seconds.</para>
    /// </summary>
    public uint MINIMUM { get; private set; }

    #endregion

    #region c'tor

    internal SOA(DNS.RRBase @base)
      : base(@base)
    {
    }

    #endregion

    #region override

    public override string ToString(int namePadding)
    {
      // example.com. IN SOA master.example.com. hostmaster.example.com.  2007061501 3600 1800 604800 600
      return String.Join(" ", new string[] { 
        Base.NAME.PadLeft(namePadding),
        Base.CLASS.ToString(), 
        Base.TypeString, 
        MNAME.ToString(), 
        RNAME.ToString(), 
        SERIAL.ToString(),
        REFRESH.ToString(), 
        RETRY.ToString(), 
        EXPIRE.ToString(), 
        Base.TTL.ToString()
      });
    }

    internal override void ReadRDATA(Internal.ByteReader reader)
    {
      MNAME = reader.ReadDomain();
      RNAME = reader.ReadDomain();
      SERIAL = reader.ReadUint32();
      REFRESH = reader.ReadUint32();
      RETRY = reader.ReadUint32();
      EXPIRE = reader.ReadUint32();
      MINIMUM = reader.ReadUint32();
    }

    protected override bool EqualsRDATA(RR rr)
    {
      return MNAME.Equals(((SOA)rr).MNAME) &&
        RNAME.Equals(((SOA)rr).RNAME) &&
        SERIAL.Equals(((SOA)rr).SERIAL) &&
        REFRESH.Equals(((SOA)rr).REFRESH) &&
        RETRY.Equals(((SOA)rr).RETRY) &&
        EXPIRE.Equals(((SOA)rr).EXPIRE) &&
        MINIMUM.Equals(((SOA)rr).MINIMUM);
    }

    #endregion

  }
}

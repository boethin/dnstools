/*
 * File: Boethin.Net.DnsTools.DnsClient/DNS/Records/SOA.cs
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

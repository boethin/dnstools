/*
 * File: Boethin.Net.DnsTools.DnsClient/DNS/Records/PTR.cs
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

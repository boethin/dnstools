/*
 * File: Boethin.Net.DnsTools.DnsClient/DNS/Records/NS.cs
 *
 * This file is part of Boethin.Net.DnsTools, a DNS debugging library.
 * Copyright (C) 2013 Sebastian Boethin <sebastian@boethin.eu>
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

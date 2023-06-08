/*
 * File: Boethin.Net.DnsTools.DnsClient/DNS/Records/CNAME.cs
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

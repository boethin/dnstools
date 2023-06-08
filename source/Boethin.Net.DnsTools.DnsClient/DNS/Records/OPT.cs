/*
 * File: Boethin.Net.DnsTools.DnsClient/DNS/Records/OPT.cs
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

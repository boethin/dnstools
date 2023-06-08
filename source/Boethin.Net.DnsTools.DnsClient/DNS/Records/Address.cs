/*
 * File: Boethin.Net.DnsTools.DnsClient/DNS/Records/Address.cs
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

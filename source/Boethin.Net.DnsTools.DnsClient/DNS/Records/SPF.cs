/*
 * File: Boethin.Net.DnsTools.DnsClient/DNS/Records/SPF.cs
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
using System.Linq;
using System.Text;

namespace Boethin.Net.DnsTools.DnsClient.DNS.Records
{

  /// <summary>
  /// RR containing a SPF (Sender Policy Framework) entry. Similar to TXT records.
  /// </summary>
  [Serializable] 
  public class SPF : TXT
  {

    // [RFC 4408]
    // 3.1.1. DNS Resource Record Types
    //
    // This document defines a new DNS RR of type SPF, code 99.  The format
    // of this type is identical to the TXT RR [RFC1035].  For either type,
    // the character content of the record is encoded as [US-ASCII].

    // elsewhere (3. SPF Records):
    //
    // The SPF record is a single string of text.
    //

    // Thus, an SPF record is treated here a TXT record consisting of only one ASCII-String.

    #region public get

    /// <summary>
    /// The string defining the SPF entry.
    /// </summary>
    public string SPFString
    {
      get { return Encoding.ASCII.GetString(DATA.First()); }
    }

    #endregion

    #region c'tor

    internal SPF(DNS.RRBase rrbase)
      : base(rrbase)
    {
    }

    #endregion

  }
}

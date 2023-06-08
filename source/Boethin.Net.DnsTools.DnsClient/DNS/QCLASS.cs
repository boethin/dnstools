/*
 * File: Boethin.Net.DnsTools.DnsClient/DNS/QCLASS.cs
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

namespace Boethin.Net.DnsTools.DnsClient.DNS
{

  /// <summary>
  /// a two octet code that specifies the class of the query.
  /// For example, the QCLASS field is IN for the Internet.
  /// </summary>
  public enum QCLASS : ushort
  {

    /// <summary>
    /// The Internet (1).
    /// [RFC 1035]
    /// </summary>
    IN = 1,

    #region omitted

    // [RFC 1035]
    // CS              2 the CSNET class (Obsolete - used only for examples in
    //                 some obsolete RFCs)

    #endregion

    /// <summary>
    /// The CHAOS class (3).
    /// [RFC 1035]
    /// </summary>
    CH = 3,

    /// <summary>
    /// Hesiod [Dyer 87] (4).
    /// [RFC 1035]
    /// </summary>
    HS = 4

    

  }
}

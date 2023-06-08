/*
 * File: Boethin.Net.DnsTools.DnsClient/DNS/RCODE.cs
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

namespace Boethin.Net.DnsTools.DnsClient.DNS
{

  // RCODE           Response code - this 4 bit field is set as part of
  //                 responses.  The values have the following
  //                 interpretation:
  //

  public enum RCODE
  {


    /// <summary>
    /// No error condition.
    /// [RFC 1035]
    /// </summary>
    NoError = 0,

    /// <summary>
    /// Format error - The name server was
    /// unable to interpret the query.
    /// [RFC 1035]
    /// </summary>
    FormatError = 1,

    /// <summary>
    /// Server failure - The name server was
    /// unable to process this query due to a
    /// problem with the name server.
    /// [RFC 1035]
    /// </summary>
    ServerFailure = 2,

    /// <summary>
    /// Name Error - Meaningful only for
    /// responses from an authoritative name
    /// server, this code signifies that the
    /// domain name referenced in the query does
    /// not exist.
    /// [RFC 1035]
    /// </summary>
    NameError = 3,

    /// <summary>
    /// Not Implemented - The name server does
    /// not support the requested kind of query.
    /// [RFC 1035]
    /// </summary>
    NotImplemented = 4,

    /// <summary>
    /// Refused - The name server refuses to
    /// perform the specified operation for
    /// policy reasons.
    /// [RFC 1035]
    /// </summary>
    Refused = 5,

    /// <summary>
    /// 6-15: Reserved for future use.
    /// </summary>
    Other = 6



  }

}

/*
 * File: Boethin.Net.DnsTools.DnsClient/DNS/OPCODE.cs
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

namespace Boethin.Net.DnsTools.DnsClient.DNS
{
  public enum OPCODE : byte
  {

    /// <summary>
    /// A standard query (0).
    /// [RFC 1035]
    /// </summary>
    QUERY = 0,

    #region omitted

    // [RFC 1035], obsoleted by [RFC 3425]
    // 1               an inverse query (IQUERY)

    #endregion

    /// <summary>
    /// A server status request (2)
    /// [RFC 1035]
    /// </summary>
    STATUS = 2,

    // 3 Unassigned

    /// <summary>
    /// The DNS NOTIFY transaction allows master servers to inform slave
    /// servers when the zone has changed (4).
    /// [RFC 1996]
    /// </summary>
    NOTIFY = 4,

    /// <summary>
    /// Using the UPDATE (5) opcode, it is possible to add
    /// or delete RRs or RRsets from a specified zone. 
    /// [RFC2136]
    /// </summary>
    UPDATE = 5


    // 6-15 Unassigned

  }
}

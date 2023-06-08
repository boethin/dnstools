/*
 * File: Boethin.Net.DnsTools.DnsClient/DNS/Records/AAAA.cs
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
using System.Net;

namespace Boethin.Net.DnsTools.DnsClient.DNS.Records
{

  /// <summary>
  /// IPv6 address record.
  /// </summary>
  [Serializable] 
  public class AAAA : Address
  {

    #region c'tor

    internal AAAA(DNS.RRBase rrbase)
      : base(rrbase)
    {
    }

    #endregion

    #region internal

    internal override void ReadRDATA(Internal.ByteReader reader)
    {
      if (Base.RDLENGTH != 16)
        throw new InvalidResponseException(String.Format(
          "Invalid RDLENGTH value {0}: expected 16.",
          Base.RDLENGTH));

      byte[] buf = reader.ReadBytes(16);
      ADDRESS = new IPAddress(buf);
      if (ADDRESS.AddressFamily != System.Net.Sockets.AddressFamily.InterNetworkV6)
        throw new InvalidResponseException(String.Format(
          "Not an IPv6 address: {0}.", ADDRESS.ToString()));
    }

    #endregion

  }
}

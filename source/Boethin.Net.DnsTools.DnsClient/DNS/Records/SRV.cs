/*
 * File: Boethin.Net.DnsTools.DnsClient/DNS/Records/SRV.cs
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

  /// <summary>
  /// A DNS RR for specifying the location of services (DNS SRV).
  /// [RFC 2782]
  /// </summary>
  [Serializable] 
  public class SRV : RR
  {

    // [RFC 2782]
    // The format of the SRV RR

    #region RDATA

    /// <summary>
    /// The priority of this target host. A client MUST attempt to
    /// contact the target host with the lowest-numbered priority it can
    /// reach; target hosts with the same priority SHOULD be tried in an
    /// order defined by the weight field. 
    /// [RFC 2782]
    /// </summary>
    public ushort PRIORITY { get; private set; }

    /// <summary>
    /// A server selection mechanism.  The weight field specifies a
    /// relative weight for entries with the same priority. Larger
    /// weights SHOULD be given a proportionately higher probability of
    /// being selected.
    /// [RFC 2782]
    /// </summary>
    public ushort WEIGHT { get; private set; }

    /// <summary>
    /// The port on this target host of this service.
    /// [RFC 2782]
    /// </summary>
    public ushort PORT { get; private set; }

    /// <summary>
    /// The domain name of the target host.
    /// [RFC 2782]
    /// </summary>
    public string TARGET { get; private set; }

    #endregion

    #region c'tor

    internal SRV(DNS.RRBase rrbase)
      : base(rrbase)
    {
    }

    #endregion

    #region override

    public override string ToString(int namePadding)
    {
      // _sip._tcp.example.com. 86400 IN SRV 0 5 5060 sipserver.example.com.
      return String.Join(" ", new string[] { 
        Base.ToString(namePadding), 
        PRIORITY.ToString(),
        WEIGHT.ToString(),
        PORT.ToString(),
        TARGET.ToString()
      });
    }

    internal override void ReadRDATA(Internal.ByteReader reader)
    {
      PRIORITY = reader.ReadUIn16();
      WEIGHT = reader.ReadUIn16();
      PORT = reader.ReadUIn16();
      TARGET = reader.ReadDomain();
    }

    protected override bool EqualsRDATA(RR rr)
    {
      return PRIORITY.Equals(((SRV)rr).PRIORITY) &&
        WEIGHT.Equals(((SRV)rr).WEIGHT) &&
        PORT.Equals(((SRV)rr).PORT) &&
        TARGET.Equals(((SRV)rr).TARGET);
    }

    #endregion

  }
}

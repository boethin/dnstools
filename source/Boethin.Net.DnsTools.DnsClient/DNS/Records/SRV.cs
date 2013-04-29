/*
 * File: Boethin.Net.DnsTools.DnsClient/DNS/Records/SRV.cs
 *
 * This file is part of Boethin.Net.DnsTools, a DNS debugging library.
 * Copyright (C) 2013 Sebastian Boethin <sebastian@boethin.eu>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
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

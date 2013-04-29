/*
 * File: Boethin.Net.DnsTools.DnsClient/DNS/OPCODE.cs
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

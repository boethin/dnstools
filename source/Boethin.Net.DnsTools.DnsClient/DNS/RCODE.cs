/*
 * File: Boethin.Net.DnsTools.DnsClient/DNS/RCODE.cs
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

/*
 * File: Boethin.Net.DnsTools.DnsClient/DNS/QCLASS.cs
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

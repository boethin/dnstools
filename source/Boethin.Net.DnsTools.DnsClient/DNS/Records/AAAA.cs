/*
 * File: Boethin.Net.DnsTools.DnsClient/DNS/Records/AAAA.cs
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

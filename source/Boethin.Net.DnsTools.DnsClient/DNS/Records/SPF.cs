/*
 * File: Boethin.Net.DnsTools.DnsClient/DNS/Records/SPF.cs
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
using System.Linq;
using System.Text;

namespace Boethin.Net.DnsTools.DnsClient.DNS.Records
{

  /// <summary>
  /// RR containing a SPF (Sender Policy Framework) entry. Similar to TXT records.
  /// </summary>
  [Serializable] 
  public class SPF : TXT
  {

    // [RFC 4408]
    // 3.1.1. DNS Resource Record Types
    //
    // This document defines a new DNS RR of type SPF, code 99.  The format
    // of this type is identical to the TXT RR [RFC1035].  For either type,
    // the character content of the record is encoded as [US-ASCII].

    // elsewhere (3. SPF Records):
    //
    // The SPF record is a single string of text.
    //

    // Thus, an SPF record is treated here a TXT record consisting of only one ASCII-String.

    #region public get

    /// <summary>
    /// The string defining the SPF entry.
    /// </summary>
    public string SPFString
    {
      get { return Encoding.ASCII.GetString(DATA.First()); }
    }

    #endregion

    #region c'tor

    internal SPF(DNS.RRBase rrbase)
      : base(rrbase)
    {
    }

    #endregion

  }
}

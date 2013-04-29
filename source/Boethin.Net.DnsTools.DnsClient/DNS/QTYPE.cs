/*
 * File: Boethin.Net.DnsTools.DnsClient/DNS/QTYPE.cs
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

  // http://www.iana.org/assignments/dns-parameters/dns-parameters.xml
  // http://docstore.mik.ua/orelly/networking_2ndEd/dns/appb_01.htm

  // http://www.dnsqueries.com/en/domain_check.php

  // http://en.wikipedia.org/wiki/List_of_DNS_record_types


  /// <summary>
  /// A two octet code which specifies the type of the query.
  /// The values for this field include all codes valid for a
  /// TYPE field, together with some more general codes which
  /// can match more than one type of RR.
  /// </summary>
  public enum QTYPE : ushort
  {

    #region [RFC 1035]

    // [RFC 1035] 3.2.2. TYPE values
    //
    // TYPE fields are used in resource records.  Note that these types are a
    // subset of QTYPEs.


    /// <summary>
    /// IPv4 host address (1)
    /// [RFC 1035]
    /// </summary>
    A = 1,

    /// <summary>
    /// An authoritative name server (2).
    /// [RFC 1035]
    /// </summary>
    NS = 2,

    #region omitted

    // MD              3 a mail destination (Obsolete - use MX)

    // MF              4 a mail forwarder (Obsolete - use MX)

    #endregion

    /// <summary>
    /// The canonical name for an alias (5)
    /// [RFC 1035]
    /// </summary>
    CNAME = 5,

    /// <summary>
    /// Marks the start of a zone of authority (6).
    /// [RFC 1035]
    /// </summary>
    SOA = 6,

    #region omitted

    // MB              7 a mailbox domain name (EXPERIMENTAL)

    // MG              8 a mail group member (EXPERIMENTAL)

    // MR              9 a mail rename domain name (EXPERIMENTAL)

    // NULL            10 a null RR (EXPERIMENTAL)

    // WKS             11 a well known service description

    #endregion

    /// <summary>
    /// A domain name pointer (12).
    /// [RFC 1035]
    /// </summary>
    PTR = 12,

    // HINFO           13 host information

    // MINFO           14 mailbox or mail list information

    /// <summary>
    /// Mail exchange (15).
    /// [RFC 1035]
    /// </summary>
    MX = 15,

    /// <summary>
    /// Text strings (16).
    /// [RFC 1035]
    /// </summary>
    TXT = 16,


    // [RFC 1035] 3.2.3. QTYPE values
    //
    // QTYPE fields appear in the question part of a query.  QTYPES are a
    // superset of TYPEs, hence all TYPEs are valid QTYPEs.  In addition, the
    // following QTYPEs are defined:

    /// <summary>
    /// A request for a transfer of an entire zone.
    /// [RFC 1035]
    /// </summary>
    AXFR = 252,

    #region omitted

    // MAILB           253 A request for mailbox-related records (MB, MG or MR)

    // MAILA           254 A request for mail agent RRs (Obsolete - see MX)

    #endregion

    /// <summary>
    /// A request for all records (255).
    /// [RFC 1035]
    /// </summary>
    ANY = 255,

    #endregion

    #region [RFC 3596] AAAA

    /// <summary>
    /// A single IPv6 address (28). 
    /// [RFC 3596]
    /// </summary>
    AAAA = 28,

    #endregion

    #region [RFC 2782] SRV

    /// <summary>
    /// A DNS RR for specifying the location of services (DNS SRV).
    /// [RFC 2782]
    /// </summary>
    SRV = 33,

    #endregion

    #region [RFC 3403] NAPTR

    /// <summary>
    /// Name Authority Pointer (NAPTR).
    /// [RFC 3403]
    /// </summary>
    NAPTR = 35,

    #endregion

    #region [RFC 4408] SPF

    /// <summary>
    /// An SPF record is a DNS Resource Record (RR) that declares which hosts
    /// are, and are not, authorized to use a domain name for the &quot;HELO&quot; and
    /// &quot;MAIL FROM&quot;" identities.
    /// [RFC 4408]
    /// </summary>
    SPF = 99

    #endregion


  }
}

/*
 * File: Boethin.Net.DnsTools.DnsClient/DNS/Records/NAPTR.cs
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
  ///  The Naming Authority Pointer (NAPTR) RR is essential for the
  ///  Dynamic Delegation Discovery System (DDDS).
  ///  [RFC 3403]
  /// </summary>
  [Serializable] 
  public class NAPTR : RR
  {

    // [RFC 3403]
    // 4.1 Packet Format
    //
    //    The packet format of the NAPTR RR is given below.  The DNS type code
    //    for NAPTR is 35.
    //
    //       The packet format for the NAPTR record is as follows
    //                                        1  1  1  1  1  1
    //          0  1  2  3  4  5  6  7  8  9  0  1  2  3  4  5
    //        +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    //        |                     ORDER                     |
    //        +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    //        |                   PREFERENCE                  |
    //        +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    //        /                     FLAGS                     /
    //        +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    //        /                   SERVICES                    /
    //        +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    //        /                    REGEXP                     /
    //        +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    //        /                  REPLACEMENT                  /
    //        /                                               /
    //        +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    //
    //    <character-string> and <domain-name> as used here are defined in RFC
    //    1035 [7].

    #region RDATA

    /// <summary>
    /// A 16-bit unsigned integer specifying the order in which the NAPTR
    /// records MUST be processed in order to accurately represent the
    /// ordered list of Rules.
    /// </summary>
    public ushort ORDER { get; private set; }

    /// <summary>
    /// A 16-bit unsigned integer that specifies the order in which NAPTR 
    /// records with equal Order values should be
    /// processed.
    /// </summary>
    public ushort PREFERENCE { get; private set; }

    /// <summary>
    /// Flags to control aspects of the rewriting and interpretation of the fields 
    /// in the record. Flags are single characters from the set A-Z and 0-9. The case of the
    /// alphabetic characters is not significant. The field can be empty.
    /// </summary>
    public string FLAGS { get; private set; }

    /// <summary>
    /// A &lt;character-string&gt; that specifies the Service Parameters
    /// applicable to this this delegation path. 
    /// </summary>
    public Datagram SERVICES { get; private set; }

    /// <summary>
    /// A &lt;character-string&gt; containing a substitution expression that is
    /// applied to the original string held by the client in order to
    /// construct the next domain name to lookup. 
    /// </summary>
    public Datagram REGEXP { get; private set; }

    /// <summary>
    /// A &lt;domain-name&gt; which is the next domain-name to query for
    /// depending on the potential values found in the flags field.  This
    /// field is used when the regular expression is a simple replacement
    /// operation.
    /// </summary>
    public string REPLACEMENT { get; private set; }

    #endregion

    #region c'tor

    internal NAPTR(DNS.RRBase rrbase)
      : base(rrbase)
    {
    }

    #endregion

    #region override

    public override string ToString(int namePadding)
    {
      return String.Join(" ", new string[] { 
        Base.ToString(namePadding), 
        ORDER.ToString(),
        PREFERENCE.ToString(),
        String.Format("\"{0}\"", FLAGS),
        SERVICES.ToQuotedString(),
        REGEXP.ToQuotedString(),
        REPLACEMENT.ToString()
      });
    }

    internal override void ReadRDATA(Internal.ByteReader reader)
    {
      ORDER = reader.ReadUIn16();
      PREFERENCE = reader.ReadUIn16();
      FLAGS = Encoding.ASCII.GetString(reader.ReadCharacterString()).ToUpper();
      SERVICES = reader.ReadCharacterString();
      REGEXP = reader.ReadCharacterString();
      REPLACEMENT = reader.ReadDomain();
    }

    protected override bool EqualsRDATA(RR rr)
    {
      return ORDER.Equals(((NAPTR)rr).ORDER) &&
        PREFERENCE.Equals(((NAPTR)rr).PREFERENCE) &&
        FLAGS.Equals(((NAPTR)rr).FLAGS) &&
        SERVICES.Equals(((NAPTR)rr).SERVICES) &&
        REGEXP.Equals(((NAPTR)rr).REGEXP) &&
        REPLACEMENT.Equals(((NAPTR)rr).REPLACEMENT);
    }

    #endregion

  }
}

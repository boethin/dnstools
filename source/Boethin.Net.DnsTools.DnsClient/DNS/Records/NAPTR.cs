/*
 * File: Boethin.Net.DnsTools.DnsClient/DNS/Records/NAPTR.cs
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

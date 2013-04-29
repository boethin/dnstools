/*
 * File: Boethin.Net.DnsTools.DnsClient/DNS/Records/TXT.cs
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

  [Serializable]
  public class TXT : RR
  {

    // [RFC 1035]
    // 3.3.14. TXT RDATA format
    //
    //     +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    //     /                   TXT-DATA                    /
    //     +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    //
    // where:
    //
    // TXT-DATA        One or more <character-string>s.
    //
    // TXT RRs are used to hold descriptive text. The semantics of the text
    // depends on the domain where it is found.
    // 

    // <character-string>:
    // defined elsewhere (3.3. Standard RRs):
    //
    // <character-string> is a single length octet followed by that number of characters.
    // <character-string> is treated as binary information, and can be up to 256 characters
    // in length (including the length octet).

    #region private

    private Datagram[] Strings;

    #endregion

    #region RDATA

    /// <summary>
    /// One or more &lt;character-string&gt;s.
    /// <para>A &lt;character-string&gt; is treated as binary information, 
    /// and can be up to 255 characters in length.</para>
    /// </summary>
    public IEnumerable<Datagram> DATA
    {
      get { return Strings.AsEnumerable(); }
    }

    #endregion

    #region c'tor

    internal TXT(DNS.RRBase rrbase)
      : base(rrbase)
    {
    }

    #endregion

    #region override

    public override string ToString(int namePadding)
    {
      // server1.example.com.  3600  IN  TXT  "Hello World"
      return String.Join(" ", new string[] { 
        Base.ToString(namePadding), 
        String.Join(" ", DATA.Select(s => s.ToQuotedString()).ToArray())
      });
    }
    
    internal override void ReadRDATA(Internal.ByteReader reader)
    {
      // The format of the data within a DNS TXT record is one or more
      // strings, packed together in memory without any intervening gaps
      // or padding bytes for word alignment.
      //
      // The format of each constituent string within the DNS TXT record is
      // a single length byte, followed by 0-255 bytes of text data.

      // TXT-DATA strings are not guaranteed to consist purely of ASCII printable
      // characters though this is usually the case.

      List<Datagram> strings = new List<Datagram>();
      for (int total = 0; total < Base.RDLENGTH; )
      {
        byte length = reader.ReadByte();
        if (length > 0)
        {
          if (total + length >= Base.RDLENGTH)
            throw new InvalidResponseException(
              "Invalid length byte in TXT record: String data would exceed RDLENGTH.");
          strings.Add(reader.ReadBytes(length));
        }
        total += (length + 1);
      }
      Strings = strings.ToArray();
    }

    protected override bool EqualsRDATA(RR rr)
    {
      if (Strings.Length != ((TXT)rr).Strings.Length)
        return false;
      for (int i = 0; i < Strings.Length; i++)
      {
        if (Strings[i].Equals(((TXT)rr).Strings))
          return false;
      }
      return true;
    }

    #endregion

    //#region static

    //private static string GetEscapedString(Datagram data)
    //{
    //  StringBuilder sb = new StringBuilder();
    //  sb.Append('"');
    //  foreach (byte v in data)
    //  {
    //    if (v == '\\') // escape backslash
    //      sb.Append("\\\\");
    //    else if (v == '"') // escape double quote
    //      sb.Append("\\\"");
    //    else if (v >= 0x20 && v <= 0x7A) // printable ASCII
    //      sb.Append((char)v);
    //    else // display any other value as a perl-like escape sequence
    //      sb.Append(String.Format("\\x{0:x2}", (int)v));
    //  }
    //  sb.Append('"');
    //  return sb.ToString();
    //}

    //#endregion

  }
}

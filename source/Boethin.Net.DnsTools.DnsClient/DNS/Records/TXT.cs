/*
 * File: Boethin.Net.DnsTools.DnsClient/DNS/Records/TXT.cs
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

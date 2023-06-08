/*
 * File: Boethin.Net.DnsTools.DnsClient/DNS/RRBase.cs
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

namespace Boethin.Net.DnsTools.DnsClient.DNS
{

  /// <summary>
  /// The basic data common to all resource records.
  /// </summary>
  [Serializable]
  public sealed class RRBase : Internal.IResponseReader
  {

    #region public get

    /// <summary>
    /// A domain name to which this resource record pertains.
    /// [RFC 1035]
    /// </summary>
    public string NAME { get; private set; }

    /// <summary>
    /// Two octets containing one of the RR type codes.  This
    /// field specifies the meaning of the data in the RDATA
    /// field.
    /// [RFC 1035]
    /// <para>The value is null if the type is not implemented.</para>
    /// </summary>
    public Nullable<QTYPE> TYPE { get; private set; }

    /// <summary>
    /// The numeric value of the TYPE field.
    /// </summary>
    public ushort TypeValue { get; private set; }

    /// <summary>
    /// The name of the TYPE field or it's numeric value if the type is not implemented.
    /// </summary>
    public string TypeString { get; private set; }

    /// <summary>
    /// Two octets which specify the class of the data in the
    /// RDATA field.
    /// [RFC 1035]
    /// </summary>
    public QCLASS CLASS { get; private set; }

    /// <summary>
    /// A 32 bit unsigned integer that specifies the time
    /// interval (in seconds) that the resource record may be
    /// cached before it should be discarded.  Zero values are
    /// interpreted to mean that the RR can only be used for the
    /// transaction in progress, and should not be cached.
    /// [RFC 1035]
    /// </summary>
    public uint TTL { get; private set; }

    // RDLENGTH        an unsigned 16 bit integer that specifies the length in
    //                 octets of the RDATA field.
    //
    // RDATA           a variable length string of octets that describes the
    //                 resource.  The format of this information varies
    //                 according to the TYPE and CLASS of the resource record.
    //                 For example, the if the TYPE is A and the CLASS is IN,
    //                 the RDATA field is a 4 octet ARPA Internet address.
    internal ushort RDLENGTH { get; private set; }

    #endregion

    #region c'tor

    internal RRBase()
    {
    }

    #endregion

    #region IResponseReader

    void Internal.IResponseReader.ReadResponse(Internal.ByteReader reader)
    {
      NAME = reader.ReadDomain();

      // TYPE may be not understood
      TypeValue = reader.ReadUIn16();
      if (Enum.IsDefined(typeof(QTYPE), TypeValue))
      {
        // known type
        TYPE = (QTYPE)TypeValue;
        TypeString = TYPE.ToString();
      }
      else
      {
        // unknown type
        TYPE = null;
        TypeString = String.Format("[{0}]", TypeValue.ToString());
      }
      CLASS = reader.ReadUIn16Enum<QCLASS>();
      TTL = reader.ReadUint32();
      RDLENGTH = reader.ReadUIn16();
    }

    #endregion

    #region override

    public override bool Equals(object obj)
    {
      return Equals(obj as RRBase);
    }

    public bool Equals(RRBase rrbase)
    {
      if (object.ReferenceEquals(null, rrbase))
        return false;
      return
        NAME.Equals(rrbase.NAME) &&
        TypeValue.Equals(rrbase.TypeValue) &&
        CLASS.Equals(rrbase.CLASS) &&
        TTL.Equals(rrbase.TTL);
    }

    public override int GetHashCode()
    {
      // ignore CLASS since it's always IN
      return NAME.GetHashCode() ^ TypeValue;
    }

    public override string ToString()
    {
      return ToString(0);
    }

    public string ToString(int namePadding)
    {
      return String.Join(" ", new string[] { 
        NAME.PadLeft(namePadding), 
        TTL.ToString(), 
        CLASS.ToString(), 
        TypeString
      });
    }


    #endregion

  }
}

/*
 * File: Boethin.Net.DnsTools.DnsClient/DNS/Header.cs
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

  /// <summary>
  /// DNS message header.
  /// </summary>
  [Serializable] 
  public class Header : Internal.IRequestWriter, Internal.IResponseReader
  {

    // The header contains the following fields:
    //
    //                                     1  1  1  1  1  1
    //       0  1  2  3  4  5  6  7  8  9  0  1  2  3  4  5
    //     +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    //     |                      ID                       |
    //     +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    //     |QR|   Opcode  |AA|TC|RD|RA|   Z    |   RCODE   |
    //     +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    //     |                    QDCOUNT                    |
    //     +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    //     |                    ANCOUNT                    |
    //     +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    //     |                    NSCOUNT                    |
    //     +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    //     |                    ARCOUNT                    |
    //     +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+


    /// <summary>
    /// A 16 bit identifier assigned by the program that
    /// generates any kind of query. This identifier is copied
    /// the corresponding reply and can be used by the requester
    /// to match up replies to outstanding queries.
    /// </summary>
    public UInt16 ID { get; internal set; }

    /// <summary>
    /// A one bit field that specifies whether this message is a
    /// query (0), or a response (1).
    /// </summary>
    public QR QR { get; internal set; }

    /// <summary>
    /// A four bit field that specifies kind of query in this
    /// message.  This value is set by the originator of a query
    /// and copied into the response.
    /// </summary>
    public OPCODE OPCODE { get; internal set; }

    /// <summary>
    /// Authoritative Answer - this bit is valid in responses,
    /// and specifies that the responding name server is an
    /// authority for the domain name in question section.
    /// </summary>
    public bool AA { get; internal set; }

    /// <summary>
    /// Truncation - specifies that this message was truncated
    /// due to length greater than that permitted on the
    /// transmission channel.
    /// </summary>
    public bool TC { get; internal set; }

    /// <summary>
    /// Recursion Desired - this bit may be set in a query and
    /// is copied into the response.  If RD is set, it directs
    /// the name server to pursue the query recursively.
    /// </summary>
    public bool RD { get; internal set; }

    /// <summary>
    /// Recursion Available - this be is set or cleared in a
    /// response, and denotes whether recursive query support is
    /// available in the name server.
    /// </summary>
    public bool RA { get; internal set; }

    /// <summary>
    /// Response code - this 4 bit field is set as part of
    /// responses.
    /// </summary>
    public RCODE RCODE { get; internal set; }

    public byte RCODEValue { get; internal set; }

    /// <summary>
    /// an unsigned 16 bit integer specifying the number of
    /// entries in the question section.
    /// </summary>
    public UInt16 QDCOUNT { get; internal set; }

    /// <summary>
    /// an unsigned 16 bit integer specifying the number of
    /// resource records in the answer section.
    /// </summary>
    public UInt16 ANCOUNT { get; internal set; }

    /// <summary>
    /// an unsigned 16 bit integer specifying the number of name
    /// server resource records in the authority records
    /// section.
    /// </summary>
    public UInt16 NSCOUNT { get; internal set; }

    /// <summary>
    /// an unsigned 16 bit integer specifying the number of
    /// resource records in the additional records section.
    /// </summary>
    public UInt16 ARCOUNT { get; internal set; }


    #region IDataObject

    void Internal.IRequestWriter.WriteRequest(Internal.ByteWriter writer)
    {
      writer.WriteUInt16(ID);
      writer.WriteRequestFlags(OPCODE, RD);
      writer.WriteUInt16(QDCOUNT);
      writer.WriteUInt16(0); // ANCOUNT
      writer.WriteUInt16(0); // NSCOUNT
      writer.WriteUInt16(0); // ARCOUNT
    }

    void Internal.IResponseReader.ReadResponse(Internal.ByteReader reader)
    {
      ID = reader.ReadUIn16();
      ReadResponseFlags(reader.ReadUIn16());
      QDCOUNT = reader.ReadUIn16();
      ANCOUNT = reader.ReadUIn16();
      NSCOUNT = reader.ReadUIn16();
      ARCOUNT = reader.ReadUIn16();
    }

    #endregion

    public override string ToString()
    {
      switch (QR)
      {
        case DNS.QR.Q:
          return string.Format("Request: {0}", 
            OPCODE.ToString()
            );

        case DNS.QR.R:
          return string.Format("{0}: {1} RD={2} RA={3} {4} {5}", 
            (AA ? "Authoritative Response" : "Non-Authoritative Response"),
            OPCODE.ToString(),
            (RD ? "1" : "0"),
            (RA ? "1" : "0"),
            RCODE.ToString(),
            (TC ? "TRUNCATED " : "")
            );

      }
      // throw new InvalidOperationException("Unexpected QR value.");
      // CA1065	Do not raise exceptions in unexpected locations	
      return null;
    }

    #region private

    private void ReadResponseFlags(ushort flags)
    {

      int qr = flags & 0x8000;       // 1000000000000000
      if (qr == 0)
        throw new InvalidOperationException(
          "Zero QR flag in response header.");
      QR = DNS.QR.R;

      int opcode = (flags & 0x7800) >> 10;   //  111100000000000
      if (!Enum.IsDefined(typeof(DNS.OPCODE), (byte)opcode))
        throw new InvalidOperationException(String.Format(
          "Unexpected OPCODE {0} in response header.", opcode));
      OPCODE = (DNS.OPCODE)opcode;

      int aa = flags & 0x400;                //      10000000000
      AA = (aa != 0);

      int tc = flags & 0x200;         //       1000000000
      TC = (tc != 0);

      int rd = flags & 0x100;         //        100000000
      RD = (rd != 0);

      int ra = flags & 0x80;         //         10000000
      RA = (ra != 0);

      int rcode = flags & 0xF;      //             1111
      if (rcode > (int)DNS.RCODE.Other)
        rcode = (int)DNS.RCODE.Other;
      RCODE = (DNS.RCODE)rcode;
      RCODEValue = (byte)rcode;

    }

    #endregion


  }
}

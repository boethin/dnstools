/*
 * File: Boethin.Net.DnsTools.DnsClient/Response.cs
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
using System.IO;

namespace Boethin.Net.DnsTools.DnsClient
{

  /// <summary>
  /// A name server response.
  /// </summary>
  [Serializable] 
  public class Response : DNS.Message, Internal.IResponseReader
  {

    #region private

    // This field is actually readonly but cannot be declared so, because 
    // otherwise deserialization fails:
    //   "Field in TypedReferences cannot be static or init only."
    //private readonly Datagram _Data;
    private Datagram _Data;

    #endregion

    #region public get

    public override Datagram Data
    {
      get { return _Data; }
    }

    #endregion

    #region c'tor

    private Response(Datagram data, DateTime timestamp)
    {
      _Data = data;
      Timestamp = timestamp;
    }

    #endregion


    #region IResponseReader

    void Internal.IResponseReader.ReadResponse(Internal.ByteReader reader)
    {

      // header
      ((Internal.IResponseReader)Header).ReadResponse(reader);

      // question section
      for (int i = 0; i < Header.QDCOUNT; i++)
      {
        DNS.Question q = new DNS.Question();
        ((Internal.IResponseReader)q).ReadResponse(reader);
        Questions.Add(q);
      }

      // answer section
      ReadRecords(reader, AnswerRecordsList, Header.ANCOUNT);

      // authority records section.
      ReadRecords(reader, AuthorityRecordsList, Header.NSCOUNT);

      // additional records section
      ReadRecords(reader, AdditionalRecordsList, Header.ARCOUNT);

    }

    #endregion


    public override string ToString()
    {
      return string.Format("{0}: {1} RD={2} RA={3} {4} {5}",
        (Header.AA ? "Authoritative Response" : "Non-Authoritative Response"),
        Header.OPCODE.ToString(),
        (Header.RD ? "1" : "0"),
        (Header.RA ? "1" : "0"),
        Header.RCODE.ToString(),
        (Header.TC ? "TRUNCATED " : "")
        );
    }


    internal static Response Create(Datagram data, DateTime timestamp)
    {
      Response response = new Response(data, timestamp);
      using (MemoryStream ms = new MemoryStream())
      {
        ms.Write(data, 0, data.Length);
        ms.Seek(0L, SeekOrigin.Begin);
        ((Internal.IResponseReader)response).ReadResponse(new Internal.ByteReader(ms));
      }
      return response;
    }

    #region private static

    private static void ReadRecords(Internal.ByteReader reader, IList<DNS.RR> records, int count)
    {
      for (int i = 0; i < count; i++)
      {
        DNS.RRBase rrbase = new DNS.RRBase();
        ((Internal.IResponseReader)rrbase).ReadResponse(reader);
        DNS.RR rr = DNS.RR.CreateInstance(rrbase);
        rr.ReadRDATA(reader);
        records.Add(rr);
      }
    }

    #endregion

  }
}

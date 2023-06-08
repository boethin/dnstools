/*
 * File: Boethin.Net.DnsTools.DnsClient/Response.cs
 *
 * This file is part of Boethin.Net.DnsTools, a DNS debugging library.
 * Copyright (C) 2013 Sebastian Boethin <boethin@xn--domain.net>
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

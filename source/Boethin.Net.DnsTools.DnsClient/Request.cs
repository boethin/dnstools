/*
 * File: Boethin.Net.DnsTools.DnsClient/Request.cs
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
using System.IO;
using System.Linq;

namespace Boethin.Net.DnsTools.DnsClient
{

  [Serializable] 
  public class Request : DNS.Message, Internal.IRequestWriter
  {

    #region private

    private Nullable<Datagram> _Data = null;

    #endregion

    #region public get

    public override Datagram Data
    {
      get
      {
        if (_Data == null)
          _Data = CreateData();
        return (Datagram)_Data;
      }
    }

    #endregion

    #region c'tor

    /// <summary>
    /// Create a new DNS request.
    /// </summary>
    /// <param name="id">A 16 bit identifier assigned by the program that generates any kind of query.</param>
    /// <param name="rd">Recursion Desired - If RD is set, it directs the name server to pursue the query recursively.</param>
    /// <param name="opcode">A four bit field that specifies kind of query in this message.</param>
    /// <param name="question"></param>
    public Request(ushort id, bool rd, DNS.OPCODE opcode, DNS.Question question)
    {
      if (Object.ReferenceEquals(null, question))
        throw new ArgumentNullException("question");

      Header.ID = id;
      Header.QR = DNS.QR.Q;
      Header.RD = rd;
      Header.OPCODE = opcode;
      Questions.Add(question);
    }

    /// <summary>
    /// Create a new QUERY DNS request.
    /// </summary>
    /// <param name="rd">Recursion Desired - If RD is set, it directs the name server to pursue the query recursively.</param>
    /// <param name="question">The DNS question.</param>
    public Request(bool rd, DNS.Question question)
    {
      if (Object.ReferenceEquals(null, question))
        throw new ArgumentNullException("question");

      Header.ID = (ushort)System.DateTime.Now.Ticks;
      Header.QR = DNS.QR.Q;
      Header.RD = rd;
      Header.OPCODE = DNS.OPCODE.QUERY;
      Questions.Add(question);
    }

    #endregion

    #region IRequestWriter

    void Internal.IRequestWriter.WriteRequest(Internal.ByteWriter writer)
    {
      // header
      if (Questions.Count > (int)ushort.MaxValue)
        throw new InvalidOperationException("Too many questions.");
      Header.QDCOUNT = (ushort)Questions.Count;
      ((Internal.IRequestWriter)Header).WriteRequest(writer);

      // question section
      foreach (Internal.IRequestWriter q in Questions.Select(q => (Internal.IRequestWriter)q))
      {
        q.WriteRequest(writer);
      }
    }

    #endregion

    #region override

    public override string ToString()
    {
      return string.Format("{0} RD={1}: {2}",
        Header.OPCODE.ToString(), (Header.RD ? "1" : "0"), Questions.First().ToString());
    }

    #endregion

    #region private

    private Datagram CreateData()
    {
      // create message datgram
      using (MemoryStream ms = new MemoryStream())
      {
        ((Internal.IRequestWriter)this).WriteRequest(new Internal.ByteWriter(ms));
        Datagram data = new Datagram((int)ms.Length);
        ms.Seek(0L, SeekOrigin.Begin);
        if (ms.Read(data, 0, data.Length) < data.Length)
          throw new EndOfStreamException("Cannot read from memory stream.");
        return data;
      }
    }

    #endregion

  }
}

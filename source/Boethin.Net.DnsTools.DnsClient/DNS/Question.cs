/*
 * File: Boethin.Net.DnsTools.DnsClient/DNS/Question.cs
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
  /// The question section is used to carry the "question" in most queries,
  /// i.e., the parameters that define what is being asked.  The section
  /// contains QDCOUNT (usually 1) entries, each of the following format:
  /// </summary>
  [Serializable]
  public class Question : Internal.IRequestWriter, Internal.IResponseReader
  {

    // 4.1.2. Question section format
    //
    //                                     1  1  1  1  1  1
    //       0  1  2  3  4  5  6  7  8  9  0  1  2  3  4  5
    //     +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    //     |                                               |
    //     /                     QNAME                     /
    //     /                                               /
    //     +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    //     |                     QTYPE                     |
    //     +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    //     |                     QCLASS                    |
    //     +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    //

    /// <summary>
    /// a domain name represented as a sequence of labels
    /// </summary>
    public string QNAME { get; set; }

    /// <summary>
    /// a two octet code which specifies the type of the query.
    /// </summary>
    public QTYPE QTYPE { get; set; }

    /// <summary>
    /// a two octet code that specifies the class of the query.
    /// </summary>
    public QCLASS QCLASS { get; set; }


    #region c'tor

    /// <summary>
    /// Initialize a new DNS question.
    /// </summary>
    /// <param name="qname">A domain name represented as a sequence of labels.</param>
    /// <param name="qtype">A two octet code which specifies the type of the query.</param>
    /// <param name="qclass">A two octet code that specifies the class of the query.</param>
    public Question(string qname, QTYPE qtype, QCLASS qclass)
    {
      if (String.IsNullOrEmpty(qname))
        throw new ArgumentNullException("qname", "Value cannot be null or empty.");

      this.QNAME = qname;
      this.QTYPE = qtype;
      this.QCLASS = qclass;
    }

    /// <summary>
    /// Initialize a new DNS question using QCLASS IN.
    /// </summary>
    /// <param name="qname">A domain name represented as a sequence of labels.</param>
    /// <param name="qtype">A two octet code which specifies the type of the query.</param>
    public Question(string qname, QTYPE qtype)
    {
      if (String.IsNullOrEmpty(qname))
        throw new ArgumentNullException("qname", "Value cannot be null or empty.");

      this.QNAME = qname;
      this.QTYPE = qtype;
      this.QCLASS = DNS.QCLASS.IN;
    }

    internal Question()
    { 
    }
    
    #endregion



    #region IDataHandler

    void Internal.IRequestWriter.WriteRequest(Internal.ByteWriter writer)
    {
      writer.WriteDomain(QNAME);
      writer.WriteUInt16Enum<QTYPE>(QTYPE);
      writer.WriteUInt16Enum<QCLASS>(QCLASS);
    }

    void Internal.IResponseReader.ReadResponse(Internal.ByteReader reader)
    {
      QNAME = reader.ReadDomain();
      QTYPE = reader.ReadUIn16Enum<QTYPE>();
      QCLASS = reader.ReadUIn16Enum<QCLASS>();
    }

    #endregion

    public override string ToString()
    {
      return String.Format("{0} {1} {2}", QNAME, QCLASS.ToString(), QTYPE.ToString());
    }


  }
}

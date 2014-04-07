/*
 * File: Boethin.Net.DnsTools.DnsClient/DNS/Question.cs
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

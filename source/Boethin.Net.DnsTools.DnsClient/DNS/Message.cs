/*
 * File: Boethin.Net.DnsTools.DnsClient/DNS/Message.cs
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

namespace Boethin.Net.DnsTools.DnsClient.DNS
{

  [Serializable]
  public abstract class Message
  {

    #region private

    private readonly Header _Header = new Header();

    private readonly IList<Question> _Questions = new List<Question>();

    // resource records in the answer section
    private readonly IList<DNS.RR> _AnswerRecords = new List<DNS.RR>();

    // name server resource records in the authority records section.
    private readonly IList<DNS.RR> _AuthorityRecords = new List<DNS.RR>();

    private readonly IList<DNS.RR> _AdditionalRecords = new List<DNS.RR>();

    #endregion

    #region public get

    /// <summary>
    /// The message timestamp. Calculate the difference between request and response 
    /// timestamp to get the time ellapsed during the lookup transaction.
    /// <para>The value is initialized when the message is sent or received.</para>
    /// </summary>
    public DateTime Timestamp { get; internal set; }

    /// <summary>
    /// The raw message byte data.
    /// <para>The value is initialized when the message is sent or received.</para>
    /// </summary>
    public abstract Datagram Data { get; }

    /// <summary>
    /// The message header.
    /// </summary>
    public Header Header
    {
      get { return _Header; }
    }

    /// <summary>
    /// Carries the query name and other query parameters.
    /// <para>Although [RFC 1035] states a list of questions, in practice there is 
    /// always only one question per request.</para>
    /// </summary>
    public IList<Question> Questions
    {
      get { return _Questions; }
    }

    /// <summary>
    /// Carries RRs which directly answer the query.
    /// </summary>
    public IEnumerable<DNS.RR> AnswerRecords
    {
      get { return _AnswerRecords.AsEnumerable(); }
    }

    protected IList<DNS.RR> AnswerRecordsList
    {
      get { return _AnswerRecords; }
    }

    /// <summary>
    /// Carries RRs which describe other authoritative servers.
    /// May optionally carry the SOA RR for the authoritative
    /// data in the answer section.
    /// </summary>
    public IEnumerable<DNS.RR> AuthorityRecords
    {
      get { return _AuthorityRecords.AsEnumerable(); }
    }

    protected IList<DNS.RR> AuthorityRecordsList
    {
      get { return _AuthorityRecords; }
    }

    /// <summary>
    /// Carries RRs which may be helpful in using the RRs in the
    /// other sections.
    /// </summary>
    public IEnumerable<DNS.RR> AdditionalRecords
    {
      get { return _AdditionalRecords.AsEnumerable(); }
    }

    protected IList<DNS.RR> AdditionalRecordsList
    {
      get { return _AdditionalRecords; }
    }

    #endregion

    #region internal

    internal void SetTimestamp()
    {
      Timestamp = DateTime.Now;
    }

    #endregion

  }
}

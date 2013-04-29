/*
 * File: Boethin.Net.DnsTools.DnsClient/DNS/Message.cs
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

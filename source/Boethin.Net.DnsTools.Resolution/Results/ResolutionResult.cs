/*
 * File: Boethin.Net.DnsTools.Resolution/Results/ResolutionResult.cs
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
using System.Linq;
using System.Collections.Generic;

namespace Boethin.Net.DnsTools.Resolution.Results
{

  /// <summary>
  /// The result of a name server request, consisting of the server's response and 
  /// the set of name servers responsible for the response.
  /// </summary>
  [Serializable]
  public class ResolutionResult
  {

    #region private

    private readonly NameServerCollection _Authorities;

    #endregion

    #region public get

    // TODO
    public readonly int NestingLevel;


    /// <summary>
    /// The request that has been sent to the name server.
    /// </summary>
    public readonly DnsClient.Request Request;

    /// <summary>
    /// The actual name server response.
    /// </summary>
    public readonly DnsClient.Response Response;

    /// <summary>
    /// The set of name servers where the current server has been chosen from.
    /// The current server is the selected item in the collection.
    /// </summary>
    public NameServerCollection Authorities
    {
      get { return _Authorities; }
    }

    /// <summary>
    /// Authories for the next step. If not null, one of them may be given to the MoveNext method.
    /// </summary>
    public NameServerCollection NextAuthorities { get; internal set; }

    public QueryState QueryState { get; internal set; }

    /// <summary>
    /// How long it took to get the server's response.
    /// </summary>
    public TimeSpan Duration
    {
      get
      {
        return Response.Timestamp.Subtract(Request.Timestamp);
      }
    }

    #endregion

    #region LogMessage

    internal readonly IList<Logging.LogMessage> PreLogMessageList = new List<Logging.LogMessage>();

    public IEnumerable<Logging.LogMessage> PreLogMessages
    {
      get { return PreLogMessageList.AsEnumerable(); }
    }

    internal readonly IList<Logging.LogMessage> PostLogMessageList = new List<Logging.LogMessage>();

    public IEnumerable<Logging.LogMessage> PostLogMessages
    {
      get { return PostLogMessageList.AsEnumerable(); }
    }

    #endregion

    #region c'tor

    internal ResolutionResult(int nestingLevel, DnsClient.Request request, DnsClient.Response response, NameServerCollection authorities)
    {
      this.NestingLevel = nestingLevel;
      this.Request = request;
      this.Response = response;
      _Authorities = authorities;
    }

    #endregion

  }
}

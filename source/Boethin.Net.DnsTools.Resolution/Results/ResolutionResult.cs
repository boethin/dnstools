/*
 * File: Boethin.Net.DnsTools.Resolution/Results/ResolutionResult.cs
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

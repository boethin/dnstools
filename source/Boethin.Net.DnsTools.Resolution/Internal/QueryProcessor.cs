/*
 * File: Boethin.Net.DnsTools.Resolution/Internal/QueryProcessor.cs
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
using Boethin.Net.DnsTools.Resolution.Extensions;

namespace Boethin.Net.DnsTools.Resolution.Internal
{
  internal class QueryProcessor : Logging.ILogMessageCreator
  {

    #region public readonly

    public readonly DnsClient.Response Response;

    public readonly DnsDomain SNAME;

    public readonly DnsClient.DNS.QTYPE STYPE;

    #endregion

    public event DnsClient.Logging.LogMessageEventHandler LogMessageCreated;

    #region c'tor

    public QueryProcessor(DnsDomain sname, DnsClient.DNS.QTYPE stype, DnsClient.Response response)
    {
      this.SNAME = sname;
      this.STYPE = stype;
      this.Response = response;
    }

    #endregion

    #region public

    public QueryResult Process()
    {

      // [RFC 1034], 5.3.3. Algorithm
      //
      // 4. Analyze the response, either:
      //
      //   a. if the response answers the question or contains a name
      //     error, cache the data as well as returning it back to
      //     the client.
      //
      //   b. if the response contains a better delegation to other
      //     servers, cache the delegation information, and go to
      //     step 2.
      //
      //   c. if the response shows a CNAME and that is not the
      //     answer itself, cache the CNAME, change the SNAME to the
      //     canonical name in the CNAME RR and go to step 1.
      //
      //   d. if the response shows a servers failure or other
      //     bizarre contents, delete the server from the SLIST and
      //     go back to step 3.
      //
      // Step 4 involves analyzing responses.  The resolver should be highly
      // paranoid in its parsing of responses.



      // Verify the question section matches the query.
      if (Response.Questions.Count == 1)
      {
        DnsClient.DNS.Question q = Response.Questions.First();
        if (!this.SNAME.Equals(q.QNAME) || this.STYPE != q.QTYPE)
          throw new QueryException(QueryState.MissingQuestion);
      }
      else
      {
        throw new QueryException(QueryState.MissingQuestion);
      }

      // Check for unhandled RCODE.
      if (!(Response.Header.RCODE == DnsClient.DNS.RCODE.NoError ||
        Response.Header.RCODE == DnsClient.DNS.RCODE.NameError))
      {
        return new QueryResult { State = QueryState.UnexpectedRCode };
      }

      // RCODE.NoError or RCODE.NameError

      if (!Response.Header.AA)
      {
        // Non-Authoritative Answer

        NameServerCollection nextAuthorities = FindAuthorities(SNAME);
        if (nextAuthorities.Any())
        {
          // Referral response: authorities is not empty.

          // Check for relevant answer RRs matching STYPE within additional RRs
          IEnumerable<DnsClient.DNS.RR> additional = Response.AdditionalRecords.Where(
            rr => SNAME.Equals(rr.Base.NAME)).OfQuestion(STYPE);
          if (additional.Any())
          {
            // Found answer RRs matching the query.
            return new QueryResult
            {
              Answers = additional,
              State = QueryState.NonAuthoritativeAnswer
            };
          }

          return new QueryResult
          {
            NextAuthorities = nextAuthorities,
            State = QueryState.NextAuthorities
          };
        }

        return new QueryResult { State = QueryState.MissingAuthorities };
      }

      // Authoritative Answer

      // Process CNAMEs unless the question is CNAME.
      // Even on RCODE.NameError CNAMEs need to be applied
      // (cf.: [RFC 2308], 2.1 - Name Error).
      bool cnamed = false;
      DnsDomain canonical = SNAME;
      if (STYPE != DnsClient.DNS.QTYPE.CNAME)
      {
        // resolve internal CNAME chain
        cnamed = FindCanonical(ref canonical);
      }

      if (Response.Header.RCODE == DnsClient.DNS.RCODE.NameError)
      {
        // RCODE.NameError

        // [RFC 2308], 2.1 - Name Error
        //
        // Name errors (NXDOMAIN) are indicated by the presence of "Name Error"
        // in the RCODE field.  In this case the domain referred to by the QNAME
        // does not exist.  Note: the answer section may have SIG and CNAME RRs
        // and the authority section may have SOA, NXT [RFC2065] and SIG RRsets.
        //
        // It is possible to distinguish between a referral and a NXDOMAIN
        // response by the presense of NXDOMAIN in the RCODE regardless of the
        // presence of NS or SOA records in the authority section.

        return new QueryResult
        {
          CanonicalName = cnamed ? canonical : null,
          SOA = Response.AuthorityRecords.FirstOrDefault<
            DnsClient.DNS.Records.SOA>(),
          State = QueryState.NxDomain
        };
      }

      // Authoritative RCODE.NoError

      // Check for relevant answer RRs matching the canonical name and STYPE.
      IEnumerable<DnsClient.DNS.RR> answers = Response.AnswerRecords.Where(
        rr => canonical.Equals(rr.Base.NAME)).OfQuestion(STYPE);
      if (answers.Any())
      {
        // Found answer RRs matching the query.
        return new QueryResult
        {
          CanonicalName = cnamed ? canonical : null,
          Answers = answers,
          State = QueryState.AuthoritativeAnswer
        };
      }

      // NoData or referral CNAME response

      // [RFC 2308], 2.2 - No Data
      //
      // It is possible to distinguish between a NODATA and a referral
      // response by the presence of a SOA record in the authority section or
      // the absence of NS records in the authority section.

      DnsClient.DNS.Records.SOA soa = Response.AuthorityRecords.FirstOrDefault<
        DnsClient.DNS.Records.SOA>();
      NameServerCollection authorities = FindAuthorities(canonical);
      if (!object.ReferenceEquals(null, soa) || authorities.Any())
      {
        // NODATA
        // Do not treat as referral (cf.: [RFC 2308], 2.2 - No Data)
        return new QueryResult
        {
          SOA = soa,
          CanonicalName = cnamed ? canonical : null,
          State = QueryState.NoData
        };
      }

      // Referral response: authorities is not empty.

      if (cnamed)
      {
        // referral CNAME
        return new QueryResult
        {
          CanonicalName = canonical,
          NextAuthorities = FindAuthorities(canonical),
          State = QueryState.FollowCName
        };
      }

      return new QueryResult { State = QueryState.EmptyResponse };
    }

    #endregion

    #region private

    private NameServerCollection FindAuthorities(DnsDomain sname)
    {
      // NS RRs from Authority Section, resolved by Additional address RRs.
      return NameServerCollection.Find(
        sname, Response.AuthorityRecords, Response.AdditionalRecords);
    }

    private bool FindCanonical(ref DnsDomain sname)
    {
      bool mapped = false;

      // Resolve CNAME chains within the zone itself
      DnsClient.DNS.Records.CNAME cname;
      List<DnsDomain> loopctrl = new List<DnsDomain>();
      while (FindCanonical(sname, out cname))
      {
        // name is an alias.
        if (sname.Equals(cname.CANONICAL))
        {
          // SERVER_ERROR
          // CName pointing to itself.
          throw new QueryException(QueryState.CNameSelf, sname);
        }
        if (loopctrl.Contains(sname))
        {
          // SERVER_ERROR
          // Loop detected in local CNAME chain.
          throw new QueryException(QueryState.LocalCNameLoop, sname);
        }

        DnsDomain canonical = (DnsDomain)cname.CANONICAL;
        LogMessageCreate(Logging.LogMessageText.RESPONSE.WARN.CNameFound(sname, canonical));
        sname = canonical;
        loopctrl.Add((DnsDomain)cname.CANONICAL);
        mapped = true;
      }
      return mapped;
    }

    private bool FindCanonical(DnsDomain sname, out DnsClient.DNS.Records.CNAME cname)
    {
      IEnumerable<DnsClient.DNS.Records.CNAME> cnames = Response.AnswerRecords.Select<
        DnsClient.DNS.Records.CNAME>().Where(cn => sname.Equals(cn.Base.NAME));
      switch (cnames.Count())
      {
        case 0:
          cname = null;
          return false;
        case 1:
          cname = cnames.First();
          return true;
        default:
          // Ambigious CNAME records
          throw new QueryException(QueryState.AmbigiousCNAME, sname);
      }
    }

    private void LogMessageCreate(Logging.LogMessage message)
    {
      if (LogMessageCreated != null)
      {
        LogMessageCreated(this, new DnsClient.Logging.LogMessageEventArgs(message));
      }
    }


    #endregion

  }
}

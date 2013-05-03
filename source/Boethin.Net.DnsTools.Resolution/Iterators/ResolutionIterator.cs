/*
 * File: Boethin.Net.DnsTools.Resolution/Iterators/ResolutionIterator.cs
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
using System.Net;
using System.Reflection;

namespace Boethin.Net.DnsTools.Resolution.Iterators
{

  [Serializable]
  public class ResolutionIterator : ResolutionIteratorBase, Logging.ILogMessageCreator
  {

    #region private

    private Results.ResolutionResult LastAddressResult = null;

    private DnsClient.DNS.Records.Address[] ResolvedAddresses = null;

    private AddressIterator AddressIterator = null;

    private CNameIterator CNameIterator = null;

    private bool RootCacheState = true;

    //private bool TCRepeating = false;

    //private readonly bool IsSubIterator;

    #endregion

    internal readonly int NestingLevel;

    #region event LogMessageCreated

    public event DnsClient.Logging.LogMessageEventHandler LogMessageCreated;

    #endregion

    #region c'tor

    internal ResolutionIterator(Resolver resolver, DnsClient.DNS.QTYPE question, Caching.AddressCache addressCache)
      : base(resolver, question, addressCache)
    {
      NestingLevel = 0;
    }

    // sub iterators: Address / CName
    protected ResolutionIterator(Resolver resolver, DnsClient.DNS.QTYPE question, Caching.AddressCache addressCache, int nestingLevel)
      : base(resolver, question, addressCache)
    {
      this.NestingLevel = nestingLevel;
    }



    //// private sub-iterator
    //private ResolutionIterator(ResolutionIterator iterator, DnsDomain domain, DnsClient.DNS.QTYPE question)
    //  : base(new Resolver(iterator.Resolver, domain), question, iterator.AddressCache)
    //{
    //  IsSubIterator = true;
    //}

    #endregion

    #region override

    public override void Reset()
    {
      base.Reset();
      LastAddressResult = null;
      AddressIterator = null;
      CNameIterator = null;
      //TCRepeating = false;
      RootCacheState = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        try
        {
          if (AddressIterator != null)
            ((IDisposable)AddressIterator).Dispose();
          if (CNameIterator != null)
            ((IDisposable)CNameIterator).Dispose();
        }
        finally
        {
          AddressIterator = null;
          CNameIterator = null;
        }
      }
      base.Dispose(disposing);
    }

    protected override Results.ResolutionResult GetNextResult(out bool isCompleted, NameServer selected)
    {

      // Iteration may be delegated to CNameIterator if the requested name
      // has been identified as alias.
      if (!object.ReferenceEquals(null, CNameIterator))
      {
        CNameIterator.EnsureConnection(Resolver.Client);
        return CNameIterator.GetNextResult(out isCompleted, selected);
      }

      // A subsequent iteration may be running if a name server address needs to be resolved.
      if (!object.ReferenceEquals(null, AddressIterator))
      {
        AddressIterator.EnsureConnection(Resolver.Client);

        bool isAddressIteratorCompleted;
        Results.ResolutionResult addressResult = AddressIterator.GetNextResult(out isAddressIteratorCompleted, selected);
        if (isAddressIteratorCompleted)
        {
          // remember the last result from the subsequent iteration
          ResolvedAddresses = AddressIterator.GetAddresses(addressResult).ToArray();
          if (!ResolvedAddresses.Any())
          { 
            // Failed to resolve
            isCompleted = true;
          }
          
          LastAddressResult = addressResult;

          AddressIterator.Dispose();
          AddressIterator = null;
        }
        isCompleted = false;
        return addressResult;
      }

      // Apply address result from previous step.
      if (!object.ReferenceEquals(null, LastAddressResult))
      {
        // An address iteration has been completed in the previous step.
        try
        {
          // Apply the resulting address records to the selected authority.
          if (LastAddressResult.Response.Header.AA)
          { 
            // fetch addresses from authoritative respone
            IEnumerable<DnsClient.DNS.Records.Address> addresses = LastAddressResult.Response.AnswerRecords.OfType<
              DnsClient.DNS.Records.Address>().Where(a => StoredAuthorities.Selected.Name.Equals(a.Base.NAME));
            if (addresses.Any())
            {
              StoredAuthorities.Selected.ApplyAddresses(addresses);
              OnNameServerResolved(StoredAuthorities.Selected);
            }
          }
        }
        finally
        {
          LastAddressResult = null;
        }
      }

      else
      {
        // The standard case
        // Ensure StoredAuthorities with a resolved selected name server.

        if (object.ReferenceEquals(null, StoredAuthorities)) // initial state
        {
          StoredAuthorities = GetStartAuthorities();
          OnResolutionStart(StoredAuthorities);
        }
        else
        {
          OnResolutionContinue(StoredAuthorities);
        }

        // apply cached address records to the set of authorities
        StoredAuthorities.ApplyCache(AddressCache);

        // Select randomly any name server unless one was explicitly chosen.
        if (!object.ReferenceEquals(null, selected))
        {
          StoredAuthorities.SelectOne(selected);
        }
        else
        {
          selected = StoredAuthorities.SelectAny();
        }
        OnAuthoritySelected(StoredAuthorities);

        if (!selected.IsResolved)
        {
          // start a subsequent iteration to resolve the selected name server
          OnNameServerResolution(selected);
          AddressIterator = CreateAddressIterator(selected.Name);

          return GetNextResult(out isCompleted, null);
        }
      }


      // Perform the actual name server request.
      Results.ResolutionResult result = Request(StoredAuthorities, out isCompleted);

      try // Internal.QueryException
      {
        Internal.QueryProcessor query = new Internal.QueryProcessor(Resolver.Domain, Question, result.Response);
        Internal.QueryResult qr = query.Process();
        result.QueryState = qr.State;
        switch (qr.State)
        {

          case QueryState.NextAuthorities:
            //StoredAuthorities = GetResultAuthorities(result);
            isCompleted = false;
            OnNextAuthorities(StoredAuthorities.Selected, qr.NextAuthorities);

            StoredAuthorities = qr.NextAuthorities;
            result.NextAuthorities = StoredAuthorities;
           

            // "Received {1} '{0}' authorities from '{2}'."
            //LogMessageCreate(Logging.LogMessageText.RESPONSE.INFO.NextAuthoritiesReceived(  StoredAuthorities.Selected, result.NextAuthorities));
            break;

          case QueryState.FollowCName:
            // CNAME found.
            isCompleted = true;
            OnCNameFound(qr.CanonicalName);

            // Follow, if it's configured.
            if (Resolver.Options.FollowCanonical)
            {
              // Start a new iteration with the canonical name and the same question.
              // Name servers may or may not be given.
              CNameIterator = CreateCNameIterator(qr.CanonicalName, qr.NextAuthorities);
              isCompleted = false;
            }
            break;

          case QueryState.NonAuthoritativeAnswer:
            isCompleted = true;
            OnResolutionSucceeded(result.Response, qr.Answers);
            break;

          case QueryState.AuthoritativeAnswer:
            // At least one record of the requested type was found.
            isCompleted = true;
            OnResolutionSucceeded(result.Response, qr.Answers);
            break;

          case QueryState.NxDomain:
            isCompleted = true;
            LogMessageCreate(Logging.LogMessageText.RESPONSE.ERROR.NameNotExisting(qr.CanonicalName ?? Resolver.Domain));
            break;

          case QueryState.NoData:
          case QueryState.EmptyResponse:
            isCompleted = true;
            LogMessageCreate(Logging.LogMessageText.RESPONSE.ERROR.NegativeResponse(qr.State));
            break;

          case QueryState.UnexpectedRCode:
            isCompleted = true;
            OnRCodeNonZero(result.Response);
            break;

        }
      }
      catch (Internal.QueryException ex)
      {
        isCompleted = true;
        result.QueryState = ex.State;
        LogMessageCreate(Logging.LogMessageText.RESPONSE.ERROR.ResponseInvalid(ex.State));
      }


      return result;



      // =====================================================================

      #region hidden
      //// Dealing with non-Zero RCODE.
      //if (isCompleted && result.Response.Header.RCODE != DnsClient.DNS.RCODE.NoError)
      //{
      //  OnRCodeNonZero(result.Response);
      //  return result;
      //}

      //// cache response address records
      //AddressCache.Set(result.Response.AnswerRecords.OfType<DnsClient.DNS.Records.Address>());
      //if (!Resolver.Options.StrictAuhtoritative) // optionally cache also additional address records
      //{
      //  AddressCache.Set(result.Response.AdditionalRecords.Select<DnsClient.DNS.Records.Address>());
      //}

      //if (result.Response.Header.TC)
      //{
      //  // If the result was truncated, the request can be repeated once.
      //  if (Resolver.Options.RepeatTruncated && !TCRepeating)
      //  {
      //    TCRepeating = true;
      //    isCompleted = false;
      //    OnRepeatTruncated();
      //    return result;
      //  }
      //}
      //TCRepeating = false;

      //if (isCompleted)
      //{
      //  // There was an authoritative response.

      //  DnsClient.DNS.Records.SOA soa = result.Response.AnswerRecords.OfType<DnsClient.DNS.Records.SOA>().FirstOrDefault();


      //  // answer records matching the question
      //  IEnumerable<DnsClient.DNS.RR> requested = result.Response.AnswerRecords;
      //  if (Question != DnsClient.DNS.QTYPE.ANY)
      //  {
      //    // A particular record type has been queried (not ANY).
      //    requested = requested.OfQuestion(Question);
      //  }

      //  if (requested.Any())
      //  {
      //    // At least one record of the requested type was found.
      //    OnResolutionSucceeded(result.Response, requested);
      //  }


      //  // check for CNAME
      //  //
      //  // [RFC 1034], 5.2.2. Aliases
      //  //
      //  // In most cases a resolver simply restarts the query at the new name when
      //  // it encounters a CNAME.  However, when performing the general function,
      //  // the resolver should not pursue aliases when the CNAME RR matches the
      //  // query type.  This allows queries which ask whether an alias is present.
      //  // For example, if the query type is CNAME, the user is interested in the
      //  // CNAME RR itself, and not the RRs at the name it points to.
      //  DnsClient.DNS.Records.CNAME cname;
      //  if (!object.ReferenceEquals(null, (
      //    cname = result.Response.AnswerRecords.FirstOrDefault<DnsClient.DNS.Records.CNAME>())))
      //  {
      //    // CNAME found.
      //    //OnCNameFound(cname);

      //    if (Question != DnsClient.DNS.QTYPE.CNAME)
      //    {
      //      // A CNAME can lead to the zone itself.
      //      if (!object.ReferenceEquals(null, soa) &&
      //        DnsDomain.Equals(soa.Base.NAME, cname.CANONICAL))
      //      {

      //        // TODO


      //      }


      //    }



      //    // Follow, if it's configured and the question was not CNAME.
      //    if (!Question.Equals(DnsClient.DNS.QTYPE.CNAME) &&
      //      Resolver.Options.FollowCanonical)
      //    {
      //      // Start a new iteration with the canonical name and the same question.
      //      CNameIterator = CreateCNameIterator((DnsDomain)cname.CANONICAL);
      //      isCompleted = false;
      //    }
      //  }

      //  else
      //  {
      //    // no matching CNAME found
      //    if (!requested.Any())
      //    {
      //      // Neither an appropriate CNAME nor the requested RR type was found.
      //      OnMissingRecords(result.Response);
      //    }
      //  }



      //  if (NestingLevel == 0)
      //  {
      //    // "Resolution finished."
      //    LogMessageCreate(Logging.LogMessageText.RESPONSE.INFO.ResolutionFinished(IterationCount));
      //  }


      //}

      //else
      //{
      //  // The iteration is not yet completed: get authorities for the next step.
      //  StoredAuthorities = GetResultAuthorities(result);
      //  result.NextAuthorities = StoredAuthorities;
      //  if (StoredAuthorities.IsEmpty)
      //    throw new ResolverException(
      //      "No authority name servers were found although the respone is non-authoritative.");

      //  // "Received {1} '{0}' authorities from '{2}'."
      //  LogMessageCreate(Logging.LogMessageText.RESPONSE.INFO.NextAuthoritiesReceived(result.Authorities.Selected, result.NextAuthorities));
      //}

      //return result;
      #endregion

    }



    #endregion

    #region logging events

    protected virtual void OnResolutionStart(NameServerCollection authorities)
    {
      // "Starting resolution of '{0}' with a '{1}' authority name server."
      LogMessageCreate(Logging.LogMessageText.REQUEST.ResolutionStart(Resolver.Domain, authorities));
    }

    protected virtual void OnResolutionContinue(NameServerCollection authorities)
    {
      // "Continuing resolution of '{0}' with a '{1}' authority."
      LogMessageCreate(Logging.LogMessageText.REQUEST.INFO.ContinuingResultion(Resolver.Domain, authorities));
    }

    protected virtual void OnNextAuthorities(NameServer ns, NameServerCollection authorities)
    {
      foreach (IGrouping<int, NameServer> nsg in authorities.Groupings)
      {
        LogMessageCreate(Logging.LogMessageText.RESPONSE.INFO.NextAuthoritiesReceived(
          ns.Name, nsg.First().Zone, authorities));
      }
    }


    protected virtual void OnAuthoritySelected(NameServerCollection authorities)
    {
      // "Name server '{0}' has been selected."
      LogMessageCreate(Logging.LogMessageText.REQUEST.INFO.AuthoritySelected(authorities));
    }

    protected virtual void OnNameServerResolution(NameServer ns)
    {
      // "Name server '{0}' needs to be resolved first."
      LogMessageCreate(Logging.LogMessageText.REQUEST.INFO.ResolvingNameServer(ns));
    }

    protected virtual void OnNameServerResolved(NameServer ns)
    {
      // "Name server '{0}' has been resolved, continuing with '{1}'."
      LogMessageCreate(Logging.LogMessageText.REQUEST.INFO.NameServerResolved(ns, Resolver.Domain));
    }

    protected virtual void OnRepeatTruncated()
    {
      // "A truncated answer has been received, so the request needs to be repeated via TCP."
      LogMessageCreate(Logging.LogMessageText.RESPONSE.WARN.RepeatingTruncatedAnswer());
    }

    protected virtual void OnRCodeNonZero(DnsClient.Response response)
    {
      switch (response.Header.RCODE)
      {
        case DnsClient.DNS.RCODE.NameError:
          // "The name '{0}' does not exist."
          LogMessageCreate(Logging.LogMessageText.RESPONSE.ERROR.NameNotExisting(Resolver.Domain));
          break;

        case DnsClient.DNS.RCODE.NotImplemented:
          // "The name server does not support queries of type '{0}'."
          LogMessageCreate(Logging.LogMessageText.RESPONSE.ERROR.QueryNotImplemented(Question));
          break;

        case DnsClient.DNS.RCODE.Refused:
          // "The name server refuses to perform the specified operation for policy easons."
          LogMessageCreate(Logging.LogMessageText.RESPONSE.ERROR.ServerRefused());
          break;

        case DnsClient.DNS.RCODE.ServerFailure:
          // "The name server was unable to process this query due to a problem with the name server."
          LogMessageCreate(Logging.LogMessageText.RESPONSE.ERROR.ServerFailure());
          break;

        default:
          // "Unexpected RCODE value {0} '{1}' received."
          LogMessageCreate(Logging.LogMessageText.RESPONSE.ERROR.UnexpectedRCode(response.Header.RCODEValue, response.Header.RCODE));
          break;

      }
    }

    protected virtual void OnCNameFound(DnsDomain cname)
    {
      // "The name '{0}' is an alias for the canonical name '{1}'."
      LogMessageCreate(Logging.LogMessageText.RESPONSE.WARN.CNameFound(Resolver.Domain, cname));

      if (!Resolver.Options.FollowCanonical)
      {
        // "The resolver is not configured to follow the canonical name."
        LogMessageCreate(Logging.LogMessageText.RESPONSE.WARN.CNameNoFollow());
      }

    }

    protected virtual void OnResolutionSucceeded(DnsClient.Response response, IEnumerable<DnsClient.DNS.RR> matching)
    {
      foreach (DnsClient.DNS.RR rr in matching)
      {
        Logging.LogMessage message;

        // try to find logging method by QTYPE
        string methodName = String.Format(
          "ResourceRecordFound{0}", rr.Base.TYPE.ToString());
        MethodInfo method;
        if (!object.ReferenceEquals(null, (method = typeof(
          Logging.LogMessageText.RESPONSE.INFO).GetMethod(methodName,
          BindingFlags.Static | BindingFlags.Public))))
        {
          message = (Logging.LogMessage)method.Invoke(null, new object[] { response, rr });
        }
        else
        {
          message = Logging.LogMessageText.RESPONSE.INFO.ResourceRecordFound(response, rr);
        }

        LogMessageCreate(message);
      }
    }

    protected virtual void OnMissingRecords(DnsClient.Response response)
    {
      // "No Resource Record of type {0} was found."
      LogMessageCreate(Logging.LogMessageText.RESPONSE.ERROR.MissingRecords(Question));
    }

    //protected virtual void OnResolutionCompleted(DnsClient.DNS.QTYPE question, DnsClient.Response response)
    //{
    //  IEnumerable<DnsClient.DNS.RR> requested = response.AnswerRecords.OfQuestion(question);
    //  if (requested.Any())
    //  {
    //    // The requested record type was found.
    //    LogMessageCreate(Logging.LogMessageText.RESPONSE.ResolutionSucceded(Resolver.Domain, Question, requested));
    //  }

    //  else
    //  {
    //    // The requested record type was NOT found.
    //    if (question == DnsClient.DNS.QTYPE.ANY)
    //    {
    //      // The question is ANY, so the result is acceptable.
    //      LogMessageCreate(Logging.LogMessageText.RESPONSE.AnyResolutionSucceded(response.AnswerRecords));
    //    }

    //    else
    //    {
    //      // maybe CNAME
    //    }
    //  }
    //}

    #endregion

    #region private

    private Results.ResolutionResult Request(NameServerCollection authorities, out bool isCompleted)
    {
      // perform name server request
      DnsClient.Request request;
      IPAddress address = authorities.Selected.GetFirstAddress(Resolver.Options.IPVersion);

      DnsClient.Response response = Resolver.LookUp(address, Question, out request);
      Results.ResolutionResult result = new Results.ResolutionResult(this.NestingLevel, request, response, authorities);

      // "Request took {0}ms."
      LogMessageCreate(Logging.LogMessageText.RESPONSE.INFO.RequestDuration(result.Duration.TotalMilliseconds));

      if (response.Header.AA)
      {
        // authoritative response
        isCompleted = true;
      }
      else
      {
        // [RFC 1035]
        // Name Error - Meaningful only for responses from an authoritative name server,
        // this code signifies that the  domain name referenced in the query does not exist.

        // stop if the response was any other than NoError or NameError
        isCompleted = !(
          response.Header.RCODE == DnsClient.DNS.RCODE.NoError ||
          response.Header.RCODE == DnsClient.DNS.RCODE.NameError);
      }


      return result;
    }

    private NameServerCollection GetStartAuthorities()
    {
      NameServerCollection authorities = null;

      // initial state:
      // get cached top level name servers or root hints
      if (Resolver.Options.UseRootCache)
        authorities = RootServers.Cache.Get(Resolver.Domain);
      if (authorities == null)
      {
        // either RootCache disabled or not found in cache
        authorities = RootServers.RootHints;
        RootCacheState = false;
      }

      return authorities;
    }

    private NameServerCollection GetResultAuthorities(Results.ResolutionResult result)
    {
      // get authority records from existing result


      NameServerCollection authorities = NameServerCollection.Create(
        result.Response.AuthorityRecords, result.Response.AdditionalRecords);

      // save top level name server cache
      if (!RootCacheState)
      {
        RootServers.Cache.Set(authorities.ZoneName, authorities);
        RootCacheState = true;
      }

      return authorities;
    }

    // The connectivity state may have get lost, e.g. during serialization.
    private void EnsureConnection(DnsClient.IDnsClient client)
    {
      Resolver.Connect(client);
    }

    //private DnsClient.DNS.QTYPE GetAddressQuestion()
    //{
    //  switch (Resolver.Options.IPVersion)
    //  {
    //    case IPVersion.IPv4:
    //      return DnsClient.DNS.QTYPE.A;
    //    case IPVersion.IPv6:
    //      return DnsClient.DNS.QTYPE.AAAA;
    //    default:
    //      return DnsClient.DNS.QTYPE.ANY;
    //  }
    //}

    //private ResolutionIterator CreateSubIterator(DnsDomain domain, DnsClient.DNS.QTYPE question)
    //{
    //  ResolutionIterator iterator = new ResolutionIterator(this, domain, question);
    //  if (LogMessageCreated != null)
    //  {
    //    iterator.LogMessageCreated += new DnsClient.Logging.LogMessageEventHandler(OnSubIteratorLogMessage); 
    //  }
    //  return iterator;
    //}

    private AddressIterator CreateAddressIterator(DnsDomain domain)
    {
      AddressIterator iterator = new Iterators.AddressIterator(this, domain, Resolver.Options.IPVersion);
      if (LogMessageCreated != null)
      {
        iterator.LogMessageCreated += new DnsClient.Logging.LogMessageEventHandler(OnSubIteratorLogMessage);
      }
      return iterator;
    }

    private Iterators.CNameIterator CreateCNameIterator(DnsDomain cname, NameServerCollection authorities)
    {
      // TODO
      // apply authorties

      Iterators.CNameIterator iterator = new CNameIterator(this, (DnsDomain)cname);
      if (LogMessageCreated != null)
      {
        iterator.LogMessageCreated += new DnsClient.Logging.LogMessageEventHandler(OnSubIteratorLogMessage);
      }
      return iterator;
    }

    private void OnSubIteratorLogMessage(Object sender, DnsClient.Logging.LogMessageEventArgs e)
    {
      if (LogMessageCreated != null)
      {
        LogMessageCreated(sender, e);
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

/*
 * File: Boethin.Net.DnsTools.Resolution/Logging/LogMessageText.cs
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
using Boethin.Net.DnsTools.DnsClient.Logging;

namespace Boethin.Net.DnsTools.Resolution.Logging
{
  internal static class LogMessageText
  {

    public static class REQUEST
    {

      public static class INFO
      {

        public static LogMessage StartingResultion(DnsDomain name)
        {
          return Create(Resources.LogMessages.StartingResultionRoot, new object[] { name });
        }

        public static LogMessage StartingResultion(DnsDomain name, NameServerCollection authorities)
        {
          return Create(Resources.LogMessages.StartingResultion, new object[] { name, authorities.ZoneName });
        }

        public static LogMessage ContinuingResultion(DnsDomain domain, NameServerCollection authorities)
        {
          return Create(Resources.LogMessages.ContinuingResultion, new object[] { domain, authorities.ZoneName });
        }

        public static LogMessage AuthoritySelected(NameServerCollection authorities)
        {
          return Create(Resources.LogMessages.AuthoritySelected, new object[] { authorities.Selected.Name });
        }

        public static LogMessage SendingRequest(System.Net.IPAddress address, DnsClient.NetworkProtocol proto)
        {
          return Create(Resources.LogMessages.SendingRequest, new object[] { address, proto });
        }

        public static LogMessage ResolvingNameServer(NameServer unresolved)
        {
          return Create(Resources.LogMessages.ResolvingNameServer, new object[] { unresolved.Name });
        }

        public static LogMessage NameServerResolved(NameServer resolved, DnsDomain domain)
        {
          return Create(Resources.LogMessages.NameServerResolved, new object[] { resolved.Name, domain });
        }

        private static LogMessage Create(string message, object[] args)
        {
          return REQUEST.Create(LogMessageLevel.INFO, message, args);
        }

      }

      private static LogMessage Create(LogMessageLevel level, string message, object[] args)
      {
        return new LogMessage(LogMessageState.REQUEST, level, message, args);
      }

      public static LogMessage ResolutionStart(DnsDomain name, NameServerCollection authorities)
      {
        if (authorities.ZoneName.Equals(DnsDomain.Root))
        {
          // "Starting resolution of '{0}' with a root name server."
          return REQUEST.INFO.StartingResultion(name);
        }

        // "Starting resolution of '{0}' with a '{1}' authority name server."
        return REQUEST.INFO.StartingResultion(name, authorities);
      }

    }

    public static class RESPONSE
    {

      public static class INFO
      {

        public static LogMessage RequestDuration(double duration)
        {
          return Create(Resources.LogMessages.RequestDuration, new object[] { duration });
        }

        public static LogMessage SingleAddressResolutionSucceeded(DnsDomain domain, System.Net.IPAddress address)
        {
          return Create(Resources.LogMessages.SingleAddressResolutionSucceeded, new object[] { domain, address });
        }

        public static LogMessage SinglePointerResolutionSucceeded(string hostname)
        {
          return Create(Resources.LogMessages.SinglePointerResolutionSucceeded, 
            new object[] { hostname });
        }

        public static LogMessage RecordResolutionSucceeded(DnsClient.DNS.QTYPE qtype)
        {
          return Create(Resources.LogMessages.RecordResolutionSucceeded, 
            new object[] { qtype });
        }

        public static LogMessage RecordResolutionSucceeded(DnsClient.DNS.QTYPE qtype, int count)
        {
          return Create(Resources.LogMessages.RecordResolutionSucceeded2, 
            new object[] { qtype, count });
        }

        public static LogMessage AnyRecordResolutionSucceeded()
        {
          return Create(Resources.LogMessages.AnyRecordResolutionSucceeded, 
            new object[] { });
        }

        public static LogMessage AnyRecordResolutionSucceeded(int count)
        {
          return Create(Resources.LogMessages.AnyRecordResolutionSucceeded2, 
            new object[] { count });
        }

        public static LogMessage NextAuthoritiesReceived(DnsDomain server, DnsDomain name, NameServerCollection nextAuthorities)
        {
          return Create(Resources.LogMessages.NextAuthoritiesReceived, 
            new object[] { server, name, nextAuthorities.Count });
        }

        public static LogMessage ResourceRecordFound(DnsClient.Response response, DnsClient.DNS.RR rr)
        {
          return Create(Resources.LogMessages.ResourceRecordFound, 
            new object[] { rr.Base.TYPE, (DnsDomain)rr.Base.NAME });
        }

        public static LogMessage ResourceRecordFoundA(DnsClient.Response response, DnsClient.DNS.Records.A rr)
        {
          return Create(Resources.LogMessages.ResourceRecordFoundA, 
            new object[] { (DnsDomain)rr.Base.NAME, rr.ADDRESS });
        }

        public static LogMessage ResourceRecordFoundAAAA(DnsClient.Response response, DnsClient.DNS.Records.AAAA rr)
        {
          return Create(Resources.LogMessages.ResourceRecordFoundAAAA, 
            new object[] { (DnsDomain)rr.Base.NAME, rr.ADDRESS });
        }

        public static LogMessage ResourceRecordFoundPTR(DnsClient.Response response, DnsClient.DNS.Records.PTR rr)
        {
          return Create(Resources.LogMessages.ResourceRecordFoundPTR,
            new object[] { (DnsDomain)rr.Base.NAME, (DnsDomain)rr.PTRDNAME });
        }

        public static LogMessage ResourceRecordFoundNS(DnsClient.Response response, DnsClient.DNS.Records.NS rr)
        {
          return Create(Resources.LogMessages.ResourceRecordFoundNS,
            new object[] { (DnsDomain)rr.NSDNAME });
        }

        public static LogMessage ResolutionFinished(int count)
        {
          return Create(Resources.LogMessages.ResolutionFinished,
            new object[] { count });
        }

        private static LogMessage Create(string message, object[] args)
        {
          return RESPONSE.Create(LogMessageLevel.INFO, message, args);
        }

      }

      public static class WARN
      {

        public static LogMessage CNameFound(DnsDomain domain, string canonical)
        {
          return Create(Resources.LogMessages.CNameFound, new object[] { domain, canonical });
        }

        public static LogMessage CNameNoFollow()
        {
          return Create(Resources.LogMessages.CNameNoFollow, new object[] { });
        }

        public static LogMessage RepeatingTruncatedAnswer()
        {
          return Create(Resources.LogMessages.RepeatingTruncatedAnswer, new object[] { });
        }

        private static LogMessage Create(string message, object[] args)
        {
          return RESPONSE.Create(LogMessageLevel.WARN, message, args);
        }

      }

      public static class ERROR
      {

        public static LogMessage MissingRecords(DnsClient.DNS.QTYPE question)
        {
          return Create(Resources.LogMessages.MissingRecords, new object[] { question });
        }

        public static LogMessage NameNotExisting(DnsDomain domain)
        {
          return Create(Resources.LogMessages.NameNotExisting, new object[] { domain });
        }

        public static LogMessage QueryNotImplemented(DnsClient.DNS.QTYPE question)
        {
          return Create(Resources.LogMessages.QueryNotImplemented, new object[] { question });
        }

        public static LogMessage ServerRefused()
        {
          return Create(Resources.LogMessages.ServerRefused, new object[] { });
        }

        public static LogMessage ServerFailure()
        {
          return Create(Resources.LogMessages.ServerFailure, new object[] { });
        }

        public static LogMessage UnexpectedRCode(int rcodeValue, DnsClient.DNS.RCODE rcode)
        {
          return Create(Resources.LogMessages.UnexpectedRCode, new object[] { rcodeValue, rcode });
        }

        public static LogMessage NegativeResponse(QueryState queryState)
        {
          return Create(Resources.LogMessages.NegativeResponse, new object[] { queryState });
        }

        public static LogMessage ResponseInvalid(QueryState queryState)
        {
          return Create(Resources.LogMessages.ResponseInvalid, new object[] { queryState });
        }

        private static LogMessage Create(string message, object[] args)
        {
          return RESPONSE.Create(LogMessageLevel.ERROR, message, args);
        }

      }

      private static LogMessage Create(LogMessageLevel level, string message, object[] args)
      {
        return new LogMessage(LogMessageState.RESPONSE, level, message, args);
      }

      #region composed LogMessages

      public static LogMessage ResolutionSucceded(
        DnsDomain domain, DnsClient.DNS.QTYPE question, IEnumerable<DnsClient.DNS.RR> requestedAnswerRecords)
      {
        // must be true:
        // requestedAnswerRecords.Any()

        int count = requestedAnswerRecords.Count();
        if (count == 1)
        {
          DnsClient.DNS.RR rr = requestedAnswerRecords.First();
          DnsClient.DNS.Records.Address address;
          DnsClient.DNS.Records.PTR pointer;

          if (!Object.ReferenceEquals(null, (address = rr as DnsClient.DNS.Records.Address)))
          {
            // "The name '{0}' was successfully resolved to {1}."
            return RESPONSE.INFO.SingleAddressResolutionSucceeded(domain, address.ADDRESS);
          }

          if (!Object.ReferenceEquals(null, (pointer = rr as DnsClient.DNS.Records.PTR)))
          {
            // "Pointer resolution succeeded with the name '{0}'."
            return RESPONSE.INFO.SinglePointerResolutionSucceeded(pointer.PTRDNAME);
          }

          // "A Resouce Records of type {0} was found."
          return RESPONSE.INFO.RecordResolutionSucceeded(question);
        }

        // "{1} Resouce Records of type {0} were found."
        return RESPONSE.INFO.RecordResolutionSucceeded(question, count);
      }

      public static LogMessage AnyResolutionSucceded(IEnumerable<DnsClient.DNS.RR> answerRecords)
      {
        int count = answerRecords.Count();
        if (count == 1)
        {
          // "The SOA was found, but no other Resource Records."
          return RESPONSE.INFO.AnyRecordResolutionSucceeded();
        }

        // "{0} Resouce Records were found."
        return RESPONSE.INFO.AnyRecordResolutionSucceeded(count);
      }

      #endregion

    }



  }
}

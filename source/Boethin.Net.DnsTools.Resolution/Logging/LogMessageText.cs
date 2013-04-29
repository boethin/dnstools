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
          return Create(String.Format(
            Resources.LogMessages.StartingResultionRoot, name));
        }

        public static LogMessage StartingResultion(DnsDomain name, NameServerCollection authorities)
        {
          return Create(String.Format(
            Resources.LogMessages.StartingResultion, name, authorities.ZoneName));
        }

        public static LogMessage ContinuingResultion(DnsDomain domain, NameServerCollection authorities)
        {
          return Create(String.Format(
            Resources.LogMessages.ContinuingResultion, domain.ToString(), authorities.ZoneName));
        }

        public static LogMessage AuthoritySelected(NameServerCollection authorities)
        {
          return Create(String.Format(
            Resources.LogMessages.AuthoritySelected, authorities.Selected.Name.ToString()));
        }

        public static LogMessage SendingRequest(System.Net.IPAddress address, DnsClient.NetworkProtocol proto)
        {
          return Create(String.Format(
            Resources.LogMessages.SendingRequest, address.ToString(), proto.ToString()));
        }

        public static LogMessage ResolvingNameServer(NameServer unresolved)
        {
          return Create(String.Format(
            Resources.LogMessages.ResolvingNameServer, unresolved.Name.ToString()));
        }

        public static LogMessage NameServerResolved(NameServer resolved, DnsDomain domain)
        {
          return Create(String.Format(
            Resources.LogMessages.NameServerResolved, resolved.Name.ToString(), domain.ToString()));
        }

        private static LogMessage Create(string message)
        {
          return REQUEST.Create(LogMessageLevel.INFO, message);
        }

      }

      private static LogMessage Create(LogMessageLevel level, string message)
      {
        return new LogMessage(LogMessageState.REQUEST, level, message);
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
          return Create(String.Format(
            Resources.LogMessages.RequestDuration, duration));
        }

        public static LogMessage SingleAddressResolutionSucceeded(DnsDomain domain, System.Net.IPAddress address)
        {
          return Create(String.Format(
            Resources.LogMessages.SingleAddressResolutionSucceeded, domain.ToString(), address.ToString()));
        }

        public static LogMessage SinglePointerResolutionSucceeded(string hostname)
        {
          return Create(String.Format(
            Resources.LogMessages.SinglePointerResolutionSucceeded, hostname));
        }

        public static LogMessage RecordResolutionSucceeded(DnsClient.DNS.QTYPE qtype)
        {
          return Create(String.Format(
            Resources.LogMessages.RecordResolutionSucceeded, qtype.ToString()));
        }

        public static LogMessage RecordResolutionSucceeded(DnsClient.DNS.QTYPE qtype, int count)
        {
          return Create(String.Format(
            Resources.LogMessages.RecordResolutionSucceeded2, qtype.ToString(), count));
        }

        public static LogMessage AnyRecordResolutionSucceeded()
        {
          return Create(String.Format(
            Resources.LogMessages.AnyRecordResolutionSucceeded));
        }

        public static LogMessage AnyRecordResolutionSucceeded(int count)
        {
          return Create(String.Format(
            Resources.LogMessages.AnyRecordResolutionSucceeded2, count));
        }

        public static LogMessage NextAuthoritiesReceived(DnsDomain server, DnsDomain name, NameServerCollection nextAuthorities)
        {
          // {2} authorities of '{1}' from '{0}'.
          return Create(String.Format(
            Resources.LogMessages.NextAuthoritiesReceived, server.ToString(), name.ToString(), nextAuthorities.Count));
        }

        public static LogMessage ResourceRecordFound(DnsClient.Response response, DnsClient.DNS.RR rr)
        {
          return Create(String.Format(Resources.LogMessages.ResourceRecordFound, rr.Base.TYPE, rr.Base.NAME));
        }

        public static LogMessage ResourceRecordFoundA(DnsClient.Response response, DnsClient.DNS.Records.A rr)
        {
          return Create(String.Format(Resources.LogMessages.ResourceRecordFoundA, rr.Base.NAME, rr.ADDRESS.ToString()));
        }

        public static LogMessage ResourceRecordFoundAAAA(DnsClient.Response response, DnsClient.DNS.Records.AAAA rr)
        {
          return Create(String.Format(Resources.LogMessages.ResourceRecordFoundAAAA, rr.Base.NAME, rr.ADDRESS.ToString()));
        }

        public static LogMessage ResourceRecordFoundPTR(DnsClient.Response response, DnsClient.DNS.Records.PTR rr)
        {
          return Create(String.Format(Resources.LogMessages.ResourceRecordFoundPTR, rr.Base.NAME, rr.PTRDNAME));
        }

        public static LogMessage ResourceRecordFoundNS(DnsClient.Response response, DnsClient.DNS.Records.NS rr)
        {
          

          return Create(String.Format(Resources.LogMessages.ResourceRecordFoundNS, rr.NSDNAME));
        }

        public static LogMessage ResolutionFinished(int count)
        {
          return Create(String.Format(
            Resources.LogMessages.ResolutionFinished, count));
        }

        private static LogMessage Create(string message)
        {
          return RESPONSE.Create(LogMessageLevel.INFO, message);
        }

      }

      public static class WARN
      {

        public static LogMessage CNameFound(DnsDomain domain, string canonical)
        {
          return Create(String.Format(
            Resources.LogMessages.CNameFound, domain.ToString(), canonical));
        }

        public static LogMessage CNameNoFollow()
        {
          return Create(String.Format(
            Resources.LogMessages.CNameNoFollow));
        }

        public static LogMessage RepeatingTruncatedAnswer()
        {
          return Create(String.Format(
            Resources.LogMessages.RepeatingTruncatedAnswer));
        }

        private static LogMessage Create(string message)
        {
          return RESPONSE.Create(LogMessageLevel.WARN, message);
        }

      }

      public static class ERROR
      {

        public static LogMessage MissingRecords(DnsClient.DNS.QTYPE question)
        {
          return Create(String.Format(
            Resources.LogMessages.MissingRecords, question.ToString()));
        }

        public static LogMessage NameNotExisting(DnsDomain domain)
        {
          return Create(String.Format(
            Resources.LogMessages.NameNotExisting, domain));
        }

        public static LogMessage QueryNotImplemented(DnsClient.DNS.QTYPE question)
        {
          return Create(String.Format(
            Resources.LogMessages.QueryNotImplemented, question.ToString()));
        }

        public static LogMessage ServerRefused()
        {
          return Create(String.Format(
            Resources.LogMessages.ServerRefused));
        }

        public static LogMessage ServerFailure()
        {
          return Create(String.Format(
            Resources.LogMessages.ServerFailure));
        }

        public static LogMessage UnexpectedRCode(int rcodeValue, DnsClient.DNS.RCODE rcode)
        {
          return Create(String.Format(
            Resources.LogMessages.UnexpectedRCode, rcodeValue, rcode.ToString()));
        }

        public static LogMessage NegativeResponse(QueryState queryState)
        {
          return Create(String.Format(
            Resources.LogMessages.NegativeResponse, queryState.ToString()));
        }

        public static LogMessage ResponseInvalid(QueryState queryState)
        {
          return Create(String.Format(
            Resources.LogMessages.ResponseInvalid, queryState.ToString()));
        }

        private static LogMessage Create(string message)
        {
          return RESPONSE.Create(LogMessageLevel.ERROR, message);
        }
        
      }

      private static LogMessage Create(LogMessageLevel level, string message)
      {
        return new LogMessage(LogMessageState.RESPONSE, level, message);
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

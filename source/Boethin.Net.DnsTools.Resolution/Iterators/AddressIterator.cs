/*
 * File: Boethin.Net.DnsTools.Resolution/Iterators/AddressIterator.cs
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

namespace Boethin.Net.DnsTools.Resolution.Iterators
{

  [Serializable]
  public class AddressIterator : ResolutionIterator
  {

    internal AddressIterator(ResolutionIterator iterator, DnsDomain domain, IPVersion ipVersion)
      : base(new Resolver(iterator.Resolver.Options, domain),
      GetAddressQuestion(ipVersion), iterator.AddressCache, iterator.NestingLevel + 1)
    {
    }

    public IEnumerable<DnsClient.DNS.Records.Address> Addresses = null;




    protected override Results.ResolutionResult GetNextResult(out bool isCompleted, NameServer selected)
    {
      Results.ResolutionResult result = base.GetNextResult(out isCompleted, selected);
      if (isCompleted)
      {

        IEnumerable<DnsClient.DNS.RR> rrset = null;
        switch (result.QueryState)
        {
          case QueryState.AuthoritativeAnswer:
            rrset = result.Response.AnswerRecords;
            break;

          case QueryState.NonAuthoritativeAnswer:
            rrset = result.Response.AdditionalRecords;
            break;
        }
        if (!object.ReferenceEquals(null, rrset))
        {
          this.Addresses = rrset.Select<DnsClient.DNS.Records.Address>().Where(
            a => this.Resolver.Domain.Equals(a.Base.NAME));
        }

      }
      return result;
    }

    public IEnumerable<DnsClient.DNS.Records.Address> GetAddresses(Results.ResolutionResult result)
    {
      IEnumerable<DnsClient.DNS.RR> rrset = null;
      switch (result.QueryState)
      {
        case QueryState.AuthoritativeAnswer:
          rrset = result.Response.AnswerRecords;
          break;

        case QueryState.NonAuthoritativeAnswer:
          rrset = result.Response.AdditionalRecords;
          break;
      }
      if (!object.ReferenceEquals(null, rrset))
      {
        return rrset.Select<DnsClient.DNS.Records.Address>().Where(
          a => this.Resolver.Domain.Equals(a.Base.NAME));
      }

      return null;
    }


    //internal static IEnumerable<DnsClient.DNS.Records.Address> GetAddresses(DnsDomain sname, DnsClient.Response response)
    //{
    //  IEnumerable<DnsClient.DNS.RR> rrset;
    //  if (response.Header.AA)
    //  {
    //    rrset = response.AnswerRecords;
    //  }
    //  else 
    //  {
    //    rrset = response.AdditionalRecords;
    //  }

    //  return rrset.Select<DnsClient.DNS.Records.Address>().Where(
    //    a => sname.Equals(a.Base.NAME));
    //}

    internal static DnsClient.DNS.QTYPE GetAddressQuestion(IPVersion ip)
    {
      switch (ip)
      {
        case IPVersion.IPv4:
          return DnsClient.DNS.QTYPE.A;
        case IPVersion.IPv6:
          return DnsClient.DNS.QTYPE.AAAA;
        default:
          return DnsClient.DNS.QTYPE.ANY;
      }
    }


  }
}

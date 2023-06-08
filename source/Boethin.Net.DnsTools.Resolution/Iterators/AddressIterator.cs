/*
 * File: Boethin.Net.DnsTools.Resolution/Iterators/AddressIterator.cs
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
using Boethin.Net.DnsTools.Resolution.Extensions;

namespace Boethin.Net.DnsTools.Resolution.Iterators
{

  [Serializable]
  public class AddressIterator : ResolutionIterator
  {

    internal AddressIterator(ResolutionIterator iterator, DnsDomain domain, IPVersion ipVersion)
      : base(new DomainResolver(iterator.Resolver.Options, domain),
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

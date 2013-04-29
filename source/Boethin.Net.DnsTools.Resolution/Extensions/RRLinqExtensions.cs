/*
 * File: Boethin.Net.DnsTools.Resolution/Extensions/RRLinqExtensions.cs
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

namespace Boethin.Net.DnsTools.Resolution.Extensions
{
  public static class RRLinqExtensions
  {

    /// <summary>
    /// Extract a specified set of RRs from a generic list of RRs.
    /// </summary>
    /// <typeparam name="TRecord"></typeparam>
    /// <param name="records"></param>
    /// <returns></returns>
    public static IEnumerable<TRecord> Select<TRecord>(this IEnumerable<DnsClient.DNS.RR> records)
      where TRecord : DnsClient.DNS.RR
    {
      if (object.ReferenceEquals(null, records))
        throw new ArgumentNullException("records");

      return records.Where(
        rr => typeof(TRecord).IsAssignableFrom(rr.GetType())).Select<DnsClient.DNS.RR, TRecord>(
        rr => rr as TRecord);
    }

    public static TRecord First<TRecord>(this IEnumerable<DnsClient.DNS.RR> records)
      where TRecord : DnsClient.DNS.RR
    {
      if (object.ReferenceEquals(null, records))
        throw new ArgumentNullException("records");

      return records.Select<TRecord>().First();
    }

    public static TRecord FirstOrDefault<TRecord>(this IEnumerable<DnsClient.DNS.RR> records)
      where TRecord : DnsClient.DNS.RR
    {
      if (object.ReferenceEquals(null, records))
        throw new ArgumentNullException("records");

      return records.Select<TRecord>().FirstOrDefault();
    }


    //public static IEnumerable<DnsClient.DNS.RR> Select(this IEnumerable<DnsClient.DNS.RR> records, DnsClient.DNS.QTYPE type)
    //{
    //  return records.Where(rr => type.Equals(rr.Base.TYPE));
    //}

    public static IEnumerable<DnsClient.DNS.RR> OfQuestion(this IEnumerable<DnsClient.DNS.RR> records, DnsClient.DNS.QTYPE question)
    {
      if (question == DnsClient.DNS.QTYPE.ANY)
        return records;

      return records.Where(rr => rr.Base.TYPE.Equals(question));
    }




  }
}

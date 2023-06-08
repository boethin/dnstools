/*
 * File: Boethin.Net.DnsTools.Resolution/Extensions/RRLinqExtensions.cs
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

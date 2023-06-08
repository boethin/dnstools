/*
 * File: Boethin.Net.DnsTools.Resolution/Caching/AddressCache.cs
 *
 * This file is part of Boethin.Net.DnsTools, a DNS debugging library.
 * Copyright (C) 2013 Sebastian Boethin <sebastian@boethin.eu>
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

namespace Boethin.Net.DnsTools.Resolution.Caching
{

  /// <summary>
  /// A cache for address records.
  /// </summary>
  [Serializable]
  public class AddressCache : DomainCache<IEnumerable<DnsClient.DNS.Records.Address>>
  {

    /// <summary>
    /// Add a list of address records without expiration to the cache.
    /// </summary>
    /// <param name="records">A list of address records.</param>
    public void Set(IEnumerable<DnsClient.DNS.Records.Address> records)
    {
      if (object.ReferenceEquals(null, records))
        throw new ArgumentNullException("records");

      // set records, ordered by name
      foreach (IGrouping<DnsDomain, DnsClient.DNS.Records.Address> group in
        records.GroupBy<DnsClient.DNS.Records.Address, DnsDomain>(
        r => (DnsDomain)r.Base.NAME))
      {
        Set(group.Key, new ExpirableElement<IEnumerable<DnsClient.DNS.Records.Address>>(group.ToArray()));
      }

    }

    /// <summary>
    /// Add a list of address records to the cache, where the expiration is calculated 
    /// from a given timestamp and the TTL values. Records with zero TTL are ignored.
    /// <para>The minimum of all TTL values is used for each name.</para>
    /// </summary>
    /// <param name="timestamp">The timestamp determining the cache expiration.</param>
    /// <param name="records">A list of address records.</param>
    public void Set(DateTime timestamp, IEnumerable<DnsClient.DNS.Records.Address> records)
    {
      if (object.ReferenceEquals(null, records))
        throw new ArgumentNullException("records");

      // set records, ordered by name
      foreach (IGrouping<DnsDomain, DnsClient.DNS.Records.Address> group in
        records.Where(
          r => r.Base.TTL > 0).GroupBy<DnsClient.DNS.Records.Address, DnsDomain>(
          r => (DnsDomain)r.Base.NAME))
      {
        Set(group.Key, new ExpirableElement<IEnumerable<DnsClient.DNS.Records.Address>>(
            group.ToArray(), timestamp.AddSeconds(group.Min(r => r.Base.TTL))));
      }
    }

  }
}

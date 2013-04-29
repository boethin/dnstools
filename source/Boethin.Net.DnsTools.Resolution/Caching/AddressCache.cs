/*
 * File: Boethin.Net.DnsTools.Resolution/Caching/AddressCache.cs
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

/*
 * File: Boethin.Net.DnsTools.Resolution/NameServerCollection.cs
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
using Boethin.Net.DnsTools.Resolution.Extensions;

namespace Boethin.Net.DnsTools.Resolution
{

  /// <summary>
  /// These NS RRs list the names of hosts for a zone at or above SNAME [RFC 1034].
  /// <para>A collection of name server objects with the capability to mark one of them as selected.</para>
  /// </summary>
  [Serializable]
  public class NameServerCollection : IEnumerable<NameServer>
  {

    #region private

    private readonly DnsDomain _ZoneName;

    private readonly IDictionary<DnsDomain, NameServer> NameServers;

    private DnsDomain SelectedName = null;

    #endregion

    #region Empty

    public static readonly NameServerCollection Empty = new NameServerCollection();

    #endregion

    #region public get

    /// <summary>
    /// Gets the number of elements contained in the name server collection.
    /// </summary>
    public int Count
    {
      get { return NameServers.Count; }
    }

    /// <summary>
    /// Whether or not the collection is empty.
    /// </summary>
    public bool IsEmpty
    {
      get { return Count == 0; }
    }

    /// <summary>
    /// The zone name where all name servers are at or above. 
    /// The value is null for the empty collection.
    /// </summary>
    public DnsDomain ZoneName
    {
      get { return _ZoneName; }
    }

    public IEnumerable<IGrouping<int, NameServer>> Groupings
    {
      get
      {
        return NameServers.Values.GroupBy(
          ns => ns.Name.Level).OrderByDescending(g => g.Key);
      }
    }

    /// <summary>
    /// The selected name server, only valid if one has been selected.
    /// </summary>
    public NameServer Selected
    {
      get
      {
        if (object.ReferenceEquals(null, SelectedName))
          throw new InvalidOperationException(
            "No item in the collection was marked as selected.");
        return NameServers[SelectedName];
      }
    }

    public IEnumerable<NameServer> Prioritized
    {
      get
      {
        return NameServers.Values.AsEnumerable(); // TODO
        //throw new NotImplementedException(); 
      }
    }

    #endregion

    #region c'tor

    // Internal c'tor for RootServers.
    // Create instances through Find().
    internal NameServerCollection(DnsDomain zoneName,
      IEnumerable<NameServer> nameservers)
    {
      if (object.ReferenceEquals(null, zoneName))
        throw new ArgumentNullException("zoneName");
      if (object.ReferenceEquals(null, nameservers))
        throw new ArgumentNullException("nameservers");

      _ZoneName = zoneName;
      NameServers = nameservers.ToDictionary(s => s.Name, s => s);
    }

    // Empty c'tor
    private NameServerCollection()
    {
      _ZoneName = null;
      NameServers = new Dictionary<DnsDomain, NameServer>();
    }

    #endregion

    #region public

    public NameServer this[int index]
    {
      get
      {
        return NameServers.Values.ElementAt(index);
      }
    }

    /// <summary>
    /// Mark one item in the collection of name servers as the selected one.
    /// <para>The method is invalid for an empty set of name servers.</para>
    /// </summary>
    /// <param name="selected"></param>
    public void SelectOne(NameServer selected)
    {
      if (IsEmpty)
        throw new InvalidOperationException(
          "The method is invalid for an empty set of name servers.");

      if (object.ReferenceEquals(null, selected))
        throw new ArgumentNullException("selected");
      try
      {
        SelectedName = NameServers[selected.Name].Name;
      }
      catch (Exception ex)
      {
        throw new ArgumentException(
          "The given name server was not found in the collection.", "selected", ex);
      }
    }

    /// <summary>
    /// Randomly select one item from the collection of name servers (resolved items prefered).
    /// <para>The method is invalid for an empty set of name servers.</para>
    /// </summary>
    /// <returns></returns>
    public NameServer SelectAny()
    {
      if (IsEmpty)
        throw new InvalidOperationException(
          "The method is invalid for an empty set of name servers.");

      // At least one name server exists.
      NameServer selected = null;
      Random r = new Random((int)DateTime.Now.Ticks ^ DateTime.Now.Millisecond);

      // Try selecting on each name level, from highest to lowest.
      foreach (IGrouping<int, NameServer> nsg in Groupings)
      {
        int c = nsg.Count(), resc;
        if (c == 0)
          continue;

        // prefer resolved servers
        IEnumerable<NameServer> resolved = NameServers.Values.Where(
          ns => ns.IsResolved);
        if ((resc = resolved.Count()) > 0)
        {
          selected = resolved.ElementAt(r.Next(0, resc));
        }
        else
        {
          // get any
          selected = nsg.ElementAt(r.Next(0, c));
        }
      }

      // selected cannot be null
      SelectedName = selected.Name;
      return selected;
    }

    internal void ApplyCache(Caching.AddressCache addressCache)
    {
      foreach (NameServer ns in NameServers.Values.Where(ns => !ns.IsResolved))
      {
        IEnumerable<DnsClient.DNS.Records.Address> addressRecords = addressCache.Get(ns.Name, DateTime.Now);
        if (addressRecords != null)
          ns.ApplyAddresses(addressRecords);
      }
    }

    #endregion

    #region IEnumerable<NameServer>

    IEnumerator<NameServer> IEnumerable<NameServer>.GetEnumerator()
    {
      return NameServers.Values.AsEnumerable().GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return ((System.Collections.IEnumerable)NameServers.Values).GetEnumerator();
    }

    #endregion

    #region static

    internal static NameServerCollection Create(
      IEnumerable<DnsClient.DNS.RR> records, IEnumerable<DnsClient.DNS.RR> addresses = null)
    {
      // NS records from Authority Section
      IEnumerable<DnsClient.DNS.Records.NS> nsRecords = records.Select<DnsClient.DNS.Records.NS>();

      DnsClient.DNS.Records.NS ns1 = nsRecords.FirstOrDefault();
      if (ns1 == null)
        return Empty;

      // There are NS RRs.
      NameServerCollection result = new NameServerCollection(
        (DnsDomain)ns1.Base.NAME, nsRecords.Select(ns => new NameServer(
          (DnsDomain)ns.Base.NAME, (DnsDomain)ns.NSDNAME)));

      // Apply address records from Additional Section (A or AAAA) if any
      if (addresses != null)
      {
        foreach (DnsClient.DNS.Records.Address addr in addresses.Select(
          rr => rr as DnsClient.DNS.Records.Address).Where(a => a != null))
        {
          DnsDomain name = DnsDomain.Parse(addr.Base.NAME);
          if (result.NameServers.ContainsKey(name))
            result.NameServers[name].Addresses.Add(addr.ADDRESS);
        }
      }

      return result;
    }

    #endregion

    internal static NameServerCollection Find(DnsDomain domain,
      IEnumerable<DnsClient.DNS.RR> records, IEnumerable<DnsClient.DNS.RR> addresses)
    {

      // [RFC 1034]
      // 5.3.3. Algorithm
      //
      // 2. Find the best servers to ask.
      //
      // Step 2 looks for a name server to ask for the required data.  The
      // general strategy is to look for locally-available name server RRs,
      // starting at SNAME, then the parent domain name of SNAME, the
      // grandparent, and so on toward the root.  Thus if SNAME were
      // Mockapetris.ISI.EDU, this step would look for NS RRs for
      // Mockapetris.ISI.EDU, then ISI.EDU, then EDU, and then . (the root).
      // These NS RRs list the names of hosts for a zone at or above SNAME. Copy
      // the names into SLIST.


      // NS records from Authority Section above domain (SNAME)
      IEnumerable<DnsClient.DNS.Records.NS> nservers = records.Select<
        DnsClient.DNS.Records.NS>().Where(ns => domain.IsBelow(ns.Base.NAME));
      if (!nservers.Any())
        return Empty;

      // There are relevant NS RRs.
      NameServerCollection result = new NameServerCollection(
        domain, nservers.Select(ns => new NameServer(
          (DnsDomain)ns.Base.NAME, (DnsDomain)ns.NSDNAME)));

      // Apply address records from Additional Section (A or AAAA) if any
      if (!object.ReferenceEquals(null, addresses))
      {
        foreach (DnsClient.DNS.Records.Address addr in addresses.Select<
          DnsClient.DNS.Records.Address>())
        {
          DnsDomain hostname = (DnsDomain)addr.Base.NAME;
          if (result.NameServers.ContainsKey(hostname))
            result.NameServers[hostname].Addresses.Add(addr.ADDRESS);
        }
      }

      return result;
    }

  }
}

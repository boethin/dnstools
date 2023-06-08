/*
 * File: Boethin.Net.DnsTools.Resolution/RootServers.cs
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
using System.Net;

namespace Boethin.Net.DnsTools.Resolution
{
  public static class RootServers
  {

    // The RootHints must by kept in sync manually with the IANA source:
    // http://www.iana.org/domains/root/servers
    //
    // Operators who manage a DNS recursive resolver typically need to configure a "root hints file".
    // This file contains the names and IP addresses of the root servers, so the software can bootstrap
    // the DNS resolution process. For many pieces of software, this list comes built into the software.
    //
    public static readonly NameServerCollection RootHints = new NameServerCollection(
      DnsDomain.Root, new NameServer[] { 

      // a.root-servers.net 	198.41.0.4, 2001:503:ba3e::2:30 	VeriSign, Inc.
      new NameServer(DnsDomain.Root, (DnsDomain)"a.root-servers.net", new IPAddress[] {
        IPAddress.Parse("198.41.0.4"), IPAddress.Parse("2001:503:ba3e::2:30")}),

      // b.root-servers.net 	192.228.79.201 	University of Southern California (ISI)
      new NameServer(DnsDomain.Root,(DnsDomain)"b.root-servers.net", new IPAddress[] {
        IPAddress.Parse("192.228.79.201")}),

      // c.root-servers.net 	192.33.4.12 	Cogent Communications
      new NameServer(DnsDomain.Root,(DnsDomain)"c.root-servers.net", new IPAddress[] {
        IPAddress.Parse("192.33.4.12")}),

      // d.root-servers.net 	128.8.10.90, 2001:500:2d::d 	University of Maryland
      new NameServer(DnsDomain.Root,(DnsDomain)"d.root-servers.net", new IPAddress[] {
        IPAddress.Parse("128.8.10.90"), IPAddress.Parse("2001:500:2d::d")}),

      // e.root-servers.net 	192.203.230.10 	NASA (Ames Research Center)
      new NameServer(DnsDomain.Root,(DnsDomain)"e.root-servers.net", new IPAddress[] {
        IPAddress.Parse("192.203.230.10")}),

      // f.root-servers.net 	192.5.5.241, 2001:500:2f::f 	Internet Systems Consortium, Inc.
      new NameServer(DnsDomain.Root,(DnsDomain)"f.root-servers.net", new IPAddress[] {
        IPAddress.Parse("192.5.5.241"), IPAddress.Parse("2001:500:2f::f")}),

      // g.root-servers.net 	192.112.36.4 	US Department of Defence (NIC)
      new NameServer(DnsDomain.Root,(DnsDomain)"g.root-servers.net", new IPAddress[] {
        IPAddress.Parse("192.112.36.4")}),

      // hh.root-servers.net 	128.63.2.53, 2001:500:1::803f:235 	US Army (Research Lab)
      new NameServer(DnsDomain.Root,(DnsDomain)"h.root-servers.net", new IPAddress[] {
        IPAddress.Parse("128.63.2.53"), IPAddress.Parse("2001:500:1::803f:235")}),

      // i.root-servers.net 	192.36.148.17, 2001:7fe::53 	Netnod
      new NameServer(DnsDomain.Root,(DnsDomain)"i.root-servers.net", new IPAddress[] {
        IPAddress.Parse("192.36.148.17"), IPAddress.Parse("2001:7fe::53")}),

        // j.root-servers.net 	192.58.128.30, 2001:503:c27::2:30 	VeriSign, Inc.
      new NameServer(DnsDomain.Root,(DnsDomain)"j.root-servers.net", new IPAddress[] {
        IPAddress.Parse("192.58.128.30"), IPAddress.Parse("2001:503:c27::2:30")}),

        // k.root-servers.net 	193.0.14.129, 2001:7fd::1 	RIPE NCC
      new NameServer(DnsDomain.Root,(DnsDomain)"k.root-servers.net", new IPAddress[] {
        IPAddress.Parse("193.0.14.129"), IPAddress.Parse("2001:7fd::1")}),

        // l.root-servers.net 	199.7.83.42, 2001:500:3::42 	ICANN
      new NameServer(DnsDomain.Root,(DnsDomain)"l.root-servers.net", new IPAddress[] {
        IPAddress.Parse("199.7.83.42"), IPAddress.Parse("2001:500:3::42")}),

        // m.root-servers.net 	202.12.27.33, 2001:dc3::35 	WIDE Project
      new NameServer(DnsDomain.Root,(DnsDomain)"m.root-servers.net", new IPAddress[] {
        IPAddress.Parse("202.12.27.33"), IPAddress.Parse("2001:dc3::35")})

    });

    /// <summary>
    /// In-memory cache for top level name servers.
    /// </summary>
    internal static readonly Caching.DomainTreeCache<NameServerCollection> Cache = new Caching.DomainTreeCache<NameServerCollection>();

    //// singleton
    //internal sealed class TopLevelCache
    //{

    //  private readonly Caching.TreeCache<NameServerCollection> Tree = new Caching.TreeCache<NameServerCollection>();

    //  internal TopLevelCache()
    //  {
    //  }

    //  internal NameServerCollection Find(DnsDomain domain)
    //  {
    //    return Tree.Get(domain);
    //  }

    //  internal void Save(DnsClient.Response rootResponse)
    //  {
    //    NameServerCollection authorities = NameServerCollection.Create(
    //      rootResponse.AuthorityRecords, rootResponse.AdditionalRecords);
    //    if (!authorities.IsEmpty)
    //    {
    //      Tree.Set(authorities.ZoneName, authorities);
    //    } 
    //  }

    //}

  }
}

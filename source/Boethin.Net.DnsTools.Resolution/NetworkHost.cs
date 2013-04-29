/*
 * File: Boethin.Net.DnsTools.Resolution/NetworkHost.cs
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
using System.Net;

namespace Boethin.Net.DnsTools.Resolution
{

  [Serializable] 
  public abstract class NetworkHost
  {

    #region private

    private readonly DnsDomain _Name;

    private readonly IList<IPAddress> _Addresses;

    #endregion

    #region public get

    /// <summary>
    /// The server's host name.
    /// </summary>
    public DnsDomain Name
    {
      get { return _Name; }
    }

    /// <summary>
    /// The list of the server's IP addresses.
    /// <para>The list is empty if the server's name has not been resolved yet.</para>
    /// </summary>
    public IList<IPAddress> Addresses
    {
      get { return _Addresses; }
    }

    /// <summary>
    /// Whether or not the name is resolved to a list of IP addresses.
    /// </summary>
    public bool IsResolved
    {
      get { return _Addresses.Count > 0; }
    }

    #endregion


    #region c'tor

    public NetworkHost(DnsDomain name)
    {
      if (name == null)
        throw new ArgumentNullException("name", "Value cannot be null.");

      _Name = name;
      _Addresses = new List<IPAddress>();
    }

    public NetworkHost(string name)
    {
      if (name == null)
        throw new ArgumentNullException("name", "Value cannot be null.");

      _Name = DnsDomain.Parse(name);
      _Addresses = new List<IPAddress>();
    }

    public NetworkHost(DnsDomain name, IEnumerable<IPAddress> addresses)
    {
      if (name == null)
        throw new ArgumentNullException("name", "Value cannot be null.");
      if (addresses == null)
        throw new ArgumentNullException("addresses", "Value cannot be null.");

      _Name = name;
      _Addresses = new List<IPAddress>(addresses);
    }

    public NetworkHost(string name, IEnumerable<IPAddress> addresses)
    {
      if (name == null)
        throw new ArgumentNullException("name", "Value cannot be null.");
      if (addresses == null)
        throw new ArgumentNullException("addresses", "Value cannot be null.");

      _Name = DnsDomain.Parse(name);
      _Addresses = new List<IPAddress>(addresses);
    }

    #endregion

    #region override

    public override string ToString()
    {
      if (IsResolved)
        return String.Format("{0} {1}",
          Name.ToString(), String.Join(" ", Addresses.Select(a => String.Format("[{0}]", a.ToString())).ToArray()));
      return _Name.ToString();
    }

    #endregion


  }
}

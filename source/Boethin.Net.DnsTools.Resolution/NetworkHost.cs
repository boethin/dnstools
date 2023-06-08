/*
 * File: Boethin.Net.DnsTools.Resolution/NetworkHost.cs
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

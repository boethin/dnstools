/*
 * File: Boethin.Net.DnsTools.Resolution/DnsDomain.cs
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

namespace Boethin.Net.DnsTools.Resolution
{

  /// <summary>
  /// A DNS domain is either the root '.' or a collection of DNS labels, starting with the topmost one.
  /// <para>See [RFC 1034] 3.1, &quot;Name space specifications and terminology&quot; and [RFC 2782] for symbolic names.</para>
  /// </summary>
  [Serializable]
  public class DnsDomain : IEnumerable<DnsLabel>
  {

    #region private

    private readonly DnsLabel[] _Labels;

    private readonly string _Domain;

    #endregion

    public static readonly DnsDomain Root = new DnsDomain();

    #region public get

    public int Level
    {
      get { return _Labels.Count(); }
    }

    /// <summary>
    /// The actual number of characters of the FQDN (i.e. including the trailing dot).
    /// </summary>
    public int Length
    {
      get { return _Domain.Length; }
    }

    #endregion

    #region c'tor

    // Root
    private DnsDomain()
    {
      _Labels = new DnsLabel[] { };
      _Domain = ".";
    }

    private DnsDomain(IEnumerable<DnsLabel> labels, string domain)
    {
      _Labels = labels.ToArray();
      _Domain = domain;
    }

    #endregion

    #region override

    public override string ToString()
    {
      return _Domain;
    }

    public override bool Equals(object obj)
    {
      DnsDomain n = obj as DnsDomain;
      if ((object)n == null)
      {
        string s = obj as string;
        if ((object)s == null)
          return false;
        return Equals(s);
      }
      return Equals(n);
    }

    public bool Equals(string str)
    {
      if (String.IsNullOrEmpty(str))
        return false;
      DnsDomain n;
      if (!TryParse(str, out n))
        return false;
      return Equals(n);
    }

    public bool Equals(DnsDomain obj)
    {
      if (obj == null)
        return false;
      return _Domain.Equals(obj._Domain);
    }

    public override int GetHashCode()
    {
      return _Domain.GetHashCode();
    }

    #endregion

    #region public

    public bool IsBelow(string domain)
    {
      return IsBelow((DnsDomain)domain);
    }

    public bool IsBelow(DnsDomain domain)
    {
      return this._Domain.EndsWith(domain._Domain);
    }

    #endregion

    #region IEnumerable<Label>

    public IEnumerator<DnsLabel> GetEnumerator()
    {
      return _Labels.AsEnumerable().GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return ((System.Collections.IEnumerable)_Labels).GetEnumerator();
    }

    #endregion
    
    #region operator

    public static implicit operator string(DnsDomain value)
    {
      return value._Domain;
    }

    public static explicit operator DnsDomain(string value)
    {
      return Parse(value);
    }

    #endregion

    #region static

    public static DnsDomain Parse(string input)
    {
      if (object.ReferenceEquals(null, input))
        throw new ArgumentNullException("input");

      input = input.Trim();

      if (input.Equals("."))
        return Root;

      if (input.Length == 0)
        throw new ArgumentException("Domain name cannot by empty.");

      if (input.Length > 255)
        throw new ArgumentException("Domain name too long (more than 255 caharacters).");

      if (input[input.Length - 1] == '.')
        input = input.Substring(0, input.Length - 1);

      IEnumerable<DnsLabel> labels = input.Split('.').Select(s => DnsLabel.Parse(s));
      return new DnsDomain(labels.Reverse(), String.Join(".", labels.Select(a => (a.ToString())).ToArray()) + '.');
    }

    public static bool TryParse(string input, out DnsDomain domain)
    {
      if (object.ReferenceEquals(null, input))
        throw new ArgumentNullException("input");

      domain = null;
      input = input.Trim();

      if (input.Equals("."))
      {
        domain = Root;
        return true;
      }

      if (input.Length == 0)
        return false;

      if (input.Length > 255)
        return false;

      if (input[input.Length - 1] == '.')
        input = input.Substring(0, input.Length - 1);

      IEnumerable<DnsLabel> labels = input.Split('.').Select(s => DnsLabel.Parse(s));
      domain = new DnsDomain(labels.Reverse(), String.Join(".", labels.Select(a => (a.ToString())).ToArray()) + '.');
      return true;
    }

    public static bool Equals(string s1, string s2)
    {
      if (String.IsNullOrEmpty(s1) || String.IsNullOrEmpty(s2))
        return object.ReferenceEquals(s1, s2);
      return ((DnsDomain)s1).Equals(s2);
    }

    #endregion


  }

}

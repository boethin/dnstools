/*
 * File: Boethin.Net.DnsTools.Resolution/Caching/DomainTreeCache.cs
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

namespace Boethin.Net.DnsTools.Resolution.Caching
{

  [Serializable]
  public class DomainTreeCache<TElement> : IDomainCache<TElement>
    where TElement : class
  {

    #region private

    private readonly TreeNode _Nodes = new TreeNode();

    #endregion

    #region public

    public void Set(DnsDomain domain, TElement element)
    {
      Set(domain, new ExpirableElement<TElement>(element));
    }

    public void Set(DnsDomain name, IExpirableElement<TElement> element)
    {
      if (object.ReferenceEquals(null, name))
        throw new ArgumentNullException("name");
      if (object.ReferenceEquals(null, element))
        throw new ArgumentNullException("element");

      // iterate through domain labels starting from top level
      using (IEnumerator<DnsLabel> en = name.GetEnumerator())
      {
        TreeNode nodes = _Nodes;
        while (en.MoveNext())
        {
          TreeNode subNodes;
          if (!nodes.TryGetValue(en.Current, out subNodes))
          {
            //subNodes = new TreeNode();
            nodes.Add(en.Current, (subNodes = new TreeNode()));
            nodes = subNodes;
          }
        }

        // now here goes the leaf
        nodes.Leaf = element;
      }
    }

    public TElement Get(DnsDomain domain)
    {
      return Get(domain, DateTime.Now);
    }

    /// <summary>
    /// Find the best matching leaf for a domain.
    /// </summary>
    /// <param name="domain">The domain, considered as a search path.</param>
    /// <param name="now">Reference date determining cache expiration.</param>
    /// <returns></returns>
    public TElement Get(DnsDomain domain, DateTime now)
    {
      if (object.ReferenceEquals(null, domain))
        throw new ArgumentNullException("domain");

      // Best match search:
      // If one searches for "www.example.com", the search will result in the
      // leaf of "example.com" if there is one, but none for "www.example.com".

      // iterate throgh domain labels starting from top level
      using (IEnumerator<DnsLabel> en = domain.GetEnumerator())
      {
        TreeNode nodes = _Nodes;
        while (en.MoveNext())
        {
          TreeNode subNodes;
          if (nodes.TryGetValue(en.Current, out subNodes))
          {
            nodes = subNodes;
            continue;
          }

          // return this leaf if it exists and is not expired
          return nodes.TryGetElement(now);
        }
      }
      return null;
    }

    #endregion

    #region TreeNode

    [Serializable]
    private class TreeNode : IDictionary<DnsLabel, TreeNode>
    {

      private readonly IDictionary<DnsLabel, TreeNode> SubNodes = new Dictionary<DnsLabel, TreeNode>();

      public IExpirableElement<TElement> Leaf { get; set; }

      #region c'tor

      public TreeNode()
      {
      }

      #endregion

      public TElement TryGetElement(DateTime now)
      {
        if (object.ReferenceEquals(null, Leaf))
          return null;
        if (Leaf.IsExpired(now))
        {
          Leaf = null;
          return null;
        }
        return Leaf.Element;
      }

      #region IDictionary<Label, TreeNode>

      public ICollection<DnsLabel> Keys
      {
        get { return SubNodes.Keys; }
      }

      public ICollection<TreeNode> Values
      {
        get { return SubNodes.Values; }
      }

      public TreeNode this[DnsLabel key]
      {
        get { return SubNodes[key]; }
        set { SubNodes[key] = value; }
      }

      public void Add(DnsLabel key, TreeNode value)
      {
        SubNodes.Add(key, value);
      }

      public bool ContainsKey(DnsLabel key)
      {
        return SubNodes.ContainsKey(key);
      }

      public bool Remove(DnsLabel key)
      {
        return SubNodes.Remove(key);
      }

      public bool TryGetValue(DnsLabel key, out TreeNode value)
      {
        return SubNodes.TryGetValue(key, out value);
      }

      public void Add(KeyValuePair<DnsLabel, TreeNode> item)
      {
        SubNodes.Add(item);
      }

      public void Clear()
      {
        SubNodes.Clear();
      }

      public bool Contains(KeyValuePair<DnsLabel, TreeNode> item)
      {
        return SubNodes.Contains(item);
      }

      public void CopyTo(KeyValuePair<DnsLabel, TreeNode>[] array, int arrayIndex)
      {
        SubNodes.CopyTo(array, arrayIndex);
      }

      public int Count
      {
        get { return SubNodes.Count; }
      }

      public bool IsReadOnly
      {
        get { return SubNodes.IsReadOnly; }
      }

      public bool Remove(KeyValuePair<DnsLabel, TreeNode> item)
      {
        return SubNodes.Remove(item);
      }

      public IEnumerator<KeyValuePair<DnsLabel, TreeNode>> GetEnumerator()
      {
        return SubNodes.GetEnumerator();
      }

      System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
      {
        return ((System.Collections.IEnumerable)SubNodes).GetEnumerator();
      }

      #endregion

    }

    #endregion


  }
}

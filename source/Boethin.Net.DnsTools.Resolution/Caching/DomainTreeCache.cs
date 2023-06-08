/*
 * File: Boethin.Net.DnsTools.Resolution/Caching/DomainTreeCache.cs
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

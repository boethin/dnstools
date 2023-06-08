/*
 * File: Boethin.Net.DnsTools.Resolution/Caching/ExpirableElement.cs
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

namespace Boethin.Net.DnsTools.Resolution.Caching
{

  /// <summary>
  /// A generic cache element with expiration time (considered as system local time).
  /// </summary>
  /// <typeparam name="TElement"></typeparam>
  [Serializable]
  public class ExpirableElement<TElement> : IExpirableElement<TElement>
    where TElement : class
  {

    /// <summary>
    /// The expirable element.
    /// </summary>
    private readonly TElement _Element;

    /// <summary>
    /// The absolute value of expiration considered as system local time.
    /// <para>A value of DateTime.MaxValue indicates that the element does not expire at all.</para>
    /// </summary>
    public readonly DateTime Expiration;


    #region c'tor

    /// <summary>
    /// Create a new instance of an expirablke element that does never expire.
    /// </summary>
    /// <param name="element">The expirable element.</param>
    public ExpirableElement(TElement element)
    {
      if (object.ReferenceEquals(null, element))
        throw new ArgumentNullException("element");

      _Element = element;
      Expiration = DateTime.MaxValue;
    }

    /// <summary>
    /// Create a new instance of an expirable element with a given expiration time.
    /// </summary>
    /// <param name="element">The expirable element.</param>
    /// <param name="expiration">The absolute value of expiration considered as system local time.</param>
    public ExpirableElement(TElement element, DateTime expiration)
    {
      if (object.ReferenceEquals(null, element))
        throw new ArgumentNullException("element");

      _Element = element;
      Expiration = expiration;
    }

    #endregion

    #region public

    public TElement Element
    {
      get { return _Element; }
    }

    public bool IsExpired(DateTime now)
    {
      return now > Expiration;
    }

    #endregion

    #region IExpirableElement<TElement>

    TElement IExpirableElement<TElement>.Element
    {
      get { return ((ExpirableElement<TElement>)this).Element; }
    }

    bool IExpirableElement<TElement>.IsExpired(DateTime now)
    {
      return ((ExpirableElement<TElement>)this).IsExpired(now);
    }

    #endregion

  }

}

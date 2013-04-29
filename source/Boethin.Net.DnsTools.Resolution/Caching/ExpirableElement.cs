/*
 * File: Boethin.Net.DnsTools.Resolution/Caching/ExpirableElement.cs
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
    public readonly TElement Element;

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

      Element = element;
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

      Element = element;
      Expiration = expiration;
    }

    #endregion

    #region IExpirableElement<TElement>

    TElement IExpirableElement<TElement>.Element
    {
      get { return Element; }
    }

    bool IExpirableElement<TElement>.IsExpired(DateTime now)
    {
      return now > Expiration;
    }

    #endregion

  }

}

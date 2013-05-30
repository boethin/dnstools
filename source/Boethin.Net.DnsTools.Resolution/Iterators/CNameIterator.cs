/*
 * File: Boethin.Net.DnsTools.Resolution/Iterators/CNameIterator.cs
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

namespace Boethin.Net.DnsTools.Resolution.Iterators
{

  [Serializable]
  public class CNameIterator : ResolutionIterator
  {

    internal CNameIterator(ResolutionIterator iterator, DnsDomain domain)
      : base(new DomainResolver(iterator.Resolver.Options, domain), 
      iterator.Question, iterator.AddressCache, iterator.NestingLevel + 1)
    {
    }


  }
}

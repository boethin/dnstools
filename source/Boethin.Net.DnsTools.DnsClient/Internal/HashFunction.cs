/*
 * File: Boethin.Net.DnsTools.DnsClient/Internal/HashFunction.cs
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

namespace Boethin.Net.DnsTools.DnsClient.Internal
{

  // Bob Jenkins hash function with 96-bit internal state. 
  // http://bretm.home.comcast.net/~bretm/hash/7.html

  internal class HashFunction
  {

    uint a, b, c;

    private void Mix()
    {
      a -= b; a -= c; a ^= (c >> 13);
      b -= c; b -= a; b ^= (a << 8);
      c -= a; c -= b; c ^= (b >> 13);
      a -= b; a -= c; a ^= (c >> 12);
      b -= c; b -= a; b ^= (a << 16);
      c -= a; c -= b; c ^= (b >> 5);
      a -= b; a -= c; a ^= (c >> 3);
      b -= c; b -= a; b ^= (a << 10);
      c -= a; c -= b; c ^= (b >> 15);
    }

    public int ComputeHash(byte[] data)
    {
      int len = data.Length;
      a = b = 0x9e3779b9;
      c = 0;
      int i = 0;
      while (i + 12 <= len)
      {
        a += (uint)data[i++] |
            ((uint)data[i++] << 8) |
            ((uint)data[i++] << 16) |
            ((uint)data[i++] << 24);
        b += (uint)data[i++] |
            ((uint)data[i++] << 8) |
            ((uint)data[i++] << 16) |
            ((uint)data[i++] << 24);
        c += (uint)data[i++] |
            ((uint)data[i++] << 8) |
            ((uint)data[i++] << 16) |
            ((uint)data[i++] << 24);
        Mix();
      }
      c += (uint)len;
      if (i < len)
        a += data[i++];
      if (i < len)
        a += (uint)data[i++] << 8;
      if (i < len)
        a += (uint)data[i++] << 16;
      if (i < len)
        a += (uint)data[i++] << 24;
      if (i < len)
        b += (uint)data[i++];
      if (i < len)
        b += (uint)data[i++] << 8;
      if (i < len)
        b += (uint)data[i++] << 16;
      if (i < len)
        b += (uint)data[i++] << 24;
      if (i < len)
        c += (uint)data[i++] << 8;
      if (i < len)
        c += (uint)data[i++] << 16;
      if (i < len)
        c += (uint)data[i++] << 24;
      Mix();
      return (int)c;
    }

  }
}

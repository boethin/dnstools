/*
 * File: Boethin.Net.DnsTools.DnsClient/Internal/HashFunction.cs
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

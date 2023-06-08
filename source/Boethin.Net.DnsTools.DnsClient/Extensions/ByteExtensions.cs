/*
 * File: Boethin.Net.DnsTools.DnsClient/Extensions/ByteExtensions.cs
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

namespace Boethin.Net.DnsTools.DnsClient.Extensions
{
  public static class ByteExtensions
  {

    // Byte conversion extensions using network order (big-endian).
    //
    // System.BitConverter is not suitable for dealing with numbers in byte
    // streams because it's byte ordering depends on the current architecture.


    /// <summary>
    /// Convert a 16bit integer to a 2-byte array using network order (big-endian).
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static byte[] GetNetworkBytes(this UInt16 value)
    {
      return new byte[] { 
        (byte)((value & 0xFF00) >> 8), (byte)(value & 0xFF) };
    }

    /// <summary>
    /// Convert a 2-byte array to a 16bit integer, assuming network order (big-endian).
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static UInt16 AsNetworkUint16(this byte[] value)
    {
      if (value.Length < 2)
        throw new ArgumentException("The operation is only valid for arrays of at least 2 bytes.");
      return (UInt16)(value[0] << 8 | value[1]);
    }




  }
}

/*
 * File: Boethin.Net.DnsTools.DnsClient/Extensions/ByteExtensions.cs
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

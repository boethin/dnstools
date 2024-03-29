/*
 * File: Boethin.Net.DnsTools.DnsClient/Datagram.cs
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
using System.IO;
using Boethin.Net.DnsTools.DnsClient.Extensions;

namespace Boethin.Net.DnsTools.DnsClient
{

  /// <summary>
  /// A Datagram is a struct representing a byte array.
  /// </summary>
  [Serializable]
  public struct Datagram : IEnumerable<byte>
  {

    #region private

    // may be null if one uses the default constructor
    private readonly byte[] Bytes;

    #endregion

    #region Empty

    /// <summary>
    /// The 0-byte datagram.
    /// </summary>
    public static readonly Datagram Empty = new Datagram(0);

    #endregion

    #region public get

    /// <summary>
    /// Whether or not the underlying byte array is empty.
    /// </summary>
    public bool IsEmpty
    {
      get { return object.ReferenceEquals(null, Bytes) || Bytes.Length == 0; }
    }

    /// <summary>
    /// The length of the underlying byte array.
    /// </summary>
    public int Length
    {
      get { return object.ReferenceEquals(null, Bytes) ? 0 : Bytes.Length; }
    }

    #endregion

    #region c'tor

    // Structs cannot contain explicit parameterless constructors	

    public Datagram(int length)
    {
      Bytes = new byte[length];
    }

    public Datagram(byte[] bytes)
    {
      Bytes = bytes;
    }

    #endregion

    #region public

    public byte this[int index]
    {
      get { return GetByteAt(index); }
    }

    #endregion

    #region IEnumerable<byte>

    public IEnumerator<byte> GetEnumerator()
    {
      return Bytes.AsEnumerable().GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return Bytes.GetEnumerator();
    }

    #endregion

    #region ToString

    private const string EmptyToString = "[<empty>]";

    public string ToLengthString()
    {
      if (IsEmpty)
        return EmptyToString;
      return String.Format("[{0} bytes]", Length);
    }

    public string ToHexString()
    {
      if (IsEmpty)
        return EmptyToString;
      return String.Join(" ", Bytes.Select(b => String.Format("{0:x2}", b)).ToArray());
    }

    public string ToQuotedString(int maxLength = int.MaxValue)
    {
      StringBuilder sb = new StringBuilder();
      sb.Append('"');
      bool ellipsed = false;
      foreach (byte v in Bytes)
      {
        if (v == '\\') // escape backslash
          sb.Append("\\\\");
        else if (v == '"') // escape double quote
          sb.Append("\\\"");
        else if (v >= 0x20 && v <= 0x7A) // printable ASCII
          sb.Append((char)v);
        else // display any other value as a perl-like escape sequence
          sb.Append(String.Format("\\x{0:x2}", (int)v));
        if (sb.Length >= maxLength)
        {
          sb.Append("[...]");
          ellipsed = true;
        }
      }
      sb.Append('"');
      if (ellipsed)
        sb.AppendFormat(" {0}", ToLengthString());
      return sb.ToString();
    }

    #endregion

    #region override

    public override string ToString()
    {
      return ToLengthString();
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals(null, obj))
        return false;
      if (obj.GetType().Equals(typeof(byte[])))
        return Equals(new Datagram((byte[])obj));
      if (obj.GetType().Equals(typeof(Datagram)))
        return Equals((Datagram)obj);
      return false;
    }

    public bool Equals(Datagram data)
    {
      if (IsEmpty)
        return data.IsEmpty;
      if (Length != data.Length)
        return false;
      for (int i = 0; i < Bytes.Length; i++)
      {
        if (Bytes[i] != data.Bytes[i])
          return false;
      }
      return true;
    }

    public override int GetHashCode()
    {
      return new Internal.HashFunction().ComputeHash(Bytes);
    }

    #endregion

    #region private

    private byte GetByteAt(int index)
    {
      if (IsEmpty || !(0 <= index && index < Bytes.Length))
        throw new IndexOutOfRangeException(
          "Index was outside the bounds of the array.");
      return Bytes[index];
    }

    #endregion

    #region operator

    public static bool operator ==(Datagram a, Datagram b)
    {
      return Equals(a, b);
    }

    public static bool operator !=(Datagram a, Datagram b)
    {
      return !Equals(a, b);
    }

    public static implicit operator byte[](Datagram value)
    {
      if (value.IsEmpty)
        return new byte[0];
      return value.Bytes;
    }

    public static explicit operator Datagram(byte[] value)
    {
      if (object.ReferenceEquals(null, value) || value.Length == 0)
        return Empty;
      return new Datagram(value);
    }

    #endregion

    #region public static

    public static bool Equals(Datagram a, Datagram b)
    {
      // If both are null, or both are same instance, return true.
      if (System.Object.ReferenceEquals(a, b))
      {
        return true;
      }

      // If one is null, but not both, return false.
      if (((object)a == null) || ((object)b == null))
      {
        return false;
      }

      return a.Equals(b);
    }

    #endregion

    #region internal static

    // Copy the datagram to a new one prefixed with a two byte length field as used by TCP transport.
    internal static Datagram GetPrefixed(Datagram data)
    {
      if (data.Length > ushort.MaxValue)
        throw new OverflowException("The length of the datagram exceeds two bytes.");

      Datagram result = new Datagram(data.Length + 2);
      ushort length = (ushort)data.Length;
      byte[] prefix = length.GetNetworkBytes();
      prefix.CopyTo(result.Bytes, 0);
      data.Bytes.CopyTo(result.Bytes, 2);
      return result;
    }

    #endregion

  }
}

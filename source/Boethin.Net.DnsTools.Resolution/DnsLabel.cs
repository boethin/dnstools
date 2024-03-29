/*
 * File: Boethin.Net.DnsTools.Resolution/DnsLabel.cs
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

namespace Boethin.Net.DnsTools.Resolution
{

  [Serializable]
  public class DnsLabel
  {

    #region private

    private readonly string _Label;

    #endregion

    #region c'tor

    private DnsLabel(string label)
    {
      _Label = label;
    }

    #endregion

    #region override

    public override string ToString()
    {
      return _Label;
    }

    public override bool Equals(object obj)
    {
      DnsLabel label;
      if (object.ReferenceEquals(null, (label = obj as DnsLabel)))
      {
        string str;
        if (object.ReferenceEquals(null, (str = obj as string)))
          return false;
        return Equals(str);
      }
      return Equals(label);
    }

    public bool Equals(string str)
    {
      DnsLabel label;
      if (String.IsNullOrEmpty(str))
        return false;
      if (!TryParse(str, out label))
        return false;
      return Equals(label);
    }

    public bool Equals(DnsLabel label)
    {
      if (object.ReferenceEquals(null, label))
        return false;
      return _Label.Equals(label._Label);
    }

    public override int GetHashCode()
    {
      return _Label.GetHashCode();
    }

    #endregion

    #region operator

    public static implicit operator string(DnsLabel value)
    {
      return value._Label;
    }

    public static explicit operator DnsLabel(string value)
    {
      return Parse(value);
    }

    #endregion

    #region static Parse

    public static DnsLabel Parse(string input)
    {
      if (object.ReferenceEquals(null, input))
        throw new ArgumentNullException("input");

      input = input.Trim();

      if (input.Length == 0)
        throw new ArgumentException("Label cannot by empty.");
      if (input.Length > 63)
        throw new ArgumentException("Label too long (more than 63 caharacters).");

      string check;
      input = GetNormalized(input, out check);

      if (check.Length > 0)
      {
        if (check[0] == '-')
          throw new ArgumentException(
            "A label must not start with a leading hyphen minus '-' character.");
        if (check[check.Length - 1] == '-')
          throw new ArgumentException(
            "A label must not end with a trailing hyphen minus '-' character.");
        foreach (int c in check.ToCharArray().Select(c => (int)c))
        {
          if (!IsLDH(c))
            throw new ArgumentException(String.Format(
              "Label contains an invalid character: U+{0:X4}.", c));
        }
      }

      return new DnsLabel(input);
    }

    public static bool TryParse(string input, out DnsLabel label)
    {
      if (object.ReferenceEquals(null, input))
        throw new ArgumentNullException("input");

      label = null;
      input = input.Trim();

      if (input.Length == 0)
        return false;

      if (input.Length > 63)
        return false;

      string check;
      input = GetNormalized(input, out check);

      if (check.Length > 0)
      {
        if (check[0] == '-')
          return false;
        if (check[check.Length - 1] == '-')
          return false;
        foreach (int c in check.ToCharArray().Select(c => (int)c))
        {
          if (!IsLDH(c))
            return false;
        }
      }

      label = new DnsLabel(input);
      return true;
    }

    private static string GetNormalized(string input, out string check)
    {
      // DNS labels are case insensitive (even the underscore things).
      input = input.ToLower();

      // DNS label may start with one underscore.

      // [RFC 2782]
      // The symbolic name of the desired protocol, with an underscore
      // (_) prepended to prevent collisions with DNS labels that occur
      // in nature.
      check = input;
      if (check.Length > 0 && check[0] == '_')
        check = check.Substring(1);
      return input;
    }

    private static bool IsLDH(int c)
    {
      // LDH characters:
      // 0041..005A and 0061..007A), digits (0030..0039), and the hyphen-minus (U+002D)
      return (('A' <= c && c <= 'Z') || ('a' <= c && c <= 'z') || ('0' <= c && c <= '9') || c == '-');
    }

    #endregion

  }


}

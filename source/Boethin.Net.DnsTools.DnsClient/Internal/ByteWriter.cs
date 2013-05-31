/*
 * File: Boethin.Net.DnsTools.DnsClient/Internal/ByteWriter.cs
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
using System.IO;
using System.Text;

namespace Boethin.Net.DnsTools.DnsClient.Internal
{
  internal class ByteWriter
  {

    #region private

    private readonly Stream Message;

    #endregion

    #region c'tor

    public ByteWriter(Stream message)
    {
      if (object.ReferenceEquals(null, message))
        throw new ArgumentNullException("message");
      if (!message.CanWrite)
        throw new ArgumentException(
          "The stream must be writable.", "message");
      Message = message;
    }

    #endregion

    #region public

    public void WriteByte(byte value)
    {
      Message.WriteByte(value);
    }

    public void WriteUInt16(ushort value)
    {
      WriteUInt16(Message, value);
    }

    public void WriteUInt16Enum<TUInt16>(TUInt16 value)
    {
      WriteUInt16((ushort) Enum.ToObject(typeof(TUInt16), value));
    }

    public void WriteUInt32(uint value)
    {
      WriteUInt16((ushort)((value & 0xFFFF0000) >> 16));
      WriteUInt16((ushort)(value & 0xFFFF));
    }

    public void WriteRequestFlags(DNS.OPCODE opcode, bool rd)
    {
      // only OPCODE and RD may be non-zero
      WriteUInt16((ushort)((((int)opcode & 0xF) << 11) | ((rd ? 1 : 0) << 8)));
    }

    public void WriteLabel(string value, int charIndex, int count)
    {
      byte[] buf = Encoding.ASCII.GetBytes(value.ToCharArray(), charIndex, count);
      if (buf.Length > (int)byte.MaxValue)
        throw new OverflowException(String.Format(
          "Label length of {0} exceeds byte size.", buf.Length));
      Message.WriteByte((byte)buf.Length); // length octet
      Message.Write(buf, 0, buf.Length); // string octets
    }

    public void WriteDomain(string domain)
    {
      // a domain name represented as a sequence of labels, where
      // each label consists of a length octet followed by that
      // number of octets.  The domain name terminates with the
      // zero length octet for the null label of the root.
      for (int position = 0; position < domain.Length; position++)
      {
        int length = domain.IndexOf('.', position) - position;
        if (length < 0)
          length = domain.Length - position;
        WriteLabel(domain, position, length);
        position += length;
      }
      Message.WriteByte((byte)0);
    }

    #endregion

    #region static

    public static void WriteUInt16(Stream s, ushort value)
    {
      s.Write(new byte[] { 
        (byte)((value & 0xFF00) >> 8), (byte)(value & 0xFF) }, 0, 2);
    }

    #endregion

  }
}

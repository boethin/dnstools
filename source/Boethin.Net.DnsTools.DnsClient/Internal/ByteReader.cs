/*
 * File: Boethin.Net.DnsTools.DnsClient/Internal/ByteReader.cs
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
using System.IO;

namespace Boethin.Net.DnsTools.DnsClient.Internal
{
  internal class ByteReader
  {

    private const ushort CompressionFlags = 0xC0; // 1100 0000

    private const ushort OffsetMask = 0x3F; // 0011 1111

    private readonly Stream _Message;


    public ByteReader(Stream message)
    {
      _Message = message;
    }

    public byte ReadByte()
    {
      return (byte)_Message.ReadByte();
    }

    public char ReadChar()
    {
      return (char)ReadByte();
    }

    public Datagram ReadBytes(int count)
    {
      Datagram result = new Datagram(count);
      if (_Message.Read(result, 0, count) < count)
        throw new InvalidOperationException("Stream ended unexpectedly.");
      return result;
    }

    public ushort ReadUIn16()
    {
      //byte hi = ReadByte(), lo = ReadByte();
      //return (ushort)(hi << 8 | lo);
      return ReadUIn16(_Message);
    }

    public TUInt16 ReadUIn16Enum<TUInt16>()
    {
      ushort v = ReadUIn16();
      if (!Enum.IsDefined(typeof(TUInt16), v))
        throw new InvalidOperationException(String.Format(
          "Undefined value {0} for enum type {1}.", v, typeof(TUInt16).FullName));
      return (TUInt16)Enum.ToObject(typeof(TUInt16), v);
    }

    public uint ReadUint32()
    {
      ushort hi = ReadUIn16(), lo = ReadUIn16();
      return (uint)(hi << 16 | lo);
    }

    //public void ReadResponseFlags(DNS.Header header)
    //{
    //  ushort flags = ReadUIn16();

    //  int qr = flags & 0x8000;       // 1000000000000000
    //  if (qr == 0)
    //    throw new InvalidOperationException(
    //      "Zero QR flag in response header.");
    //  header.QR = DNS.QR.R;

    //  int opcode = (flags & 0x7800) >> 10;   //  111100000000000
    //  if (!Enum.IsDefined(typeof(DNS.OPCODE), (byte)opcode))
    //    throw new InvalidOperationException(String.Format(
    //      "Unexpected OPCODE {0} within response header.", opcode));
    //  header.OPCODE = (DNS.OPCODE)opcode;

    //  int aa = flags & 0x400;                //      10000000000
    //  header.AA = (aa != 0);

    //  int tc = flags & 0x200;         //       1000000000
    //  header.TC = (tc != 0);

    //  int rd = flags & 0x100;         //        100000000
    //  header.RD = (rd != 0);

    //  int ra = flags & 0x80;         //         10000000
    //  header.RA = (ra != 0);

    //  int rcode = flags & 0xF;      //             1111
    //  if (rcode > (int)DNS.RCODE.Other)
    //    rcode = (int)DNS.RCODE.Other;
    //  header.RCODE = (DNS.RCODE)rcode;
    //  header.RCODEValue = (byte)rcode;

    //}

    public Datagram ReadCharacterString()
    {
      // <character-string> is a single length octet followed by that number of characters.

      // Unlike a domain label, a <character-string> is not guaranteed to contain only of ASCII.
      byte length = ReadByte();
      if (length > 0)
        return ReadBytes(length);
      return Datagram.Empty;
    }

    private string ReadAscii(int length)
    {
      return Encoding.ASCII.GetString(ReadBytes(length));
      //byte[] buf = new byte[length];
      //_Message.Read(buf, 0, length);
      //return Encoding.ASCII.GetString(buf);
    }

    //public Datagram ReadString()
    //{
    //  byte length = ReadByte();
    //  if (length > 0)
    //  {
    //    return ReadBytes(length);
    //  }
    //  return new Datagram();
    //}

    public string ReadDomain()
    {
      StringBuilder result = new StringBuilder();

      // <domain-name> is a domain name represented as a series of labels, and
      // terminated by a label with zero length.

      // a domain name represented as a sequence of labels, where
      // each label consists of a length octet followed by that
      // number of octets.  The domain name terminates with the
      // zero length octet for the null label of the root.

      int length;
      while ((length = ReadByte()) != 0)
      {
        if ((length & CompressionFlags) == CompressionFlags) // 1100 0000
        {
          // 4.1.4. Message compression

          // The pointer takes the form of a two octet sequence:
          //
          //     +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
          //     | 1  1|                OFFSET                   |
          //     +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
          long offset = (long)((length & OffsetMask) << 8 | ReadByte());

          // recursively seek pointer position
          long pos = _Message.Position;
          _Message.Seek(offset, SeekOrigin.Begin);
          try
          {
            string domain = (new ByteReader(_Message)).ReadDomain();
            result.Append(domain);
          }
          finally
          {
            _Message.Seek(pos, SeekOrigin.Begin);
          }
          return result.ToString();
        }

        if (length > 0)
        {
          result.Append(ReadAscii(length));
        }

        // if size of next label isn't null (end of domain name) add a period ready for next label
        result.Append('.');
      }

      if (result.Length == 0) // the empty domain is the root
        result.Append('.');

      return result.ToString();
    }

    public void SeekForward(long offset)
    {
      _Message.Seek(offset, SeekOrigin.Current);
    }


    public static ushort ReadUIn16(Stream s)
    {
      //int hi, lo;
      //if (0 > (hi = s.ReadByte()) || 0 > (lo = s.ReadByte()))
      //  throw new Exception("Unexpected end of stream.");
      //return (ushort)(((hi << 8) & 0xFF00) | (lo & 0x00FF));

      byte[] buf = new byte[2];
      if (s.Read(buf, 0, 2) < 2)
        throw new Exception("Unexpected end of stream.");
      return (ushort)(buf[0] << 8 | buf[1]);
    }


  }
}

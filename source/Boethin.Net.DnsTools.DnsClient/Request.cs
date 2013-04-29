/*
 * File: Boethin.Net.DnsTools.DnsClient/Request.cs
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
using System.Linq;

namespace Boethin.Net.DnsTools.DnsClient
{

  [Serializable] 
  public class Request : DNS.Message, Internal.IRequestWriter
  {

    #region private

    private Nullable<Datagram> _Data = null;

    #endregion

    #region public get

    public override Datagram Data
    {
      get
      {
        if (_Data == null)
          _Data = CreateData();
        return (Datagram)_Data;
      }
    }

    #endregion

    #region c'tor

    public Request(ushort id, bool rd, DNS.OPCODE opcode, DNS.Question question)
    {
      Header.ID = id;
      Header.QR = DNS.QR.Q;
      Header.RD = rd;
      Header.OPCODE = opcode;
      Questions.Add(question);
    }

    #endregion

    #region IRequestWriter

    void Internal.IRequestWriter.WriteRequest(Internal.ByteWriter writer)
    {
      // header
      if (Questions.Count > (int)ushort.MaxValue)
        throw new InvalidOperationException("Too many questions.");
      Header.QDCOUNT = (ushort)Questions.Count;
      ((Internal.IRequestWriter)Header).WriteRequest(writer);

      // question section
      foreach (Internal.IRequestWriter q in Questions.Select(q => (Internal.IRequestWriter)q))
      {
        q.WriteRequest(writer);
      }
    }

    #endregion

    #region override

    public override string ToString()
    {
      return string.Format("{0} RD={1}: {2}",
        Header.OPCODE.ToString(), (Header.RD ? "1" : "0"), Questions.First().ToString());
    }

    #endregion

    #region private

    private Datagram CreateData()
    {
      // create message datgram
      using (MemoryStream ms = new MemoryStream())
      {
        ((Internal.IRequestWriter)this).WriteRequest(new Internal.ByteWriter(ms));
        Datagram data = new Datagram((int)ms.Length);
        ms.Seek(0L, SeekOrigin.Begin);
        if (ms.Read(data, 0, data.Length) < data.Length)
          throw new EndOfStreamException("Cannot read from memory stream.");
        return data;
      }
    }

    #endregion

  }
}

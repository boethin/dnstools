/*
 * File: Boethin.Net.DnsTools.DnsClient/MessageEvent.cs
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
using System.Net;

namespace Boethin.Net.DnsTools.DnsClient
{

  public delegate void MessageEventHandler(object sender, MessageEventArgs e);

  public class MessageEventArgs : System.EventArgs
  {

    private readonly DNS.Message _Message;

    private readonly Stream _Data;

    private readonly IPEndPoint _EndPoint;


    public Stream Data
    {
      get { return _Data; }
    }

    public DNS.Message Message
    {
      get { return _Message; }
    }

    public IPEndPoint EndPoint
    {
      get { return _EndPoint; }
    }

    internal MessageEventArgs(DNS.Message message, Stream data, IPEndPoint ep)
    {
      _Message = message;
      _Data = data;
      _EndPoint = ep;
    }

  }
}

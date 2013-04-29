/*
 * File: Boethin.Net.DnsTools.DnsClient/IDnsClient.cs
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
using System.Net;

namespace Boethin.Net.DnsTools.DnsClient
{
  public interface IDnsClient : IDisposable
  {

    //event MessageEventHandler RequestSending;

    //event MessageEventHandler ResponseReceived;

    event Logging.LogMessageEventHandler LogMessageCreated;

    NetworkProtocol NetworkProtocol { get; }
    
    bool Connected { get; }

    void Connect(IPAddress address);

    void Connect(IPEndPoint endpoint);

    void Close();

    Response LookUp(Request request);

    IAsyncResult BeginLookUp(Request request, AsyncCallback callback, object state);

    Response EndLookUp(IAsyncResult asyncResult);

  }
}

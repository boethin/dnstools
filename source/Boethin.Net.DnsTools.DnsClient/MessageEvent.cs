/*
 * File: Boethin.Net.DnsTools.DnsClient/MessageEvent.cs
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
using System.IO;
using System.Net;

namespace Boethin.Net.DnsTools.DnsClient
{

  public delegate void MessageEventHandler(object sender, MessageEventArgs e);

  public class MessageEventArgs : System.EventArgs
  {

    #region private readonly

    private readonly DNS.Message _Message;

    private readonly Stream _Data;

    private readonly IPEndPoint _EndPoint;

    #endregion

    #region public

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

    #endregion

    #region c'tor

    internal MessageEventArgs(DNS.Message message, Stream data, IPEndPoint ep)
    {
      _Message = message;
      _Data = data;
      _EndPoint = ep;
    }

    #endregion

  }
}

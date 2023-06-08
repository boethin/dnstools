/*
 * File: Boethin.Net.DnsTools.Resolution/NameServerEvent.cs
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

namespace Boethin.Net.DnsTools.Resolution
{

  public delegate void NameServerEventHandler(object sender, NameServerEventArgs e);

  public class NameServerEventArgs : System.EventArgs
  {

    private readonly NameServer _NameServer;

    private readonly DnsClient.DNS.QTYPE _QuestionType;

    private readonly string _Domain;


    public NameServer NameServer
    {
      get { return _NameServer; }
    }

    public DnsClient.DNS.QTYPE QuestionType
    {
      get { return _QuestionType; }
    }

    public string Domain
    {
      get { return _Domain; }
    }

    internal NameServerEventArgs(NameServer server, DnsClient.DNS.QTYPE question, string domain)
    {
      _NameServer = server;
      _QuestionType = question;
      _Domain = domain;
    }

    public override string ToString()
    {
      return String.Format("{0}: {1} {2}", 
        _NameServer.ToString(), _QuestionType.ToString(), _Domain.ToString());
    }

  }
}

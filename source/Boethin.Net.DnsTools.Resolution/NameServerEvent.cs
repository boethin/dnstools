/*
 * File: Boethin.Net.DnsTools.Resolution/NameServerEvent.cs
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

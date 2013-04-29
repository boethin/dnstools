/*
 * File: Boethin.Net.DnsTools.DnsClient/InvalidResponseException.cs
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

namespace Boethin.Net.DnsTools.DnsClient
{

  /// <summary>
  /// An exception of this type is thrown when a response from the server was not understood by the 
  /// client or would lead to inconsistent response data. This meight point either to a misconfigured 
  /// name server or a serious bug in the client software.
  /// </summary>
  public class InvalidResponseException : System.Exception
  {

    internal InvalidResponseException(string message)
      : base(message)
    { 
    }

    internal InvalidResponseException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

  }
}

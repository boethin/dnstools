/*
 * File: Boethin.Net.DnsTools.Resolution/Options.cs
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

namespace Boethin.Net.DnsTools.Resolution
{

  [Serializable]
  public sealed class Options
  {

    /// <summary>
    /// Whether or not to use in-memory cache for top level name servers. The root server cache is
    /// maintained during application life-time.
    /// <para>This option is recommended for performance reasons, since root server responses rarely change.</para>
    /// </summary>
    public bool UseRootCache { get; set; }

    /// <summary>
    /// Whether or not to accept address records only from authoritative answers, i.e. ignore 
    /// address records given by additional records from non-authoritative answers.
    /// <para>This option may considerably increase the amount of requests within an iterative resolution.</para> 
    /// </summary>
    public bool StrictAuhtoritative { get; set; }

    /// <summary>
    /// IP version flag.
    /// <para>IPv6 is prefered if both IPv4 and IPv6 flags are set (DualStack), 
    /// otherwise the specified IP version is required.</para>
    /// </summary>
    public IPVersion IPVersion { get; set; }

    /// <summary>
    /// Whether or not to repeat a name server request once if the TRUNCATED flag was set in the response.
    /// <para>This option gives the chance to reconnect the resolver with a TCP client before 
    /// performing the next step in order to get a response that exceeds 512 bytes.</para>
    /// </summary>
    public bool RepeatTruncated { get; set; }

    /// <summary>
    /// Whether or not the resolver should pursue aliases. That is, if a resolution process 
    /// ends up without the requested RR types but with a CNAME RR, the resolver can start a
    /// new iteration with the same question onn  the given canonical name.
    /// </summary>
    public bool FollowCanonical { get; set; }

    /// <summary>
    /// If the given maximum amount of iterations is reached during a resultion, an exception will be thrown.
    /// <para>If the value is not greater than zero, loop control is disabled.</para>
    /// </summary>
    public int IterationLoopControl { get; set; }

    /// <summary>
    /// Default option set, suitable for the most standard cases.
    /// </summary>
    public static readonly Options Default = new Options
    {
      UseRootCache = true,
      StrictAuhtoritative = false,
      IPVersion = IPVersion.IPv4,
      RepeatTruncated = true,
      FollowCanonical = true,
      IterationLoopControl = 25
    };


  }
}

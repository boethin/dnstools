/*
 * File: Boethin.Net.DnsTools.Resolution/Options.cs
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

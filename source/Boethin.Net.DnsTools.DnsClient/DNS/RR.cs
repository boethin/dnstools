/*
 * File: Boethin.Net.DnsTools.DnsClient/DNS/RR.cs
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
using System.Reflection;
using System.Threading;

namespace Boethin.Net.DnsTools.DnsClient.DNS
{

  /// <summary>
  /// The bese class for all resource records.
  /// </summary>
  [Serializable] 
  public abstract class RR
  {

    // [RFC 1035]
    // 4.1.3. Resource record format
    //
    // The answer, authority, and additional sections all share the same
    // format: a variable number of resource records, where the number of
    // records is specified in the corresponding count field in the header.
    // Each resource record has the following format:
    //                                     1  1  1  1  1  1
    //       0  1  2  3  4  5  6  7  8  9  0  1  2  3  4  5
    //     +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    //     |                                               |
    //     /                                               /
    //     /                      NAME                     /
    //     |                                               |
    //     +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    //     |                      TYPE                     |
    //     +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    //     |                     CLASS                     |
    //     +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    //     |                      TTL                      |
    //     |                                               |
    //     +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    //     |                   RDLENGTH                    |
    //     +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--|
    //     /                     RDATA                     /
    //     /                                               /
    //     +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

    #region private

    // Anything except RDATA
    private readonly RRBase _Base;

    #endregion

    #region public get

    /// <summary>
    /// The basic data common to all resource records.
    /// </summary>
    public RRBase Base
    {
      get { return _Base; }
    }

    #endregion
    
    #region c'tor

    internal RR(RRBase rrbase)
    {
      _Base = rrbase;
    }

    #endregion

    #region override

    public override string ToString()
    {
      return ToString(0);
    }

    public abstract string ToString(int namePadding);

    /// <summary>
    /// Determines whether or not the specified object is equal to the current instance.
    /// </summary>
    /// <param name="obj">The object to compare with.</param>
    /// <returns></returns>
    public override bool Equals(object obj)
    {
      return Equals(obj as RR);
    }

    /// <summary>
    /// Determines whether or not the specified resource record is equal to the current instance.
    /// </summary>
    /// <param name="rr">The resource record to compare with.</param>
    /// <returns></returns>
    public bool Equals(RR rr)
    {
      if (object.ReferenceEquals(null, rr))
        return false;
      if (false == GetType().Equals(rr.GetType()))
        return false;
      if (false == Base.Equals(rr.Base))
        return false;
      return this.EqualsRDATA(rr);
    }

    public override int GetHashCode()
    {
      return Base.GetHashCode();
    }

    #endregion

    #region RDATA

    internal abstract void ReadRDATA(Internal.ByteReader reader);

    protected abstract bool EqualsRDATA(RR rr);

    #endregion

    #region internal static

    internal static RR CreateInstance(RRBase rrbase)
    {
      RR result;

      // Each RR type must have the same namespace and
      // the name of an RR type must coincide with a TYPE value.
      string typename;
      if (rrbase.TYPE != null)
        typename = typeof(Records.Default).Namespace + '.' + rrbase.TYPE.ToString();
      else
        typename = typeof(Records.Default).FullName; // unknown type

      Assembly assembly = Assembly.GetExecutingAssembly();
      try
      {
        // if the required type is not implemented, fall back to Defaulz
        Type type;
        if (null == (type = assembly.GetType(typename)))
          type = typeof(Records.Default);

        try
        {
          // Each RR type must provide an internal c'tor taking one RRBase argument.
          result = Activator.CreateInstance(
            type, BindingFlags.Instance | BindingFlags.NonPublic, null,
            new object[] { rrbase }, null) as RR;
        }
        catch (System.MissingMethodException ex)
        {
          throw new InvalidOperationException(
            "Each RR type must provide an internal c'tor taking one RRBase argument.", ex);
        }
      }
      catch (Exception ex)
      {
        throw new TypeLoadException(String.Format(
          "Instance of type '{0}' could not be created from assembly {1}.",
          typename, assembly.ToString()), ex);
      }
      return result;
    }

    #endregion

  }
}

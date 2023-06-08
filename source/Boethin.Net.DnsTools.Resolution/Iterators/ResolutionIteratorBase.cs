/*
 * File: Boethin.Net.DnsTools.Resolution/Iterators/ResolutionIteratorBase.cs
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
using Boethin.Net.DnsTools.Resolution.Extensions;

namespace Boethin.Net.DnsTools.Resolution.Iterators
{

  [Serializable]
  public abstract class ResolutionIteratorBase : IEnumerator<Results.ResolutionResult>
  {

    #region private

    private readonly DomainResolver _Resolver;

    private readonly DnsClient.DNS.QTYPE _Question;

    private Results.ResolutionResult CurrentResult = null;

    private int _IterationCount = 0;

    private bool IsCompleted = false;

    #endregion

    #region protected

    internal readonly Caching.AddressCache AddressCache;

    protected NameServerCollection StoredAuthorities = null;

    protected int IterationCount
    {
      get { return _IterationCount; }
    }

    protected Results.ResolutionResult LastResult
    {
      get 
      {
        return CurrentResult;
      }
    }

    #endregion

    #region public get

    public DnsClient.DNS.QTYPE Question
    {
      get { return _Question; }
    }

    public DomainResolver Resolver
    {
      get { return _Resolver; }
    }

    public Results.ResolutionResult Current
    {
      get
      {
        if (StoredAuthorities == null)
          throw new InvalidOperationException("Enumeration has not started.");
        if (CurrentResult == null)
          throw new InvalidOperationException("Enumeration already finished.");
        return CurrentResult;
      }
    }

    #endregion

    #region c'tor

    internal ResolutionIteratorBase(DomainResolver resolver, DnsClient.DNS.QTYPE question, Caching.AddressCache addressCache)
    {
      _Resolver = resolver;
      _Question = question;
      AddressCache = addressCache;
    }

    #endregion

    #region public

    public bool MoveNext(NameServer selected = null)
    {

      // Verify the selected name server occurs within the NextAuthorities in the previous result.
      if (!object.ReferenceEquals(null, selected))
      {
        DnsDomain selectedName = selected.Name;
        try
        {
          // Throws if either NullReferenceException or InvalidOperationException on failure.
          selected = CurrentResult.NextAuthorities.First(n => n.Name.Equals(selectedName));
        }
        catch (Exception ex)
        {
          throw new ArgumentException(String.Format(
            "The selected name server was not taken from the NextAuthorities in the precious result: {0}", 
            selectedName.ToString()), "selected", ex);
        }
      }

      if (IsCompleted)
        return false;

      // Apply loop control.
      if (_Resolver.Options.IterationLoopControl > 0)
      {
        if (_IterationCount >= Resolver.Options.IterationLoopControl)
          throw new ResolverException("Maximum number of iterations has been reached.");
      }

      // next
      try
      {
        CurrentResult = GetNextResult(out IsCompleted, selected);
      }
      finally
      {
        _IterationCount++;
      }
      return true;
    }

    public virtual void Reset()
    {
      CurrentResult = null;
      IsCompleted = false;
      _IterationCount = 0;
      StoredAuthorities = null;
    }

    #endregion

    #region IEnumerator<Results.ResolutionResult>

    Results.ResolutionResult IEnumerator<Results.ResolutionResult>.Current
    {
      get { return ((ResolutionIteratorBase)this).Current; }
    }

    object System.Collections.IEnumerator.Current
    {
      get { return (object)((IEnumerator<Results.ResolutionResult>)this).Current; }
    }

    bool System.Collections.IEnumerator.MoveNext()
    {
      return ((ResolutionIteratorBase)this).MoveNext();
    }

    void System.Collections.IEnumerator.Reset()
    {
      ((ResolutionIteratorBase)this).Reset();
    }

    #endregion

    #region IDisposable

    private bool disposed = false;

    //void IDisposable.Dispose()
    //{
    //  ((ResolutionIteratorBase)this).Dispose();
    //}

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(Boolean disposing)
    {
      if (!disposed)
      {
        if (disposing)
        {

        }
        disposed = true;
      }
    }

    ~ResolutionIteratorBase()
    {
      Dispose(false);
    }

    #endregion

    #region protected

    protected abstract Results.ResolutionResult GetNextResult(out bool isCompleted, NameServer selected = null);

    #endregion

  }
}

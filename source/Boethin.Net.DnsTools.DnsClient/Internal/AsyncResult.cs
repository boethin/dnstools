/*
 * File: Boethin.Net.DnsTools.DnsClient/Internal/AsyncResult.cs
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Boethin.Net.DnsTools.DnsClient.Internal
{

  // http://msdn.microsoft.com/en-us/magazine/cc163467.aspx

  internal class AsyncResultNoResult : IAsyncResult
  {
    // Fields set at construction which never change while 
    // operation is pending
    private readonly AsyncCallback m_AsyncCallback;
    private readonly Object m_AsyncState;

    // Fields set at construction which do change after 
    // operation completes
    private const Int32 c_StatePending = 0;
    private const Int32 c_StateCompletedSynchronously = 1;
    private const Int32 c_StateCompletedAsynchronously = 2;
    private Int32 m_CompletedState = c_StatePending;

    // Field that may or may not get set depending on usage
    private ManualResetEvent m_AsyncWaitHandle;

    // Fields set when operation completes
    private Exception m_exception = null;

    public AsyncResultNoResult(AsyncCallback asyncCallback, Object state)
    {
      m_AsyncCallback = asyncCallback;
      m_AsyncState = state;
    }

    public void SetAsCompleted(Exception exception, Boolean completedSynchronously)
    {
      // Passing null for exception means no error occurred. 
      // This is the common case
      m_exception = exception;

      // The m_CompletedState field MUST be set prior calling the callback
      Int32 prevState = Interlocked.Exchange(ref m_CompletedState,
         completedSynchronously ? c_StateCompletedSynchronously :
         c_StateCompletedAsynchronously);
      if (prevState != c_StatePending)
        throw new InvalidOperationException("You can set a result only once", exception);

      // If the event exists, set it
      if (m_AsyncWaitHandle != null) m_AsyncWaitHandle.Set();

      // If a callback method was set, call it
      if (m_AsyncCallback != null) m_AsyncCallback(this);
    }

    public void EndInvoke()
    {
        // This method assumes that only 1 thread calls EndInvoke 
        // for this object
        if (!IsCompleted)
        {
          // If the operation isn't done, wait for it
          AsyncWaitHandle.WaitOne();
          AsyncWaitHandle.Close();
          m_AsyncWaitHandle = null;  // Allow early GC
        }

        // Operation is done: if an exception occured, throw it
        if (m_exception != null)
        {
          throw new Exception(String.Format(
            "{0} was thrown in thread #{1} carrying state ({2}).",
            m_exception.GetType().FullName, 
            Thread.CurrentThread.ManagedThreadId, 
            m_AsyncState),
            m_exception);
        }
    }

    #region Implementation of IAsyncResult

    public Object AsyncState { get { return m_AsyncState; } }

    public Boolean CompletedSynchronously
    {
      get
      {
        return Thread.VolatileRead(ref m_CompletedState) ==
            c_StateCompletedSynchronously;
      }
    }

    public WaitHandle AsyncWaitHandle
    {
      get
      {
        if (m_AsyncWaitHandle == null)
        {
          Boolean done = IsCompleted;
          ManualResetEvent mre = new ManualResetEvent(done);
          if (Interlocked.CompareExchange(ref m_AsyncWaitHandle,
             mre, null) != null)
          {
            // Another thread created this object's event; dispose 
            // the event we just created
            mre.Close();
          }
          else
          {
            if (!done && IsCompleted)
            {
              // If the operation wasn't done when we created 
              // the event but now it is done, set the event
              m_AsyncWaitHandle.Set();
            }
          }
        }
        return m_AsyncWaitHandle;
      }
    }

    public Boolean IsCompleted
    {
      get
      {
        return Thread.VolatileRead(ref m_CompletedState) !=
            c_StatePending;
      }
    }

    #endregion

  }

  internal class AsyncResult<TResult> : AsyncResultNoResult
  {
    // Field set when operation completes
    private TResult m_result = default(TResult);

    public AsyncResult(AsyncCallback asyncCallback, Object state)
      : base(asyncCallback, state)
    {
    }

    public void SetAsCompleted(TResult result, Boolean completedSynchronously)
    {
      // Save the asynchronous operation's result
      m_result = result;

      // Tell the base class that the operation completed 
      // sucessfully (no exception)
      base.SetAsCompleted(null, completedSynchronously);
    }

    new public TResult EndInvoke()
    {
      base.EndInvoke(); // Wait until operation has completed 
      return m_result;  // Return the result (if above didn't throw)
    }

  }



}


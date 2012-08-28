﻿/*!
 * Cloudeo SDK C# bindings.
 * http://www.cloudeo.tv
 *
 * Copyright (C) SayMama Ltd 2012
 * Released under the BSD license.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CDO;
namespace CDOTest
{
    using System.Threading;

    class InitListener : CDO.PlatformInitListener
    {

        public InitListener(CountdownLatch latch)
        {
            _latch = latch;
        }

        public void onInitProgressChanged(InitProgressChangedEvent e)
        {

        }


        public void onInitStateChanged(InitStateChangedEvent e)
        {
            _initState = e.state;
            _latch.Signal();
        }

        public InitStateChangedEvent.InitState initState
        {
            get { return this._initState; }
        }

        private InitStateChangedEvent.InitState _initState;
        private CountdownLatch _latch;

    }

    public class CountdownLatch
    {
        private int m_remain;
   
        
        public CountdownLatch(int count = 1)
        {
            m_remain = count;
            m_event = new ManualResetEvent(false);
        }

        public void Signal()
        {
            // The last thread to signal also sets the event.
            if (Interlocked.Decrement(ref m_remain) == 0)
                m_event.Set();
        }

        public bool Wait(int timeout = 2000)
        {
            return m_event.WaitOne(timeout);
        }
    }
}

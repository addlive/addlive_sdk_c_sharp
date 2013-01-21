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

using NUnit.Framework;
using ADL;

namespace ADLTest
{
    [TestFixture]
    class CloudeoServiceListenerTest: AbstractCloudeoServiceTest
    {

        class MockEventListener:AddLiveServiceListenerAdapter
        {

            public EchoEvent receivedEvent;


            private CountdownLatch _latch;

            public MockEventListener(CountdownLatch latch)
            {
                _latch = latch;
            }

            public override void onEchoEvent(EchoEvent e) 
            {
                receivedEvent = e;
                _latch.Signal();
            }
        }

        [Test]
        public void testEchoNotification()
        {
                
            CountdownLatch latch = new CountdownLatch();
            MockEventListener listener = new MockEventListener(latch);
            _service.addServiceListener(createVoidResponder(), listener);
            awaitVoidResult("addServiceListener");
            _service.sendEchoNotification(createVoidResponder(), "whatever");
            awaitVoidResult();
            Assert.IsTrue(latch.Wait(), "Got timeout when waiting for the event");
            Assert.IsNotNull(listener.receivedEvent);
            Assert.AreEqual("whatever", listener.receivedEvent.echoValue);
        }

    }
}

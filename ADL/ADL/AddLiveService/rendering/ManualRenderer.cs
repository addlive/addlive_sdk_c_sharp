/*!
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
using System.Threading;

namespace ADL
{
    public delegate void InvaldateHandler(object sender, EventArgs args);

    public class ManualRenderer
    {

        public event InvaldateHandler Invalidated;

        /// <summary>
        /// 
        /// </summary>
        private IntPtr _platformHandle;

        /// <summary>
        /// 
        /// </summary>
        private int _rendererId;

        /// <summary>
        /// 
        /// </summary>
        private EventWaitHandle stoppedEvent;

        /// <summary>
        /// 
        /// </summary>
        private invalidate_clbck_t _invalidateCallback;
        private PreDisposeHandlerDelegate _preDisposeDelegate;

        /// <summary>
        /// 
        /// </summary>
        private adl_void_rclbck_t _stopRHandler;

        internal ManualRenderer(IntPtr platformHandle,
            PreDisposeHandlerDelegate preDisposeDelegate)
        {
            _platformHandle = platformHandle;
            _preDisposeDelegate = preDisposeDelegate;
            _rendererId = -1;
            _invalidateCallback = new invalidate_clbck_t(invalidateClbck);
        }


        ~ManualRenderer()
        {
            stop(false);
        }

        public void draw(DrawRequest r)
        {
            ADLDrawRequest nativeR = r.toNative();
            nativeR.rendererId = _rendererId;
            nativeR.windowHandle = r.hdc;
            NativeAPI.adl_draw(_platformHandle, ref nativeR);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="runPreDisposeDelegate"></param>
        internal void stop(bool runPreDisposeDelegate = true)
        {
            if (_rendererId < 0)
            {
                return;
            }
            stoppedEvent = new ManualResetEvent(false);
            _stopRHandler = new adl_void_rclbck_t(stopRHandler);
            NativeAPI.adl_stop_render(_stopRHandler, _platformHandle,
                IntPtr.Zero, _rendererId);
            stoppedEvent.WaitOne(2000);
            if (runPreDisposeDelegate)
                _preDisposeDelegate(_rendererId);
            _rendererId = -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="opaque"></param>
        /// <param name="error"></param>
        private void stopRHandler(IntPtr opaque, ref ADLError error)
        {
            stoppedEvent.Set();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal invalidate_clbck_t getInvalidateClbck()
        {
            return _invalidateCallback;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="opaque"></param>
        private void invalidateClbck(IntPtr opaque)
        {
            try
            {
                if(Invalidated != null)
                    Invalidated(this, EventArgs.Empty);
            }
            catch (Exception) { }
        }

        /// <summary>
        /// 
        /// </summary>
        internal int rendererId { set { _rendererId = value; } }


    }
}

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

namespace ADL
{
    public class MediaPublishOptions
    {

        public string windowId;

        public int nativeWidth;


        internal static ADLMediaPublishOptions toNative(MediaPublishOptions options)
        {
            ADLMediaPublishOptions result = new ADLMediaPublishOptions();
            if (options != null)
            {
                result.windowId = StringHelper.toNative(options.windowId);
                result.nativeWidth = options.nativeWidth;
            }
            return result;
        }
    }
}

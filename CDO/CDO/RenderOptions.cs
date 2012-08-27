﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CDO
{
    public class VideoScalingFilter
    {
        private string stringValue;

        internal string StringValue { get { return stringValue; } }

        private VideoScalingFilter() {}
        private VideoScalingFilter(string s) { stringValue = s;  }

        public static VideoScalingFilter FAST_BILINEAR = new VideoScalingFilter("FAST_BILINEAR");
        public static VideoScalingFilter BICUBIC = new VideoScalingFilter("BICUBIC");
    }

    public class RenderOptions
    {
        private string _sinkId;
        private bool _mirror;
        private VideoScalingFilter _filter;
        private invalidate_clbck_t _invalidateClbck;
        
        public string sinkId { get { return _sinkId; } set { _sinkId = value; } }
        public bool mirror { get { return _mirror; } set { _mirror = value; } }
        public VideoScalingFilter filter { get { return _filter; } set { _filter = value; } }
        internal invalidate_clbck_t invalidateClbck {set { _invalidateClbck = value;} }



        internal static CDORenderRequest toNative(RenderOptions options)
        {
            CDORenderRequest result = new CDORenderRequest();
            result.sinkId = StringHelper.toNative(options.sinkId);
            result.mirror = options.mirror;
            result.filter = StringHelper.toNative(options.filter.StringValue);
            result.opaque = IntPtr.Zero;
            result.invalidateCallback = options._invalidateClbck;
            return result;
        }
    }

    
}

/*!
 * Cloudeo SDK C# bindings.
 * http://www.cloudeo.tv
 *
 * Copyright (C) SayMama Ltd 2012
 * Released under the BSD license.
 */

namespace ADL
{
    internal static class StringHelper
    {
        internal static ADLString toNative(string s)
        {
            ADLString result = new ADLString();
            if (s != null)
            {
                result.body = s;
                result.length = (uint)s.Length;
            }
            return result;
        }

        internal static string fromNative(ADLString cdos)
        {
            return cdos.body;
        }
    }
}
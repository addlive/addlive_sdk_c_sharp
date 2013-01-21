﻿/*!
 * Cloudeo SDK C# bindings.
 * http://www.cloudeo.tv
 *
 * Copyright (C) SayMama Ltd 2012
 * Released under the BSD license.
 */

using System;

namespace ADL
{
    public sealed class ConnectionType
    {
        private string stringValue;

        public string StringValue
        {
            get { return stringValue; }
        }

         /**
         * Non-constructable
         */
        private ConnectionType(string s)
        {
            stringValue = s;
        }

        public static ConnectionType NOT_CONNECTED  = new ConnectionType("MEDIA_TRANSPORT_TYPE_NOT_CONNECTED");
        public static ConnectionType UDP_RELAY      = new ConnectionType("MEDIA_TRANSPORT_TYPE_UDP_RELAY");
        public static ConnectionType UDP_P2P        = new ConnectionType("MEDIA_TRANSPORT_TYPE_UDP_P2P");
        public static ConnectionType TCP_RELAY      = new ConnectionType("MEDIA_TRANSPORT_TYPE_TCP_RELAY");

        internal static ConnectionType FromString(string s)
        {
            if (String.Equals(s, NOT_CONNECTED.StringValue, StringComparison.InvariantCultureIgnoreCase))
                return NOT_CONNECTED;
            else if (String.Equals(s, UDP_RELAY.StringValue, StringComparison.InvariantCultureIgnoreCase))
                return UDP_RELAY;
            else if (String.Equals(s, UDP_P2P.StringValue, StringComparison.InvariantCultureIgnoreCase))
                return UDP_P2P;
            else if (String.Equals(s, TCP_RELAY.StringValue, StringComparison.InvariantCultureIgnoreCase))
                return TCP_RELAY;
            else
                return null;
        }
    }
}

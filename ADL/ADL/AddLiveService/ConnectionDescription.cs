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

    public class VideoStreamDescription
    {

        public uint maxWidth;

        public uint maxHeight;

        public uint maxFps;

        bool useAdaptation = true;

        internal ADLVideoStreamDescriptor toNative()
        {
            var vd = new ADLVideoStreamDescriptor();
            vd.maxFps = maxFps;
            vd.maxHeight = maxHeight;
            vd.maxWidth = maxWidth;
            vd.useAdaptation = useAdaptation;
            return vd;
        }

        internal string toJSON()
        {
            return String.Format(@"{{""maxWidth: {0}, ""maxHeight"": {1}, 
""maxFps"": {2}, ""useAdaptation"": {3}}}", maxWidth, maxHeight, maxFps, lowercaseBool(useAdaptation));
        }

        string lowercaseBool(bool a)
        {
            return a ? "true" : "false";
        }
    }

    public class AuthDetails
    {
        public long expires;

        public long userId;

        public string salt;

        public string signature;

        internal ADLAuthDetails toNative()
        {
            var aad = new ADLAuthDetails();
            aad.expires = expires;
            aad.userId = userId;
            aad.salt = StringHelper.toNative(salt);
            aad.signature = StringHelper.toNative(signature);
            return aad;
        }

        internal string toJSON()
        {
            string json = "";
            json = "{" +
                    "\"expires\":" + expires +
                    ",\"userId\":" + userId +
                    ",\"salt\":\"" + salt + "\"" +
                    ",\"signature\":\"" + signature + "\"}";
            return json;
        }
    }

    public class ConnectionDescription
    {

        public ConnectionDescription()
        {
            videoStream = new VideoStreamDescription();
            authDetails = new AuthDetails();
        }


        public string url;

        public string scopeId;

        public bool autopublishVideo;
        
        public bool autopublishAudio;

        public VideoStreamDescription videoStream;

        public AuthDetails authDetails;

        string lowercaseBool(bool a)
        {
            return a ? "true" : "false";
        }

        internal ADLConnectionDescription toNative()
        {
            var cd = new ADLConnectionDescription();
            cd.autopublishAudio = autopublishAudio;
            cd.autopublishVideo = autopublishVideo;
            cd.scopeId = StringHelper.toNative(scopeId);
            cd.url = StringHelper.toNative(url);
            cd.authDetails = authDetails.toNative();
            cd.videoStream = videoStream.toNative();
            return cd;
        }

        internal string toJSON()
        {
            string json = String.Format(@"{{""videoStream"": {0}, 
""autopublishVideo"": {1}, ""autopublishAudio"": {2}, ""url"": ""{3}"", ""scopeId"": ""{4}"",
""authDetails"": {5}}}", videoStream.toJSON(), lowercaseBool(autopublishVideo),
                     lowercaseBool(autopublishAudio), url, scopeId, authDetails.toJSON());

            return json;
        }

    }
}

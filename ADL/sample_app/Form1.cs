﻿/*!
 * Cloudeo SDK C# bindings.
 * http://www.cloudeo.tv
 *
 * Copyright (C) SayMama Ltd 2012
 * Released under the BSD license.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ADL;
using System.Security.Cryptography;

namespace sample_app
{
    public partial class Form1 : Form
    {

        private const int APPLICATION_ID = 1;
        private const string APP_SHARED_SECRET = "secret_key_here";
        
        private bool _localVideoStarted;

        private AddLiveServiceEventDispatcher _eDispatcher;
        private static Random random = new Random((int)DateTime.Now.Ticks);

        private string connectedScopeId;

        #region Initialization

        public Form1()
        {
            InitializeComponent();
        }
    
        private void Form1_Load(object sender, EventArgs e)
        {
            logD("Form loaded; Initializing Cloudeo platform");
            PlatformInitListenerDispatcher dispatcher = new PlatformInitListenerDispatcher();
            dispatcher.StateChanged += onADLInitStateChanged;
            _localVideoStarted = false;
            Platform.init(dispatcher);
        }

        private void onADLInitStateChanged(object sender, InitStateChangedEvent e)
        {
            if (e.state == InitStateChangedEvent.InitState.ERROR)
            {
                logE("Failed to initialize platform. Cause: " + e.errMessage + "(" + e.errCode + ")");
                return;
            }
            logD("Platform initialized. Proceeding with the initialization");
            Platform.Service.getVersion(Platform.R<string>(onVersion, genErrHandler("getVersion")));
            initializeCDOEventListener();
            Platform.Service.setApplicationId(genGenericResponder<object>("setApplicationId"), APPLICATION_ID);
            Platform.Service.addServiceListener(genGenericResponder<object>("addServiceListener"), _eDispatcher); 
            
            Platform.Service.getAudioCaptureDeviceNames(
                                Platform.R<Dictionary<string, string>>(onAudioCaptureDevices));
            Platform.Service.getAudioOutputDeviceNames(
                Platform.R<Dictionary<string, string>>(onAudioOutputDevices));
            Platform.Service.getVideoCaptureDeviceNames(
                Platform.R<Dictionary<string, string>>(onVideoCaptureDevices));  
        }

        
        delegate void SetTextDelegate(Label label, string text);
        private void onVersion(string version)
        {
            logD("Got platform version: " + version);
            SetTextDelegate setVersionD = delegate(Label label, string text)
            {
                label.Text = text;
            };
            Invoke(setVersionD, new object[] { versionLabel, version });
        }

        private void onAudioCaptureDevices(Dictionary<string, string> devs)
        {
            logD("Got audio capture devices (" + devs.Count + ")");
            Invoke(new SetDevsDelegate(setDevs), new object[] { micSelect, devs});
        }

        private void onAudioOutputDevices(Dictionary<string, string> devs)
        {
            logD("Got audio output devices (" + devs.Count + ")");
            Invoke(new SetDevsDelegate(setDevs), new object[] { spkSelect, devs });
        }

        private void onVideoCaptureDevices(Dictionary<string, string> devs)
        {
            logD("Got video capture devices (" + devs.Count + ")");
            Invoke(new SetDevsDelegate(setDevs), new object[] { camSelect, devs });
        }

        delegate void SetDevsDelegate(ComboBox select, Dictionary<string,string> devs);
        private void setDevs(ComboBox select, Dictionary<string, string> devs)
        {
            select.DataSource = new BindingSource(devs, null);
            select.DisplayMember = "Value";
            select.ValueMember = "Key";
        }

        #endregion

        #region user events handling

        private void camSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            KeyValuePair<string, string> dev = (KeyValuePair<string, string>)camSelect.SelectedValue;
            string devId = dev.Key;
            string devName = dev.Value;
            logD("Setting camera to: " + devName);
            Platform.Service.setVideoCaptureDevice(
                Platform.R<object>(maybeStartLocalVideo, genErrHandler("setVideoCaptureDevice")), devId);
        }

        private void micSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            KeyValuePair<string, string> dev = (KeyValuePair<string, string>)micSelect.SelectedValue;
            string devId = dev.Key;
            string devName = dev.Value;
            logD("Setting mic to: " + devName);
            Platform.Service.setAudioCaptureDevice(genGenericResponder<object>("setAudioCaptureDevice"), devId);
        }

        private void spkSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            KeyValuePair<string, string> dev = (KeyValuePair<string, string>)spkSelect.SelectedValue;
            string devId = dev.Key;
            string devName = dev.Value;
            logD("Setting speakers to: " + devName);
            Platform.Service.setAudioOutputDevice(genGenericResponder<object>("setAudioOutputDevice"), devId);

        }

        private void connectBtn_Click(object sender, EventArgs e)
        {            
            string scopeId = scopeIdInput.Text;
            long userId = random.Next(1000);
            logD("Connecting to scope with id: " + scopeId);
            ConnectionDescription connDescr = new ConnectionDescription();
            connDescr.autopublishAudio = publishAudioCheckBox.Checked;
            connDescr.autopublishVideo = publishVideoCheckBox.Checked;
            //connDescr.url = "174.127.76.179:443/" + scopeId;
            connDescr.videoStream.maxWidth = 320;
            connDescr.videoStream.maxHeight = 240;
            connDescr.videoStream.maxFps = 5;

            connDescr.authDetails = genAuthDetails(scopeId, userId);
            connDescr.scopeId = scopeId;
            connectedScopeId = scopeId;
            Platform.Service.connect(genGenericResponder<object>("connect"), connDescr);
        }

        private void disconnectBtn_Click(object sender, EventArgs e)
        {
            Platform.Service.disconnect(genGenericResponder<object>
                ("disconnect"), connectedScopeId);
        }

        #endregion

        #region ADL events processing

        private void initializeCDOEventListener()
        {
            _eDispatcher = new AddLiveServiceEventDispatcher();
            _eDispatcher.UserEvent += onUserEvent;
            _eDispatcher.MediaConnTypeChanged += onMediaConnTypeChanged;
            _eDispatcher.MediaStream += onMediaStream;
        }

        void onUserEvent(object sender, UserStateChangedEvent e)
        {
            logD("Got new user event for user with id: " + e.UserId);
            logD("User just " + (e.IsConnected ? "joined" : "left") + "the scope");
            if (e.IsConnected)
            {
                logD("User is " + (!e.AudioPublished ? "not" : "") + " publishing audio stream");
                logD("User is " + (!e.VideoPublished ? "not" : "") + " publishing video stream");
                if(e.VideoPublished)
                {
                    logD("Creating renderer for remote user sink: " + e.VideoSinkId);
                    RenderOptions rOptions = new RenderOptions();
                    rOptions.mirror = true;
                    rOptions.sinkId = e.VideoSinkId;
                    rOptions.filter = VideoScalingFilter.FAST_BILINEAR;
                    ResultHandler<RenderingWidget> rHandler = 
                        new ResultHandler<RenderingWidget>((_1) => appendRenderer(remoteVideoRenderer, _1));
                    Platform.renderSink(Platform.R<RenderingWidget>(rHandler, 
                        genErrHandler("renderSink")), rOptions);
                        }
            }
            else
            {
            }

        }

        void onMediaConnTypeChanged(object sender, MediaConnTypeChangedEvent e)
        {
            logD("Got new media connection type: " + e.ConnType.StringValue);
        }

        void onMediaStream(object sender, UserStateChangedEvent e)
        {
            logD("Got new media stream event for user with id: " + e.UserId + 
                ". Media type: " + e.MediaType);
        }

        #endregion

        #region private helpers

        private string randomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        private AuthDetails genAuthDetails(string scopeId, long userId)
        {
            // Fill the simple fields
            AuthDetails authDetails = new AuthDetails();
            authDetails.expires =                                                        // 5 minutes 
                (long) (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds + 300;
            authDetails.userId = userId;
            authDetails.salt = randomString(100);

            // Calculate and fill the signature
            string signatureBody = "" + APPLICATION_ID + scopeId + userId + 
                authDetails.salt + authDetails.expires + APP_SHARED_SECRET;
            ASCIIEncoding enc = new ASCIIEncoding();
            byte[] sigBodyBinary = enc.GetBytes(signatureBody);
            SHA256 hasher = SHA256Managed.Create();
            byte[] sigBinary = hasher.ComputeHash(sigBodyBinary);
            authDetails.signature = BitConverter.ToString(sigBinary).Replace("-", "");
            
            return authDetails;
        }

        private void maybeStartLocalVideo(object nothing = null)
        {
            if (_localVideoStarted)
                return;
            logD("Starting local video");
            Platform.Service.startLocalVideo(Platform.R<string>(
                onLocalVideoStarted,
                genErrHandler("startLocalVideo")));
                    
        }

        private void onLocalVideoStarted(string sinkId)
        {
            logD("Local video started. Creating renderer");
            RenderOptions rOptions = new RenderOptions();
            rOptions.mirror = true;
            rOptions.sinkId = sinkId;
            rOptions.filter = VideoScalingFilter.FAST_BILINEAR;
            ResultHandler<RenderingWidget> rHandler = 
                new ResultHandler<RenderingWidget>((_1) => appendRenderer(localVideoRenderer, _1));
            Platform.renderSink(Platform.R<RenderingWidget>(rHandler, 
                genErrHandler("renderSink")), rOptions);
        }

        private void appendRenderer(Panel container, RenderingWidget widget)
        {
            if (localVideoRenderer.InvokeRequired)
            {
                localVideoRenderer.Invoke(
                    new Action<Panel, RenderingWidget>(appendRenderer), 
                    new object[]{ container, widget});
                return;
            }
            logD("Local video feed renderer creaed. Appending to scene");
            widget.Width = localVideoRenderer.Width;
            widget.Height = localVideoRenderer.Height;
            container.Controls.Add(widget);
        }

        #endregion

        #region responders generation

        private void genericErrorHandler(string methodName, int errCode, 
            string errMessage)
        {
            logE("Got error when processing method: " + methodName + ". Cause: " + 
                errMessage + "(" + errCode + ")");
        }

        private ErrHandler genErrHandler(string methodName)
        {
            return new ErrHandler((_1, _2) => genericErrorHandler(methodName, _1, _2));
        }

        private Responder<T> genGenericResponder<T>(string methodName)
        {
            return Platform.R<T>(
                delegate(T sth) {
                    logD("Got successfull result of method call: " + methodName);
            }, genErrHandler(methodName));
        }

        #endregion

        #region Logging

        private void logD(string msg)        
        {
            if (logsSink.InvokeRequired)
                Invoke(new Action<string>(logD), msg);
            else
                appendLog("DEBUG", msg);
        }

        private void appendLog(string level, string msg)
        {
            
            logsSink.AppendText(DateTime.Now.ToString("HH:mm:ss"));
            logsSink.AppendText(" ["+level+"] = ");
            logsSink.AppendText(msg);
            logsSink.AppendText("\n");
        }

        private void logE(string msg)
        {
            if (logsSink.InvokeRequired)
                Invoke(new Action<string>(logE), msg);
            else
            {
                logsSink.SelectionStart = logsSink.TextLength;
                logsSink.SelectionLength = 0;

                logsSink.SelectionColor = Color.Red;
                appendLog("ERROR", msg);
                logsSink.SelectionColor = logsSink.ForeColor;
            }
        }

        #endregion

        private void publishAudioCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (publishAudioCheckBox.Checked)
            {
                Platform.Service.publish(genGenericResponder<object>("publishAudio"), connectedScopeId,
                    MediaType.AUDIO, null);
            }
            else
            {
                Platform.Service.unpublish(genGenericResponder<object>("unpublishAudio"), connectedScopeId,
                    MediaType.AUDIO);
            }
        }

        private void publishVideoCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (publishVideoCheckBox.Checked)
            {
                Platform.Service.publish(genGenericResponder<object>("publishVideo"), connectedScopeId,
                    MediaType.VIDEO, null);
            }
            else
            {
                Platform.Service.unpublish(genGenericResponder<object>("unpublishVideo"), connectedScopeId,
                        MediaType.VIDEO);
            }
        }


    }
}

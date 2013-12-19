/*!
 * Cloudeo SDK C# bindings.
 * http://www.cloudeo.tv
 *
 * Copyright (C) SayMama Ltd 2012
 * Released under the BSD license.
 */
using System;
using System.Collections.Generic;

namespace ADL
{

    class NativeServiceListenerAdapter
    {

        private AddLiveServiceListener _listener;

        private on_video_frame_size_changed_clbck_t
                _on_video_frame_size_changed_callback_t;
        private on_connection_lost_clbck_t
                _on_connection_lost_callback_t;
        private on_user_event_clbck_t
                _on_user_event_callback_t;
        private on_media_stream_clbck_t
                _on_media_stream_callback_t;
        private on_mic_activity_clbck_t
                _on_mic_activity_callback_t;
        private on_mic_gain_clbck_t
                _on_mic_gain_callback_t;
        private on_device_list_changed_clbck_t
                _on_device_list_changed_callback_t;
        private on_media_stats_clbck_t
                _on_media_stats_callback_t;
        private on_message_clbck_t _on_message_callback_t;
        private on_media_conn_type_changed_clbck_t
                _on_media_conn_type_changed_callback_t;
        private on_media_interrupt_clbck_t
                _on_media_interrupt_callback_t;
        private on_media_issue_clbck_t
                _on_media_issue_callback_t;
        private on_session_reconnected_clbck_t
                _on_session_reconnected_callback_t;


        public NativeServiceListenerAdapter(AddLiveServiceListener listener)
        {
            _listener = listener;
            _on_video_frame_size_changed_callback_t =
                new on_video_frame_size_changed_clbck_t(
                    on_video_frame_size_changed_callback_t);
            _on_connection_lost_callback_t = new on_connection_lost_clbck_t(
                on_connection_lost_callback_t);
            _on_user_event_callback_t = new on_user_event_clbck_t(
                on_user_event_callback_t);
            _on_media_stream_callback_t = new on_media_stream_clbck_t(
                on_media_stream_callback_t);
            _on_mic_activity_callback_t = new on_mic_activity_clbck_t(
                on_mic_activity_callback_t);
            _on_mic_gain_callback_t = new on_mic_gain_clbck_t(
                on_mic_gain_callback_t);
            _on_device_list_changed_callback_t =
                new on_device_list_changed_clbck_t(
                    on_device_list_changed_callback_t);
            _on_media_stats_callback_t =
                new on_media_stats_clbck_t(on_media_stats_callback_t);
            _on_message_callback_t =
                new on_message_clbck_t(on_message_callback_t);
            _on_media_conn_type_changed_callback_t =
                new on_media_conn_type_changed_clbck_t(
                    on_media_conn_type_changed_callback_t);
            _on_media_interrupt_callback_t = new on_media_interrupt_clbck_t(on_media_interrupt_callback_t);
            _on_media_issue_callback_t = new on_media_issue_clbck_t(on_media_issue_callback_t);
            _on_session_reconnected_callback_t = new on_session_reconnected_clbck_t(on_session_reconnected_callback_t);
        }

        public ADLServiceListener toNative()
        {

            ADLServiceListener nListener = new ADLServiceListener();
            nListener.opaque = IntPtr.Zero;
            nListener.onConnectionLost = _on_connection_lost_callback_t;
            nListener.onDeviceListChanged = _on_device_list_changed_callback_t;
            nListener.onSessionReconnected = _on_session_reconnected_callback_t;
            nListener.onMediaConnTypeChanged =
                _on_media_conn_type_changed_callback_t;
            nListener.onMediaStats = _on_media_stats_callback_t;
            nListener.onMediaStreamEvent = _on_media_stream_callback_t;
            nListener.onMessage = _on_message_callback_t;
            nListener.onMicActivity = _on_mic_activity_callback_t;
            nListener.onMicGain = _on_mic_gain_callback_t;
            nListener.onUserEvent = _on_user_event_callback_t;
            nListener.onVideoFrameSizeChanged =
                _on_video_frame_size_changed_callback_t;
            nListener.onMediaIssue =
                _on_media_issue_callback_t;
            nListener.onMediaInterrupt =
                _on_media_interrupt_callback_t;
            return nListener;

        }


        // CDOServiceListener callback handlers

        private void on_video_frame_size_changed_callback_t(IntPtr opaque,
            ref ADLVideoFrameSizeChangedEvent e)
        {
            try
            {
                if (_listener != null)
                    _listener.onVideoFrameSizeChanged(
                        VideoFrameSizeChangedEvent.FromNative(e));
            }
            catch (Exception )
            {
            }
        }

        private void on_connection_lost_callback_t(IntPtr opaque,
            ref ADLConnectionLostEvent e)
        {
            try
            {
                if (_listener != null)
                    _listener.onConnectionLost(
                        ConnectionLostEvent.FromNative(e));
            }
            catch (Exception )
            {
            }
        }

        private void on_user_event_callback_t(IntPtr opaque,
            ref ADLUserStateChangedEvent e)
        {
            try
            {
                if (_listener != null)
                    _listener.onUserEvent(
                        UserStateChangedEvent.FromNative(e));
            }
            catch (Exception )
            {
            }
        }

        private void on_media_stream_callback_t(IntPtr opaque,
            ref ADLUserStateChangedEvent e)
        {
            try
            {
                if (_listener != null)
                    _listener.onMediaStreamEvent(
                        UserStateChangedEvent.FromNative(e));
            }
            catch (Exception )
            {
            }
        }

        private void on_mic_activity_callback_t(IntPtr opaque,
            ref ADLMicActivityEvent e)
        {
            try
            {
                if (_listener != null)
                    _listener.onMicActivity(
                        MicActivityEvent.FromNative(e));
            }
            catch (Exception )
            {
            }
        }

        private void on_mic_gain_callback_t(IntPtr opaque,
            ref ADLMicGainEvent e)
        {
            try
            {
                if (_listener != null)
                    _listener.onMicGain(MicGainEvent.FromNative(e));
            }
            catch (Exception )
            {
            }
        }

        private void on_device_list_changed_callback_t(IntPtr opaque,
            ref ADLDeviceListChangedEvent e)
        {
            try
            {
                if (_listener != null)
                    _listener.onDeviceListChanged(
                        DeviceListChangedEvent.FromNative(e));
            }
            catch (Exception )
            {
            }
        }

        private void on_media_stats_callback_t(IntPtr opaque,
            ref ADLMediaStatsEvent e)
        {
            try
            {
                Console.Error.WriteLine("Media stats: " + e.stats.bitRate);
                if (_listener != null)
                    _listener.onMediaStats(
                        MediaStatsEvent.FromNative(e));
            }
            catch (Exception )
            {
            }
        }

        private void on_message_callback_t(IntPtr opaque,
            ref ADLMessageEvent e)
        {
            try
            {
                if (_listener != null)
                    _listener.onMessage(MessageEvent.FromNative(e));
            }
            catch (Exception )
            {
            }
        }

        private void on_media_conn_type_changed_callback_t(IntPtr opaque,
            ref ADLMediaConnTypeChangedEvent e)
        {
            try
            {

                if (_listener != null)
                    _listener.onMediaConnTypeChanged(
                        MediaConnTypeChangedEvent.FromNative(e));
            }
            catch (Exception )
            {
            }
        }

        private void on_media_interrupt_callback_t(IntPtr opaque, ref ADLMediaInterruptEvent e)
        {
            try
            {
                if (_listener != null)
                    _listener.onMediaInterruptEvent(MediaInterruptEvent.FromNative(e));
            }
            catch (Exception )
            {
            }
        }

        private void on_media_issue_callback_t(IntPtr opaque, ref ADLMediaIssueEvent e)
        {
            try
            {
                if (_listener != null)
                    _listener.onMediaIssueEvent(MediaIssueEvent.FromNative(e));
            }
            catch (Exception)
            {
            }
        }

        private void on_session_reconnected_callback_t(IntPtr opaque, ref ADLSessionReconnectedEvent e)
        {
            try
            {
                if (_listener != null)
                    _listener.onSessionReconnectedEvent(SessionReconnectedEvent.FromNative(e));
            }
            catch (Exception)
            {
            }
        }
    }
}

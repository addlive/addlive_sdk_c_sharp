/*!
 * Cloudeo SDK C# bindings.
 * http://www.cloudeo.tv
 *
 * Copyright (C) SayMama Ltd 2012
 * Released under the BSD license.
 */

using System.Runtime.InteropServices;
using System;


namespace ADL
{
    using ADLH = IntPtr;

    /**
     * =====================================================================
     *  General Data type definitions
     * =====================================================================
     */

    static class Constants
    {
        /**
         * Max length of the String used to communicate with Cloudeo
         */
        public const int ADL_STRING_MAX_LEN = 5120;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct ADLString
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.ADL_STRING_MAX_LEN)]
        private byte[] _body;
        public UInt32 length;

        public string body
        {
            get
            {
                int len = 0;
                while (_body[len] != 0) ++len;
                return System.Text.Encoding.UTF8.GetString(_body, 0, len);
            }
            set
            {
                _body = new byte[Constants.ADL_STRING_MAX_LEN];
                System.Text.Encoding.UTF8.GetBytes(value, 0, value.Length, _body, 0);
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct ADLError
    {
        public int err_code;
        //[MarshalAs(UnmanagedType.LPStruct)]
        public ADLString err_message;
    }


    /**
     * Device description used by the Cloudeo platform.
     */
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct ADLDevice
    {
        public ADLString label;
        public ADLString id;
    }


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct ADLMediaPublishOptions
    {
        /**
            *
            */
        public ADLString windowId;

        /**
            *
            */
        public int nativeWidth;
    }

    /**
        * =====================================================================
        *  Rendering structures
        * =====================================================================
        */

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void invalidate_clbck_t(IntPtr opaque);

    /**
        * Structure defining all attributes required to start rendering video
        * sink.
        */
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct ADLRenderRequest
    {
        /**
            * Id of video sink to render.
            */
        public ADLString sinkId;
        /**
            * Name of scaling filter to be used when scaling the frames.
            */
        public ADLString filter;

        /**
            * Flag defining whether the frames should be mirrored or not.
            */
        [MarshalAs(UnmanagedType.U1)]
        public bool mirror;
        /**
            * Opaque, platform specific window handle used for rendering
            * purposes.
            */
        public IntPtr windowHandle;
        /**
            * Opaque pointer passed to the invalidateCallback
            */
        public IntPtr opaque;
        /**
            * Callback that should be used to indicate the need to redraw the
            * content of rendering window.
            */
        public invalidate_clbck_t invalidateCallback;
    }

    /**
        * Defines all attributes needed to redraw video feed.
        */
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct ADLDrawRequest
    {
        /**
            * Area of the target window to render on.
            */
        public int top;
        public int left;
        public int bottom;
        public int right;
        /**
            * Platform-specific window handle.
            */
        public IntPtr windowHandle;
        /**
            * Id of renderer which should be affected by the Draw request.
            */
        public int rendererId;
    }

    /**
     * Describes single screen sharing source.
     */
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct ADLScreenCaptureSource
    {
        /**
         * Unique ID of a window.
         */
        public ADLString id;

        /**
         * Title (or caption) of a window.
         */
        public ADLString title;

        /**
         * Actual snaphot of window content.
         */
        public IntPtr  imageData;

        /**
         * Size of image data in bytes.
         */
        public UIntPtr imageDataLen;

        /**
         * Width of the window snapshot.
         */
        public int width;

        /**
         * Height of the window snapshot.
         */
        public int height;
    };

    /**
        * =====================================================================
        *  CloudeoServiceListener-related definitions
        * =====================================================================
        */

    /**
        * Events
        * =====================================================================
        */

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct ADLVideoFrameSizeChangedEvent
    {
        public ADLString sinkId;
        public int height;
        public int width;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct ADLConnectionLostEvent
    {
        public ADLString scopeId;
        public int errCode;
        public ADLString errMessage;
    } ;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct ADLUserStateChangedEvent
    {
        public ADLString scopeId;

        public Int64 userId;

        [MarshalAs(UnmanagedType.U1)]
        public bool isConnected;

        [MarshalAs(UnmanagedType.U1)]
        public bool audioPublished;

        [MarshalAs(UnmanagedType.U1)]
        public bool videoPublished;
        public ADLString videoSinkId;

        [MarshalAs(UnmanagedType.U1)]
        public bool screenPublished;
        public ADLString screenSinkId;

        public ADLString mediaType;

    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct ADLMicActivityEvent
    {
        public int activity;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct ADLMicGainEvent
    {
        public int gain;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct ADLDeviceListChangedEvent
    {
        [MarshalAs(UnmanagedType.U1)]
        public bool audioIn;
        [MarshalAs(UnmanagedType.U1)]
        public bool audioOut;
        [MarshalAs(UnmanagedType.U1)]
        public bool videoIn;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct ADLMediaStats
    {
        public int layer;      // video only
        public float bitRate;
        public float cpu;
        public float totalCpu;
        public float rtt;
        public float queueDelay;
        public float psnr;       // video only
        public float fps;        // video only
        public int totalLoss;
        public float loss;
        public int quality;
        public float jbLength;
        public float avgJitter;
        public float maxJitter;
        public float avOffset;    // audio only
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct ADLMediaStatsEvent
    {
        public ADLString scopeId;
        public ADLString mediaType;
        public Int64 remoteUserId;
        public ADLMediaStats stats;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct ADLMessageEvent
    {
        public ADLString data;
        public Int64 srcUserId;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct ADLMediaConnTypeChangedEvent
    {
        public ADLString scopeId;
        public ADLString mediaType;
        public ADLString connectionType;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct ADLEchoEvent
    {
        public ADLString echoValue;
    }


    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void on_video_frame_size_changed_clbck_t(IntPtr opaque,
        ref ADLVideoFrameSizeChangedEvent e);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void on_connection_lost_clbck_t(IntPtr opaque,
        ref ADLConnectionLostEvent e);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void on_user_event_clbck_t(IntPtr opaque,
        ref ADLUserStateChangedEvent e);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void on_media_stream_clbck_t(IntPtr opaque,
        ref ADLUserStateChangedEvent e);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void on_mic_activity_clbck_t(IntPtr opaque,
        ref ADLMicActivityEvent e);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void on_mic_gain_clbck_t(IntPtr opaque,
        ref ADLMicGainEvent e);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void on_device_list_changed_clbck_t(IntPtr opaque,
        ref ADLDeviceListChangedEvent e);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void on_media_stats_clbck_t(IntPtr opaque,
        ref ADLMediaStatsEvent e);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void on_message_clbck_t(IntPtr opaque,
        ref ADLMessageEvent e);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void on_media_conn_type_changed_clbck_t(IntPtr opaque,
        ref ADLMediaConnTypeChangedEvent e);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void on_echo_clbck_t(IntPtr opaque, ref ADLEchoEvent e);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct ADLServiceListener
    {
        public IntPtr opaque;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public on_video_frame_size_changed_clbck_t onVideoFrameSizeChanged;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public on_connection_lost_clbck_t onConnectionLost;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public on_user_event_clbck_t onUserEvent;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public on_media_stream_clbck_t onMediaStreamEvent;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public on_mic_activity_clbck_t onMicActivity;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public on_mic_gain_clbck_t onMicGain;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public on_device_list_changed_clbck_t onDeviceListChanged;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public on_media_stats_clbck_t onMediaStats;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public on_message_clbck_t onMessage;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public on_media_conn_type_changed_clbck_t onMediaConnTypeChanged;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public on_echo_clbck_t onEcho;
    }

    /**
     * =====================================================================
     *  Platform initialization
     * =====================================================================
     */

    /**
     * Platform initialization options
     */
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct ADLInitOptions
    {
        /**
         * Path to the Cloudeo Logic shared library.
         */
        public ADLString logicLibPath;

        /**
         * URL for runtime config.
         */
        public ADLString configUrl;
    }


    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void adl_void_rclbck_t(IntPtr opaque,
        ref ADLError error);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void adl_string_rclbck_t(IntPtr opaque,
        ref ADLError error, ref ADLString str);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void adl_int_rclbck_t(IntPtr opaque,
        ref ADLError error, int i);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void adl_get_device_names_rclbck_t(IntPtr opaque,
        ref ADLError error, IntPtr device, UIntPtr resultListLen);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void adl_platform_init_done_clbck(IntPtr ptr,
        ref ADLError err, ADLH h);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void adl_platform_init_progress_clbck(IntPtr ptr,
        short sh);

    /**
     * Defines a signature for receiving a screen capture pseudo-devices list (device maps to a window).
     * 
     * @param opaque        opaque pointer passed as the 3rd param to the function 
     *                      invocation
     * @param err           error indicator
     * @param resultList    list of devices
     * @param resultListLen size of the devices list.
     */

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void adl_get_screen_capture_srcs_rclbck_t(IntPtr opaque, ref ADLError err,
        IntPtr resultList, UIntPtr resultListLen);

    internal class NativeAPI
    {
        [DllImport("adl_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool adl_no_error(ref ADLError error);


        /**
         *
         * @param resultCallback
         * @param initializationOptions
         * @param opaque
         */
        [DllImport("adl_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void adl_init_platform(
            adl_platform_init_done_clbck resultCallback,
            ref ADLInitOptions initializationOptions,
            IntPtr opaque);

        [DllImport("adl_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void adl_release_platform(ADLH handle);

        /**
         * =============================================================================
         *  Platform API
         * =============================================================================
         */




        /**
         * Retrieves version of the SDK
         *
         * @since 0.1.0
         * @param resultHandler Address of a function receiving the result of the call
         * @param handle
         * @param opaque
         */
        [DllImport("adl_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void adl_get_version(
            adl_string_rclbck_t resultHandler, ADLH handle, IntPtr opaque);

        [DllImport("adl_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void adl_set_application_id(adl_void_rclbck_t rclbck,
        ADLH handle, IntPtr opaque, long applicationId);


        [DllImport("adl_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void adl_add_service_listener(
            adl_void_rclbck_t resultHandler, ADLH handle, IntPtr opaque,
            ref ADLServiceListener listener);

        [DllImport("adl_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void adl_send_echo_notification(
            adl_void_rclbck_t resultHandler, ADLH handle, IntPtr opaque,
            ref ADLString content);

        //=====================================================================
        //=============== Video devices dealing ===============================
        //=====================================================================
        /**
         * Retrieves list of currently installed video capture devices.
         *
         * @param rclbck result callback
         * @param handle platform handle
         * @param opaque PIMPL pointer
         */
        [DllImport("adl_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void adl_get_video_capture_device_names(
            adl_get_device_names_rclbck_t rclbck, ADLH handle, IntPtr opaque);


        /**
         * Sets the video capture device to be used by platform
         *
         * @param rclbck
         * @param handle
         * @param opaque
         * @param device_id
         */
        [DllImport("adl_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void adl_set_video_capture_device(
            adl_void_rclbck_t rclbck, ADLH handle, IntPtr opaque,
            ref ADLString device_id);

        /**
         *
         * @param rclbck
         * @param handle
         * @param opaque
         */
        [DllImport("adl_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void adl_get_video_capture_device(
            adl_string_rclbck_t rclbck, ADLH handle, IntPtr opaque);

        //=============================================================================
        //=============== Audio capture devices dealing ===============================
        //=============================================================================
        /**
         * Retrieves list of currently installed video capture devices.
         *
         * @param rclbck result callback
         * @param handle platform handle
         * @param opaque PIMPL pointer
         */
        [DllImport("adl_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void adl_get_audio_capture_device_names(
            adl_get_device_names_rclbck_t rclbck, ADLH handle, IntPtr opaque);


        /**
         * Sets the video capture device to be used by platform
         *
         * @param rclbck
         * @param handle
         * @param opaque
         * @param device_id
         */
        [DllImport("adl_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void adl_set_audio_capture_device(
            adl_void_rclbck_t rclbck, ADLH handle, IntPtr opaque,
            ref ADLString device_id);

        /**
         *
         * @param rclbck
         * @param handle
         * @param opaque
         */
        [DllImport("adl_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void adl_get_audio_capture_device(
            adl_string_rclbck_t rclbck, ADLH handle, IntPtr opaque);

        //============================================================================
        //=============== Audio output devices dealing ===============================
        //============================================================================
        /**
         * Retrieves list of currently installed video capture devices.
         *
         * @param rclbck result callback
         * @param handle platform handle
         * @param opaque PIMPL pointer
         */
        [DllImport("adl_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void adl_get_audio_output_device_names(
            adl_get_device_names_rclbck_t rclbck, ADLH handle, IntPtr opaque);


        /**
         * Sets the video capture device to be used by platform
         *
         * @param rclbck
         * @param handle
         * @param opaque
         * @param device_id
         */
        [DllImport("adl_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void adl_set_audio_output_device(
            adl_void_rclbck_t rclbck, ADLH handle, IntPtr opaque,
            ref ADLString device_id);

        /**
         *
         * @param rclbck
         * @param handle
         * @param opaque
         */
        [DllImport("adl_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void adl_get_audio_output_device(
            adl_string_rclbck_t rclbck, ADLH handle, IntPtr opaque);

        [DllImport("adl_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void adl_start_playing_test_sound(
            adl_void_rclbck_t rclbck, ADLH handle, IntPtr opaque);

        [DllImport("adl_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void adl_stop_playing_test_sound(
            adl_void_rclbck_t rclbck, ADLH handle, IntPtr opaque);

        [DllImport("adl_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void adl_set_volume(
            adl_void_rclbck_t rclbck, ADLH handle, IntPtr opaque, int volume);

        [DllImport("adl_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void adl_get_volume(
            adl_int_rclbck_t rclbck, ADLH handle, IntPtr opaque);

        [DllImport("adl_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void adl_monitor_mic_activity(
            adl_void_rclbck_t rclbck, ADLH handle, IntPtr opaque, bool monitor);

        //========================================================================
        //=============== Local preview management ===============================
        //========================================================================
        /**
         *
         * @param rclbck
         * @param handle
         * @param opaque
         */
        [DllImport("adl_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void adl_start_local_video(
            adl_string_rclbck_t rclbck, ADLH handle, IntPtr opaque);

        /**
         *
         * @param rclbck
         * @param handle
         * @param opaque
         */
        [DllImport("adl_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void adl_stop_local_video(
            adl_void_rclbck_t rclbck, ADLH handle, IntPtr opaque);


        //========================================================================
        //=============== Screen sharing           ===============================
        //========================================================================

        [DllImport("adl_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void adl_get_screen_capture_sources(
                adl_get_screen_capture_srcs_rclbck_t rclbck, ADLH handle,
                IntPtr opaque, int targetWidth);

        
        //============================================================
        //=============== Connectivity ===============================
        //============================================================


        /**
         *
         * @param rclbck
         * @param handle
         * @param opaque
         * @param connDescr
         */
        [DllImport("adl_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void adl_connect_string(adl_void_rclbck_t rclbck,
            ADLH handle, IntPtr opaque, ref ADLString connDescr);

        /**
         *
         * @param rclbck
         * @param handle
         * @param opaque
         * @param scopeId
         */
        [DllImport("adl_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void adl_disconnect(adl_void_rclbck_t rclbck,
            ADLH handle, IntPtr opaque, ref ADLString scopeId);

        [DllImport("adl_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void adl_publish(adl_void_rclbck_t rclbck,
            ADLH handle, IntPtr opaque, ref ADLString scopeId,
            ref ADLString what, ref ADLMediaPublishOptions options);

        [DllImport("adl_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void adl_unpublish(adl_void_rclbck_t rclbck,
            ADLH handle, IntPtr opaque, ref ADLString scopeId,
            ref ADLString what);

        [DllImport("adl_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void adl_send_message(adl_void_rclbck_t rclbck,
            ADLH handle, IntPtr opaque, ref ADLString scopeId,
            [MarshalAs(UnmanagedType.LPStr)]string msgBody,
            UIntPtr msgSize, ref Int64 recipientId);

        //=========================================================
        //=============== Statistics ==============================
        //=========================================================

        [DllImport("adl_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void adl_start_measuring_stats(adl_void_rclbck_t rclbck, 
            ADLH handle, IntPtr opaque, ref ADLString scopeId, int interval);

        [DllImport("adl_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void adl_stop_measuring_stats(adl_void_rclbck_t rclbck, ADLH handle, 
            IntPtr opaque, ref ADLString scopeId);

        //=========================================================
        //=============== Rendering ===============================
        //=========================================================
        /**
         *
         * @param request
         * @return
         */
        [DllImport("adl_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void adl_render_sink(adl_int_rclbck_t rclbck,
            ADLH handle, IntPtr opaque, ref ADLRenderRequest request);

        /**
         *
         * @param rendererId
         */
        [DllImport("adl_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void adl_stop_render(adl_void_rclbck_t rclbck,
            ADLH handle, IntPtr opaque, int rendererId);

        /**
         *
         * @param request
         * @return
         */
        [DllImport("adl_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void adl_draw(ADLH handle,
            ref ADLDrawRequest request);
    }
}

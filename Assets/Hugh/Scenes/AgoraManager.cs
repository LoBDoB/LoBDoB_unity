using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Agora.Rtc;
using Agora.Util;
using UnityEngine.Serialization;
using Logger = Agora.Util.Logger;


public class AgoraManager : MonoBehaviour

{
    [FormerlySerializedAs("appIdInput")]
    [SerializeField]
    private AppIdInput _appIdInput;

    [Header("_____________Basic Configuration_____________")]
    [FormerlySerializedAs("APP_ID")]
    [SerializeField]
    private string _appID = "";

    [FormerlySerializedAs("TOKEN")]
    [SerializeField]
    private string _token = "";

    [FormerlySerializedAs("CHANNEL_NAME")]
    [SerializeField]
    private string _channelName = "";

    public Text LogText;
    internal Logger Log;
    internal IRtcEngineEx RtcEngine = null;

    public uint Uid1 = 123;
    public uint Uid2 = 456;

    private Dropdown _winIdSelect;
    private Button _startShareBtn;
    private Button _stopShareBtn;


    private void ScreenShareJoinChannel()
    {
        ChannelMediaOptions options = new ChannelMediaOptions();
        options.autoSubscribeAudio.SetValue(false);
        options.autoSubscribeVideo.SetValue(false);
        options.publishCameraTrack.SetValue(false);
        options.publishScreenTrack.SetValue(true);
        options.enableAudioRecordingOrPlayout.SetValue(false);
#if UNITY_ANDROID || UNITY_IPHONE
            options.publishScreenCaptureAudio.SetValue(true);
            options.publishScreenCaptureVideo.SetValue(true);
#endif
        options.clientRoleType.SetValue(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
        var ret = RtcEngine.JoinChannelEx(_token, new RtcConnection(_channelName, this.Uid2), options);
        Debug.Log("JoinChannelEx returns: " + ret);
    }
}
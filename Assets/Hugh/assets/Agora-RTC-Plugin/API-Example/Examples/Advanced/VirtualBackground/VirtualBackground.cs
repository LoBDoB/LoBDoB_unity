using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Agora.Rtc;
using Agora.Util;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Logger = Agora.Util.Logger;

public class VirtualBackground : MonoBehaviour
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

    public uint uid1 = 10;
    public uint uid2;
    private static GameObject playerVideo;
    private static GameObject screenVideo;

    public static int userCount;
    public List<GameObject> othersTransform = new List<GameObject>();
    private static List<GameObject> otherTransformUse;

    Button _startShareBtn;
    Button _stopShareBtn;
    Dropdown _winIdSelect;


    public List<uint> otherID = new List<uint>();

    public Button a;
    public Button b;


    GameObject obj;
    //

    public string savePath = "/Users/iseungmin/Downloads/";


    //start screen share relate function

    public void OnStartShareBtnClick()
    {
        if (RtcEngine == null) return;

        if (_startShareBtn != null) _startShareBtn.gameObject.SetActive(false);
        if (_stopShareBtn != null) _stopShareBtn.gameObject.SetActive(true);

#if UNITY_ANDROID || UNITY_IPHONE
            var parameters2 = new ScreenCaptureParameters2();
            parameters2.captureAudio = true;
            parameters2.captureVideo = true;
            var nRet = RtcEngine.StartScreenCapture(parameters2);
            this.Log.UpdateLog("StartScreenCapture :" + nRet);
#else
        RtcEngine.StopScreenCapture();
        if (_winIdSelect == null) return;
        var option = _winIdSelect.options[_winIdSelect.value].text;
        if (string.IsNullOrEmpty(option)) return;

        if (option.Contains("ScreenCaptureSourceType_Window"))
        {
            var windowId = option.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1];
            Log.UpdateLog(string.Format(">>>>> Start sharing {0}", windowId));
            var nRet = RtcEngine.StartScreenCaptureByWindowId(ulong.Parse(windowId), default(Rectangle),
                    default(ScreenCaptureParameters));
            this.Log.UpdateLog("StartScreenCaptureByWindowId:" + nRet);
        }
        else
        {
            var dispId = uint.Parse(option.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1]);
            Log.UpdateLog(string.Format(">>>>> Start sharing display {0}", dispId));
            var nRet = RtcEngine.StartScreenCaptureByDisplayId(dispId, default(Rectangle),
                new ScreenCaptureParameters { captureMouseCursor = true, frameRate = 30 });
            this.Log.UpdateLog("StartScreenCaptureByDisplayId:" + nRet);
        }
#endif
        HideUIBtn();
        ScreenShareJoinChannel();
    }

    public void ScreenShareJoinChannel()
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
        var ret = RtcEngine.JoinChannelEx(_token, new RtcConnection(_channelName, this.uid2), options);
        Debug.Log("JoinChannelEx returns: " + ret);
    }


    //stop the screen relation function
    public void OnStopShareBtnClick()
    {
        ScreenShareLeaveChannel();
        if (_startShareBtn != null) _startShareBtn.gameObject.SetActive(true);
        if (_stopShareBtn != null) _stopShareBtn.gameObject.SetActive(false);
        RtcEngine.StopScreenCapture();
    }


    private void ScreenShareLeaveChannel()
    {
        RtcEngine.LeaveChannelEx(new RtcConnection(_channelName, uid2));
    }
    //

    //
    private void PrepareScreenCapture()
    {
        _winIdSelect = GameObject.Find("winIdSelect").GetComponent<Dropdown>();

        if (_winIdSelect == null || RtcEngine == null) return;

        _winIdSelect.ClearOptions();

        SIZE t = new SIZE();
        t.width = 360;
        t.height = 240;
        SIZE s = new SIZE();
        s.width = 360;
        s.height = 240;
        var info = RtcEngine.GetScreenCaptureSources(t, s, true);


        _winIdSelect.AddOptions(info.Select(w =>
                new Dropdown.OptionData(
                    string.Format("{0}:{1} | {2}", w.sourceName, w.sourceTitle, w.sourceId)))
            .ToList());
    }



    public bool activate = false;
    public void ActivateScreenShare()
    {


        if (activate == true)
        {
            activate = false;
            _winIdSelect.gameObject.SetActive(false);
            a.gameObject.SetActive(false);
            b.gameObject.SetActive(false);
        }
        else
        {
            activate = true;
            _winIdSelect.gameObject.SetActive(true);
            a.gameObject.SetActive(true);
            b.gameObject.SetActive(true);
        }
    }

    void HideUIBtn()
    {
        //yield return new WaitForSeconds(5f);
        _winIdSelect.gameObject.SetActive(false);
        a.gameObject.SetActive(false);
        b.gameObject.SetActive(false);
    }

    //private void OnEnable()
    //{
    //    uid1 = (uint)UnityEngine.Random.Range(0, 3000);
    //    uid2 = (uint)UnityEngine.Random.Range(4000,6000);
    //    BringTransform();
    //    GetObject();
    //    LoadAssetData();
    //    chatIcon.SetActive(false);

    //    if (CheckAppId())
    //    {

    //        InitEngine();
    //        PrepareScreenCapture();
    //        InitLogFilePath();
    //        JoinChannel();
    //        OnStartButtonPress();

    //        HideUIBtn();
    //    }
    //    chatInputField.onSubmit.AddListener(delegate { onSendButtonPress(); });

    //}

    public InputField idInput;
    public InputField tokenInput;

    public void GoGo()
    {
        uid1 = UInt32.Parse(idInput.text);
        _token = tokenInput.text;
        StartCoroutine(GoAhead());
    }

    IEnumerator GoAhead()
    {
        yield return null;

        idInput.gameObject.SetActive(false);
        //uid1 = (uint)UnityEngine.Random.Range(0, 3000);
        uid2 = (uint)UnityEngine.Random.Range(4000, 6000);
        BringTransform();
        GetObject();
        LoadAssetData(_token);
        chatIcon.SetActive(false);

        if (CheckAppId())
        {

            InitEngine();
            PrepareScreenCapture();
            InitLogFilePath();
            JoinChannel();
            OnStartButtonPress();

            HideUIBtn();
        }
        chatInputField.onSubmit.AddListener(delegate { onSendButtonPress(); });
    }


    private void BringTransform()
    {
        otherTransformUse = othersTransform;
        //Debug.Log(userPositions[0]);
    }

    // Update is called once per frame
    private void Update()
    {
        PermissionHelper.RequestMicrophontPermission();
        PermissionHelper.RequestCameraPermission();
    }


    public static void ExtendScreenShareView(GameObject btn)
    {

        RawImage objImg = btn.GetComponent<RawImage>();

        Debug.LogError(objImg.material);



    }

    //Show data in AgoraBasicProfile
    [ContextMenu("ShowAgoraBasicProfileData")]
    private void LoadAssetData(string token)
    {
        if (_appIdInput == null) return;
        _appID = _appIdInput.appID;
        _token = token;
        _channelName = _appIdInput.channelName;
    }

    private bool CheckAppId()
    {
        Log = new Logger(LogText);
        return Log.DebugAssert(_appID.Length > 10, "Please fill in your appId in API-Example/profile/appIdInput.asset");
    }

    private void InitEngine()
    {
        RtcEngine = Agora.Rtc.RtcEngine.CreateAgoraRtcEngineEx();
        UserEventHandler handler = new UserEventHandler(this);
        RtcEngineContext context = new RtcEngineContext(_appID, 0,
                                    CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_LIVE_BROADCASTING,
                                    AUDIO_SCENARIO_TYPE.AUDIO_SCENARIO_DEFAULT);
        RtcEngine.Initialize(context);
        RtcEngine.InitEventHandler(new UserEventHandler(this));
    }

    private void JoinChannel()
    {
        RtcEngine.EnableAudio();
        RtcEngine.EnableVideo();
        RtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);

        ChannelMediaOptions options = new ChannelMediaOptions();
        options.autoSubscribeAudio.SetValue(true);
        options.autoSubscribeVideo.SetValue(true);

        options.publishCameraTrack.SetValue(true);
        options.publishScreenTrack.SetValue(false);
        options.enableAudioRecordingOrPlayout.SetValue(true);
        options.clientRoleType.SetValue(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
        RtcEngine.JoinChannel(_token, _channelName, this.uid1, options);
    }

    private void InitLogFilePath()
    {
        var path = Application.persistentDataPath + "/rtc.log";
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            path = path.Replace('/', '\\');
#endif
        var nRet = RtcEngine.SetLogFile(path);
        this.Log.UpdateLog(string.Format("logPath:{0},nRet:{1}", path, nRet));
    }


    private void OnStartButtonPress()
    {
        var source = new VirtualBackgroundSource();
        source.background_source_type = BACKGROUND_SOURCE_TYPE.BACKGROUND_COLOR;
        source.color = 0X02FF00;
        var segproperty = new SegmentationProperty();
        var nRet = RtcEngine.EnableVirtualBackground(true, source, segproperty, MEDIA_SOURCE_TYPE.PRIMARY_CAMERA_SOURCE);
        this.Log.UpdateLog("EnableVirtualBackground true :" + nRet);

    }


    private void OnDestroy()
    {
        Debug.Log("OnDestroy");
        if (RtcEngine == null) return;
        RtcEngine.InitEventHandler(null);
        RtcEngine.LeaveChannel();
        RtcEngine.Dispose();
    }

    internal string GetChannelName()
    {
        return _channelName;
    }

    internal static void GetObject()
    {
        playerVideo = GameObject.Find("MinePosition");
        screenVideo = GameObject.Find("ScreenVideo");
    }

    #region -- Video Render UI Logic ---


    internal static void MakeVideoView(uint uid, string channelId = "", VIDEO_SOURCE_TYPE videoSourceType = VIDEO_SOURCE_TYPE.VIDEO_SOURCE_CAMERA)
    {

        if (videoSourceType == VIDEO_SOURCE_TYPE.VIDEO_SOURCE_CAMERA || videoSourceType == VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE)
        {
            GameObject player = Instantiate(playerVideo);
            VideoSurface videoSurface = player.transform.GetChild(0).GetChild(0).GetComponent<VideoSurface>();
            if (ReferenceEquals(videoSurface, null)) return;
            //mine

            if (uid == 0)
            {
                //Transform screen_user = GameObject.Find("laptop_User").transform.GetChild(0);
                //Transform transform_screen = GameObject.Find("laptop_User").transform.GetChild(0).GetChild(0);

                //player.transform.SetParent(screen_user.transform);
                //player.transform.position = transform_screen.transform.position;
                //player.transform.rotation = transform_screen.transform.rotation;
                //player.transform.localScale = transform_screen.transform.localScale;

                //player.transform.position = userPositions[0];


                Transform transform_screen = GameObject.Find("laptop_User").transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0);
                Transform userImage = player.transform.GetChild(0).GetChild(0);
                RectTransform userRect = userImage.GetComponent<RectTransform>();
                userImage.SetParent(transform_screen);

                userRect.localPosition = new Vector3(0, 0, 0);
                userRect.sizeDelta = new Vector2(2.415f, 1.6124f);
                userRect.localEulerAngles = new Vector3(180, 0, 0);
                userRect.localScale = new Vector3(1, 1, 1);


                userRect.name = "MainCameraView";
                videoSurface.SetForUser(uid, channelId);
                videoSurface.SetEnable(true);
            }
            
            //differet user
            else if (uid > 4000)
            {
                Transform transform_screen = GameObject.Find("laptop_User").transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0);
                Transform screen = player.transform.GetChild(0).GetChild(0);
                RectTransform screen_Size = screen.GetComponent<RectTransform>();


                screen.transform.SetParent(transform_screen);
                screen_Size.localPosition = new Vector3(0, 0, 0);
                screen_Size.sizeDelta = new Vector2(2.5f, 1f);
                screen_Size.localEulerAngles = new Vector3(180, 0, 0);
                screen_Size.localScale = new Vector3(1, 1, 1);



                player.name = uid.ToString();
                videoSurface.SetForUser(uid, channelId, VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);
                videoSurface.SetEnable(true);


            }
            else
            {
                userCount += 1;
                //player.transform.position = userPositions[userCount];

                player.transform.position = otherTransformUse[userCount - 1].transform.position;
                player.transform.rotation = otherTransformUse[userCount - 1].transform.rotation;
                player.transform.localScale = otherTransformUse[userCount - 1].transform.localScale;


                player.name = uid.ToString();
                videoSurface.SetForUser(uid, channelId, VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);
                videoSurface.SetEnable(true);
            }
        }

        else if (videoSourceType == VIDEO_SOURCE_TYPE.VIDEO_SOURCE_SCREEN)
        {
            GameObject player = Instantiate(screenVideo);
            Transform screen = player.transform.GetChild(0).GetChild(0);
            RectTransform screen_Size = screen.GetComponent<RectTransform>();


            Debug.LogError(screen);
            VideoSurface videoSurface = player.transform.GetChild(0).GetChild(0).GetComponent<VideoSurface>();

            if (ReferenceEquals(videoSurface, null)) return;

            Transform transform_screen = GameObject.Find("laptop_User").transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0);

            screen.transform.SetParent(transform_screen);
            screen_Size.localPosition = new Vector3(0, 0, 0);
            screen_Size.sizeDelta = new Vector2(2.415f, 1.6124f);
            screen_Size.localEulerAngles = new Vector3(180, 180, 0);
            screen_Size.localScale = new Vector3(1, 1, 1);

            screen.name = "ScreenShareView";
            videoSurface.SetForUser(uid, channelId, VIDEO_SOURCE_TYPE.VIDEO_SOURCE_SCREEN);
            videoSurface.SetEnable(true);
            //mine
            //if (uid == 0)
            //{
                
            //}
            ////differet user
            //else
            //{
            //    Transform transform_screen = GameObject.Find("laptop_User").transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0);

            //    screen.transform.SetParent(transform_screen);
            //    screen_Size.localPosition = new Vector3(0, 0, 0);
            //    screen_Size.sizeDelta = new Vector2(2.415f, 1.6124f);
            //    screen_Size.localEulerAngles = new Vector3(180, 180, 0);
            //    screen_Size.localScale = new Vector3(1, 1, 1);

            //    player.name = uid.ToString();
            //    videoSurface.SetForUser(uid, channelId, VIDEO_SOURCE_TYPE.VIDEO_SOURCE_SCREEN);
            //    videoSurface.SetEnable(true);
            //}
        }
    }

    internal static void DestroyVideoView(string name)
    {
        var go = GameObject.Find(name);
        if (!ReferenceEquals(go, null))
        {
            Destroy(go);
        }
    }


    //chatting 기능 관련 로직
    //public GameObject messageInfo;
    //public Transform messageContent;
    public InputField chatInputField;
    public GameObject chat;
    public GameObject chatIcon;
    private int _streamId = -1;
    public ChatManager chatManager;

    public void HideChattingUI()
    {
        chat.SetActive(false);
        chatIcon.SetActive(true);
    }

    public void ChatIconClick()
    {
        chatIcon.SetActive(false);
        chat.SetActive(true);
    }

    public void InstantiateMessage(string id, string chatText)
    {
        //GameObject message = Instantiate(messageInfo,messageContent);
        //message.transform.GetChild(0).GetComponent<Text>().text = id;
        //message.transform.GetChild(2).GetComponent<Text>().text = chatText;

        //base.InstantiateMessage(id,chatText);

        //if (id == uid1.ToString())
        //{
        //    chatManager.Chat(true, chatText, "나", null);
        //    chatInputField.text = "";
        //    //GUI.FocusControl(null);
        //}

        if (id == uid1.ToString())
        {
            chatManager.Chat(true, chatText, "나", null);
            chatInputField.text = "";
            //GUI.FocusControl(null);
        }
        else if (id == "1001")
        {
            //chatManager.Chat(false, chatText, id.ToString(), null);
            chatManager.Chat(false, chatText, "HARRY", null);
            chatInputField.text = "";
        }
        else if (id == "1002")
        {
            chatManager.Chat(false, chatText, "Chloe", null);
            chatInputField.text = "";
        }
        else if (id == "1003")
        {
            chatManager.Chat(false, chatText, "DITTO", null);
            chatInputField.text = "";
        }
        else if (id == "1004")
        {
            chatManager.Chat(false, chatText, "BENEDICT", null);
            chatInputField.text = "";
        }
        else if (id == "1005")
        {
            chatManager.Chat(false, chatText, "JSON", null);
            chatInputField.text = "";
        }


    }

    public void SendStreamMessage(int streamId, string message)
    {
        byte[] byteArray = System.Text.Encoding.Unicode.GetBytes(message);
        var nRet = RtcEngine.SendStreamMessage(streamId, byteArray, Convert.ToUInt32(byteArray.Length));
        this.Log.UpdateLog("SendStreamMessage :" + nRet);
    }
    private int CreateDataStreamId()
    {
        if (this._streamId == -1)
        {
            var config = new DataStreamConfig();
            config.syncWithAudio = false;
            config.ordered = true;
            var nRet = RtcEngine.CreateDataStream(ref this._streamId, config);
            this.Log.UpdateLog(string.Format("CreateDataStream: nRet{0}, streamId{1}", nRet, _streamId));
        }
        return _streamId;
    }
    private void onSendButtonPress()
    {

        if (chatInputField.text == "")
        {
            Log.UpdateLog("Dont send empty message!");
        }

        int streamId = this.CreateDataStreamId();
        if (streamId < 0)
        {
            Log.UpdateLog("CreateDataStream failed!");
            return;
        }
        else
        {
            SendStreamMessage(streamId, chatInputField.text);
            InstantiateMessage(uid1.ToString(), chatInputField.text);


        }
    }




    #endregion
}

#region -- Agora Event ---

internal class UserEventHandler : IRtcEngineEventHandler
{
    private readonly VirtualBackground _desktopScreenShare;

    internal UserEventHandler(VirtualBackground desktopScreenShare)
    {
        _desktopScreenShare = desktopScreenShare;
    }

    public override void OnError(int err, string msg)
    {
        _desktopScreenShare.Log.UpdateLog(string.Format("OnError err: {0}, msg: {1}", err, msg));
    }

    public override void OnJoinChannelSuccess(RtcConnection connection, int elapsed)
    {
        int build = 0;
        _desktopScreenShare.Log.UpdateLog(string.Format("sdk version: ${0}",
            _desktopScreenShare.RtcEngine.GetVersion(ref build)));
        _desktopScreenShare.Log.UpdateLog(
            string.Format("OnJoinChannelSuccess channelName: {0}, uid: {1}, elapsed: {2}",
                            connection.channelId, connection.localUid, elapsed));
        if (connection.localUid == _desktopScreenShare.uid1)
        {
            VirtualBackground.MakeVideoView(0);
        }
        else if (connection.localUid == _desktopScreenShare.uid2)
        {
            VirtualBackground.MakeVideoView(0, "", VIDEO_SOURCE_TYPE.VIDEO_SOURCE_SCREEN);
        }
    }

    public override void OnRejoinChannelSuccess(RtcConnection connection, int elapsed)
    {
        _desktopScreenShare.Log.UpdateLog("OnRejoinChannelSuccess");
    }

    public override void OnLeaveChannel(RtcConnection connection, RtcStats stats)
    {
        _desktopScreenShare.Log.UpdateLog("OnLeaveChannel");
        if (connection.localUid == _desktopScreenShare.uid1)
        {
            VirtualBackground.DestroyVideoView("MainCameraView");
        }
        else if (connection.localUid == _desktopScreenShare.uid2)
        {
            VirtualBackground.DestroyVideoView("ScreenShareView");
        }
    }

    public override void OnStreamMessage(RtcConnection connection, uint remoteUid, int streamId, byte[] data, uint length, ulong sentTs)
    {
        string streamMessage = System.Text.Encoding.Unicode.GetString(data);

        _desktopScreenShare.InstantiateMessage(remoteUid.ToString(), streamMessage);

        Debug.LogError(string.Format("OnStreamMessage remoteUid: {0}, stream message: {1}", remoteUid, streamMessage));
        //Debug.LogError("arrive");
        //_desktopScreenShare.BytesToFile(data,_desktopScreenShare.savePath+"plz.png");
    }

    public override void OnClientRoleChanged(RtcConnection connection, CLIENT_ROLE_TYPE oldRole, CLIENT_ROLE_TYPE newRole)
    {
        _desktopScreenShare.Log.UpdateLog("OnClientRoleChanged");
    }

    public override void OnUserJoined(RtcConnection connection, uint uid, int elapsed)
    {
        for (int i = 0; i < _desktopScreenShare.otherID.Count(); i++)
        {
            if (_desktopScreenShare.otherID[i] == uid)
            {
                return;
            }
            
        }
        


        _desktopScreenShare.Log.UpdateLog(string.Format("OnUserJoined uid: ${0} elapsed: ${1}", uid, elapsed));
        if (uid != _desktopScreenShare.uid1 && uid != _desktopScreenShare.uid2)
        {
            _desktopScreenShare.otherID.Add(uid);
            //Debug.LogError("111");
            VirtualBackground.MakeVideoView(uid, _desktopScreenShare.GetChannelName(), VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);
        }
    }

    public override void OnUserOffline(RtcConnection connection, uint uid, USER_OFFLINE_REASON_TYPE reason)
    {
        _desktopScreenShare.Log.UpdateLog(string.Format("OnUserOffLine uid: ${0}, reason: ${1}", uid,
            (int)reason));
        if (uid != _desktopScreenShare.uid1 && uid != _desktopScreenShare.uid2)
        {
            VirtualBackground.DestroyVideoView(uid.ToString());
        }
    }
}

#endregion
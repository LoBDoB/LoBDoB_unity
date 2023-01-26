using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using Agora.Rtc;
using Agora.Util;
using Logger = Agora.Util.Logger;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Agora_RTC_Plugin.API_Example.Examples.Advanced.VirtualBackground
{
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
        internal IRtcEngine RtcEngine = null;

        public uint uid1 = 10;

        private static GameObject playerVideo;
        public static int userCount;




        public List<GameObject> othersTransform = new List<GameObject>();

        private static List<GameObject> otherTransformUse;

        //private static List<Vector3> userRotation;

        //private static List<Vector3> userScale;

        private void Start()
        {
            uid1 = (uint)Random.Range(0, 3000);
            BringTransform();
            GetObject();
            LoadAssetData();
            if (CheckAppId())
            {
                InitEngine();
                InitLogFilePath();
                JoinChannel();
                OnStartButtonPress();
            }
            
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

        //Show data in AgoraBasicProfile
        [ContextMenu("ShowAgoraBasicProfileData")]
        private void LoadAssetData()
        {
            if (_appIdInput == null) return;
            _appID = _appIdInput.appID;
            _token = _appIdInput.token;
            _channelName = _appIdInput.channelName;
        }

        private bool CheckAppId()
        {
            Log = new Logger(LogText);
            return Log.DebugAssert(_appID.Length > 10, "Please fill in your appId in API-Example/profile/appIdInput.asset");
        }

        private void InitEngine()
        {
            RtcEngine = Agora.Rtc.RtcEngine.CreateAgoraRtcEngine();
            UserEventHandler handler = new UserEventHandler(this);
            RtcEngineContext context = new RtcEngineContext(_appID, 0,
                                        CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_LIVE_BROADCASTING,
                                        AUDIO_SCENARIO_TYPE.AUDIO_SCENARIO_DEFAULT);
            RtcEngine.Initialize(context);
            RtcEngine.InitEventHandler(handler);
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
            
        }

        #region -- Video Render UI Logic ---


        internal static void MakeVideoView(uint uid, string channelId = "")
        {
            

            GameObject player = Instantiate(playerVideo);
            VideoSurface videoSurface = player.transform.GetChild(0).GetChild(0).GetComponent<VideoSurface>();

            if (ReferenceEquals(videoSurface, null)) return;

            //mine
            if (uid == 0)
            {
                Transform screen_user = GameObject.Find("laptop_User").transform.GetChild(0);
                Transform transform_screen = GameObject.Find("laptop_User").transform.GetChild(0).GetChild(0);

                player.transform.SetParent(screen_user.transform);
                player.transform.position = transform_screen.transform.position;
                player.transform.rotation = transform_screen.transform.rotation;
                player.transform.localScale = transform_screen.transform.localScale;

                //player.transform.position = userPositions[0];
                player.name = uid.ToString();
                videoSurface.SetForUser(uid, channelId);
                videoSurface.SetEnable(true);
            }

            //differet user
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

        internal static void DestroyVideoView(uint uid)
        {
            var go = GameObject.Find(uid.ToString());
            if (!ReferenceEquals(go, null))
            {
                Destroy(go);
            }
        }

        #endregion
    }

    #region -- Agora Event ---

    internal class UserEventHandler : IRtcEngineEventHandler
    {
        private readonly VirtualBackground _sample;

        internal UserEventHandler(VirtualBackground sample)
        {
            _sample = sample;
        }

        public override void OnError(int err, string msg)
        {
            _sample.Log.UpdateLog(string.Format("OnError err: {0}, msg: {1}", err, msg));
        }
        //mine player user join
        public override void OnJoinChannelSuccess(RtcConnection connection, int elapsed)
        {
            //_sample.userCount += 1;
            int build = 0;
            Debug.Log("Agora: OnJoinChannelSuccess ");
            _sample.Log.UpdateLog(string.Format("sdk version: ${0}",
                _sample.RtcEngine.GetVersion(ref build)));
            _sample.Log.UpdateLog(
                string.Format("OnJoinChannelSuccess channelName: {0}, uid: {1}, elapsed: {2}",
                                connection.channelId, connection.localUid, elapsed));
            //Debug.LogError(connection.localUid);
            VirtualBackground.MakeVideoView(0);
        }

        public override void OnRejoinChannelSuccess(RtcConnection connection, int elapsed)
        {
            _sample.Log.UpdateLog("OnRejoinChannelSuccess");
        }

        public override void OnLeaveChannel(RtcConnection connection, RtcStats stats)
        {
            _sample.Log.UpdateLog("OnLeaveChannel");
            VirtualBackground.DestroyVideoView(0);
        }

        public override void OnClientRoleChanged(RtcConnection connection, CLIENT_ROLE_TYPE oldRole, CLIENT_ROLE_TYPE newRole)
        {
            _sample.Log.UpdateLog("OnClientRoleChanged");
        }

        //different user join 
        public override void OnUserJoined(RtcConnection connection, uint uid, int elapsed)
        {
            //_sample.userCount += 1;
            _sample.Log.UpdateLog(string.Format("OnUserJoined uid: ${0} elapsed: ${1}", uid, elapsed));
            VirtualBackground.MakeVideoView(uid, _sample.GetChannelName());
        }

        public override void OnUserOffline(RtcConnection connection, uint uid, USER_OFFLINE_REASON_TYPE reason)
        {
            _sample.Log.UpdateLog(string.Format("OnUserOffLine uid: ${0}, reason: ${1}", uid,
                (int)reason));
            VirtualBackground.DestroyVideoView(uid);
        }
    }

    #endregion
}
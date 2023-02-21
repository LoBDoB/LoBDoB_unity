using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class Test_Connect : MonoBehaviourPunCallbacks
{
    void Start()
    {
        //PhotonNetwork.Disconnect();
        PhotonNetwork.ConnectUsingSettings();
    }


    //마스터 서버 접속성공시 호출(Lobby에 진입할 수 없는 상태)
    public override void OnConnected()
    {
        base.OnConnected();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }

    //마스터 서버 접속성공시 호출(Lobby에 진입할 수 있는 상태)
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);

        //내 닉네임 설정
        PhotonNetwork.NickName = "Player_" + Random.Range(1, 1000);
        //로비 진입 요청
        PhotonNetwork.JoinLobby();
    }

    //로비 진입 성공시 호출
    public override void OnJoinedLobby()
    {

        base.OnJoinedLobby();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);

        CreateRoom();
    }

    bool called = false;

    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 0;
        roomOptions.IsVisible = true;

        PhotonNetwork.CreateRoom("Room", roomOptions);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("OnCreatedRoom");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("OnCreateRoomFailed , " + returnCode + ", " + message);
        JoinRoom();
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom("Room");
    }
    public Vector3 playerSpawnPos = Vector3.zero;
    public Quaternion playerSpawnRot = Quaternion.identity;
    public Vector3 camSpawnPos = Vector3.zero;
    public Quaternion camSpawnRot = Quaternion.identity;
    public bool owl = true;
    GameObject player;
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("OnJoinedRoom");

        if (owl)
        {
            player = PhotonNetwork.Instantiate("Player_owl", playerSpawnPos, playerSpawnRot);
            PhotonNetwork.Instantiate("CamFollow_owl", camSpawnPos, camSpawnRot);
        }
        else
        {
            player = PhotonNetwork.Instantiate("Player_avatar", playerSpawnPos, playerSpawnRot);
            PhotonNetwork.Instantiate("CamFollow_avatar", camSpawnPos, camSpawnRot);
        }

        //player.name = PhotonNetwork.NickName;
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("OnJoinRoomFailed, " + returnCode + ", " + message);
    }

}
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Harry_GameManager : MonoBehaviour
{
    public static Harry_GameManager Instance;

    public Dictionary<string, bool> Deco_Use = new Dictionary<string, bool>();

    Text nickName;

    bool player_CanMove = true;
    public bool Player_CanMove { get { return player_CanMove; } set { player_CanMove = value; } }
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        nickName = GameObject.Find("Nickname").GetComponent<Text>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (Camera.main == null)
            return;
        else
            nickName.text = PhotonNetwork.NickName;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            foreach (string key in Deco_Use.Keys)
            {
                print(key + " " + Deco_Use[key].ToString());
            }
        }
    }
}

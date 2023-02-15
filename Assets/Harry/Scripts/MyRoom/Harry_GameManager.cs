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

    bool player_CanMove = true;
    public bool Player_CanMove { get { return player_CanMove; } set { player_CanMove = value; } }
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

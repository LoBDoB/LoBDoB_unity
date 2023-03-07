using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LectureRoom : MonoBehaviour
{
    static public LectureRoom Instance;

    public UserID userid;


    public Dictionary<string,uint> Id = new Dictionary<string, uint>();


    public List<string> nickName = new List<string>();

    public List<uint> uid = new List<uint>();

    private void Start()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    
}

public class UserID
{
    public string nickName;
    public uint uid_FaceCamera;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harry_SquareCam : MonoBehaviour
{
    bool isOn = false;
    public bool IsOn
    {
        get { return isOn; }
        set
        {
            if (isOn != value)
            {
                isOn = value;

                if (isOn)
                {
                    iTween.MoveTo(gameObject, iTween.Hash("x", 0f, "y", 1.578f, "z", 0.161f, "islocal", true, "time", 1f, "easetype", iTween.EaseType.easeOutCirc));
                    iTween.RotateTo(gameObject, iTween.Hash("x", 21.851f, "y", 0f, "z", 0f, "islocal", true, "time", 1f, "easetype", iTween.EaseType.easeOutCirc));
                }
                else
                {
                    iTween.MoveTo(gameObject, iTween.Hash("x", 0f, "y", 1f, "z", -3f, "islocal", true, "time", 1f, "easetype", iTween.EaseType.easeOutCirc));
                    iTween.RotateTo(gameObject, iTween.Hash("x", 0f, "y", 0f, "z", 0f, "islocal", true, "time", 1f, "easetype", iTween.EaseType.easeOutCirc));
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        IsOn = Harry_SquareManager.Instance.isTablet;
    }
}

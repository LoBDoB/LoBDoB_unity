using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harry_Tablet : MonoBehaviour
{
    Harry_CamController cam;

    bool isOn = false;
    public bool IsOn
    {
        get { return isOn; }
        set
        {
            if (isOn != value && cam != null)
            {
                isOn = value;

                if (isOn)
                {
                    iTween.MoveTo(gameObject, iTween.Hash("x", -0.187f, "y", -0.033f, "z", 0.322f, "islocal", true, "time", 1f, "easetype", iTween.EaseType.easeOutCirc));

                    cam.StartInter(Harry_SquareManager.Instance.player.transform.position + new Vector3(0, 1.6f, 0.15f),
                        Harry_SquareManager.Instance.player.transform.eulerAngles + new Vector3(21f, 0, 0));
                    Camera.main.cullingMask = Camera.main.cullingMask & ~(1 << LayerMask.NameToLayer("Player"));
                    Cursor.visible = true;
                }
                else
                {
                    iTween.MoveTo(gameObject, iTween.Hash("x", -0.349000007f, "y", -0.158999994f, "z", 0.244000003f, "islocal", true, "time", 1f, "easetype", iTween.EaseType.easeOutCirc));
                    cam.EndInter();
                    Camera.main.cullingMask = -1;
                }
            }
        }
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        cam = transform.parent.transform.parent.GetComponent<Harry_CamController>();
    }

    // Update is called once per frame
    void Update()
    {
        IsOn = Harry_SquareManager.Instance.isTablet;
    }
}

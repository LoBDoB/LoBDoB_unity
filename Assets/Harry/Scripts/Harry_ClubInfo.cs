using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Harry_ClubInfo : MonoBehaviour
{
    public Club clubInfo;
    Canvas canvas;
    Text clubName;
    Image clubImage;

    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        canvas = transform.GetChild(0).GetComponent<Canvas>();

        clubName = canvas.transform.Find("Name").GetComponent<Text>();
        clubImage = canvas.transform.Find("Image").GetComponent<Image>();

        clubName.text = clubInfo.name;
        StartCoroutine(DownloadImage(clubInfo.img_url));

        StartCoroutine("WaitForCam");
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindWithTag("Player") != null && player == null)
        {
            player = GameObject.FindWithTag("Player");
        }

        if (player)
        {
            // �׻� ī�޶� �ٶ󺸰�
            canvas.transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
            // �÷��̾ ������ ���� UI�� Ű��, �־����� ��
            if (Vector3.Magnitude(transform.position - player.transform.position) >= transform.localScale.x * 1.5f)
            {
                canvas.gameObject.SetActive(false);
            }
            else
            {
                canvas.gameObject.SetActive(true);
            }
        }
    }

    // ī�޶� ������ ��ٷȴٰ� World Space Camera�� ����
    IEnumerator WaitForCam()
    {
        while (Camera.main == null)
        {
            yield return null;
        }

        canvas.GetComponent<Canvas>().worldCamera = Camera.main;
    }

    // ����Ǿ� �ִ� URL���� �̹��� �ٿ�ε� �� ����
    IEnumerator DownloadImage(string url)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Texture2D tex = ((DownloadHandlerTexture)www.downloadHandler).texture;
                clubImage.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
                Debug.Log("Image Download Complete");
            }
        }
    }
}

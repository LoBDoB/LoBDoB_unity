using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.Networking;


public class Crwaling : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetText());
    }

    public string result;
    IEnumerator GetText()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://lily.sunmoon.ac.kr/Page2/Story/Notice.aspx?ca=001");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            //다운로드 해서 텍스트로 뽑아냄
            //Debug.Log(www.downloadHandler.text);
            
            //byte[] results = www.downloadHandler.data;
            
            if (www.downloadHandler.text.Contains("alignL"))
            {
                Debug.Log("in");
            }

            

            //Debug.LogWarning(str);
        }
    }
}



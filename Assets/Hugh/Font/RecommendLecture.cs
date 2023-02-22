using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;

using UnityEngine.UI;

public class RecommendLecture : MonoBehaviour
{
    string url = "http://192.168.50.55:8000/LRS/";
    public Texture[] lecture_State_Texture;
    public RawImage[] lecture_ImageObject;
    public RawImage[] lecture_StateObject;

    public GameObject safari;


    private void OnEnable()
    {
        StartCoroutine(SendMessaget("컴퓨터네트크","공학"));
    }


    IEnumerator SendMessaget(string name,string aff)
    {
        //string json = JsonUtility.ToJson(userInput);
        WWWForm wWForm = new WWWForm();

        wWForm.AddField("name", name);
        wWForm.AddField("aff", aff);

        UnityWebRequest www = UnityWebRequest.Post(url, wWForm);

        yield return www.SendWebRequest();

        if (www != null)
        {
            var result1 = www.downloadHandler.text;
            //JsonParsing(result1);
            Debug.LogError(result1);
            JsonParsing(result1);
        }
        if (count == 0) 
        {

            www.Dispose();
        }
    }

    public int count = 0;
    void JsonParsing(string text)
    {
        JObject json = JObject.Parse(text);
        JToken jt = json["searchResult"];
        Debug.LogError(jt);
        foreach (JProperty j in jt)
        {
            Debug.LogError("key name : " + j.Name);
            Debug.LogError("name : "+ j.Value["수강 가능 여부"]);
            Debug.LogError("name : " + j.Value["URL 강좌 링크"]);
            Debug.LogError("name : " + j.Value["이미지 url"]);


            if (count < 3) 
            {
                Button btn = lecture_ImageObject[count].gameObject.GetComponent<Button>();
                btn.onClick.AddListener(() => SurfingUrl(j.Value["URL 강좌 링크"].ToString()));
                if ("진행중" == j.Value["수강 가능 여부"].ToString())
                {
                    lecture_StateObject[count].texture = lecture_State_Texture[1];
                }
                else
                {
                    lecture_ImageObject[count].texture = lecture_State_Texture[0];
                }


                StartCoroutine(DownloadTexture(lecture_ImageObject[count], j.Value["이미지 url"].ToString()));

                //Debug.LogError("데이터엔지니어s ");
                count += 1;
            }
            


            
        }
    }

    IEnumerator DownloadTexture(RawImage img, string url)
    {
        //Debug.LogError($"요청: {url}");
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("실패");
            Debug.LogError(url);
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("imageDownload");
            img.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }
    }



    public void SurfingUrl(string url)
    {
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        safari.SetActive(true);
        WebViewScript webViewScript = safari.GetComponent<WebViewScript>();
        webViewScript.StartWebView(url);
        //webViewScript.webViewObject.LoadURL(url);

#elif UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || UNITY_EDITOR_LINUX
        Debug.Log(url);
        Application.OpenURL(url);
#endif

    }
}

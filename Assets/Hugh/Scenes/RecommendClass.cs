using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;

using UnityEngine.UI;


public class RecommendClass : MonoBehaviour
{
    //public RawImage img;

    public Transform content;

    public Texture[] lecture_State_IMG;

    public InputField search;

    public GameObject lecture;

    public GameObject safari;


    string url = "http://192.168.50.132:8000/lecture_model/";

    public List<string> lecture_Title = new List<string>();
    public List<string> lecture_State = new List<string>();
    public List<string> lecture_URL = new List<string>();
    //public List<string> lecture_Line = new List<string>();
    public List<string> lecture_IMG_URL = new List<string>();


    public string dummyJson = @"{
  ""searchResult"": {
    ""인공지능과 기계학습"": {
      ""수강 가능 여부"": ""청강가능"",
      ""URL 강좌 링크"": ""http://www.kmooc.kr/courses/course-v1:KAISTk+KCS470+2017_K0203/about"",
      ""이미지 url"": ""http://www.kmooc.kr/asset-v1:KAISTk+KCS470+2017_K0203+type@asset+block@_크기변환_asset-v1_KAISTk_KCS470_2017_K0203_type_asset_block_banner2.jpg""
    },
    ""인공지능을 위한 기계학습 입문"": {
      ""수강 가능 여부"": ""진행중"",
      ""URL 강좌 링크"": ""http://www.kmooc.kr/courses/course-v1:KHUk+SW_KH005+2022_T2_1/about"",
      ""이미지 url"": ""http://www.kmooc.kr/asset-v1:KHUk+SW_KH005+2022_T2_1+type@asset+block@22_경희대_인공지능을위한기계학습입문_이원희__썸네일_1.jpg""
    },
    ""AI프로그래밍"": {
      ""수강 가능 여부"": ""진행중"",
      ""URL 강좌 링크"": ""http://www.kmooc.kr/courses/course-v1:HansungK+HSKMOOCk04+2022_T1/about"",
      ""이미지 url"": ""http://www.kmooc.kr/asset-v1:HansungK+HSKMOOCk04+2022_T1+type@asset+block@썸네일_AI프로그래밍_326x200.jpg""
    }
  }
}";


    public void SearchField()
    {
        //StartCoroutine(SendMessaget(search.text));
        //search.text = "";
        JsonParsing(dummyJson);
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


    IEnumerator SendMessaget(string json)
    {
        //string json = JsonUtility.ToJson(userInput);
        WWWForm wWForm = new WWWForm();

        wWForm.AddField("username", json);


        UnityWebRequest www = UnityWebRequest.Post(url, wWForm);

        yield return www.SendWebRequest();

        if (www != null)
        {
            var result1 = www.downloadHandler.text;
            //JsonParsing(result1);
            //Debug.LogError(result1);
            JsonParsing(result1);
        }
        www.Dispose();
    }


    int cout = 0;
    public void JsonParsing(string text)
    {
        JObject json = JObject.Parse(text);
        JToken jt = json["searchResult"];
        Debug.LogError(jt);

        Transform[] childList = content.GetComponentsInChildren<Transform>();


        if (childList != null)
        {
            for (int i = 1; i< childList.Length; i++)
            {
                if (childList[i] != transform)
                {
                    Destroy(childList[i].gameObject);
                }
            }
        }

        lecture_Title = new List<string>();
        lecture_State = new List<string>();
        lecture_IMG_URL = new List<string>();
        lecture_URL = new List<string>();


        foreach (JProperty j in jt)
        {
            //Debug.LogError("key name : " + j.Name);

            cout += 1;

            lecture_Title.Add(j.Name);
            lecture_State.Add(j.Value["수강 가능 여부"].ToString());
            //lecture_Line.Add(j.Value["강좌 계열"].ToString());
            lecture_URL.Add(j.Value["URL 강좌 링크"].ToString());
            lecture_IMG_URL.Add(j.Value["이미지 url"].ToString());




            RawImage showImage = Instantiate(lecture).GetComponent<RawImage>();

            Button btn = showImage.gameObject.AddComponent<Button>();
            btn.onClick.AddListener(() => SurfingUrl(j.Value["URL 강좌 링크"].ToString()));


            if (j.Value["수강 가능 여부"].ToString() == "청강가능")
            {
                showImage.transform.GetChild(0).GetComponent<RawImage>().texture = lecture_State_IMG[1];
            }
            else
            {
                showImage.transform.GetChild(0).GetComponent<RawImage>().texture = lecture_State_IMG[0];
            }
 
            showImage.transform.SetParent(content);
            StartCoroutine(DownloadTexture(showImage, j.Value["이미지 url"].ToString()));
            
        }
        cout = 0;
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

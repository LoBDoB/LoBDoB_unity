using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public class RecommendClass : MonoBehaviour
{

    string url = "http://192.168.50.55:8000/lecture_model/";

    private void Start()
    {
        StartCoroutine(SendMessaget("과."));
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



    public void JsonParsing(string json)
    {
        JObject jobject = JObject.Parse(json);
        Debug.LogError(jobject.ToString());

        // JSON 데이터 하위 객체인 members 객체의 name 값을 반복적으로 접근하는 방법
        JToken jToken = jobject["searchResult"];

        JToken jToken1 = jobject["editWord"];

        Debug.LogError(jToken);
        //Debug.LogError(jToken1);




        var data = JsonConvert.DeserializeObject<UnitTest1>(json);
        Debug.Log(data);

    }
}

public class UnitTest1
{
    public class Data
    {
        public Infolist[] InfoList { get; set; }

        public class Infolist
        {
            public string Name { get; set; }
            public string Habby { get; set; }
            public string Nmbr { get; set; }
        }
    }
}

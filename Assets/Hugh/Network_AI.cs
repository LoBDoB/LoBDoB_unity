using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Threading.Tasks;

public class Network_AI : MonoBehaviour
{

    string url = "http://192.168.50.55:8000/button_model/";



    // Start is called before the first frame update
    void Start()
    {

        UserInput userInput = new UserInput();

        userInput.value = "설정";


        string json = JsonUtility.ToJson(userInput);

        Debug.LogError(json);


        StartCoroutine(SendMessaget(json));

        //StartCoroutine(Upload("http://192.168.50.215:5000/create",json));

    }

    IEnumerator UnityWebRequestGETTest()
    {
        // UnityWebRequest에 내장되있는 GET 메소드를 사용한다.
        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();  // 응답이 올때까지 대기한다.

        if (www.error == null)  // 에러가 나지 않으면 동작.
        {
            Debug.Log(www.downloadHandler.text);
        }
        else
        {
            Debug.Log("error");
        }
    }

    IEnumerator SendMessaget(string json)
    {
        WWWForm wWForm = new WWWForm();

        wWForm.AddField("username", json);


        UnityWebRequest www = UnityWebRequest.Post(url, wWForm);

        yield return www.SendWebRequest();

        if (www != null)
        {
            var result1 = www.downloadHandler.text;
            JsonParsing(result1);
            //Debug.LogError(result1);
        }
        www.Dispose();
    }


    //IEnumerator Upload(string URL, string json)
    //{
    //    using (UnityWebRequest request = UnityWebRequest.Post(URL, json))
    //    {
    //        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
    //        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
    //        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
    //        request.SetRequestHeader("Contnet-Type", "application/json");


    //        yield return request.SendWebRequest();



    //        if (request.isNetworkError || request.isHttpError)
    //        {
    //            Debug.LogError(request.error);
    //        }
    //        else
    //        {
    //            Debug.Log(request.downloadHandler.text);
    //        }

    //        request.Dispose();
    //    }
    //}


    void JsonParsing(string json)
    {
        JObject jobject = JObject.Parse(json);
        Debug.LogError(jobject.ToString());

        // JSON 데이터 하위 객체인 members 객체의 name 값을 반복적으로 접근하는 방법
        JToken jToken = jobject["username"];
        Debug.LogError(jToken);


       
        foreach (JToken data in jToken)
        {
            Debug.LogError(data);
        }

    }

}

public class SearchRecommend
{
    public List<string> value;

}

public class UserInput
{
    public string value;

}

public class Keyword
{
    public string name;
   
}




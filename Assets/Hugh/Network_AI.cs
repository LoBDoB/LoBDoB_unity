using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Network_AI : MonoBehaviour
{

    string url = "http://127.0.0.1:8000/register";



    // Start is called before the first frame update
    void Start()
    {
        
        User user1 = new User();

        user1.name = "seung";
        user1.ID = "asdf";



        string json = JsonUtility.ToJson(user1);

        Debug.LogError(json);


        StartCoroutine(SendMessaget());

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

    IEnumerator SendMessaget()
    {
        WWWForm wWForm = new WWWForm();

        wWForm.AddField("username","1111");
        

        UnityWebRequest www = UnityWebRequest.Post(url, wWForm);

        yield return www.SendWebRequest();

        if (www != null)
        {
            var result = www.downloadHandler.text;
            Debug.LogError(result);
        }
        www.Dispose();
    }


    IEnumerator Upload(string URL, string json)
    {
        using (UnityWebRequest request = UnityWebRequest.Post(URL, json))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Contnet-Type","application/json");


            yield return request.SendWebRequest();



            if (request.isNetworkError || request.isHttpError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
            }

            request.Dispose();
        }
    }


}



public class Users
{
    public List<User> users = new List<User>();
}

public class User
{
    public string name;
    public string ID;

}




using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WebViewScript : MonoBehaviour
{

    //public static WebViewScript Instance;
    public WebViewObject webViewObject;
    public GameObject safariPanel;

    public int currnetURL;
    // Use this for initialization
    public List<string> surfSaveList = new List<string>();
    public int saveURLCount;


    public bool extend = false;


    public GameObject safariIcon;

    public bool shutDown = false;


    //private void Awake()
    //{
    //    Instance = this;
    //}

    void Start()
    {

        StartWebView("http://www.naver.com");
    }

    public void ChatWebview(string url)
    {
        webViewObject =
        (new GameObject("WebViewObject")).AddComponent<WebViewObject>();

        webViewObject.Init(1148, 720);
        //webViewObject.Init((msg) =>
        //{
        //    Debug.Log(string.Format("CallFromJS[{0}]", msg));
        //});

        webViewObject.LoadURL(url);
        webViewObject.SetVisibility(true);
        //webViewObject.SetMargins(3, 3, 3, 3);
    }

    public void OnSafariGetURL(string url)
    {
        webViewObject.LoadURL(url);
    }


    // Update is called once per frame
    string url = "http://www.naver.com";
    void Update()
    {

        if (Input.GetKey(KeyCode.Escape))
        {
            Destroy(webViewObject);
            return;
        }
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Destroy(webViewObject);
                return;
            }

        }
    }



    public void XBtn()
    {
        shutDown = true;
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX   
        surfSaveList = WebViewObject.Instance.surfingList;
        saveURLCount = WebViewObject.Instance.currentURL;
        
#endif
        webViewObject.gameObject.SetActive(false);
        safariIcon.SetActive(true);
        safariPanel.SetActive(false);


//        PlayerItem m_playerItem=PhotonManager.Instance.findPlayerItemByID((int)ScreenShareWhileVideoCall.Instance.Uid2);
        //GameObject player = GameObject.Find(m_playerItem .Nickname.text+ "(user)");
        //PlayerItem playerScript = player.GetComponent<PlayerItem>();
        //playerScript.webviewStart = false;
        //player.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
        //Debug.LogError(player);
        
    }
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
    public void _BTN()
    {

        shutDown = true;
        surfSaveList = WebViewObject.Instance.surfingList;
        saveURLCount = WebViewObject.Instance.currentURL;
        webViewObject.gameObject.SetActive(false);
        safariIcon.SetActive(true);
        safariPanel.SetActive(false);



        //PlayerItem m_playerItem = PhotonManager.Instance.findPlayerItemByID((int)ScreenShareWhileVideoCall.Instance.Uid2);
        //GameObject player = GameObject.Find(m_playerItem.Nickname.text + "(user)");
        //PlayerItem playerScript = player.GetComponent<PlayerItem>();
        //playerScript.webviewStart = false;
        //player.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
        //Debug.LogError(player);


    }

    public void safariBTN()
    {
        if (shutDown == true)
        {
            shutDown = false;
            safariIcon.SetActive(false);
            webViewObject.gameObject.SetActive(true);
            safariPanel.SetActive(true);


            //PlayerItem m_playerItem = PhotonManager.Instance.findPlayerItemByID((int)ScreenShareWhileVideoCall.Instance.Uid2);
            //GameObject player = GameObject.Find(m_playerItem.Nickname.text + "(user)");
            //PlayerItem playerScript = player.GetComponent<PlayerItem>();
            //playerScript.webviewStart = true;
            //player.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
            //Debug.LogError(player);

            
        }
    }

    public void ExtendSize()
    {
        if (extend == false)
        {
            extend = true;
            surfSaveList = WebViewObject.Instance.surfingList;
            saveURLCount = WebViewObject.Instance.currentURL;
            Destroy(webViewObject);
            Destroy(webViewObject.gameObject);


            webViewObject =
            (new GameObject("WebViewObject")).AddComponent<WebViewObject>();


            webViewObject.Init(1920, 1000);


            if (saveURLCount - 1 >= 0)
            {
                webViewObject.LoadURL(surfSaveList[saveURLCount - 1]);
            }
            else
            {
                webViewObject.LoadURL(surfSaveList[saveURLCount - 1]);
            }
            RectTransform safariPanelRect = safariPanel.GetComponent<RectTransform>();
            webViewObject.SetVisibility(true);


            webViewObject.rect = new Rect(0, 0, 1920, 1000);
            //safariPanelRect.sizeDelta = new Vector2(1920, 1080);


            safariPanel.GetComponent<Animation>().Play("SafariSizeUp");
            //sharescreen area
            //ScreenShareWhileVideoCall.Instance._rect = new Rect(0, 0, 1920, 1000);



            //GameObject playerID = GameObject.Find(UI_StartPanel.Instance.userName + "(user)");

            //PlayerItem playerScript = playerID.GetComponent<PlayerItem>();
            //Transform playerCanvas = playerID.transform.GetChild(1).GetChild(0);
            //playerCanvas.gameObject.SetActive(true);
            //RawImage playerWebImage = playerCanvas.GetComponent<RawImage>();
            //StartCoroutine(BringWebTexture(playerWebImage));

        }
        else
        {
            extend = false;
            surfSaveList = WebViewObject.Instance.surfingList;
            saveURLCount = WebViewObject.Instance.currentURL;
            Destroy(webViewObject);
            Destroy(webViewObject.gameObject);


            webViewObject =
            (new GameObject("WebViewObject")).AddComponent<WebViewObject>();

            webViewObject.Init(1148, 720);
            webViewObject.LoadURL(surfSaveList[saveURLCount - 1]);
            webViewObject.SetVisibility(true);
            webViewObject.rect = new Rect(0, 0, 1148, 700);
            RectTransform safariPanelRect = safariPanel.GetComponent<RectTransform>();
            safariPanelRect.sizeDelta = new Vector2(1148, 780);
            //safariPanel.GetComponent<Animation>().Play("SafariSizeDown");
            //ScreenShareWhileVideoCall.Instance._rect = new Rect(0, 0, 1148, 700);

            //GameObject playerID = GameObject.Find(UI_StartPanel.Instance.userName + "(user)");

            //PlayerItem playerScript = playerID.GetComponent<PlayerItem>();
            //Transform playerCanvas = playerID.transform.GetChild(1).GetChild(0);
            //playerCanvas.gameObject.SetActive(true);
            //RawImage playerWebImage = playerCanvas.GetComponent<RawImage>();
            //StartCoroutine(BringWebTexture(playerWebImage));
        }

    }

    IEnumerator BringWebTexture(RawImage webImageTexture)
    {
        yield return new WaitForSeconds(1f);
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        webImageTexture.texture = WebViewObject.Instance.texture;
#endif

    }


    public void StartWebView(string url)
    {
        string strUrl = url;

        webViewObject =
        (new GameObject("WebViewObject")).AddComponent<WebViewObject>();

        webViewObject.Init(1148, 720);
        //webViewObject.Init((msg) =>
        //{
        //    Debug.Log(string.Format("CallFromJS[{0}]", msg));
        //});

        webViewObject.LoadURL(strUrl);
        webViewObject.SetVisibility(true);
        

    }
#endif
}

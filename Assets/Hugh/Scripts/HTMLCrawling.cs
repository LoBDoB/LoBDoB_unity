using System.Collections;
using System.Collections.Generic;
using HtmlAgilityPack;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class HTMLCrawling : MonoBehaviour
{
    string url = "https://lily.sunmoon.ac.kr/Page2/Story/Notice.aspx";


    public GameObject notice;
    public GameObject noticePanel;
    public GameObject safari;
    public GameObject selectPanel;



    string html;

    int num = 0;

    int checkTr;

    public List<string> titleList;
    public List<string> authorList;
    public List<string> dateList;
    public List<string> urlList;



    void HacksaPanelClickFunction(int num)
    {
        string panelURL = $"https://lily.sunmoon.ac.kr/Page2/Story/Notice.aspx?ca=00{num}";
        Debug.LogError(panelURL);

        for (int i = 0; i < noticePanel.transform.childCount; i++)
        {
            Debug.LogError(noticePanel.transform.GetChild(i).gameObject);
            Destroy(noticePanel.transform.GetChild(i).gameObject);
        }
        //NGUITools.DestroyImmediate(child.gameObject);
        //transform.DetachChildren();

        Crawling(panelURL);


        
    }


    void Crawling(string URL)
    {
        HtmlWeb web = new HtmlWeb();
        HtmlDocument htmlDoc = web.Load(URL);
        Debug.Log(htmlDoc.Text);

        html = htmlDoc.Text;
        htmlDoc.LoadHtml(html);


        var htmlNodesTitle = htmlDoc.DocumentNode.SelectNodes("//body//div//td//a");
        //var htmlNodesLink = htmlDoc.DocumentNode.SelectNodes("//body//div//td////a[@href]");
        var htmlNodesAuthor = htmlDoc.DocumentNode.SelectNodes("//body//div//td");


        foreach (var i in htmlNodesTitle)
        {
            num += 1;
            titleList.Add(i.InnerHtml);

            //Debug.Log(i.OuterHtml.IndexOf(">"));

            string summaryUrl = i.OuterHtml.Remove(0, 9);
            summaryUrl = summaryUrl.Substring(0, i.OuterHtml.IndexOf(">") - 10);


            urlList.Add("https://lily.sunmoon.ac.kr" + summaryUrl);



        }

        foreach (var j in htmlNodesAuthor)
        {
            if (j.InnerHtml.Contains("</") == false && j.InnerHtml.Contains(".png") == false)
            {
                int result = 0;

                if (Int32.TryParse(j.InnerHtml, out result) == false)
                {
                    checkTr += 1;

                    if (checkTr == 1)
                    {
                        authorList.Add(j.InnerHtml);
                    }

                    if (checkTr == 2)
                    {
                        dateList.Add(j.InnerHtml);
                        checkTr = 0;
                    }
                }
            }
        }

        for (int i = 0; i < titleList.Count; i++)
        {
            GameObject summary = Instantiate(notice, noticePanel.transform);

            Text num = summary.transform.GetChild(0).GetComponent<Text>();
            Text title = summary.transform.GetChild(1).GetComponent<Text>();
            Text date = summary.transform.GetChild(2).GetComponent<Text>();
            Text author = summary.transform.GetChild(3).GetComponent<Text>();

            num.text = $"{i + 1}";
            title.text = titleList[i];
            date.text = dateList[i];
            author.text = authorList[i];
            title.gameObject.name = urlList[i];

            Button clickEvent = title.GetComponent<Button>();
            clickEvent.onClick.AddListener(() => SurfingUrl(title.gameObject.name));

            
        }
    }


    void OnEnable()
    {
        for (int i = 1; i < 4; i++)
        {
            int temp = i;
            Button button = selectPanel.transform.GetChild(i).GetComponent<Button>();
            //Debug.LogError(i);
            button.onClick.AddListener(()=>HacksaPanelClickFunction(temp));
        }

        Crawling(url);

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

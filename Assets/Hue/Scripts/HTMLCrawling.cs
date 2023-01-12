using System.Collections;
using System.Collections.Generic;
using HtmlAgilityPack;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HTMLCrawling : MonoBehaviour
{
    string url = "https://lily.sunmoon.ac.kr/Page2/Story/Notice.aspx";

    public GameObject notice;
    public GameObject noticePanel;

    string html;

    int num = 0;

    int checkTr;

    public List<string> titleList;
    public List<string> authorList;
    public List<string> dateList;

    void Start()
    {
        

        HtmlWeb web = new HtmlWeb();
        HtmlDocument htmlDoc = web.Load(url);
        Debug.Log(htmlDoc.Text);

        html = htmlDoc.Text;
        htmlDoc.LoadHtml(html);


        //var node = htmlDoc.DocumentNode.SelectSingleNode("//head/title");
        //Debug.Log(node.Name);
        //Debug.Log(node.OuterHtml);

        //var node3 = htmlDoc.DocumentNode.SelectNodes("//button");
        //Debug.Log(node3);

        //foreach (var o in node3)
        //{

        //    Debug.Log("button class " + o.Attributes["class"].Value);

        //}

        //var node2 = htmlDoc.DocumentNode.SelectNodes("//body//h1");

        //foreach (var i in node2)

        //{

        //    Debug.Log("1 " + i.InnerHtml);

        //}

        //foreach (var i in node2)

        //{

        //    Debug.Log("2" + i.OuterHtml);

        //}

        var htmlNodesTitle = htmlDoc.DocumentNode.SelectNodes("//body//div//td//a");
        var htmlNodesAuthor = htmlDoc.DocumentNode.SelectNodes("//body//div//td");

        foreach (var i in htmlNodesTitle)
        {
            num += 1;
            titleList.Add(i.InnerHtml);
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

            num.text = $"{i+1}";
            title.text = titleList[i];
            date.text = dateList[i];
            author.text = authorList[i];
        }


        


        //// span tag 중에 class 가 headline 인것
        //var spanHead = htmlDoc.DocumentNode.SelectSingleNode("//div/input");
        //var spanHead1 = htmlDoc.DocumentNode.SelectSingleNode("div/input");
        //Debug.Log(spanHead.Name);
        //Debug.Log(spanHead1.Name);





        //// 전체 html 에서 a tag 중 "some_class_name" class 를 포함하는 첫번째 element
        //var div = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class, 'some_class_name')]");

        //// 그 div 안에 table > tr > td > h3 > a 를 가져옴
        //var a = div.SelectSingleNode("table/tr/td/h3/a");

        //// a 의 href 속성을 가져옴. 두번째 파라메터는 속성이 없을 경우 기본값
        //Debug.Log(a.GetAttributeValue("href", ""));
        //// a 안에 있는 텍스트 가져오기
        //Debug.Log(a.InnerText);
        //// 여러개의 노드 조회
        //var prdtImgs = htmlDoc.DocumentNode.SelectNodes("//img");
        //foreach (var prdtImg in prdtImgs)
        //{
        //    Debug.Log(prdtImg.GetAttributeValue("src", ""));
        //}

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

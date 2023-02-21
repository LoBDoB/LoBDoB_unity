using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    public GameObject yellowArea, whiteArea, dateArea;
    public RectTransform contentRect;
    public Scrollbar scrollbar;
    AreaScript lastArea;



    public void Chat(bool isSend, string text, string user, Texture picture)
    {
        if (text.Trim() == "") return;

        bool isBottom = scrollbar.value <= 0.00001f;

        AreaScript area = Instantiate(isSend ? yellowArea : whiteArea).GetComponent<AreaScript>();
        area.transform.SetParent(contentRect.transform,false);
        area.boxRect.sizeDelta = new Vector2(800,area.boxRect.sizeDelta.y);
        area.textRect.GetComponent<Text>().text = text;
        Fit(area.boxRect);



        float x = area.textRect.sizeDelta.x + 42;
        float y = area.textRect.sizeDelta.y;

        if (y > 49)
        {
            for (int i = 0; i < 200; i++)
            {
                area.boxRect.sizeDelta = new Vector2(x - i * 2, area.boxRect.sizeDelta.y);
                Fit(area.boxRect);

                if (y != area.textRect.sizeDelta.y)
                {
                    area.boxRect.sizeDelta = new Vector2(x - (i * 2) + 2, y);
                    break;
                }
            }
        }
        else area.boxRect.sizeDelta = new Vector2(x,y);



        //현재 분까지 나오는 날짜와 유저이름 대입
        DateTime t = DateTime.Now;
        area.time = t.ToString("yyyy-MM-dd-HH-mm");
        area.user = user;

        int hour = t.Hour;
        if (t.Hour == 0) hour = 12;
        else if (t.Hour > 12) hour -= 12;
        area.timeText.text = (t.Hour > 12 ? "오후 " : "오전 ") + hour + ":" + t.Minute.ToString("D2");



        bool isSame = lastArea != null && lastArea.time == area.time && lastArea.user == area.user;
        if (isSame) lastArea.timeText.text = "";
        area.tail.SetActive(!isSame);


        if (!isSend)
        {
            area.userImage.gameObject.SetActive(!isSame);
            area.userText.gameObject.SetActive(!isSame);
            area.userText.text = area.user;
        }
        Fit(area.boxRect);
        Fit(area.areaRect);
        Fit(contentRect);
        lastArea = area;
    }


    void Fit(RectTransform rect) => LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
}

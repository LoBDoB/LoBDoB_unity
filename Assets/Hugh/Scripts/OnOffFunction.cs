using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class OnOffFunction : MonoBehaviour
{
    public Image[] checkBtn;

    public RawImage[] chartIMG;
    private void OnEnable()
    {
        checkBtn[0].gameObject.SetActive(true);
        chartIMG[0].gameObject.SetActive(true);
        for (int i = 0; i < checkBtn.Count(); i++)
        {
            int k = i;
            
            Button btn = checkBtn[k].gameObject.AddComponent<Button>();
            btn.onClick.AddListener(() => CheckBTN(k));
        }
        
    }

    Color onClickColor;
    Color offClickColor;
    public void CheckBTN(int num)
    {
        Debug.Log(num);
        
        for (int i = 0; i < checkBtn.Count(); i ++)
        {
            ColorUtility.TryParseHtmlString("#B5B5B5", out offClickColor);
            checkBtn[i].color = offClickColor;
            chartIMG[i].gameObject.SetActive(false);
           
        }
        chartIMG[num].gameObject.SetActive(true);
        
        ColorUtility.TryParseHtmlString("#C0A7D7", out onClickColor);
        checkBtn[num].color = onClickColor;

    }
}

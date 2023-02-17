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
        for (int i = 0; i < checkBtn.Count(); i++)
        {
            int k = i;
            
            Button btn = checkBtn[k].gameObject.AddComponent<Button>();
            btn.onClick.AddListener(() => CheckBTN(k));
        }
        
    }


    public void CheckBTN(int num)
    {
        Debug.Log(num);
        
        for (int i = 0; i < checkBtn.Count(); i ++)
        {
            checkBtn[i].color = Color.blue;
            chartIMG[i].gameObject.SetActive(false);
           
        }
        chartIMG[num].gameObject.SetActive(true);
        checkBtn[num].color = Color.grey;
        //chartIMG[num].gameObject.SetActive(false);



    }
}

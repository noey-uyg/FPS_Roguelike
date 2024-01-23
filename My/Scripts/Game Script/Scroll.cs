using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    public ScrollData scrollData;

    [SerializeField]
    private List<ScrollData> scrollDatas;
    [SerializeField]
    private List<ScrollData> clonedScrollDatas;


    private void OnEnable()
    {
        if (!GameManager.Instance.scrollisInitialized)
        {
            InitScrollDatas();
            GameManager.Instance.scrollisInitialized = true;
        }
        CloneScrollDatas();
        SelectScroll();
    }

    private void CloneScrollDatas()
    {
        clonedScrollDatas = new List<ScrollData>(scrollDatas);
    }

    private void InitScrollDatas()
    {
        for(int i=0;i<scrollDatas.Count; i++)
        {
            scrollDatas[i].haveScroll = false;
        }
    }

    public void DeleteHaveScroll()
    {
        for (int i = 0; i < clonedScrollDatas.Count; i++)
        {
            if (clonedScrollDatas[i].haveScroll)
            {
                clonedScrollDatas.RemoveAt(i);
                i--;
            }
        }
    }

    public void SelectScroll()
    {
        int ranNum = Random.Range(0, clonedScrollDatas.Count);
        InitScroll(clonedScrollDatas[ranNum]);
    }

    public void InitScroll(ScrollData scroll)
    {
        scrollData = scroll;
    }
}

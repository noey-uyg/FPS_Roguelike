using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class LevelUPUI : MonoBehaviour
{
    RectTransform rect;
    Awakening[] awakenings;
    private void Start()
    {
        rect = GetComponent<RectTransform>();
        awakenings = GetComponentsInChildren<Awakening>(true);
    }

    public void Show()
    {
        Next();
        rect.localScale = Vector3.one;
        GameManager.Instance.isOpenTab = true;
    }

    public void Hide()
    {
        rect.localScale = Vector3.zero;
        GameManager.Instance.isOpenTab = false;
    }

    public void Select(int index)
    {
        awakenings[index].OnClick();
    }

    //각성 랜덤 등장
    void Next()
    {
        foreach(Awakening awake in awakenings)
        {
            awake.gameObject.SetActive(false);
        }

        int[] rand = new int[3];

        while (true)
        {
            for(int i = 0; i < 3; i++)
            {
                rand[i] = Random.Range(0, awakenings.Length);
            }
            //awakenings[rand[0]].data.level!= awakenings[rand[0]].data.damage.Length &&
            if (rand[0] != rand[1] && rand[0] != rand[2] && rand[1] != rand[2])
            {
                break;
            }
        }

        for(int i = 0; i<rand.Length; i++)
        {
            Awakening ranItem = awakenings[rand[i]];
            ranItem.gameObject.SetActive(true);
        }
    }
}

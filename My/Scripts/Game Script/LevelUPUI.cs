using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUPUI : MonoBehaviour
{
    RectTransform rect;
    [SerializeField]
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
        if (GameManager.Instance.isGrabber)
        {
            GameManager.Instance.grabberCount--;
        }

        if(GameManager.Instance.grabberCount == 0)
        {
            rect.localScale = Vector3.zero;
            GameManager.Instance.isOpenTab = false;
        }
    }

    public void Select(int index)
    {
        awakenings[index].OnClick();
        foreach (Awakening awake in awakenings)
        {
            awake.gameObject.SetActive(false);
        }
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

            bool isMaxLevel = awakenings[rand[0]].data.level != awakenings[rand[0]].data.damage.Length &&
                awakenings[rand[1]].data.level != awakenings[rand[1]].data.damage.Length &&
                awakenings[rand[2]].data.level != awakenings[rand[2]].data.damage.Length;

            bool isDiff = rand[0] != rand[1] && rand[0] != rand[2] && rand[1] != rand[2];

            if (isMaxLevel && isDiff)
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

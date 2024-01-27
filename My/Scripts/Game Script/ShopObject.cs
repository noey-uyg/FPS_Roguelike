using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopObject : MonoBehaviour
{
    public int healCount;
    public int upgradeCount;
    public int awakeCount;
    public int scrollCount;

    private void Start()
    {
        Init();
    }

    private void OnEnable()
    {
        Init();
    }

    void Init()
    {
        healCount = 1;
        upgradeCount = 2;
        awakeCount = 1;
        scrollCount = 1;
    }
}

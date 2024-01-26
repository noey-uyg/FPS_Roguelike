using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAwakening : MonoBehaviour
{
    public AwakeningData[] datas;
    public AwakeningData data;

    private void OnEnable()
    {
        int ranNum = Random.Range(0, datas.Length);

        data= datas[ranNum];
    }
}

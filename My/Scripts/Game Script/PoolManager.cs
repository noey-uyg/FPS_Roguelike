using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;

    [SerializeField]
    private GameObject[] prefabs;

    private List<GameObject>[] objPools;


    private void Awake()
    {
        instance = this;
        InitObjPool();
    }

    private void InitObjPool()
    {
        objPools = new List<GameObject>[prefabs.Length];

        for (int i = 0; i < objPools.Length; i++)
        {
            objPools[i] = new List<GameObject>();
        }
    }

    public GameObject ActivateObj(int index)
    {
        GameObject obj = null;

        foreach(GameObject item in objPools[index])
        {
            if (!item.activeSelf)
            {
                obj = item;
                obj.SetActive(true);
                break;
            }
        }

        if(obj == null)
        {
            obj = Instantiate(prefabs[index], transform);
            objPools[index].Add(obj);
        }

        return obj;
    }

}

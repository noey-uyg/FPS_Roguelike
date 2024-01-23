using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Scroll scroll;

    //[SerializeField]
    //private GameObject scrollDesc;

    public void AddScroll(Scroll addScroll)
    {
        scroll = addScroll;
    }


    public void ClearSlot()
    {
        scroll = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewScroll", menuName = "New Scroll/Scroll")]
public class ScrollData : ScriptableObject
{
    [Header("# Main Info")]
    public string scrollName;
    public int scrollID;
    [TextArea]
    public string scrollDesc;
    public bool haveScroll = false;
    public bool dropScroll = false;

    [Header("# Level Data")]
    public float baseDamage;
}

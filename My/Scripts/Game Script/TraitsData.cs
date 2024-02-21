using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTraits", menuName = "New Traits/Traits")]
public class TraitsData : ScriptableObject
{
    public string traitsName;
    public Sprite traitsSprite;
    public int traitsID;

    [TextArea]
    public string traitsDesc;
    public bool isUnlocked;
    public int cost;
    public int level;
    public float[] damage;

    public TraitsData[] prevTraits;

}

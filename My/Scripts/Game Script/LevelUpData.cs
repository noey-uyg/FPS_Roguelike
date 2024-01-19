using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Awakening", menuName = "New Awakening/Awakening")]
public class LevelUpData : ScriptableObject
{
    public enum AwakeningType { Hand,Gun,Axe,Common}

    public string awakeningName;
    public AwakeningType awakeningType;
    [TextArea]
    public string awakeningDesc;
    public int awakeningID;
    public int level;
    public float[] damage;

}

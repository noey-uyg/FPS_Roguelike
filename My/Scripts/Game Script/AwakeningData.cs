using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Awakening", menuName = "New Awakening/Awakening")]
public class AwakeningData : ScriptableObject
{
    public enum AwakeningType { ÁÖ¸Ô,ÃÑ,µµ³¢,°ø¿ë}
    public enum WeaponType { HAND,GUN,AXE,NONE}

    public string awakeningName;
    public AwakeningType awakeningType;
    public WeaponType weaponType;
    [TextArea]
    public string awakeningDesc;
    public int awakeningID;
    public int level;
    public float[] damage;

}

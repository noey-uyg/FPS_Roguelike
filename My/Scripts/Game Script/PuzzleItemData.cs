using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPuzzle", menuName = "New Puzzle/Puzzle")]
public class PuzzleItemData : ScriptableObject
{
    public int puzzleID;
    public Sprite puzzleIcon;
    public string puzzleName;
    [TextArea]
    public string puzzleDesc;
    public Vector2 puzzleSize;
    public bool isHave = false;
    public bool isEquip = false;

    public float damage;
}

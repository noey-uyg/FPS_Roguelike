using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleView : MonoBehaviour
{
    public GameObject[] puzzleBtn;

    int x = 1;
    int y = 1;

    private void Start()
    {
        PuzzleViewClick();
    }

    public void OnClickX(int x)
    {
        this.x = x;
    }

    public void OnClickY(int y)
    {
        this .y = y;
    }

    public void PuzzleViewClick()
    {
        for (int i = 0; i < puzzleBtn.Length; i++)
        {
            int contX = (int)puzzleBtn[i].GetComponent<Collectable>().item.puzzleSize.x;
            int contY = (int)puzzleBtn[i].GetComponent<Collectable>().item.puzzleSize.y;

            if (contX == x && contY == y)
            {
                puzzleBtn[i].SetActive(true);
            }
            else
            {
                puzzleBtn[i].SetActive(false);
            }

        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour
{
    public GameObject[] songChoices;
    public GameObject newSelection, toMove;
    private int i = 0; //,iPrev;
    public Transform leftArea; //,rightArea;

    public void SelectionRight()
    {
        //iPrev = i;
        toMove = songChoices[i];
        i++;
        if (i > 2)
        {
            i = 0;
        }
        newSelection = songChoices[i];
        ChangePositions();
    }

      public void SelectionLeft()
    {
       // iPrev = i;
        toMove = songChoices[i];
        i--;
        if (i < 0)
        {
            i = 2;
        }
        newSelection = songChoices[i];
        ChangePositions();
    }

    public void ChangePositions()
    {
       newSelection.transform.position = new Vector3 (0, 0, 0);
       toMove.transform.position = leftArea.position;
     /*  if (iPrev < i)
        {
            toMove.transform.position = leftArea.position;
        }

       else if (iPrev < i)
        {
            toMove.transform.position = rightArea.position;
        } */
    }
}

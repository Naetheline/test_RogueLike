using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGUI : MonoBehaviour
{
    private Monster monster;
    private int indexOnScreen;
    private int targetIndex;

    public void SetIndex(int index)
    {
        indexOnScreen = index;
    }

    public void SelectTarget(int index)
    {
        targetIndex = index;
    }

    
    // Call by animator -> animation event.
    public void OnContact()
    {
        CombatManager.GetInstance().MonsterAttack(indexOnScreen, targetIndex);
        targetIndex = -1;
    }

    public void WhoIAm(Monster m)
    {
        monster = m;
    }
}

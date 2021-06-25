using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetGUI : MonoBehaviour
{
    private bool isBeingAttack;
    private bool hasDodged;
    public bool HasDodged
    {
        get { return hasDodged; }
    }

    public void TryToDodge()
    {
        hasDodged = isBeingAttack;
        isBeingAttack = false;
        
    }

    public void Attacked()
    {
        hasDodged = false;
        isBeingAttack = true;
    }
}

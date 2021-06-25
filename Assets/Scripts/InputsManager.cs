using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  This class manage the player inputs.
 * 
 * 
 */
public class InputsManager : MonoBehaviour
{
    private CombatManager combatManager;
    private CombatGUI combatGUI;

    // Start is called before the first frame update
    void Start()
    {
        combatManager = CombatManager.GetInstance();
        combatGUI = GameObject.Find("CombatGUI").GetComponent<CombatGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!combatManager.InCombat)
        {
            return;
        }
        // DEBUG
        if (Input.GetKeyDown(KeyCode.K))
        {
            combatManager.StopCombat();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            // Try to do things in here...
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            combatManager.SelectTarget(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            combatManager.SelectTarget(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            combatManager.SelectTarget(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            combatManager.SelectTarget(3);
        }
    }
}

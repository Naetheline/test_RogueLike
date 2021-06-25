using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/*
 * The CombatManager is the Controller for all the combat mechanics.
 * It is in charge of starting and ending combats, 
 * And coordinating the combat itself.
 * 
 * It links the Model classes the the CombatGUI View
 * And register the different callbacks for this particular View.
 * 
 */
public class CombatManager : MonoBehaviour
{
    private static  CombatManager instance;
    public static CombatManager GetInstance()
    {
        return instance;
    }
    [SerializeField]
    private GameObject currentRoomPanel;

    private int characterTarget;
    private Character character;

    Monster[] currentMonsters;
    Pet[] currentPets;
    Room currentRoom;

    CombatGUI combatGUI;

    private bool inCombat = false;
    public bool InCombat
    {
        get { return inCombat; }
    }

    public Action<List<Monster>, Character> startCombat;
    public delegate void MonstersDefeated();
    public MonstersDefeated monstersDefeated;


    // Use this for initialization
    void Start()
    {
        combatGUI = GameObject.Find("CombatGUI").GetComponent<CombatGUI>();
        this.RegisterStartCombat(combatGUI.OnStartCombat);
        this.RegisterMonsterDefeated(combatGUI.OnMonsterDefeated);


        currentMonsters = new Monster[Room.MAX_MONSTER];
        currentPets = new Pet[Character.MAX_PET];

        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(!inCombat)
        {
            return;
        }

        
        int monsterTarget = 0;
        for (int i = 0; i < currentMonsters.Length; i++)
        {
            if(currentMonsters[i].TimeBeforeNextAttack <= 0 && !currentMonsters[i].isDead())
            {
                // Inclusive Random. The currentPets.Length is the Character.
                
                monsterTarget = UnityEngine.Random.Range(0, currentPets.Length);
                
                combatGUI.MonsterTryToAttack(i, monsterTarget);
                combatGUI.isAttacked(monsterTarget);

                currentMonsters[i].TimeBeforeNextAttack = currentMonsters[i].TimeBetweenAttacks + UnityEngine.Random.Range(0f, 2f);

            }
            else
            {
                currentMonsters[i].TimeBeforeNextAttack -= Time.deltaTime;
            }
        }
      
    }

    public void MonsterAttack(int monsterIndex, int targetIndex)
    {
        if(targetIndex == currentPets.Length)
        {
            // TODO Attack character, needs (probably a charcterGUI...)
        }
        else
        {
            if( !combatGUI.hasDodge(targetIndex))
            {
                currentMonsters[monsterIndex].Attack(currentPets[targetIndex]);
            }
            else
            {
                Debug.Log("Attacked dodged ! Nice :)");
            }
        }
    }


    public void SelectTarget(int newTarget)
    {
        characterTarget = newTarget;
    }

    public void OnRoomLoaded(Room room)
    {
        currentRoom = room;
        if (currentRoom.Monsters.Count > 0)
        {
            int i = 0;
            Array.Resize<Monster>(ref currentMonsters, currentRoom.Monsters.Count);
            foreach (Monster monster in currentRoom.Monsters)
            {
                currentMonsters[i] = monster;
                currentMonsters[i].RegisterDeath(OnMonsterDeath);
                i++;
            }

            i = 0;
            Array.Resize<Pet>(ref currentPets, character.Pets.Length);
            foreach (Pet pet in character.Pets)
            {
                if (pet != null)
                {
                    currentPets[i] = pet;
                    currentPets[i].RegisterDeath(OnPetDeath);
                    i++;
                }
            }
            currentRoomPanel.SetActive(false);
            inCombat = true;
            if (startCombat != null)
            {            
                startCombat(currentRoom.Monsters, character);
            }
        }
        else
        {
            currentRoomPanel.SetActive(true);
        }
    }

    public void OnPetDeath(Pet deadPet)
    {
        int deadIndex = currentPets.Length;
        for (int i = 0; i < currentPets.Length; i++)
        {
            if (currentPets[i] == deadPet)
            {
                combatGUI.RemovePet(i);

                deadIndex = i;
            }
            if (deadIndex < currentPets.Length && (i + 1) < currentPets.Length)
            {
                currentPets[i] = currentPets[i + 1];
                currentPets[i + 1] = null;
            }
        }
        Array.Resize<Pet>(ref currentPets, currentPets.Length -1);
    }

    public void OnMonsterDeath(Monster deadMonster)
    {

        bool allDead = true;
        for (int i = 0; i < currentMonsters.Length; i++)
        {
            if(currentMonsters[i] == deadMonster)
            {
                combatGUI.RemoveMonster(i);
            }
            if (!currentMonsters[i].isDead())
                allDead = false;
        }

        if(allDead)
            StopCombat();
    }



    public void StopCombat()
    {
        inCombat = false;
        if (monstersDefeated != null)
        {
            monstersDefeated(); 
        }
        currentRoom.DeleteMonsters();
        currentRoomPanel.SetActive(true);
    }

    public void SetCharacter(Character newCharacter)
    {
        character = newCharacter;
    }


     // Callbacks registration
    public void RegisterMonsterDefeated(MonstersDefeated callback)
    {
        monstersDefeated += callback;
    }
    public void UnregisterMonsterDefeated(MonstersDefeated callback)
    {
        monstersDefeated -= callback;

    }
    public void RegisterStartCombat(Action<List<Monster>, Character> callback)
    {
        startCombat += callback;
    }
    public void UnregisterStartCombat(Action<List<Monster>, Character> callback)
    {
        startCombat -= callback;

    }
    public void UnregisterAll()
    {
        monstersDefeated = null;
        startCombat = null;
    }
}

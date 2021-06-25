using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


/*
 * This View displays the different actors of the combat scene
 *
 */
public class CombatGUI : MonoBehaviour {

    [SerializeField]
    private GameObject[] MonstersPrefabs;
    [SerializeField]
    private GameObject[] PetsPrefabs;
    [SerializeField]
    private Vector3[] positionsOnScreenMonster;
    [SerializeField]
    private Vector3[] positionsOnScreenPet;

    
    GameObject[] instantiatedMonsters;
    GameObject[] instantiatedPets;

	void Start () {

        instantiatedMonsters = new GameObject[Room.MAX_MONSTER];
        instantiatedPets = new GameObject[Character.MAX_PET];
    }


    public void OnStartCombat(List<Monster> monsters, Character character)
    {
        int i = 0;
        foreach (Monster monster in monsters)
        {
            if(i < Room.MAX_MONSTER)
            {             
                instantiatedMonsters[i] = Instantiate(MonstersPrefabs[monster.GetBreedAsInt()], positionsOnScreenMonster[i], Quaternion.identity);
                instantiatedMonsters[i].SendMessage("WhoIAm", monster);
                instantiatedMonsters[i].GetComponent<MonsterGUI>().SetIndex(i);
                i++;
            }
        }
        i = 0;
        Array.Resize<GameObject>(ref instantiatedPets, character.Pets.Length);
        foreach (Pet pet in character.Pets)
        {
            if (pet != null) 
            {
                instantiatedPets[i] = Instantiate(PetsPrefabs[pet.GetBreedAsInt()], positionsOnScreenPet[i], Quaternion.identity);
                i++;
            }
        }
    }

    public void MonsterTryToAttack(int monsterIndex, int targetIndex)
    {
        GameObject monster = instantiatedMonsters[monsterIndex];
        monster.GetComponent<MonsterGUI>().SelectTarget(targetIndex);
        monster.GetComponent<Animator>().SetTrigger("Attack");
        
    }

    public void isAttacked(int target)
    {
        if(target == instantiatedPets.Length)
        {
            // TODO Attack character
            Debug.Log("Attacking character");
        }
        else
        {
            instantiatedPets[target].GetComponent<PetGUI>().Attacked();
        }
    }

   
    public void OnMonsterDefeated()
    {
        for (int i = Room.MAX_MONSTER - 1; i >= 0; i--)
        {
            if(instantiatedMonsters[i] != null)
                Destroy(instantiatedMonsters[i]);
            if(i < instantiatedPets.Length && instantiatedPets[i] != null)
            {
                Destroy(instantiatedPets[i]);
            }
        }
    }

    public void RemovePet(int deadPet)
    {
        Destroy(instantiatedPets[deadPet]);
        Array.Resize<GameObject>(ref instantiatedPets, instantiatedPets.Length - 1);
    }

    public void RemoveMonster(int deadMonster)
    {
        Destroy(instantiatedMonsters[deadMonster]);
    }

    public bool hasDodge(int petIndex)
    {
        return instantiatedPets[petIndex].GetComponent<PetGUI>().HasDodged;
    }

}

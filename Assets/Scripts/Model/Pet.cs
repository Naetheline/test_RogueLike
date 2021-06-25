using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pet  {

    public enum PetType { OWL, CAT, DOG, BLUEBIRD, EAGLE, DRAGON, SPIRIT }

    private string name;
    private PetType breed;
    private float timeBetweenAttacks;
    private int attackPower;

    private int currentHP;
    private int HPMax;
    private int currentXP;
    private int XPMax;

    private int level;

    private bool dead;

    Action<int> HPChanged;
    Action<Pet> death;
 

    #region Propreties
    public int CurrentHP
    {
        get
        {
            return currentHP;
        }

        set
        {
            currentHP = value;
            if (HPChanged != null)
                HPChanged(currentHP);
            if (currentHP <= 0)
                Die();
        }
    }

    public int HPMax1
    {
        get
        {
            return HPMax;
        }

        set
        {
            HPMax = value;
        }
    }

    public int CurrentXP
    {
        get
        {
            return currentXP;
        }

        set
        {
            currentXP = value;
        }
    }

    public int XPMax1
    {
        get
        {
            return XPMax;
        }

        set
        {
            XPMax = value;
        }
    }

    public int Level
    {
        get
        {
            return level;
        }

        set
        {
            level = value;
        }
    }

    public float TimeBetweenAttacks
    {
        get
        {
            return timeBetweenAttacks;
        }

        set
        {
            timeBetweenAttacks = value;
        }
    }
    #endregion

    public Pet(PetType type, int aPower, float time = 2f)
    {
        breed = type;
        attackPower = aPower;
        TimeBetweenAttacks = time;
        
        level = 1;
        HPMax = currentHP = level * 100;
    }

    // Can only be name once
    public bool SetName(string newName)
    {
        if(name == "")
        {
            name = newName;
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetBreedAsInt()
    {
        return (int)breed;
    }


    public void  Attack(Monster target)
    {
        target.CurrentHP -= attackPower * 10;
    }

    public void Die()
    {
        if(death != null)
        {
            death(this);
        }
    }

    public void RegisterHPChanged(Action<int> callback)
    {
        HPChanged += callback;
    }

    public void RegisterDeath(Action<Pet> callback)
    {
        death += callback;
    }

    public void UnregisterAll()
    {
        HPChanged = null;
        death = null;
    }

}

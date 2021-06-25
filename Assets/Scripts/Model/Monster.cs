using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Monster  {

    public enum MonsterType {Terreux, Monster02 };

    private Monster.MonsterType breed;

    private int HPMax;
    private int currentHP;

    private int attackPower;
    private int level;

    private int xp;

    private float timeBetweenAttacks;
    private float timeBeforeNextAttack;

    private bool dead;

    Action<int> HPChanged;
    Action<Monster> death;

    #region Propreties
    public int AttackPower
    {
        get
        {
            return attackPower;
        }

        set
        {
            attackPower = value;
        }
    }

    public int Xp
    {
        get
        {
            return xp;
        }

        set
        {
            xp = value;
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
            {
                dead = true;
                if(death != null)
                    death(this);
            }
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

    public float TimeBeforeNextAttack
    {
        get
        {
            return timeBeforeNextAttack;
        }

        set
        {
            timeBeforeNextAttack = value;
        }
    }
    #endregion

    public Monster(MonsterType type, int level)
    {
        this.breed = type;
        this.level = level;

        // TODO
        // Generate random stats based on breed and level.

        // Placeholder
        CurrentHP = HPMax1 = level * 100; // TODO Remove magic numbers
        AttackPower = level;
        Xp = level * 5;
        timeBetweenAttacks = 4f;
        timeBeforeNextAttack = timeBetweenAttacks;
        dead = false;
    }

    public int GetBreedAsInt()
    {
        return (int)breed;
    }

    public void Attack(Character target)
    {
        target.ChangeHP(-attackPower * 10);
    }
    public void Attack(Pet target)
    {
        target.CurrentHP -= attackPower * 10;
    }

    public bool isDead()
    {
        return dead;
    }



    public void RegisterHPChanged(Action<int> callback)
    {
        HPChanged += callback;
    }

    public void RegisterDeath(Action<Monster> callback)
    {
        death += callback;
    }

    public void UnregisterAll()
    {
        HPChanged = null;
        death = null;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room {

    public static readonly int MAX_MONSTER = 5;
    public static readonly int MAX_LOOT = 3;

    private bool hasBeenVisited;
    public bool HasBeenVisited
    {
        get
        {
            return hasBeenVisited;
        }
        set
        {
            hasBeenVisited = value;
        }
    }

    private int x;
    public int X
    {
        get
        {
            return x;
        }
    }
    private int y;
    public int Y
    {
        get
        {
            return y;
        }
    }

    private bool north;
    public bool North
    {
        get
        {
            return north;
        }
    }
    private bool east;
    public bool East
    {
        get
        {
            return east;
        }
    }
    private bool south;
    public bool South
    {
        get
        {
            return south;
        }
    }
    private bool west;
    public bool West
    {
        get
        {
            return west;
        }
    }

    private List<Monster> monsters;
    public List<Monster> Monsters
    {
        get { return monsters; }
    }
    private Dictionary<Item.ItemType, int> loot;
    public Dictionary<Item.ItemType, int> Loot
    {
        get { return loot; }
    }

    public Room(int x, int y, bool n, bool e, bool s, bool w)
    {
        this.x = x;
        this.y = y;
        this.north = n;
        this.east = e;
        this.south = s;
        this.west = w;

        hasBeenVisited = false;

        monsters = new List<Monster>(MAX_MONSTER);
        loot = new Dictionary<Item.ItemType, int>();
    }

    public void AddMonster(Monster m)
    {
        if (monsters.Count < MAX_MONSTER)
        {
            monsters.Add(m);
        }
    }

    public void AddLoot(Item.ItemType t, int qty)
    {
        if (loot.Count < MAX_LOOT)
        {
            loot.Add(t, qty);
        }
    }

    public void RemoveLoot(Item.ItemType type, int qty)
    {
        if(!loot.ContainsKey(type))
        {
            return;
        }
        if (loot[type] - qty <= 0)
        {
            loot.Remove(type);
        }
        else
        {
            loot[type] -= qty;
        }
    }

    public void DeleteMonsters()
    {
        monsters.Clear();
    }
}


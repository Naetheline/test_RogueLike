using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Character  {

    public static readonly int MAX_PET = 3;
    public static readonly int MAX_ITEM = 5;

    private int HPMax;
    private int currentHP;

    private int XPMax;
    private int currentXP;

    private int level;


    private Dictionary<Item.ItemType, int> inventory;

    private Pet[] pets;
    public Pet[] Pets
    {
        get { return pets; }
    }

    private Action<int, int> levelup;
    private Action<Item.ItemType, int> itemChanged;
    private Action<int> HPChanged;
    private Action<int> XPChanged;
    private Action<Dictionary<Item.ItemType, int>> inventoryChanged;
    

    

    public Character()
    {
        inventory = new Dictionary<Item.ItemType, int>();
        
        pets = new Pet[0];
        AddPet(new Pet(Pet.PetType.OWL, 10));
        Reset();
    }

    public void AddPet(Pet newPet)
    {
        Array.Resize<Pet>(ref pets, pets.Length + 1);
        pets[pets.Length - 1] = newPet;
        pets[pets.Length - 1].RegisterDeath(OnPetDeath);
    }

    public void OnPetDeath(Pet deadPet)
    {
        int deadIndex = MAX_PET;
        for (int i = 0; i < pets.Length; i++)
        {
            if(pets[i] == deadPet)
            {
                pets[i] = null;
                deadIndex = i;
            }
            if(deadIndex < MAX_PET && (i + 1) < MAX_PET)
            {
                pets[i] = pets[i + 1];
                pets[i + 1] = null;
            }
            
        }
        Array.Resize<Pet>(ref pets, pets.Length - 1);

    }


    public int PickUpItem(Item.ItemType type, int qty)
    {
        if(type == Item.ItemType.COIN)
        {
            if (inventory.ContainsKey(type))
            {
                inventory[type] += qty;
            }
            else
            {
                inventory.Add(type, qty);
            }
        }
        else
        {
            if (inventory.ContainsKey(type))
            {
                if (inventory[type] >= MAX_ITEM)
                {
                    return 0;
                }
                if(inventory[type] + qty > MAX_ITEM)
                {
                    qty = MAX_ITEM - inventory[type];
                }
                inventory[type] += qty;
            }
            else
            {
                inventory.Add(type, qty);
            }
        }
        if (itemChanged != null)
        {
            itemChanged(type, inventory[type]);
        }
        return qty; 
    }

    public void UseItem(Item.ItemType type, int qty)
    {
        if(inventory.ContainsKey(type) && inventory[type] > 0)
        {
            switch (type)
            {
                case Item.ItemType.HP:
                    {
                        if (ChangeHP(30))
                        {
                            inventory[type]--;
                        }
                        break;
                    }// TODO get rid of magic numbers
                case Item.ItemType.APPLE:
                    {
                        // TODO
                        // Heal pet (Heal automagically the most dammaged pet ?)
                        // Find a way to define a target.
                        break;
                    }
                default: break;
            }
            
            if (itemChanged != null)
            {
                itemChanged(type, inventory[type]);
            }
        }
    }

    public bool ChangeHP(int qty)
    {
        if(currentHP == HPMax && qty > 0)
        {
            return false;
        }
        currentHP += qty;
        if (HPChanged != null)
        {
            
            HPChanged(currentHP);
        }
        if (currentHP < 0)
        {
            currentHP = 0;
            Die();
        }
        return true;
    }
    public void ChangeXP(int qty)
    {
        currentXP += qty;
        if(currentXP >= XPMax)
        {
            currentXP -= XPMax;
            LevelUP();
        }

        if (XPChanged != null)
        {
            XPChanged(currentXP);
        }
    }

    public void Die()
    {
        Debug.Log("We died..");
        Reset();
    }

    public void LevelUP()
    {
        level++;
        HPMax += 10; // TODO get rid of magic numbers !
        XPMax *= 2; // TODO an xp curve
        if(levelup != null)
        {
            levelup(HPMax, XPMax);                                          
        }
    }

    public void Reset()
    {
        inventory.Clear();
        inventory.Add(Item.ItemType.APPLE, 2);
        inventory.Add(Item.ItemType.MANA, 0);
        inventory.Add(Item.ItemType.COIN, 0);
        HPMax = 50;
        currentHP = 50;
        XPMax = 50;
        currentXP = 0;
        level = 1;
        
        if (levelup != null)
        {
            levelup(HPMax, XPMax);
        }
        if (XPChanged != null)
        {
            XPChanged(currentXP);
        }
        if (HPChanged != null)
        {
            HPChanged(currentHP);
        }
        if (inventoryChanged != null)
        {
            inventoryChanged(inventory);
        }

    }

    // CALLBACKS REGISTRATION AND UNREGISTRATION

    public void RegisterInventoryChanged(Action<Dictionary<Item.ItemType, int>> callback)
    {
        inventoryChanged += callback;
    }
    public void UnregisterInventoryChanged(Action<Dictionary<Item.ItemType, int>> callback)
    {
        inventoryChanged -= callback;
    }
    public void RegisterLevelUp(Action<int, int> callback)
    {
        levelup += callback;
    }
    public void UnregisterLevelUp(Action<int, int> callback)
    {
        levelup -= callback;
    }
    public void RegisterItemChanged(Action<Item.ItemType, int> callback)
    {
        itemChanged += callback;
    }
    public void UnregisterItemChanged(Action<Item.ItemType, int> callback)
    {
        itemChanged -= callback;
    }
    public void RegisterHPChanged(Action<int> callback)
    {
        HPChanged += callback;
    }
    public void UnregisterHPChanged(Action<int> callback)
    {
        HPChanged -= callback;
    }
    public void RegisterXPChanged(Action<int> callback)
    {
        XPChanged += callback;
    }
    public void UnregisterXPChanged(Action<int> callback)
    {
        XPChanged -= callback;
    }

    public void UnregisterAll()
    {
        levelup = null;
        itemChanged = null;
        HPChanged = null;
        XPChanged = null;
        inventoryChanged = null;
    }
}

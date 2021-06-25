using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This game use the Model View Controller pattern. The View classes 
 * Are Monobehaviour on gameObjects. The Model classes are unaware of
 * The gameobject.
 * 
 * This class and the CombatManager.cs are the two controller. GameManager is the main one.
 * It is in charge of creating the model of the game by calling new Character and new Level,
 * Then it register the callback of the view to the respective model classes.
 * 
 * This class also gives the character to the secondary controller : CombatManager.cs
 */
public class GameManager : MonoBehaviour {

    private Level level;
    private Character mainCharacter;
    private CombatManager combatManager;

	void Start () {

        // Manage Save files here...
        NewGame();
	}

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGame();
        }
    }

    public void NewGame()
    {
        // Creation
        level = new Level();
        mainCharacter = new Character();

        // Attaching the views to their respective models
        RoomGUI roomGUI = GameObject.Find("currentRoom").GetComponent<RoomGUI>();
        roomGUI.SetLevel(level);
        level.RegisterNewRoomCallback(roomGUI.OnRoomLoaded);

        // Set both callbacks (room and level) for minimap.
        MinimapGUI minimapGUI = GameObject.Find("minimap").GetComponent<MinimapGUI>();
        level.RegisterNewRoomCallback(minimapGUI.OnRoomLoaded);
        level.RegisterNewLevelCallback(minimapGUI.OnLevelLoaded);

        LootGUI lootGUI = GameObject.Find("currentRoom").GetComponent<LootGUI>();
        lootGUI.SetCharacter(mainCharacter);
        level.RegisterNewRoomCallback(lootGUI.OnRoomLoaded);
        
        ItemsGUI itemsGUI = GameObject.Find("itemsPanel").GetComponent<ItemsGUI>();
        itemsGUI.SetCharacter(mainCharacter);
        mainCharacter.RegisterHPChanged(itemsGUI.OnHPchanged);
        mainCharacter.RegisterXPChanged(itemsGUI.OnXPchanged);
        mainCharacter.RegisterLevelUp(itemsGUI.Onlevelup);
        mainCharacter.RegisterItemChanged(itemsGUI.OnItemChanged);
        mainCharacter.RegisterInventoryChanged(itemsGUI.OnInventoryChanged);

        // CombatManger
        combatManager = GetComponent<CombatManager>();
        combatManager.SetCharacter(mainCharacter);
        level.RegisterNewRoomCallback(combatManager.OnRoomLoaded);
        combatManager.RegisterMonsterDefeated(lootGUI.OnMonstersDefeated);

        // Generating new level and new character
        level.GenerateNewLevel();
        mainCharacter.Reset();
    }

    public void ExitGame()
    {
        Debug.Log("Quitting...");
        mainCharacter.UnregisterAll();
        level.UnregisterAll();
        combatManager.UnregisterAll();
        Application.Quit();
    }

}

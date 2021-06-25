using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Level  {

    private readonly int WIDTH = 8;
    private readonly int HEIGHT = 8;

    private Room[,] map;
    private Room startRoom;
    private Room exitRoom;
    private Room currentRoom;

    private int lastX;
    private int lastY;

    private int stage = 0;

    private Action<Room> newRoomLoaded;
    private Action<Room[,]> newLevelLoaded;

    public void goNorth()
    {
        if(currentRoom.North)
        {
            LoadRoom(currentRoom.X, currentRoom.Y + 1);
        }
    }
     public void goEast()
    {
        if (currentRoom.East)
        {
            LoadRoom(currentRoom.X + 1, currentRoom.Y);
        }
    }
    public void goSouth()
    {
        if (currentRoom.South)
        {
            LoadRoom(currentRoom.X, currentRoom.Y - 1);
        }
    }
    public void goWest()
    {
        if (currentRoom.West)
        {
            LoadRoom(currentRoom.X - 1, currentRoom.Y);
        }
    }

    public void exitLevel()
    {
        if(currentRoom == exitRoom)
        {
            GenerateNewLevel();
          
        }
    }

    public void GenerateNewLevel()
    {
        stage++;
        GenerateMap();
        LoadRoom(startRoom.X, startRoom.Y);
       
    }

    public void LoadRoom(int x, int y)
    {
        // TODO sanity checks on x and y
        if (map[x,y] != null)
        {
            currentRoom = map[x, y];
            currentRoom.HasBeenVisited = true;
            if (newRoomLoaded != null)
            {
                newRoomLoaded(currentRoom);
            }
        }
    }

    public void GenerateMap()
    {
        map = new Room[WIDTH,HEIGHT];

        // SpawnRoom is called recursivly
        SpawnRoom((WIDTH - 1) / 2, (HEIGHT- 1) / 2, true, true, true, true);

        startRoom = map[(WIDTH - 1) / 2, (HEIGHT - 1) / 2];

        exitRoom = map[lastX, lastY];

        // Adding Monsters
        foreach (Room room in map)
        {
            if(room != null && room != startRoom)
            {  
                room.AddMonster(new Monster(Monster.MonsterType.Terreux, stage));
                room.AddMonster(new Monster(Monster.MonsterType.Terreux, stage));
                room.AddMonster(new Monster(Monster.MonsterType.Terreux, stage));
                room.AddMonster(new Monster(Monster.MonsterType.Monster02, stage));
            }
        }

        // Adding items


        if (newLevelLoaded != null)
        {
            newLevelLoaded(map);
        }
    }

    

    private void SpawnRoom(int x, int y, bool n, bool e, bool s, bool w)
    {
        Room toSpawn = map[x,y];
        if(toSpawn == null)
        {
            // Generating paths.
            n = n ? n : (UnityEngine.Random.value > 0.5);
            e = e ? e : (UnityEngine.Random.value > 0.5);
            s = s ? s : (UnityEngine.Random.value > 0.5);
            w = w ? w : (UnityEngine.Random.value > 0.5);

            // Checking boundaries and if a wall or door is already there
            if (x == 0 || (map[x - 1, y] != null) && !map[x - 1, y].East) w = false;
            else if (map[x - 1, y] != null && map[x - 1, y].East) w = true;
            if (y == 0 || (map[x, y - 1] != null) && !map[x, y - 1].North) s = false;
            else if (map[x, y - 1] != null && !map[x, y - 1].North) s = true;
            if (x == WIDTH - 1 || (map[x + 1, y] != null) && !map[x + 1, y].West) e = false;
            else if (map[x + 1, y] != null && !map[x + 1, y].West) e = true;
            if (y == HEIGHT - 1 || (map[x, y + 1] != null) && !map[x, y + 1].South) n = false;
            else if (map[x, y + 1] != null && !map[x, y + 1].South) n = true;

            toSpawn = new Room(x, y, n, e, s, w);
            map[x, y] = toSpawn;
            lastX = x;
            lastY = y;

            if (toSpawn.North && map[x, y + 1] == null)
            {
                 SpawnRoom(x, y + 1, false, false, true, false);
            }
            if (toSpawn.East && map[x + 1, y] == null)
            {
                SpawnRoom(x + 1, y, false, false, false, true);
            }
            if (toSpawn.South && map[x, y - 1] == null)
            {
                 SpawnRoom(x, y - 1, true, false, false, false);
            }
            if (toSpawn.West && map[x - 1, y] == null)
            {
                SpawnRoom(x - 1, y, false, true, false, false);
            }
        }
    }

    public bool isExitRoom(Room room)
    {
        return room == exitRoom;
    }

    // CALLBACKS REGISTRATION AND UNREGISTRATION

    public void RegisterNewRoomCallback(Action<Room> callback)
    {
        newRoomLoaded += callback;
    }
    public void UnregisterNewRoomCallback(Action<Room> callback)
    {
        newRoomLoaded -= callback;
    }

    public void RegisterNewLevelCallback(Action<Room[,]> callback)
    {
        newLevelLoaded += callback;
    }
    public void UnregisterNewLevelCallback(Action<Room[,]> callback)
    {
        newLevelLoaded -= callback;
    }

    public void UnregisterAll()
    {
        newRoomLoaded = null;
        newLevelLoaded = null;
    }

}

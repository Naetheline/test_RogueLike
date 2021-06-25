using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapGUI : MonoBehaviour {

    private readonly int CELLSIZE = 100;

   private Room[,] currentMap;
   private Room currentRoom;

    [SerializeField]
    private RectTransform mapUI;

    [SerializeField]
    private ScrollRect scroller;

    [SerializeField]
    private GameObject roomPrefab;

    private Dictionary<string, Sprite> roomsSprites;

    private bool mapCreated;

    private GameObject[,] rooms;

    private UnityEngine.UI.Image previousRoom;

    // Use this for initialization
    void Awake () {
        roomsSprites = new Dictionary<string, Sprite>();
        LoadSprites();
        mapCreated = false;
        
    }

    private void LoadSprites()
    {
        Sprite[] sprites;
        sprites = Resources.LoadAll<Sprite>("Rooms/");
        foreach (Sprite s in sprites)
        {
            roomsSprites.Add(s.name, s);
        }
    }


    public void OnRoomLoaded(Room room)
    {
        currentRoom = room;
        if(currentRoom.HasBeenVisited)
        {
            rooms[currentRoom.Y, currentRoom.X].GetComponent<UnityEngine.UI.Image>().sprite
                = roomsSprites[GetSpriteName(currentRoom.X, currentRoom.Y)];
        }

        if(previousRoom != null)
        {
            previousRoom.color = Color.white;
        }
        previousRoom = rooms[currentRoom.Y, currentRoom.X].GetComponent<UnityEngine.UI.Image>();
        previousRoom.color = Color.yellow;

        // Keep the current room in the visual range.
        scroller.horizontalNormalizedPosition = Mathf.InverseLerp(0, rooms.GetLength(1) - 1, currentRoom.X);
        scroller.verticalNormalizedPosition = Mathf.InverseLerp(0, rooms.GetLength(0) - 1, currentRoom.Y);
    }

    public void OnLevelLoaded(Room[,] map)
    {
        currentMap = map;

        if (!mapCreated) CreateMinimap();
        else ResetMap();
       
    }

    private void CreateMinimap()
    {
        
        mapUI.sizeDelta = new Vector2(currentMap.GetLength(0) * CELLSIZE, currentMap.GetLength(1) * CELLSIZE);
        rooms = new GameObject[currentMap.GetLength(0), currentMap.GetLength(1)];

        for (int i = 0; i < currentMap.GetLength(0); i++)
        {
            for (int j = 0; j < currentMap.GetLength(1); j++)
            {
                GameObject roomUI = Instantiate(roomPrefab, mapUI.transform);
                roomUI.GetComponent<UnityEngine.UI.Image>().sprite = roomsSprites["TRANSPARENCY"];
                rooms[i, j] = roomUI;
            }
        }
        mapCreated = true;
    }

    private void ResetMap()
    {
        mapUI.sizeDelta = new Vector2(currentMap.GetLength(0) * CELLSIZE, currentMap.GetLength(1) * CELLSIZE);

        for (int i = 0; i < currentMap.GetLength(0); i++)
        {
            for (int j = 0; j < currentMap.GetLength(1); j++)
            {
                rooms[i, j].GetComponent<UnityEngine.UI.Image>().sprite = roomsSprites["TRANSPARENCY"];
                
            }
        }
    }

    private string GetSpriteName(int x, int y)
    {
        string spriteName = "room_";
        if(currentMap[x,y] == null)
        {
            spriteName += "empty";
            return spriteName;
        }

        spriteName += (currentMap[x, y].North) ? "N" : "";
        spriteName += (currentMap[x, y].East) ? "E" : "";
        spriteName += (currentMap[x, y].South) ? "S" : "";
        spriteName += (currentMap[x, y].West) ? "W" : "";

        

        return spriteName;
    }

    private void ClearMinimap()
    {
        for (int i = mapUI.childCount - 1; i >= 0 ; i--)
        {
            Destroy(mapUI.GetChild(i));
        }
    }
}

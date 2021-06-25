using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomGUI : MonoBehaviour {

    [SerializeField]
    private Button north;
    [SerializeField]
    private Button east;
    [SerializeField]
    private Button south;
    [SerializeField]
    private Button west;
    [SerializeField]
    private Button nextLevel;

    private Level level;
    private Room currentRoom;


    public void SetLevel(Level l)
    {
        level = l;
    }

    public void OnRoomLoaded(Room room)
    {
        currentRoom = room;

        north.gameObject.SetActive(currentRoom.North);
        east.gameObject.SetActive(currentRoom.East);
        south.gameObject.SetActive(currentRoom.South);
        west.gameObject.SetActive(currentRoom.West);

       nextLevel.gameObject.SetActive(level.isExitRoom(currentRoom));
        
    }

    public void OnClickNorth()
    {
        if (level == null)
        {
            Debug.LogError("In RoomGUI : level is null !");
            return;
        }
        level.goNorth();
    }
    public void OnClickEast()
    {
        if (level == null)
        {
            Debug.LogError("In RoomGUI : level is null !");
            return;
        }
        level.goEast();
    }
    public void OnClickSouth()
    {
        if (level == null)
        {
            Debug.LogError("In RoomGUI : level is null !");
            return;
        }
        level.goSouth();
    }
    public void OnClickWest()
    {
        if (level == null)
        {
            Debug.LogError("In RoomGUI : level is null !");
            return;
        }
        level.goWest();
    }
    public void OnClickNextLevel()
    {
        if (level == null)
        {
            Debug.LogError("In RoomGUI : level is null !");
            return;
        }
        level.exitLevel();
    }
}

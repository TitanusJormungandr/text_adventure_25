using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{
    public static NavigationManager instance;
    public Room startingRoom;
    public Room currentRoom;
    public List<Room> rooms;

    public Exit toKeyNorth;

    //new delegate
    public delegate void GameOver();
    public event GameOver onGameOver;

    private Dictionary<string, Room> exitRooms = new Dictionary<string, Room>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        InputManager.instance.OnRestart += ResetGame;
        //Debug.Log(startingRoom.description);
        //toKeyNorth.isHidden = true;
        //currentRoom = startingRoom;
        //Unpack();
        //ResetGame();
    }
    public void ResetGame()
    {
        toKeyNorth.isHidden = true;
        currentRoom = startingRoom;
        Unpack();
    }

    void Unpack()
    {
        string description = currentRoom.description;
        exitRooms.Clear();
        foreach(Exit e in currentRoom.exits)
        {
            if (!e.isHidden)
            {
                description += " " + e.description;
                exitRooms.Add(e.direction.ToString(), e.room);
            }
        }

        InputManager.instance.UpdateStory(description);
        if (exitRooms.Count == 0)
            if (onGameOver != null) // is anyone listening
                onGameOver(); //invokes the event
        
    }

    public void SwitchRooms(Room room)
    {
        currentRoom = room;
        Unpack();
    }

    public bool SwitchRooms(string direction)
    {
        if(exitRooms.ContainsKey(direction)) //if that exit exists
        {
            if(!getExit(direction).isLocked || GameManager.instance.inventory.Contains("key"))
            {
                currentRoom = exitRooms[direction];
                InputManager.instance.UpdateStory("You go " + direction);
                Unpack();
                return true;
            }
        }
        return false;
    }

    Exit getExit(string direction)
    {
        foreach (Exit e in currentRoom.exits)
        {
            if (e.direction.ToString() == direction)
                return e;
        }
        return null;
    }

    public bool TakeItem(string item)
    {
        if (item == "key" && currentRoom.hasKey)
            return true;
        else if (item == "orb" && currentRoom.hasOrb)
        {
            toKeyNorth.isHidden = false;
            return true;
        }
        else
            return false;
    }

    public Room GetRoomFromName(string name)
    {
        foreach(Room aRoom in rooms)
        {
            if(aRoom.name == name) 
                return aRoom;
        }

        return null;
    }

}

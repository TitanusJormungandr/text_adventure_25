using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationController : MonoBehaviour
{
    public static NavigationController instance;
    public Room startingRoom;
    public Room currentRoom;

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
        Debug.Log(startingRoom.description);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

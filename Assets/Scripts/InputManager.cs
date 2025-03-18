using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using System.IO;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public Text storyText; // the story 
    public InputField userInput; // the input field object
    public Text inputText; // part of the input field where user enters response
    public Text placeHolderText; // part of the input field for initial placeholder text

    public delegate void Restart();
    public event Restart OnRestart;

    private string story; // holds the story to display
    private List<string> commands = new List<string>(); //valid user commands

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
        commands.Add("go");
        commands.Add("get");
        commands.Add("restart");
        commands.Add("save");
        commands.Add("commands");
        commands.Add("inventory");

        userInput.onEndEdit.AddListener(GetInput);
        story = storyText.text;
        NavigationManager.instance.onGameOver += EndGame;
        LoadInventory(); // Load inventory when the game starts
    }

    void EndGame()
    {
        UpdateStory("\nPlease enter 'restart' to play again. ");
    }

    public void UpdateStory(string msg) //update display
    {
        story += "\n" + msg;
        storyText.text = story;
    }

    void GetInput(string msg) //process input
    {
        if (msg != "")
        {
            char[] splitInfo = { ' ' };
            string[] parts = msg.ToLower().Split(splitInfo);

            if (commands.Contains(parts[0])) //if valid command
            {
                if (parts[0] == "go")
                {
                    if (NavigationManager.instance.SwitchRooms(parts[1]))
                    {
                        //fill in later
                    }
                    else
                    {
                        UpdateStory("Exit does not exist or is locked. Try again.");
                    }
                }
                else if (parts[0] == "get")
                {
                    if (NavigationManager.instance.TakeItem(parts[1]))
                    {
                        GameManager.instance.inventory.Add(parts[1]);
                        SaveInventory(); // Save inventory after adding an item
                        UpdateStory("You added a(n) " + parts[1] + " to your inventory.");
                    }
                    else
                    {
                        UpdateStory("Sorry, " + parts[1] + " does not exist in this room.");
                    }
                }
                else if (parts[0] == "restart")
                {
                    OnRestart?.Invoke();
                }
                else if (parts[0] == "save")
                {
                    GameManager.instance.Save();
                    SaveInventory(); // Ensure inventory is saved
                }
                else if (parts[0] == "commands") // Handle "commands"
                {
                    UpdateStory("Available commands: " + string.Join(", ", commands));
                }
                else if (parts[0] == "inventory") // Handle "inventory" 
                {
                    List<string> inventory = GameManager.instance.inventory;
                    string inventoryList = inventory.Count > 0 ? string.Join(", ", inventory) : "Your inventory is empty.";
                    UpdateStory("Inventory: " + inventoryList);
                }
            }
        }
        userInput.text = ""; //after input from user, reset
        userInput.ActivateInputField();
    }

    void SaveInventory()
    {
        PlayerPrefs.SetString("inventory", string.Join(",", GameManager.instance.inventory));
        PlayerPrefs.Save();
    }

    void LoadInventory()
    {
        string savedInventory = PlayerPrefs.GetString("inventory", "");
        if (!string.IsNullOrEmpty(savedInventory))
        {
            GameManager.instance.inventory = new List<string>(savedInventory.Split(','));
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public Text storyText; // the story 
    public InputField userInput; // the input field object
    public Text inputText; // part of the input field where user enters response
    public Text placeHolderText; // part of the input field for initial placeholder text
    //public Button abutton;

    //1st step to creating and using delegates
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

        userInput.onEndEdit.AddListener(GetInput); //now calls GetInput
        //abutton.onClick.AddListener(DoSomething);
        story = storyText.text;
        NavigationManager.instance.onGameOver += EndGame; // function to call when event occurs
    }

    void EndGame()
    {
        UpdateStory("\nPlease enter 'restart' to play again. ");
    }

    //void DoSomething() //event handler
    //{
        //Debug.Log("Button clicked!");
    //}

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
            string[] parts = msg.ToLower().Split(splitInfo); //['go', 'north']

            if (commands.Contains(parts[0])) //if valid command
            {
                if (parts[0] == "go") //wants to switch rooms
                {
                    if (NavigationManager.instance.SwitchRooms(parts[1])) //returns true if direction exits
                    {
                        //fill in later
                    }
                    else
                    {
                        //added the "is locked" response
                        UpdateStory("Exit does not exist or is locked. Try again.");
                    }
                }
                else if (parts[0] == "get")
                {
                    if (NavigationManager.instance.TakeItem(parts[1]))
                    {
                        GameManager.instance.inventory.Add(parts[1]);
                        UpdateStory("You added a(n) " + parts[1] + " to your inventory.");
                    }
                    else
                    {
                        UpdateStory("Sorry, " + parts[1] + " does not exist in this room.");
                    }

                }
                else if (parts[0] == "restart")
                {
                    if (OnRestart != null) //if anyone is listening
                        OnRestart();
                }
                else if (parts[0] == "save")
                {
                    GameManager.instance.Save();
                }
                //UpdateStory(msg);
            }

        }
        userInput.text = ""; //after input from user, reset
        userInput.ActivateInputField();
    }
}

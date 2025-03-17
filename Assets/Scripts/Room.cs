using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Text/Room")]
public class Room : ScriptableObject
{
    public string roomName;
    [TextArea]
    public string description;
    public Exit[] exits;

    public bool hasKey;
    public bool hasOrb;
    public bool hasCrystal;
    public bool hasTopStaff;
    public bool hasMidStaff;
    public bool hasEndStaff;
}

using UnityEngine;

public class EncounterRoom : Room
{
    //You enter the encounter room, for the first time
    //An enemy appears!
    //You look around and see the body of a dead dice man
    //You leave the encounter room, why do you have a room like this


    public override void EnterRoom()
    {
        Debug.Log("You are in the encounter room of the house, wait what?");
    }
}
using UnityEngine;
using System.Collections.Generic;
using System;

public class BattleMaster : MonoBehaviour
{
    public void StartEncounter(List<int> PlayerInventory)
    {
        List<int> playerRolls = new List<int>();
        List<int> computerRolls = new List<int>();

        DieRoller dieRoller = new DieRoller();
        int playerTurns = 0;
        int computerTurns = 0;
        bool winner = false;

        Debug.Log("DICE BATTLE!");

        int playerDieOne = PlayerInventory.Count > 0 ? PlayerInventory[0] : dieRoller.d6;
        int playerDieTwo = PlayerInventory.Count > 1 ? PlayerInventory[1] : dieRoller.d6;

        int[] computerDice = new int[] { dieRoller.d4, dieRoller.d6, dieRoller.d8, dieRoller.d20 };

        while (!winner)
        { //player turn
            playerTurns++;
            int rollOne = dieRoller.RollDie(playerDieOne);
            int rollTwo = dieRoller.RollDie(playerDieTwo);

            Debug.Log($"Player rolls: {rollOne} and {rollTwo}");

            playerRolls.Add(rollOne);
            playerRolls.Add(rollTwo);

            if (rollOne == rollTwo)
            {
                Debug.Log("MATCH!");
                winner = true;
                Debug.Log($"Player wins in {playerTurns} turns!");
                break;
            }

            //computer turn
            computerTurns++;
            int compDieOne = computerDice[UnityEngine.Random.Range(0, computerDice.Length)];
            int compDieTwo = computerDice[UnityEngine.Random.Range(0, computerDice.Length)];

            int compRollOne = dieRoller.RollDie(compDieOne);
            int compRollTwo = dieRoller.RollDie(compDieTwo);

            Debug.Log($"Computer rolls: {compRollOne} and {compRollTwo}");

            computerRolls.Add(compRollOne);
            computerRolls.Add(compRollTwo);

            if (compRollOne == compRollTwo)
            {
                Debug.Log("MATCH!");
                winner = true;
                Debug.Log($"Enemy wins in {computerTurns} turns!");
                Debug.Log("Player loses the battle.");
                Application.Quit();
                break;
            }
        }
        Debug.Log("Battle Over!");
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NUnit.Framework;
using UnityEngine.AI;

public class CombatMaster : MonoBehaviour
{
    public static CombatMaster Instance;

    [Header("Combat UI References")]
    public GameObject combatPanel;
    public TMP_Text turnText;
    public TMP_Text infoText;
    public Button attackButton;
    public Button fleeButton;
    
    [Header("Combatants")]
    public int playerHealth = 20;
    public int enemyHealth = 20;
    private int playerHP;
    private int enemyHP;

    private int turnCounter = 0;
    private bool isPlayerTurn;

    [Header("Game Over Screen")]
    public GameObject GameOverPanel;


    private List<ItemData> playerInventory;
    private System.Random srng;
    public CombatItemSlot[] itemSlots;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        srng = new System.Random();

        attackButton.onClick.AddListener(OnAttackClicked);
        fleeButton.onClick.AddListener(OnFleeClick);

        combatPanel.SetActive(false);
    }

    public void StartEncounter()
    {
        MusicMaster.Instance.PlayCombatMusic();

        playerInventory = FindFirstObjectByType<GameMaster>().Inventory;
        playerHP = playerHealth;
        enemyHP = enemyHealth;
        turnCounter = 1;
        isPlayerTurn = true;
        
         attackButton.interactable = true;
        fleeButton.interactable = true;

        combatPanel.SetActive(true);
        UpdateUI();
        infoText.text = "An enemy appears!";
    }

    private void UpdateUI()
    {
        turnText.text = $"Turn {turnCounter} - {(isPlayerTurn ? "Player" : "Enemy")} Turn";
        infoText.text = $"Player HP: {playerHP} | Enemy HP: {enemyHP}";
    }

    private void OnAttackClicked()
{
    if (!isPlayerTurn) return;

    int die1 = srng.Next(1, 7);
    int die2 = srng.Next(1, 7);
    int totalDamage = die1 + die2;

    int healAmount = 0;

    foreach (var slot in itemSlots)
    {
        if (slot != null && slot.currentItem != null && slot.currentItem.itemData != null)
        {
            if (slot.currentItem.itemData.itemType == ItemData.ItemType.potion)
            {
                // Potion heals player for dmg value
                healAmount += slot.currentItem.itemData.dmg;
            }
            else
            {
                // Weapon/other items add to damage
                totalDamage += slot.currentItem.itemData.dmg;
            }

            // Clear slot after use
            slot?.ClearSlot();
        }
    }

    // Apply potion healing
    if (healAmount > 0)
    {
        playerHP += healAmount;
        infoText.text = $"Player used a potion and healed {healAmount} HP!\n";
    }

    // Apply damage to enemy
    enemyHP -= totalDamage;
    infoText.text += $"Player attacks! Dice: {die1}+{die2}, Total Damage: {totalDamage}\nEnemy HP: {enemyHP}";

    CheckCombatEnd();

    if (enemyHP > 0)
        NextTurn();
}

    private void OnFleeClick()
    {
        if (!isPlayerTurn) return;

        bool fleeSuccess = srng.NextDouble() < 0.5;
        if (fleeSuccess)
        {
            infoText.text = "Player successfully fled the encounter!";
            EndEncounter();
        }
        else
        {
            infoText.text = "Flee attempt failed!";
            NextTurn();
        }
    }

    private void NextTurn()
    {
        isPlayerTurn = !isPlayerTurn;
        turnCounter++;

        if (!isPlayerTurn)
        {
            EnemyTurn();
        }

        UpdateUI();
    }

    private void EnemyTurn()
    {
        int enemyRoll = srng.Next(1, 7) + srng.Next(1, 7);
        playerHP -= enemyRoll;
        infoText.text = $"Enemy attacks for {enemyRoll} damage!";

        if (playerHP > 0)
        {
            isPlayerTurn = true;
            UpdateUI();
        }
    }

    private void CheckCombatEnd()
    {
        if (enemyHP <= 0)
        {
            infoText.text = "Enemy defeated! You win!";
            EndEncounter(true);
        }
        else if (playerHP <= 0)
        {
            infoText.text = "Player defeated! Game Over!";
            EndEncounter(false);
        }
    }

    private void EndEncounter(bool playerWon = false)
    {   
        MusicMaster.Instance.PlayBackgroundMusic();


        combatPanel.SetActive(false);
        attackButton.interactable = false;
        fleeButton.interactable = false;

        if (playerWon)
        {
            Debug.Log("player won");
            // update hunt list
            HuntMaster.Instance.RegisterKill();
        }
        else
        {
            Debug.Log("player lost");
            // game over return to menu
            GameOverPanel.SetActive(true);
        }
    }




}

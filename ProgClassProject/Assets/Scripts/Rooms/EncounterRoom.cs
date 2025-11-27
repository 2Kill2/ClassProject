using UnityEngine;

public class EncounterRoom : MonoBehaviour
{
    private GameMaster gm;
    private System.Random srng;
    private bool encountered = false;

    void Awake()
    {
        gm = FindAnyObjectByType<GameMaster>();
    }
    public void Initialize(int seed, GameMaster gameMaster)
    {
        srng = new System.Random(seed);
    }

    public string SearchEncounter()
    {
        if (encountered)
        {
            gm.ShowMessage("You already fought in this room");
            Debug.Log("The encounter has already been completed.");
            return "The encounter has already been completed.";
        }

        encountered = true;

        if (CombatMaster.Instance != null)
        {
            gm.ShowMessage("Encounter started!");
            CombatMaster.Instance.StartEncounter();
            Debug.Log("Encounter started!");
        }
        else
        {
            Debug.LogError("CombatMaster instance not found!");
        }

        return "An encounter has begun!";
    }
}

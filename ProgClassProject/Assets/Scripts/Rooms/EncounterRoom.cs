using UnityEngine;

public class EncounterRoom : MonoBehaviour
{
    private System.Random srng;
    private bool encountered = false;

    public void Initialize(int seed, GameMaster gameMaster)
    {
        srng = new System.Random(seed);
    }

    public string SearchEncounter()
    {
        if (encountered)
        {
            Debug.Log("The encounter has already been completed.");
            return "The encounter has already been completed.";
        }

        encountered = true;

        if (CombatMaster.Instance != null)
        {
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

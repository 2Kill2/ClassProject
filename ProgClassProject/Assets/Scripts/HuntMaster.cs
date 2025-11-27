using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HuntMaster : MonoBehaviour
{
    public static HuntMaster Instance;
    private GameMaster gm;

    [Header("Hunt Settings")]
    public int huntGoal;
    public int huntProgress;

    [Header("UI")]
    public TMP_Text huntText;
    public GameObject victoryPanel;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        gm = FindAnyObjectByType<GameMaster>();
    }

    private void Start()
    {
        StartNewHunt();
        UpdateHuntUI();
    }

    public void StartNewHunt()
    {
        huntGoal = Random.Range(5,11);
        huntProgress = 0;
        victoryPanel.SetActive(false);

        Debug.Log($"hunt started, goal {huntGoal}");
    }

    public void RegisterKill()
    {
        huntProgress++;
        gm.ShowMessage($"Hunt update: {huntProgress}/{huntGoal}");
        UpdateHuntUI();

        if (huntProgress >= huntGoal)
        {
            TriggerWinScreen();
        }
    }

    private void UpdateHuntUI()
    {
        if (huntText != null) huntText.text = $"{huntProgress}/{huntGoal} hunts completed.";
    }

    private void TriggerWinScreen()
    {
        Debug.Log("player wins game via hunts");
        victoryPanel.SetActive(true);
    }

}

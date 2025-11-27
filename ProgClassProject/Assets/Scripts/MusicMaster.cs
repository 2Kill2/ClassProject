using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class MusicMaster : MonoBehaviour
{
    public static MusicMaster Instance;

    public AudioSource bgMusic;
    public AudioSource combatMusic;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void PlayCombatMusic()
    {
        StopAllCoroutines();
        StartCoroutine(SwitchToCombat());
    }

    public void PlayBackgroundMusic()
    {
        StopAllCoroutines();
        StartCoroutine(SwitchToBackground());
    }

    private IEnumerator SwitchToCombat()
    {
        while (bgMusic.volume > 0f)
        {
            bgMusic.volume -= Time.deltaTime;
            yield return null;
        }
        bgMusic.Pause();

        combatMusic.volume = 1f;
        combatMusic.Play();
    }

    private IEnumerator SwitchToBackground()
    {
        while (combatMusic.volume > 0f)
        {
            combatMusic.volume -= Time.deltaTime;
            yield return null;
        }
        combatMusic.Pause();

        bgMusic.volume = 1f;
        bgMusic.Play();
    }
}

using UnityEngine;
using UnityEngine.Audio;

public class AudioInit : MonoBehaviour
{
    public AudioMixer mainMixer;
    void Start()
    {
        float master = PlayerPrefs.GetFloat("Master", 0.75f);
        float music = PlayerPrefs.GetFloat("Music", 0.75f);
        float sfx = PlayerPrefs.GetFloat("SFX", 0.75f);

        mainMixer.SetFloat("Master", Mathf.Log10(Mathf.Clamp(master, 0.0001f, 1f)) * 20);
        mainMixer.SetFloat("Music", Mathf.Log10(Mathf.Clamp(music, 0.0001f, 1f)) * 20);
        mainMixer.SetFloat("SFX", Mathf.Log10(Mathf.Clamp(sfx, 0.0001f, 1f)) * 20);
    }
}

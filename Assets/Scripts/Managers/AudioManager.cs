using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour {
    public AudioSource[] music;
    float[] musicMax;
    public Slider[] musicSliders;
    public AudioSource[] effects;
    float[] effectsMax;
    public Slider[] effectsSliders;

    private void Start() {
        CreateVolumes();
        GetVolumes();

        if (PlayerPrefs.GetInt("tutorialComplete") == 0) {
            AmbientSwitch(-2);
        } else
            AmbientSwitch(-1);
    }

    public void GetVolumes() {
        float musicVolume;
        float effectsVolume;
        if (PlayerPrefs.HasKey("musicVolume")) {
            musicVolume = PlayerPrefs.GetFloat("musicVolume");
            effectsVolume = PlayerPrefs.GetFloat("effectsVolume");
        } else {
            musicVolume = 1;
            effectsVolume = 1;
        }

        for (int i = 0; i < effectsSliders.Length; i++)
            effectsSliders[i].value = effectsVolume;
        for (int i = 0; i < musicSliders.Length; i++)
            musicSliders[i].value = musicVolume;

        ChangeMusicVolume(musicSliders[0]);
        ChangeEffectsVolume(effectsSliders[0]);
    }

    public void CreateVolumes() {
        musicMax = new float[music.Length];
        effectsMax = new float[effects.Length];
        for (int i = 0; i < music.Length; i++)
            musicMax[i] = music[i].volume;

        for (int i = 0; i < effects.Length; i++)
            effectsMax[i] = effects[i].volume;
    }

    public void ChangeMusicVolume(Slider slider) {
        for (int i = 0; i < music.Length; i++) {
            music[i].volume = slider.value * musicMax[i];
        }

        PlayerPrefs.SetFloat("musicVolume", slider.value);
    }

    public void ChangeEffectsVolume(Slider slider) {
        for (int i = 0; i < effects.Length; i++) {
            effects[i].volume = slider.value * effectsMax[i];
        }

        PlayerPrefs.SetFloat("effectsVolume", slider.value);
    }

    public void AmbientSwitch(int which) {
        for (int i = 0; i < music.Length; i++) {
            music[i].Stop();
        }

        if (which == -2)
            music[7].Play();
        else if (which == -1)
            music[6].Play();
        else if (which == 0)
            music[Random.Range(0, 3)].Play();
        else
            music[Random.Range(3, 6)].Play();
    }

    public void AmbientSlow(float pitch) {
        for (int i = 0; i < music.Length; i++) {
            music[i].pitch = pitch;
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadGame : MonoBehaviour
{
    public Slider sliderVolume;
    public Slider sliderMusic;
    public Slider sliderSound;    

    public void Save()
    {
        PlayerPrefs.SetFloat("Volume", sliderVolume.value);
        PlayerPrefs.SetFloat("Music", sliderMusic.value);
        PlayerPrefs.SetFloat("Sound", sliderSound.value);
        

        PlayerPrefs.Save();
    }

    public void Load()
    {

        if (PlayerPrefs.HasKey("Volume")) sliderVolume.value = PlayerPrefs.GetFloat("Volume");
        else sliderVolume.value = 1.0f;
        if (PlayerPrefs.HasKey("Music")) sliderMusic.value = PlayerPrefs.GetFloat("Music");
        else sliderMusic.value = 1.0f;
        if (PlayerPrefs.HasKey("Sound")) sliderSound.value = PlayerPrefs.GetFloat("Sound");
        else sliderSound.value = 1.0f;

    }
}

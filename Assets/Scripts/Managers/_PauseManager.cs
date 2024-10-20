//Libraries
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

//Base class
public class _PauseManager : MonoBehaviour {
    //Defining scene elements for control variables
    [SerializeField] private GameObject mainMenuButton;
    [SerializeField] private GameObject restartGameButton;
    [SerializeField] private GameObject safetyAlert;
    [SerializeField] private GameObject eventSystemObj;
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider fxVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Button masterVolumeButton;
    [SerializeField] private Button fxVolumeButton;
    [SerializeField] private Button musicVolumeButton;
    [SerializeField] private AudioMixer masterMixer;
    [SerializeField] private AudioMixer fxMixer;
    [SerializeField] private AudioMixer musicMixer;
    [SerializeField] private Sprite unmutedSprite;
    [SerializeField] private Sprite mutedSprite;
    [SerializeField] private Dropdown qualityDropdown;
    [SerializeField] private Dropdown languageDropdown;
    [SerializeField] private int nextAction;
    [SerializeField] private bool isActionConfirmed;
    [SerializeField] private Coroutine userConfirmCoroutine;

    public void ToggleFXVolume() {
        if (PlayerPrefs.GetInt("fx") != 0) {
            fxMixer.SetFloat("mixerFX", -80);
            PlayerPrefs.SetInt("fx", 0);
        } else { 
            fxMixer.SetFloat("mixerFX", 0);
            PlayerPrefs.SetInt("fx", 1);
        }
        PlayerPrefs.Save();
    }

    public void ToggleMusicVolume() {
        if (PlayerPrefs.GetInt("music") != 0) {
            musicMixer.SetFloat("mixerMusic", -80);
            PlayerPrefs.SetInt("music", 0);
        } else {
            musicMixer.SetFloat("mixerMusic", 0);
            PlayerPrefs.SetInt("music", 1);
        }
        PlayerPrefs.Save();
    }

    public void ToggleMasterVolume() {
        if (PlayerPrefs.GetInt("master") != 0) {
            masterMixer.SetFloat("mixerMaster", -80);
            PlayerPrefs.SetInt("master", 0);
            musicMixer.SetFloat("mixerMusic", -80);
            PlayerPrefs.SetInt("music", 0);
            fxMixer.SetFloat("mixerFX", -80);
            PlayerPrefs.SetInt("fx", 0);
        } else { 
            masterMixer.SetFloat("mixerMaster", 0);
            PlayerPrefs.SetInt("master", 1);
            musicMixer.SetFloat("mixerMusic", 0);
            PlayerPrefs.SetInt("music", 1);
            fxMixer.SetFloat("mixerFX", 0);
            PlayerPrefs.SetInt("fx", 1);
        }
        PlayerPrefs.Save();
    }
}
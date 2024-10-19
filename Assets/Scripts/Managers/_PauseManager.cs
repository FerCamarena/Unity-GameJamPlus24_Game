//Libraries
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

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

    //Method called once the script instance is loaded
    private void Awake() {
        //Checking if comes from another scene
        if (PlayerPrefs.GetInt("SettingsMenu") == 1) {
            //Limiting the event system to one by default
            eventSystemObj.SetActive(false);
        }
        //Loading previous preferences
        LoadPreferences();
        //Updating visuals from preferences
        InitialVisualsUpdate();
    }

    //Method for loading and displaying previous preferences
    private void LoadPreferences() {
        //Loading all the volume channels previous sessions
        masterVolumeSlider.value = PlayerPrefs.GetFloat("masterVolume", 50.0f);
        fxVolumeSlider.value = PlayerPrefs.GetFloat("fxVolume", 50.0f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume", 50.0f);
        //Loading previous quality settings from previous sessions
        qualityDropdown.value = PlayerPrefs.GetInt("qualityIndex", 0);
        //Loading previous language settings from previous sessions
        languageDropdown.value = PlayerPrefs.GetInt("languageIndex", 0);
    }

    //Method for update appareances from first call
    private void InitialVisualsUpdate() {
        //Checking if the scene is not being load from game scene
        if (PlayerPrefs.GetInt("inGame") != 1) {
            //Hiding in-game buttons
            mainMenuButton.SetActive(false);
            restartGameButton.SetActive(false);
        }
        //Updating all volume channels appareances from previous inputs
        UpdateMasterVisuals();
        UpdateFXVisuals();
        UpdateMusicVisuals();
    }

    //Method for updating the quality settings index value
    private void UpdateQualityIndex() {
        //Update the quality level based on input
        QualitySettings.SetQualityLevel(qualityDropdown.value);
        //Setting the quality setting input in preferences
        PlayerPrefs.SetInt("qualityIndex", qualityDropdown.value);
    }

    //Method for updating the language translation index value
    private void UpdateLanguageIndex() {
        //Setting the language translation setting input in preferences
        PlayerPrefs.SetInt("languageIndex", languageDropdown.value);
    }

    //Method for updating the master volume values
    private void UpdateMasterVolume() {
        //Updating the PlayerPref for master value
        PlayerPrefs.SetFloat("masterVolume", masterVolumeSlider.value);
        //Sending the values to the Master mix channel
        masterMixer.SetFloat("mixerMaster", masterVolumeSlider.value - 100);
        //Checking if it was completely muted by the slider
        if (masterVolumeSlider.value == 0.0f) {
            //Checking if FX volume wasn't muted already
            if (fxVolumeSlider.value > 0.0f) {
                //Marking the value for saving last Master volume changging FX channel
                PlayerPrefs.SetInt("fxChangedByMaster", 1);
                //Saving the previous FX volume into preferences
                PlayerPrefs.SetFloat("previousFXVolume", fxVolumeSlider.value);
            }
            if (musicVolumeSlider.value > 0.0f)
            {
                //Marking the value for saving last Master volume changging Music channel
                PlayerPrefs.SetInt("musicChangedByMaster", 1);
                //Saving the previous Music volume into preferences
                PlayerPrefs.SetFloat("previousMusicVolume", musicVolumeSlider.value);
            }
            //Mutting other channels
            fxVolumeSlider.value = 0;
            musicVolumeSlider.value = 0;
        } else {
            //Checking if able to unmute FX channel
            if (fxVolumeSlider.value == 0 && PlayerPrefs.GetInt("fxChangedByMaster") == 1) {
                //Marking the value for saving last Master volume changging FX channel
                PlayerPrefs.SetInt("fxChangedByMaster", 0);
                //Unmuting FX channel
                ToggleFXVolume();
            }
            //Checking if able to unmute Music channel
            if (musicVolumeSlider.value == 0 && PlayerPrefs.GetInt("musicChangedByMaster") == 1) {
                //Marking the value for saving last Master volume changging Music channel
                PlayerPrefs.SetInt("musicChangedByMaster", 0);
                //Unmuting Music channel
                ToggleMusicVolume();
            }
        }
        //Updating master mute button visuals from input
        UpdateMasterVisuals();
    }

    //Method for toggle Master volume mute
    private void ToggleMasterVolume() {
        //Toggling between mute and unmute Master channel based on current value
        if (masterVolumeSlider.value > 0f) {
            //Saving the previous Master volume into preferences
            PlayerPrefs.SetFloat("previousMasterVolume", masterVolumeSlider.value);
            //Checking if FX channel is unmuted
            if (fxVolumeSlider.value > 0.0f) {
                //Marking the value for saving last Master volume changging FX channel
                PlayerPrefs.SetInt("fxChangedByMaster", 1);
                //Calling the method for mute the FX
                ToggleFXVolume();
            }
            //Checking if Music channel is unmuted
            if (musicVolumeSlider.value > 0.0f) {
                //Marking the value for saving last Master volume changging Music channel
                PlayerPrefs.SetInt("musicChangedByMaster", 1);
                //Calling the method for mute the Music
                ToggleMusicVolume();
            }
            //Setting the Master channel as muted
            masterVolumeSlider.value = 0f;
        } else {
            //Loading the previous Master volume from preferences
            masterVolumeSlider.value = PlayerPrefs.GetFloat("previousMasterVolume", 50.0f);
            //Checking if FX channel is muted
            if (fxVolumeSlider.value == 0.0f && PlayerPrefs.GetInt("fxChangedByMaster") == 1) {
                //Unmarking the value for saving last Master volume changging FX channel
                PlayerPrefs.SetInt("fxChangedByMaster", 0);
                //Calling the method for unmute the FX
                ToggleFXVolume();
            }
            //Checking if Music channel is muted
            if (musicVolumeSlider.value == 0.0f && PlayerPrefs.GetInt("musicChangedByMaster") == 1) {
                //Unmarking the value for saving last Master volume changging Music channel
                PlayerPrefs.SetInt("musicChangedByMaster", 0);
                //Calling the method for unmute the Music
                ToggleMusicVolume();
            }
        }
        //Sending the values to the Master mix channel
        masterMixer.SetFloat("mixerMaster", masterVolumeSlider.value - 100);
        //Updating Master mute button visuals from current values
        UpdateMasterVisuals();
    }

    //Method for updating Master volume visuals current values
    private void UpdateMasterVisuals() {
        //Checks if the Master volume value is more than 0
        if (masterVolumeSlider.value > 0f) {
            //Changing the button sprite for the unmuted one
            masterVolumeButton.image.sprite = unmutedSprite;
        } else {
            //Changing the button sprite for the muted one
            masterVolumeButton.image.sprite = mutedSprite;
        }
    }

    //Method for updating the FX volume values
    private void UpdateFXVolume() {
        //Updating the PlayerPref for FX value
        PlayerPrefs.SetFloat("fxVolume", fxVolumeSlider.value);
        //Sending the values to the FX mix channel
        fxMixer.SetFloat("mixerFX", fxVolumeSlider.value - 100);
        //Checking if the FX channel got unmuted
        if (fxVolumeSlider.value > 0.0f) {
            //Checking if all channels were muted
            if (masterVolumeSlider.value == 0f) {
                //Unmuting all from master
                ToggleMasterVolume();
            }
        }
        //Updating FX mute button visuals from input
        UpdateFXVisuals();
    }

    //Method for toggle FX volume mute
    public void ToggleFXVolume() {
        //Toggling between mute and unmute FX channel based on current value
        if (fxVolumeSlider.value > 0f) {
            //Saving the previous FX volume into preferences
            PlayerPrefs.SetFloat("previousFXVolume", fxVolumeSlider.value);
            //Setting the FX channel as muted
            fxVolumeSlider.value = 0.0f;
        } else {
            //Loading the previous FX volume from preferences
            fxVolumeSlider.value = PlayerPrefs.GetFloat("previousFXVolume", 50.0f);
            //Checking if the other channels are muted
            if (masterVolumeSlider.value == 0.0f && musicVolumeSlider.value == 0.0f) {
                //Unmuting other channels
                masterVolumeSlider.value = PlayerPrefs.GetFloat("previousMasterVolume", 50.0f);
                musicVolumeSlider.value = PlayerPrefs.GetFloat("previousMusicVolume", 50.0f);
            }
        }
        //Sending the values to the FX mix channel
        fxMixer.SetFloat("mixerFX", fxVolumeSlider.value - 100);
        //Updating fx mute button visuals from current values
        UpdateFXVisuals();
    }

    //Method for updating FX volume visuals current values
    private void UpdateFXVisuals() {
        //Checks if the FX volume value is more than 0
        if (fxVolumeSlider.value > 0f) {
            //Changing the button sprite for the unmuted one
            fxVolumeButton.image.sprite = unmutedSprite;
        } else {
            //Changing the button sprite for the muted one
            fxVolumeButton.image.sprite = mutedSprite;
        }
    }

    //Method for updating the Music volume values
    private void UpdateMusicVolume() {
        //Updating the PlayerPref for Music value
        PlayerPrefs.SetFloat("musicVolume", musicVolumeSlider.value);
        //Sending the values to the Music mix channel
        musicMixer.SetFloat("mixerMusic", musicVolumeSlider.value - 100);
        //Checking if the Music channel got unmuted
        if (musicVolumeSlider.value > 0.0f) {
            //Checking if all channels were muted
            if (masterVolumeSlider.value == 0f) {
                //Unmuting all from master
                ToggleMasterVolume();
            }
        }
        //Updating Music mute button visuals from input
        UpdateMusicVisuals();
    }

    //Method for toggle Music volume mute
    private void ToggleMusicVolume() {
        //Toggling between mute and unmute Music channel based on current value
        if (musicVolumeSlider.value > 0f) {
            //Saving the previous Music volume into preferences
            PlayerPrefs.SetFloat("previousMusicVolume", musicVolumeSlider.value);
            //Setting the Music channel as muted
            musicVolumeSlider.value = 0f;
        } else {
            //Loading the previous Music volume from preferences
            musicVolumeSlider.value = PlayerPrefs.GetFloat("previousMusicVolume", 50.0f);
            //Checking if the other channels are muted
            if (masterVolumeSlider.value == 0.0f && fxVolumeSlider.value == 0.0f) {
                //Unmuting other channels
                masterVolumeSlider.value = PlayerPrefs.GetFloat("previousMasterVolume", 50.0f);
                fxVolumeSlider.value = PlayerPrefs.GetFloat("previousFXVolume", 50.0f);
            }
        }
        //Sending the values to the Music mix channel
        fxMixer.SetFloat("mixerFX", fxVolumeSlider.value - 100);
        //Updating Music mute button visuals from current values
        UpdateMusicVisuals();
    }

    //Method for updating Music volume visuals current values
    private void UpdateMusicVisuals() {
        //Checks if the Music volume value is more than 0
        if (musicVolumeSlider.value > 0f) {
            //Changing the button sprite for the unmuted one
            musicVolumeButton.image.sprite = unmutedSprite;
        } else {
            //Changing the button sprite for the muted one
            musicVolumeButton.image.sprite = mutedSprite;
        }
    }

    //Method to show confirm user action
    private void ShowSafetyAlert(int actionIndex) {
        //Setting the index as the new action
        nextAction = actionIndex;
        //Checking there's already a coroutine started
        if (userConfirmCoroutine != null) {
            //Canceling the previous coroutine
            StopCoroutine(userConfirmCoroutine);
        }
        //Starting a coroutine for wait user confirmation
        userConfirmCoroutine = StartCoroutine(WaitUserConfirmation());
    }

    //Method for receive and accept user action
    private void ConfirmAction() {
        //Confirming action dialog
        isActionConfirmed = true;
        //Closing settings menu value and leaving the game
        PlayerPrefs.SetInt("settingsMenu", 0);
        if(nextAction == 2) {
            PlayerPrefs.SetInt("inGame", 0);
        }
        //Saving all new configs into prefs
        PlayerPrefs.Save();
    }

    //Method for receive and cancel user action
    private void CancelAction() {
        //Cancelling action dialog
        isActionConfirmed = false;
        //Setting the action as 0 for safety
        nextAction = 0;
    }

    //Method waiting user confirmation
    private IEnumerator WaitUserConfirmation() {
        //Checking if user confirmation
        while (!isActionConfirmed)
        {
            //Returning for wait
            yield return null;
        }
        //Double checking user has already confirmed
        if (isActionConfirmed)
        {
            //Restarting the game with current user selections
            if (nextAction == 1)
            {
                //Restarting the curren scene
                SceneManager.LoadScene(7);
            }
            //Sending the user to main menu
            if (nextAction == 2)
            {
                //Sending the player to the main menu scene
                SceneManager.LoadScene(1);
            }
        }
        //Unpausing the game
        Time.timeScale = 1;
        //Checking that the coroutine is still active
        if (userConfirmCoroutine != null)
        {
            //Stopping the corourtine for safety
            StopCoroutine(userConfirmCoroutine);
            //Enabling the coroutine variable for next time
            userConfirmCoroutine = null;
        }
    }
}
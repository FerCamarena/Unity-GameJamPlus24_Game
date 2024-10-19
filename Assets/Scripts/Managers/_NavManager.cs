//Libraries
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

//Class for managging the user navigation between game scenes
public class _NavManager : MonoBehaviour {
    //Variables to handle audio setttings loaded
    [SerializeField] private AudioMixer masterMixer;
    [SerializeField] private AudioMixer fxMixer;
    [SerializeField] private AudioMixer musicMixer;

    //Method called once the object becomes active
    private void OnEnable() {
        InGameEvent.OnGameOver += GameOver;
    }
    //Method called once the object becomes inactive
    private void OnDisable() {
        InGameEvent.OnGameOver -= GameOver;
    }

    //Method called once the script instance is loaded
    private void Awake() {
        //Loading defaults preferences
        if (PlayerPrefs.GetInt("charactersUnlocked") < 1 || !PlayerPrefs.HasKey("charactersUnlocked") || PlayerPrefs.GetInt("charactersUnlocked") > 7) {
            //Checking the first time playing
            PlayerPrefs.SetInt("charactersUnlocked", 1);
        }
        if (PlayerPrefs.GetInt("settingsMenu") < 0 || !PlayerPrefs.HasKey("settingsMenu") || PlayerPrefs.GetInt("settingsMenu") > 1) {
            //Checking the first time playing
            PlayerPrefs.SetInt("settingsMenu", 0);
        }
        //Saving the player prefs settings
        PlayerPrefs.Save();
        //Loading the mixer values
        masterMixer.SetFloat("mixerMaster", PlayerPrefs.GetFloat("masterVolume") - 100);
        fxMixer.SetFloat("mixerFX", PlayerPrefs.GetFloat("fxVolume") - 100);
        musicMixer.SetFloat("mixerMusic", PlayerPrefs.GetFloat("musicVolume") - 100);
    }

    //Method for starting a new game
    private void StartGame() {
        //Stablishing game state as on
        PlayerPrefs.SetInt("inGame", 1);
        //Loading the game scene
        ChangeScene(6);
    }

    //Method for ending the game session
    public void GameOver() {
        //Stablishing game state as off
        PlayerPrefs.SetInt("inGame", 0);
        //Sending the user to the game over screen
        ChangeScene(7);
    }

    //Method for changing scenes
    public void ChangeScene(int sceneIndex) {
        //Saving user settings
        PlayerPrefs.Save();
        //Changing the scene
        SceneManager.LoadScene(sceneIndex);
    }

    //Method for exiting the game
    public void QuitGame() {
        //Enabling for both app and editor modes
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    //Method to open the settings menu
    public void SettingsMenu() {
        if(PlayerPrefs.GetInt("settingsMenu") == 1) {
            //Marking the value for settings scene closed
            PlayerPrefs.SetInt("settingsMenu", 0);
            //Unloading the settings scene
            SceneManager.UnloadSceneAsync(8);
            //Unpausing the game
            Time.timeScale = 1;
        } else {
            //Marking the value for settings scene openned
            PlayerPrefs.SetInt("settingsMenu", 1);
            //Loading the settings scene in additive mode
            SceneManager.LoadScene(8, LoadSceneMode.Additive);
            //Pausing the game
            Time.timeScale = 0;
        }
        //Saving all user preferences
        PlayerPrefs.Save();
    }

    //Method for unlocking the easter egg and sending the user to the ZyaNya portfolio
    private void SpamZyaNya() {
        //Opening the zyanya web portfolio PENDING TO COMPLETE AND UPLOAD
        //Application.OpenURL("http://linktozyanyaportfolioormedias");
    }
    
    //[DEV]Method for reset all content
    private void ResetAll() {
        //Setting PlayerPrefs
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    //[DEV]Method for reset all content
    private void ResetStats() {
        //Setting PlayerPrefs
        PlayerPrefs.Save();
    }

    //[DEV]Method for unlock all content
    private void UnlockAll() {
        //Setting PlayerPrefs
        PlayerPrefs.Save();
    }
}
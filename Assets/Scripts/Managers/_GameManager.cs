using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class _GameManager : MonoBehaviour {
    public _InputManager _Input;
    public Camera cam;

    public bool onCombat = false;
    public bool forceEnd = true;

    public GameObject character;
    public GameObject[] enemies;

    public List<GameObject> enemiesSpawned = new List<GameObject>();

    public int mapSize = 54;
    public int currentWave = 0;

    public int enemyAmount = 1;

    public AudioClip combat;
    public AudioClip building;

    public AudioSource soundtrack;

    private void Start() {
        if (!_Input) _Input = GameObject.Find("UI").GetComponent<_InputManager>();

        InitializeGame();
    }
    
    private void Update() {
        RotateWorld();

        if (_Input.K_State) {
            EndWave();
        }

        ProcessMovement();
    }

    private void InitializeGame() {
        Invoke("FirstWave", 38f);
        Invoke("EndWave", 38f + 58f);
        Invoke("SecondWave", 38f + 58f + 38f);
        Invoke("EndWave", 38f + 58f + 38f + 58f);
        Invoke("ThirdWave", 38f + 58f + 38f + 58f + 38f);
        Invoke("EndWave", 38f + 58f + 38f + 58f + 38f + 58f);
        Invoke("EndGame", 300);

        PlayerPrefs.SetInt("lastPoints", 0);
        PlayerPrefs.SetInt("lastUsed", 0);
        PlayerPrefs.Save();
    }

    public void EndGame() {
        soundtrack.clip = building;
        soundtrack.Play();
        InGameEvent.GameWon();
    }

    public void EndWave() {
        if (currentWave <= 3) InGameEvent.WaveEnded();
        if (!onCombat) character.GetComponent<NavMeshAgent>().enabled = false;
        onCombat = false;
        character = null;

        if (enemiesSpawned.Count > 0) {
            enemiesSpawned.RemoveAll(item => item == null);
            foreach (GameObject enemy in enemiesSpawned) { 
                Destroy(enemy, 0.1f);
            }
            enemiesSpawned.Clear();
        }
    }
    
    public void FirstWave() {
        soundtrack.clip = combat;
        soundtrack.Play();
        currentWave++;
        if (!onCombat) character.GetComponent<NavMeshAgent>().enabled = false;
        onCombat = true;
        character = null;
        //Calling method to stop card playing
        onCombat = true;
        InGameEvent.WaveStarted();
        //Calling method to change the music

        SummonWave();
    }
    
    public void SecondWave() {
        soundtrack.clip = combat;
        soundtrack.Play();
        currentWave++;
        if (!onCombat) character.GetComponent<NavMeshAgent>().enabled = false;
        onCombat = true;
        character = null;
        //Calling method to stop card playing

        InGameEvent.WaveStarted();

        //Calling method to change the music

        SummonWave();
    }
    
    public void ThirdWave() {
        soundtrack.clip = combat;
        soundtrack.Play();
        currentWave++;
        if (!onCombat) character.GetComponent<NavMeshAgent>().enabled = false;
        onCombat = !onCombat;
        character = null;
        //Calling method to stop card playing

        InGameEvent.WaveStarted();

        //Calling method to change the music

        SummonWave();
    }

    private void RotateWorld() {
        if (_Input.Rg_State) {
            cam.transform.RotateAround(Vector3.zero, Vector3.up, _Input.Rg_Value * -25 * Time.deltaTime);
        } else if (_Input.Lf_State) {
            cam.transform.RotateAround(Vector3.zero, Vector3.up, _Input.Lf_Value * 25 * Time.deltaTime);
        }

        if (_Input.Up_State && cam.transform.rotation.eulerAngles.x <= 80) {
            cam.transform.parent.RotateAround(Vector3.zero, -cam.transform.right, _Input.Up_Value * -10 * Time.deltaTime);
        } else if(_Input.Do_State && cam.transform.rotation.eulerAngles.x >= 25) {
            cam.transform.parent.RotateAround(Vector3.zero, -cam.transform.right, _Input.Do_Value * 10 * Time.deltaTime);
        }
    }
    
    private void ProcessMovement() {
        CheckDimension();
        if (character) MoveCharacter();
}

    private void CheckDimension(){
        if (!character) {
            if (onCombat) character = GameObject.FindGameObjectWithTag("Player3D");
            if (!onCombat) character = GameObject.FindGameObjectWithTag("Player2D");
        }
    }

    private void MoveCharacter() {
        if (!character.GetComponent<NavMeshAgent>().enabled) character.GetComponent<NavMeshAgent>().enabled = true;

        Vector3 move = Vector3.zero;
        if (_Input.A_State) {
            move += Vector3.left * _Input.A_Value;
        }
        if (_Input.D_State) {
            move += Vector3.right * _Input.D_Value;
        }
        if (_Input.W_State) {
            move += Vector3.forward * _Input.W_Value;
        }
        if (_Input.S_State) {
            move += Vector3.back * _Input.S_Value;
        }

        Vector3 rotation = Camera.main.transform.rotation.eulerAngles;
        rotation.x = 0;

        move = Quaternion.Euler(rotation) * move;

        character.GetComponent<NavMeshAgent>().destination = character.transform.localPosition + move;
    }

    private void SummonWave() {
        for(int i = 0; i < enemyAmount; i++) { 
            Vector2 pos = GetRandomPointOnMap();
            GameObject obj = Instantiate(enemies[Random.Range(0, enemies.Length)], new Vector3(pos.x, 0, pos.y),Quaternion.identity);
            enemiesSpawned.Add(obj);
        }
    }

    private Vector2 GetRandomPointOnMap() {
        float x = Random.value * (Random.value >= 0.5f ? 1 : -1);
        float y = Random.value * (Random.value >= 0.5f ? 1 : -1);
        Vector2 dir = new Vector2(x, y);

        return dir.normalized * mapSize;
    }
}
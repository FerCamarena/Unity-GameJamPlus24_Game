using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class _GameManager : MonoBehaviour {
    public _InputManager _Input;
    public Camera cam;

    public bool onCombat = false;
    public bool forceEnd = true;

    public GameObject character;
    public GameObject[] enemies;

    public List<GameObject> enemiesSpawned = new List<GameObject>();
    public List<GameObject> enemiesToSpawn = new List<GameObject>();

    public int mapSize = 54;
    public int currentWave = 0;

    public int enemyAmount = 1;

    public AudioClip combat;
    public AudioClip building;

    public AudioSource soundtrack;

    public Transform DECK;

    public GameObject[] cards;

    public List<string> letreros;
    public TextMeshProUGUI letrero;

    private void Start() {
        if (!_Input) _Input = GameObject.Find("UI").GetComponent<_InputManager>();

        InitializeGame();
    }
    
    private void Update() {
        RotateWorld();

        ProcessMovement();
    }

    public IEnumerator SpawnEnemies() {
        foreach (GameObject enemy in enemiesToSpawn) {
            Vector2 pos = GetRandomPointOnMap();
            GameObject obj = Instantiate(enemy, new Vector3(pos.x, 0, pos.y),Quaternion.identity);
            enemiesSpawned.Add(obj);
            yield return new WaitForSeconds(1);
        }
        enemiesToSpawn.Clear();
    }

    private void InitializeGame() {
        Invoke("FirstWave", 38f);
        Invoke("EndWave", 38f + 58f);
        Invoke("SecondWave", 38f + 58f + 38f);
        Invoke("EndWave", 38f + 58f + 38f + 58f);
        Invoke("ThirdWave", 38f + 58f + 38f + 58f + 38f);
        Invoke("EndWave", 38f + 58f + 38f + 58f + 38f + 58f);
        Invoke("EndGame", 300);

        if (currentWave == 0)  StartCoroutine(TuTORIAIL());

        PlayerPrefs.SetInt("lastPoints", 0);
        PlayerPrefs.SetInt("lastUsed", 0);
        PlayerPrefs.Save();
    }

    private IEnumerator TuTORIAIL() {
        foreach (string s in letreros) {
            letrero.text = s;
            yield return new WaitForSecondsRealtime(6);
        }
        Destroy(letrero.gameObject);
    }

    public void EndGame() {
        soundtrack.clip = building;
        soundtrack.Play();
        InGameEvent.GameWon();
    }

    public void EndWave() {
        if (currentWave <= 3) InGameEvent.WaveEnded();
        if (currentWave == 2) for (int i = 0; i < cards.Length; i++) Instantiate(cards[i], DECK.position, Quaternion.identity, DECK);
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

        if (currentWave == 1) for (int i = 0; i < cards.Length; i++) Instantiate(cards[i], DECK.position, Quaternion.identity, DECK);
        if (currentWave == 1) for (int i = 0; i < cards.Length; i++) Instantiate(cards[i], DECK.position, Quaternion.identity, DECK);
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
        if (enemiesToSpawn.Count > 0 && onCombat) StartCoroutine(SpawnEnemies());
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
        if (enemiesToSpawn.Count > 0 && onCombat) StartCoroutine(SpawnEnemies());
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
        if (enemiesToSpawn.Count > 0 && onCombat) StartCoroutine(SpawnEnemies());
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
        for(int i = 0; i < enemyAmount * (currentWave); i++) { 
            GameObject obj = enemies[Random.Range(0, enemies.Length)];
            enemiesToSpawn.Add(obj);
        }
    }

    private Vector2 GetRandomPointOnMap() {
        float x = Random.value * (Random.value >= 0.5f ? 1 : -1);
        float y = Random.value * (Random.value >= 0.5f ? 1 : -1);
        Vector2 dir = new Vector2(x, y);

        return dir.normalized * mapSize;
    }
}
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
    
    private void OnEnable() {
        InGameEvent.enemyKill += RemoveEnemy;
    }
    
    private void OnDisable() {
        InGameEvent.enemyKill -= RemoveEnemy;
    }

    private void InitializeGame() {
        Invoke("FirstWave", 15);
        Invoke("EndWave", 45);
        Invoke("SecondWave", 60);
        Invoke("EndWave", 90);
        Invoke("ThirdWave", 105);
        Invoke("EndWave", 135);
    }

    public void RemoveEnemy(Enemy enemy) { 
        if (enemiesSpawned.Contains(enemy.gameObject)) enemiesSpawned.Remove(enemy.gameObject);
    }

    public void EndWave() {
        if (!onCombat) character.GetComponent<NavMeshAgent>().enabled = false;
        onCombat = !onCombat;
        character = null;

        if (enemiesSpawned.Count > 0) { 
            foreach (GameObject enemy in enemiesSpawned) { 
                Destroy(enemy, 0.1f);
            }
            enemiesSpawned.Clear();
        }
    }
    
    public void FirstWave() {
        //Calling method to stop card playing
        onCombat = true;

        //Calling method to change the music

        SummonWave();
    }
    
    public void SecondWave() {
        //Calling method to stop card playing


        //Calling method to change the music

        SummonWave();
    }
    
    public void ThirdWave() {
        //Calling method to stop card playing


        //Calling method to change the music

        SummonWave();
    }

    private void RotateWorld() {
        if (_Input.Rg_State) {
            cam.transform.RotateAround(Vector3.zero, Vector3.up, _Input.Rg_Value * -.25f);
        } else if (_Input.Lf_State) {
            cam.transform.RotateAround(Vector3.zero, Vector3.up, _Input.Lf_Value * .25f);
        }

        /*if (_Input.W_State && cam.transform.rotation.eulerAngles.x <= 50) {
            Debug.Log(cam.transform.rotation.eulerAngles.x);
            cam.transform.RotateAround(Vector3.zero, Vector3.left, -_Input.W_Value);
        } else if(_Input.S_State && cam.transform.rotation.eulerAngles.x >= 10) {
            Debug.Log(cam.transform.rotation.eulerAngles.x);
            cam.transform.RotateAround(Vector3.zero, Vector3.left, _Input.S_Value);
        }*/
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
        Vector2 pos = GetRandomPointOnMap();
        GameObject obj = Instantiate(enemies[0], new Vector3(pos.x, 0, pos.y),Quaternion.identity);
        enemiesSpawned.Add(obj);
    }

    private Vector2 GetRandomPointOnMap() {
        float x = Random.value * (Random.value >= 0.5f ? 1 : -1);
        float y = Random.value * (Random.value >= 0.5f ? 1 : -1);
        Vector2 dir = new Vector2(x, y);

        return dir.normalized * mapSize;
    }
}
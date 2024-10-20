using UnityEngine;
using UnityEngine.AI;

public class _GameManager : MonoBehaviour {
    public _InputManager _Input;
    public Camera cam;

    public bool onCombat = false;

    public GameObject character;

    private void Update() {
        RotateWorld();

        if (_Input.K_State) {
            if (!onCombat) character.GetComponent<NavMeshAgent>().enabled = false;
            onCombat = !onCombat;
            character = null;
        }

        ProcessMovement();
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
    
    private void Start() {
        if (!_Input) _Input = GameObject.Find("UI").GetComponent<_InputManager>();
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
}
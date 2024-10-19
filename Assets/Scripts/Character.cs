using UnityEngine;

public class Character : MonoBehaviour {
    public _InputManager _Input;

    private void Start() {
        if (!_Input) _Input = GameObject.Find("UI").GetComponent<_InputManager>();
    }

    private void Update() {
        Move();
    }

    private void Move() {
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

        transform.Translate(move * 3 * Time.deltaTime, Space.Self);
    }
}

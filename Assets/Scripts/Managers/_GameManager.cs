
using UnityEngine;

public class _GameManager : MonoBehaviour {
    public _InputManager _Input;
    public Camera cam;

    private void Update() {
        if (_Input.D_State) {
            cam.transform.RotateAround(Vector3.zero, Vector3.up, _Input.D_Value * -.2f);
        } else if (_Input.A_State) {
            cam.transform.RotateAround(Vector3.zero, Vector3.up, _Input.A_Value * .2f);
        }

        /*if (_Input.W_State && cam.transform.rotation.eulerAngles.x <= 50) {
            Debug.Log(cam.transform.rotation.eulerAngles.x);
            cam.transform.RotateAround(Vector3.zero, Vector3.left, -_Input.W_Value);
        } else if(_Input.S_State && cam.transform.rotation.eulerAngles.x >= 10) {
            Debug.Log(cam.transform.rotation.eulerAngles.x);
            cam.transform.RotateAround(Vector3.zero, Vector3.left, _Input.S_Value);
        }*/
    }
}
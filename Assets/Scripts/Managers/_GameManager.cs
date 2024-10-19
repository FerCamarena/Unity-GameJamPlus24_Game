using UnityEngine;

public class _GameManager : MonoBehaviour {
    public _InputManager _Input;
    public Camera cam;

    public bool onCombat = false;

    private void Update() {
        RotateWorld();

        if (_Input.K_State) onCombat = !onCombat;
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
}
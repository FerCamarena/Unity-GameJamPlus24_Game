using UnityEngine;

public class _InputManager : MonoBehaviour {
    public bool D_State;
    public bool A_State;
    public bool W_State;
    public bool S_State;

    public float D_Value;
    public float A_Value;
    public float W_Value;
    public float S_Value;

    public float H_Mouse;
    public float V_Mouse;

    //TEMP
    public bool K_State;

    private void Update() {
        if(Input.GetAxis("Horizontal") > 0) {
            D_State = true;
            D_Value = Input.GetAxis("Horizontal");
        } else if (Input.GetAxis("Horizontal") < 0) {
            A_State = true;
            A_Value = Input.GetAxis("Horizontal") * -1;
        } else {
            D_State = Input.GetKey(KeyCode.D);
            A_State = Input.GetKey(KeyCode.A);

            if (Input.GetKey(KeyCode.D)) {
                D_Value = Mathf.Lerp(W_Value, 1, Time.deltaTime);
            } else if (Input.GetKey(KeyCode.A)) {
                A_Value = Mathf.Lerp(S_Value, 1, Time.deltaTime);
            }
        }

        if (Input.GetAxis("Vertical") > 0) {
            W_State = true;
            W_Value = Input.GetAxis("Vertical");
        } else if (Input.GetAxis("Vertical") < 0) {
            S_State = true;
            S_Value = Input.GetAxis("Vertical") * -1;
        } else {
            W_State = Input.GetKey(KeyCode.W);
            S_State = Input.GetKey(KeyCode.S);

            if (Input.GetKey(KeyCode.W)) {
                W_Value = Mathf.Lerp(W_Value, 1, Time.deltaTime);
            } else if (Input.GetKey(KeyCode.S)) {
                S_Value = Mathf.Lerp(S_Value, 1, Time.deltaTime);
            }
        }

        H_Mouse = Input.mousePosition.x;
        V_Mouse = Input.mousePosition.y;

        //TEMP
        K_State = Input.GetKeyDown(KeyCode.K);
    }
}
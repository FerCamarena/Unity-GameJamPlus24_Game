using UnityEngine;

public class _InputManager : MonoBehaviour {
    public bool D_State;
    public bool A_State;
    public bool W_State;
    public bool S_State;

    public bool Up_State;
    public bool Do_State;
    public bool Lf_State;
    public bool Rg_State;

    public float D_Value;
    public float A_Value;
    public float W_Value;
    public float S_Value;

    public float Up_Value;
    public float Do_Value;
    public float Lf_Value;
    public float Rg_Value;

    public float H_Mouse;
    public float V_Mouse;

    public bool Esc_State;

    //TEMP
    public bool K_State;

    private void Update() {
        if(Input.GetAxis("PanH") > 0) {
            Rg_Value = Input.GetAxis("PanH");
        } else if (Input.GetAxis("PanH") < 0) {
            Lf_Value = -Input.GetAxis("PanH");
        } else {
            Rg_Value = 0;
            Lf_Value = 0;
        }

        if (Input.GetAxis("PanV") > 0) {
            Up_Value = Input.GetAxis("PanV");
        } else if (Input.GetAxis("PanV") < 0) {
            Do_Value = -Input.GetAxis("PanV");
        } else {
            Up_Value = 0;
            Do_Value = 0;
        }
        
        if(Input.GetAxis("MovH") > 0) {
            D_Value = Input.GetAxis("MovH");
        } else if (Input.GetAxis("MovH") < 0) {
            A_Value = -Input.GetAxis("MovH");
        } else {
            D_Value = 0;
            A_Value = 0;
        }

        if (Input.GetAxis("MovV") > 0) {
            W_Value = Input.GetAxis("MovV");
        } else if (Input.GetAxis("MovV") < 0) {
            S_Value = -Input.GetAxis("MovV");
        } else {
            W_Value = 0;
            S_Value = 0;
        }

        Up_State = Input.GetAxis("PanV") > 0;
        Do_State = Input.GetAxis("PanV") < 0;
        Rg_State = Input.GetAxis("PanH") > 0;
        Lf_State = Input.GetAxis("PanH") < 0;

        W_State = Input.GetAxis("MovV") > 0;
        S_State = Input.GetAxis("MovV") < 0;
        D_State = Input.GetAxis("MovH") > 0;
        A_State = Input.GetAxis("MovH") < 0;

        H_Mouse = Input.mousePosition.x;
        V_Mouse = Input.mousePosition.y;

        Esc_State = (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7));

        //TEMP
        K_State = Input.GetKeyDown(KeyCode.K);
    }
}
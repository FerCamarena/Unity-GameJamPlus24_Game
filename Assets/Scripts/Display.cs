using Unity.AI.Navigation;
using UnityEngine;

public class Display : MonoBehaviour {
    public GameObject objectPrefab;
    public GameObject objectGenerated;
    public Transform levelParent;
    public _InputManager _Input;

    public NavMeshSurface navMesh;

    private void Start() {
        if (!_Input) _Input = GameObject.Find("UI").GetComponent<_InputManager>();
        if (!levelParent) levelParent = GameObject.Find("level").transform;
    }

    private void Update() {
        ToggleStage();
    }

    private void ToggleStage() {
        if (_Input.K_State && !objectGenerated) {
            objectGenerated = Instantiate(objectPrefab, transform.position, Quaternion.identity, levelParent);
            GetComponent<SpriteRenderer>().enabled = false;
            if (!navMesh) navMesh = GameObject.Find("navMesh3D").GetComponent<NavMeshSurface>();
            navMesh.BuildNavMesh();
        } else if (_Input.K_State &&objectGenerated) {
            Destroy(objectGenerated);
            objectGenerated = null;
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
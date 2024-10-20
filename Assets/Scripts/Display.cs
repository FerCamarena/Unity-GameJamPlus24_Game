using Unity.AI.Navigation;
using UnityEngine;

public class Display : MonoBehaviour {
    public GameObject objectPrefab;
    public GameObject objectGenerated;
    public Transform levelParent;
    public _InputManager _Input;

    public NavMeshSurface navMesh;
    
    //Method called once the object becomes active
    private void OnEnable() {
        InGameEvent.WaveStarted += ToggleStage;
        InGameEvent.WaveEnded += ToggleStage;
    }
    //Method called once the object becomes inactive
    private void OnDisable() {
        InGameEvent.WaveStarted -= ToggleStage;
        InGameEvent.WaveEnded -= ToggleStage;
    }

    private void Start() {
        if (!_Input) _Input = GameObject.Find("UI").GetComponent<_InputManager>();
        if (!levelParent) levelParent = GameObject.Find("level").transform;
    }

    private void Update() {
    }

    private void ToggleStage() {
        if (!objectGenerated) {
            objectGenerated = Instantiate(objectPrefab, transform.position, Quaternion.identity, levelParent);
            GetComponent<SpriteRenderer>().enabled = false;
            if (objectGenerated.TryGetComponent<Structure>(out Structure st)) st.displaySpawn = gameObject;
            if (objectGenerated.TryGetComponent<Ally>(out Ally a)) a.displaySpawn = gameObject;
            if (!navMesh) navMesh = GameObject.Find("navMesh3D").GetComponent<NavMeshSurface>();
            navMesh.BuildNavMesh();
        } else {
            Destroy(objectGenerated);
            objectGenerated = null;
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
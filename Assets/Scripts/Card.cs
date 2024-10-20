using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {
    public _InputManager _Input;
    public _GameManager _Game;
    public Canvas canvas;
    public Deck deck;
    public bool onDrag;
    public bool empty;

    public GameObject placeholder;
    public GameObject placeholderPrefab;
    public GameObject unitDisplayPrefab;
    public Transform level;
    public Vector3 placeholderPos;

    public NavMeshSurface navMesh;
    
    private void Start() {
        if (!level) level = GameObject.Find("level").transform;
        if (!_Input) _Input = GameObject.Find("UI").GetComponent<_InputManager>();
        if (!_Game) _Game = GameObject.Find("UI").GetComponent<_GameManager>();
    }

    private void Update() {
        if (placeholder) {
            placeholder.transform.position = placeholderPos;
        }
        SendRaycast(false);
    }

    private void SendRaycast(bool onlyCheck) {
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(_Input.H_Mouse, _Input.V_Mouse));
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) {
            if (hit.collider.tag == "Map" && !placeholder && !deck.displayed && !_Game.onCombat && onlyCheck) {
                CreatePlaceholder();
            }
            float newX = (hit.point.x) - (hit.point.x % 1);
            float newY = (hit.point.y) - (hit.point.y % 1);
            float newZ = (hit.point.z) - (hit.point.z % 1);

            placeholderPos = new Vector3(newX, newY + 0.05f, newZ);

            empty = hit.collider.gameObject.layer != 7 && hit.collider.gameObject.layer != 8;

            if (!empty || _Game.onCombat) placeholderPos = Vector3.down * 3;
        }
    }

    private void CreatePlaceholder() {
        placeholder = Instantiate(placeholderPrefab, level);
    }

    public void OnBeginDrag(PointerEventData eData) {
        deck.displayed = false;
        deck.cardSelected = this.gameObject;
        onDrag = true;
    }

    public void OnDrag(PointerEventData eData) {
        transform.position = (Vector2)transform.position + eData.delta;
        
        if (_Input.V_Mouse > Screen.height * 0.25f) {
            GetComponent<Image>().enabled = false;
            deck.displayed = false;
        } else if (_Input.V_Mouse < Screen.height * 0.1f) {
            deck.displayed = true;
            if (!GetComponent<Image>().enabled && !_Game.onCombat) GetComponent<Image>().enabled = true;
            if (placeholder) {
                Destroy(placeholder);
                placeholder = null;
            }
        }
        if (_Input.V_Mouse > Screen.height * 0.1f) {
            SendRaycast(true);
        }
    }

    public void OnEndDrag(PointerEventData eData) {
        deck.cardSelected = null;
        onDrag = false;
        if (!GetComponent<Image>().enabled && _Input.V_Mouse > Screen.height * 0.15f && !_Game.onCombat && empty) {
            CreateDisplay();
        } else if (!GetComponent<Image>().enabled) {
            GetComponent<Image>().enabled = true;
            if (!empty || _Game.onCombat) {
                Destroy(placeholder);
                placeholder = null;
            }
        }
    }

    private void CreateDisplay() {
        Destroy(placeholder);
        placeholder = null;
        Instantiate(unitDisplayPrefab, placeholderPos, Quaternion.identity, level);
        if (!navMesh) navMesh = GameObject.Find("navMesh2D").GetComponent<NavMeshSurface>();
        navMesh.BuildNavMesh();
        Destroy(gameObject);
    }
}
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {
    public _InputManager _Input;
    public Canvas canvas;
    public Deck deck;
    public bool onDrag;

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
        } else if (_Input.V_Mouse < Screen.height * 0.15f) {
            deck.displayed = true;
            if (!GetComponent<Image>().enabled) GetComponent<Image>().enabled = true;
        }
    }

    public void OnEndDrag(PointerEventData eData) {
        deck.cardSelected = null;
        if (!GetComponent<Image>().enabled && _Input.V_Mouse > Screen.height * 0.15f) {
            Destroy(gameObject);
        } else if (!GetComponent<Image>().enabled) GetComponent<Image>().enabled = true;
        onDrag = false;
    }
}
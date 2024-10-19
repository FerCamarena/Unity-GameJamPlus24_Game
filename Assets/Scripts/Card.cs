using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {
    public Camera cam;
    public Canvas canvas;
    public Deck deck;
    public bool onDrag;

    public void OnBeginDrag(PointerEventData eData) {
        transform.SetParent(canvas.transform);
        deck.cardSelected = this.gameObject;
        onDrag = true;
    }
    public void OnDrag(PointerEventData eData) {
        transform.Translate(eData.delta);
        
        if (transform.position.y > cam.pixelHeight / 5) {
            GetComponent<Image>().enabled = false;
            deck.HideDeck();
        } else if (transform.position.y < cam.pixelHeight / 20) {
            deck.ShowDeck();
            if (!GetComponent<Image>().enabled) GetComponent<Image>().enabled = true;
        }
    }

    public void OnEndDrag(PointerEventData eData) {
        transform.SetParent(deck.transform);
        deck.cardSelected = null;
        if (!GetComponent<Image>().enabled) {
            Destroy(gameObject);
        }
        onDrag = false;
    }
}
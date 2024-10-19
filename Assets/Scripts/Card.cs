using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {
    public Camera cam;
    public bool onDrag;

    public void OnBeginDrag(PointerEventData eData) {
        onDrag = true;
    }
    public void OnDrag(PointerEventData eData) {
        transform.Translate(eData.delta);
        
        if (transform.position.y > cam.pixelHeight / 5) {
            GetComponent<Image>().enabled = false;
        } else if (!GetComponent<Image>().enabled) {
            GetComponent<Image>().enabled = true;
        }
    }
    public void OnEndDrag(PointerEventData eData) {
        if (!GetComponent<Image>().enabled) {
            Destroy(gameObject);
        }
        onDrag = false;
    }
}
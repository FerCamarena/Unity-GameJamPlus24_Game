using UnityEngine;

public class Deck : MonoBehaviour {
    public GameObject cardSelected;
    public bool displayed;

    private void Update() {
        if (!cardSelected && !displayed) ShowDeck();

        if(displayed && transform.position.y < 0) { 
            float newY = Mathf.Lerp(transform.position.y, 0.0f, 10 * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, newY);
        } else if (!displayed && transform.position.y > -128) { 
            float newY = Mathf.Lerp(transform.position.y, -128.0f, 10* Time.deltaTime);
            transform.position = new Vector3(transform.position.x, newY);
        }
    }

    public void ShowDeck() {
        displayed = true;
    }

    public void HideDeck() {
        displayed = false;
    }
}
using UnityEngine;

public class Deck : MonoBehaviour {
    public GameObject cardSelected;
    public bool displayed;

    private void Start() {
    }

    private void Update() {
        if (!cardSelected && !displayed) displayed = true;

        if (displayed && transform.position.y < 0) { 
            float newY = Mathf.Lerp(transform.position.y, 0.0f, 10 * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, newY);
        } else if (!displayed && transform.position.y > Screen.height * -0.25f) { 
            float newY = Mathf.Lerp(transform.position.y, Screen.height * -0.225f, 10* Time.deltaTime);
            transform.position = new Vector3(transform.position.x, newY);
        }

        if (transform.childCount > 0) OrderCards();
    }

    private void OrderCards() {
        float startPos = 240 - (120 * (transform.childCount));
        for(int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild(i).TryGetComponent<Card>(out Card card)) { 
                if (!card.onDrag) {
                    float newX = startPos + (i * card.GetComponent<RectTransform>().rect.width * 1.05f);
                    float finalX = Mathf.Lerp(card.transform.localPosition.x, newX, Time.deltaTime * 8);
                    card.transform.localPosition = new Vector2(finalX, transform.position.y + 18);
                }
            }
        }
    }
}
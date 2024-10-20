using UnityEngine;

public class Deck : MonoBehaviour {
    public GameObject cardSelected;
    public bool displayed;

    public _GameManager _Game;
    
    private void Start() {
        if (!_Game) _Game = GameObject.Find("UI").GetComponent<_GameManager>();
    }

    private void Update() {
        if (_Game.onCombat) displayed = false;

        if (!cardSelected && !displayed && !_Game.onCombat) displayed = true;

        if (displayed && transform.position.y < 0 && !_Game.onCombat) { 
            float newY = Mathf.Lerp(transform.position.y, 0.0f, 10 * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, newY);
        } else if (!displayed && transform.position.y > Screen.height * -0.25f) { 
            float newY = Mathf.Lerp(transform.position.y, Screen.height * -0.225f, 10 * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, newY);
        }

        if (transform.childCount > 0) OrderCards();
    }

    private void OrderCards() {
        for(int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild(i).TryGetComponent<Card>(out Card card)) { 
                float startPos = (card.GetComponent<RectTransform>().rect.width / 2) - ((card.GetComponent<RectTransform>().rect.width / 2) * (transform.childCount));
                if (!card.onDrag) {
                    float newX = startPos + (i * card.GetComponent<RectTransform>().rect.width * 1.1f);
                    float finalX = Mathf.Lerp(card.transform.localPosition.x, newX, Time.deltaTime * 8);
                    card.transform.localPosition = new Vector2(finalX, transform.position.y + 18);
                }
            }
        }
    }
}
using UnityEngine;

public class Cell : MonoBehaviour {
    public enum States { Dead, Alive }

    private States _state;

    private GameObject _gameObject;

    public States State {
        set {
            _state = value;

            if (_state == States.Dead) {
                _gameObject.GetComponent<SpriteRenderer>().color = Color.black;
            } else if (_state == States.Alive) {
                _gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }

    /* * * * */

    private void Awake() {
        _gameObject = gameObject;
    }
}

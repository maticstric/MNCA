using UnityEngine;

public class ConwayGameOfLife : MonoBehaviour {
    [Header("Objects")]
    [SerializeField] private Board board;
    [Header("Stats")]
    [SerializeField] private float initialDensity;

    private int[] _currentState;
    private int[] _nextState;

    /* * * * */

    private void Awake() {
        _currentState = new int[board.Cols * board.Rows];
        _nextState = new int[board.Cols * board.Rows];
    }

    private void Start() {
        InitalizeStates();
    }

    private void OnValidate() {
        initialDensity = Mathf.Clamp(initialDensity, 0, 1);
    }

    private void Update() {
        for (int i = 0; i < board.Cols * board.Rows; i++) {
            (int x, int y) = board.GetXY(i);

            int sum = _currentState[board.GetIndex(x - 1, y - 1)] + _currentState[board.GetIndex(x - 1, y + 0)] +
                      _currentState[board.GetIndex(x - 1, y + 1)] + _currentState[board.GetIndex(x + 0, y - 1)] +
                      _currentState[board.GetIndex(x + 0, y + 1)] + _currentState[board.GetIndex(x + 1, y - 1)] +
                      _currentState[board.GetIndex(x + 1, y + 0)] + _currentState[board.GetIndex(x + 1, y + 1)];

            if (_currentState[i] == (int)Cell.States.Dead) {
                if (sum == 3) {
                    _nextState[i] = (int)Cell.States.Alive;
                }
            } else if (_currentState[i] == (int)Cell.States.Alive) {
                if (sum < 2 || sum > 3) {
                    _nextState[i] = (int)Cell.States.Dead;
                }
            }
        }

        for (int i = 0; i < board.Cols * board.Rows; i++) {
            _currentState[i] = _nextState[i];

            board.Cells[i].State = (Cell.States)_currentState[i];
        }
    }

    /* * * * */

    private void InitalizeStates() {
        for (int i = 0; i < board.Cols * board.Rows; i++) {
            _currentState[i] = (Random.value < initialDensity) ? 1 : 0;
            _nextState[i] = 0;
        }
    }
}

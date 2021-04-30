using System.Collections;
using UnityEngine;

public class LargerThanLife : MonoBehaviour {
    [Header("Objects")]
    [SerializeField] private Board board;
    [Header("Stats")]
    [SerializeField] private float initialDensity;
    [Space]
    [SerializeField] private int range;
    [SerializeField] private int b1;
    [SerializeField] private int b2;
    [SerializeField] private int a1;
    [SerializeField] private int a2;

    private int[] _currentState;
    private int[] _nextState;

    /* * * * */

    private void Awake() {
        _currentState = new int[board.Cols * board.Rows];
        _nextState = new int[board.Cols * board.Rows];

        InitalizeStates();
    }

    private void OnValidate() {
        initialDensity = Mathf.Clamp(initialDensity, 0, 1);
    }

    private void Update() {
        for (int i = 0; i < board.Cols * board.Rows; i++) {
            (int x, int y) = board.GetXY(i);

            int sum = 0;

            for (int ii = -range; ii <= range; ii++) {
                for (int jj = -range; jj <= range; jj++) {
                    sum += _currentState[board.GetIndex(x + ii, y + jj)];
                }
            }

            if (_currentState[i] == (int)Cell.States.Dead) {
                if (sum >= b1 && sum <= b2) {
                    _nextState[i] = (int)Cell.States.Alive;
                }
            } else if (_currentState[i] == (int)Cell.States.Alive) {
                if (sum < a1 || sum > a2) {
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
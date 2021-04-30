using UnityEngine;

public class Board : MonoBehaviour {
    [Header("Stats")]
    [SerializeField] private int cols;
    [SerializeField] private int rows;
    [Header("Objects")]
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Transform cellParent;
    [Space]
    [SerializeField] private Cell[] cells;

    public int Cols => cols;
    public int Rows => rows;
    public Cell[] Cells => cells;

    /* * * * */

    private void Awake() {
        cells = new Cell[cols * rows];

        InitalizeCells();
    }

    private void OnValidate() {
        cols = Mathf.Clamp(cols, 0, int.MaxValue);
        rows = Mathf.Clamp(rows, 0, int.MaxValue);
    }

    /* * * * */

    private void InitalizeCells() {
        for (int i = 0; i < cols * rows; i++) {
            (int x, int y) = GetXY(i);
            Vector3 position = new Vector3(x, y, 0);
            Quaternion rotation = Quaternion.identity;

            Cell cell = Instantiate(cellPrefab, position, rotation, cellParent).GetComponent<Cell>();

            cells[i] = cell;
        }
    }

    public (int, int) GetXY(int index) {
        int x = index % cols;
        int y = (int)Mathf.Floor((float)index / cols);

        return (x, y);
    }

    public int GetIndex(int x, int y) {
        // Wrap around for anything out of bounds                                     
        if (x < 0) {
            x += cols;
        }

        if (x > cols - 1) {
            x -= cols;
        }

        if (y < 0) {
            y += rows;
        }

        if (y > rows - 1) {
            y -= cols;
        }

        return x + y * cols;
    }
}

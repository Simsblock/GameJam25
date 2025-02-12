using UnityEngine;

[System.Serializable]
public struct Serializable2DArray<T>
{
    [SerializeField]
    private Row[] rows;

    [System.Serializable]
    public struct Row
    {
        [SerializeField]
        public T[] row;
    }

    //Methods
    public T GetValue(int rowIndex, int colIndex)
    {
        return rows[rowIndex].row[colIndex];
    }

    public void SetValue(int rowIndex, int colIndex, T value)
    {
        rows[rowIndex].row[colIndex] = value;
    }

    public int RowCount => rows.Length;
    public int ColCount => rows.Length > 0 ? rows[0].row.Length : 0;
}
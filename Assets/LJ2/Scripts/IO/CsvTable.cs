using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IO
{
    [System.Serializable]
    public class CsvTable : Csv
    {
        public string[,] Table { get; set; }

        public CsvTable(string path, char splitSymbol) : base(path, splitSymbol) { }

        /// <summary>
        /// Retrieves data from a specific cell in the table.
        /// ���̺��� Ư�� ���� �����͸� ������.
        /// </summary>
        /// <param name="row">Row index of the cell. ���� �� �ε���.</param>
        /// <param name="column">Column index of the cell. ���� �� �ε���.</param>
        /// <returns>The data at the specified cell. ������ ���� ������.</returns>
        public string GetData(int row, int column)
        {
            (int, int) rc = GetClampTableIndex(row, column);
            return Table[rc.Item1, rc.Item2];
        }

        /// <summary>
        /// Retrieves data from a specific cell using a Vector2Int for indexing.
        /// Vector2Int�� ����Ͽ� Ư�� ���� �����͸� ������.
        /// </summary>
        /// <param name="vector2">A Vector2Int representing the cell position. �� ��ġ�� ��Ÿ���� Vector2Int.</param>
        /// <returns>The data at the specified cell. ������ ���� ������.</returns>
        public string GetData(Vector2Int vector2)
        {
            (int, int) rc = GetClampTableIndex(vector2.y, vector2.x);
            return Table[rc.Item1, rc.Item2];
        }

        /// <summary>
        /// Retrieves all data from a specific row as a string array.
        /// Ư�� ���� ��� �����͸� ���ڿ� �迭�� ������.
        /// </summary>
        /// <param name="row">Row index to get data from. �����͸� ������ �� �ε���.</param>
        /// <returns>All data from the specified row as a string array. ������ ���� ��� �����͸� ���ڿ� �迭�� ��ȯ.</returns>
        public string[] GetLine(int row)
        {
            int columns = Table.GetLength(1);
            string[] line = new string[columns];

            for (int c = 0; c < columns; c++)
            {
                line[c] = Table[Mathf.Clamp(row, 0, Table.GetLength(0) - 1), c];
            }

            return line;
        }

        /// <summary>
        /// Clamps the table indices to ensure they are within the array bounds.
        /// ���̺� �ε����� Ŭ������ �迭 ���� ���� �����Ѵ�.
        /// </summary>
        /// <param name="r">Row index to clamp. Ŭ������ �� �ε���.</param>
        /// <param name="c">Column index to clamp. Ŭ������ �� �ε���.</param>
        /// <returns>A tuple containing the clamped row and column indices. Ŭ���ε� ��� �� �ε����� �����ϴ� Ʃ��.</returns>
        private (int, int) GetClampTableIndex(int r, int c)
        {
            return (Mathf.Clamp(r, 0, Table.GetLength(0) - 1),
                    Mathf.Clamp(c, 0, Table.GetLength(1) - 1));
        }

    }
}
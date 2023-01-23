using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Array<T>
{
    public List<T> cells = new List<T>();
    public T this[int index] => cells[index];
}

[System.Serializable]
public class Array2D<T>
{
    public List<Array<T>> arrays = new List<Array<T>>();
    public T this[int x, int y] => arrays[x][y];
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGraph
{
    bool HasPoint<T>(T point);
    IEnumerable<T> GetNeighbors<T>(T point);
}

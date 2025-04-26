using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IIdGenTarget
{
    string Id();
    string Name();
    bool IsExport();
}

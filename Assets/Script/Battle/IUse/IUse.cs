using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUse
{
    IUse Selected();
    bool Dragged(Vector2 mousePos, LineDrawer liner);

    void Use(int value);

    void SetTarget(GameObject input);

    int GetUseType();  /// 0: Card, 1: Item

    bool GetIsNonTarget();

    bool GetReady();

    int GetID();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem : IUse
{
    Sprite GetItemImg();

    string GetItemName();

    string GetItemText();

    void SetSlotIndex(int _index);

    int GetSlotIndex();

    void UnSetSelected();

    void SetBtnPos(Transform t_btn);
}

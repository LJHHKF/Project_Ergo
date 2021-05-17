using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem : IUse
{
    Sprite GetItemImg();

    string GetItemText();

    void SetSlotIndex(int _index);

    void DestroySelf();

    void UnSetSelected();

    void SetBtnPos(Transform t_btn);
}

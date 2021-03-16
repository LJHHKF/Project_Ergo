using UnityEngine;

public interface ICard
{

    //ICard Selected();
    void Holded();
    void Dragged(Vector2 mousePos, LineDrawer liner);

    void Use(int diceValue);

    void SetTarget(GameObject input);

    void SortingCard(int usedRP, int cntCards);

    bool GetIsNonTarget();

    void SetRenderPriority(int value);

    int GetRenderPriority();

    int GetCardID();

    //void GetCardUseInfo(out int fixP, out float flucPRate);

    //void SetUseFunc(System.Action<int> func);

    void DoTransparency();

    void UndoTransparency();
}

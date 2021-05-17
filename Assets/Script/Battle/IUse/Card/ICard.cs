using UnityEngine;

public interface ICard : IUse
{
    //bool Dragged(Vector2 mousePos, LineDrawer liner);
    //void Use(int value);
    //void SetTarget(GameObject input);
    //IUse Selected();
    void SortingCard(int usedRP, int cntCards);

    //bool GetIsNonTarget();

    void SetRenderPriority(int value);

    int GetRenderPriority();

    //bool GetReady();

    void BringUpCard(bool isSelected);

    void DoTransparency();

    void UndoTransparency();
}

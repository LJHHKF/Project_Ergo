using UnityEngine;

public interface ICard
{
    //ICard Selected();
    void Holded();
    void Dragged(Vector2 mousePos, LineDrawer liner);

    void Use(int diceValue);
    void UnUse();

    void SetTarget(GameObject input);

}

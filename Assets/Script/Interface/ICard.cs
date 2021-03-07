using UnityEngine;

public interface ICard
{
    ICard Selected();
    void Holded();
    void Dragged(Vector2 mousePos);

    void Use();
    void UnUse();

}

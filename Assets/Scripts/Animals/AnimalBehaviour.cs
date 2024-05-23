using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalBehaviour: MonoBehaviour
{
    public virtual void GetCollision() { }

    public virtual void MoveAnimal(Vector2 newPos) { }
    public virtual void CancelMoveAnimal() { }
}

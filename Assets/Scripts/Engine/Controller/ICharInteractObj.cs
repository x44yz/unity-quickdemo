using UnityEngine;

public interface ICharInteractObj
{
    Vector3 GetInteractPos();
    Quaternion GetInteractRot();
    void Interact(IInteractSource s);
}
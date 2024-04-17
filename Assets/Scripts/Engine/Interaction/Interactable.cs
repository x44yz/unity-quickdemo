using UnityEngine;

public class Interactable : MonoBehaviour, ICharInteractObj
{
    public Transform interactionLocation;
    public ConditionCollection[] conditionCollections = new ConditionCollection[0];
    public ReactionCollection defaultReactionCollection;

    public Vector3 GetInteractPos() => interactionLocation.position;
    public Quaternion GetInteractRot() => interactionLocation.rotation;

    public void Interact(IInteractSource s)
    {
        for (int i = 0; i < conditionCollections.Length; i++)
        {
            if (conditionCollections[i].CheckAndReact(s))
                return;
        }

        defaultReactionCollection.React(s);
    }
}

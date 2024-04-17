using UnityEngine;

public class ReactionCollection : MonoBehaviour
{
    public Reaction[] reactions = new Reaction[0];

    private void Start()
    {
        Interactable owenr = GetComponentInParent<Interactable>();

        for (int i = 0; i < reactions.Length; i++)
        {
            DelayedReaction delayedReaction = reactions[i] as DelayedReaction;

            if (delayedReaction)
                delayedReaction.Init(owenr);
            else
                reactions[i].Init(owenr);
        }
    }

    public void React(IInteractSource s)
    {
        for (int i = 0; i < reactions.Length; i++)
        {
            DelayedReaction delayedReaction = reactions[i] as DelayedReaction;

            if (delayedReaction)
                delayedReaction.React(this, s);
            else
                reactions[i].React(this, s);
        }
    }
}

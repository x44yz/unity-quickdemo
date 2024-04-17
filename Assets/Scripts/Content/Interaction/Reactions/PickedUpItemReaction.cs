
public class PickedUpItemReaction : DelayedReaction
{
    public Item item;
    private Inventory inventory;

    protected override void OnInit()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    protected override void OnReaction(IInteractSource s)
    {
        inventory.AddItem(item);
    }
}

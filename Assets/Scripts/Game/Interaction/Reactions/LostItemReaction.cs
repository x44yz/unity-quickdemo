
public class LostItemReaction : DelayedReaction
{
    public Item item;
    private Inventory inventory;

    protected override void OnInit()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    protected override void OnReaction(IInteractSource s)
    {
        inventory.RemoveItem(item);
    }
}

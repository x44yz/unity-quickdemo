
public class GUIPage : GUIWidget
{
    public bool showTopPage;

    public override void Show()
    {
        base.Show();

        if (showTopPage)
        {
            transform.SetAsLastSibling();
        }
    }
}
using UnityEngine;

public class TextReaction : Reaction
{
    public string message;
    public Color textColor = Color.white;
    public float delay;

    private TextManager textManager;

    protected override void OnInit()
    {
        textManager = FindObjectOfType<TextManager>();
    }

    protected override void OnReaction()
    {
        textManager.DisplayMessage(message, textColor, delay);
    }
}
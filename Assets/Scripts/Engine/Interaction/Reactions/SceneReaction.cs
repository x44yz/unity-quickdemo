
public class SceneReaction : Reaction
{
    public string sceneName;
    public string startingPointInLoadedScene;
    // public SaveData playerSaveData;

    private SceneController sceneController;

    protected override void OnInit()
    {
        sceneController = FindObjectOfType<SceneController>();
    }

    protected override void OnReaction(IInteractSource s)
    {
        // playerSaveData.Save (PlayerMovement.startingPositionKey, startingPointInLoadedScene);
        throw new System.NotImplementedException();

        sceneController.FadeAndLoadScene(this);
    }
}
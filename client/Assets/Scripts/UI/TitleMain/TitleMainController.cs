using UnityEngine;

public class TitleMainController : MonoBehaviour
{
    public SceneController SceneController;
    public string MainSceneName;
    public void OnClickedStartBtn()
    {
        SceneController.ChangeScene(MainSceneName);
    }
}

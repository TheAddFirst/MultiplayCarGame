using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour
{

    public void RelaodGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }
}

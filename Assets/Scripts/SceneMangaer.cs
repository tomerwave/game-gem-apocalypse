using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent (typeof(BoxCollider))]
public class SceneMangaer : MonoBehaviour
{

    private int managerIndex = 1;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            managerIndex++;
            SceneManager.LoadScene(managerIndex);
        }
    }

    public void NextSceneButton()
    {
        SceneManager.LoadScene(1);
    }

    public void RetryButton()
    {
        SceneManager.LoadScene(managerIndex);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}

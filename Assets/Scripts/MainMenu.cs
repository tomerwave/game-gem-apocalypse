using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject VolumeSlider;
    private static MainMenu instance;
    private float volume;
    private bool mute;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        mute=false;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplyVolumeToAllMusic();
    }

    public void ApplyVolumeToAllMusic()
    {
        AudioSource[] allMusic = FindObjectsOfType<AudioSource>();
        volume = mute?0:volume;
        
        foreach (var m in allMusic)
            m.gameObject.GetComponent<AudioSource>().volume = volume;
    }

    public void OnPlay()
    {
        SceneManager.LoadScene("Level1");
    }
    public void ChangeVolume()
    {
        volume = VolumeSlider.GetComponent<Slider>().value;
    }
    public void MutePressed()
    {
        mute = !mute;
        VolumeSlider.GetComponent<Slider>().interactable = !mute;
    }
    public void ExitPressed()
    {
        Application.Quit();
    }
}

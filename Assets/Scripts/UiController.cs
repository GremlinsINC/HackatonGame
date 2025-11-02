using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    public Transform menu;
    public static bool isPaused = false;

    private IReadOnlyList<AudioSource> allAudioSources;

    void Awake(){
        TogglePause();
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            menu.gameObject.SetActive(true);
            TogglePause();
        }
    }

    public void OnStartGame(){
        menu.gameObject.SetActive(false);
        TogglePause();
    }

    public void OnExitGame(){
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void TogglePause()
    {
        SceneCacheManager.Instance.RefreshCacheOfType<AudioSource>(); 
        allAudioSources = SceneCacheManager.Instance.GetCachedOfType<AudioSource>();
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            foreach(var audioSource in allAudioSources)
            {
                Debug.Log("AudioSource on pause");
                audioSource.Pause();
            }
        }
        else
        {
            Time.timeScale = 1f;
            foreach(var audioSource in allAudioSources)
            {
                audioSource.Play();
            }
        }
    }
}

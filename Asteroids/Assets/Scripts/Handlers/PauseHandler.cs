using UnityEngine;

public class PauseHandler : MonoBehaviour
{
    private bool _isPaused = false;

    [SerializeField] private GameObject _pauseMenu;

    public void Pause()
    {
        _isPaused = true;
        Time.timeScale = 0f;
        if (_pauseMenu != null)
            _pauseMenu.SetActive(_isPaused);

        AudioListener.pause = _isPaused;
    }

    public void Resume()
    {
        _isPaused = false;
        Time.timeScale = 1f;
        if (_pauseMenu != null)
            _pauseMenu.SetActive(_isPaused);

        AudioListener.pause = _isPaused;
    }
}

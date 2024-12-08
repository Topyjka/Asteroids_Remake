using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverScrene;

    private PauseHandler _pauseHandler;

    private void Awake()
    {
        _pauseHandler = gameObject.AddComponent<PauseHandler>();
    }

    private void OnEnable()
    {
        PlayerHealth.OnLivesDepleted += GameOver;
    }

    private void OnDisable()
    {
        PlayerHealth.OnLivesDepleted -= GameOver;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
        _pauseHandler.Resume();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        _pauseHandler.Resume();
    }

    public void GameOver()
    {
        _gameOverScrene.SetActive(true);
        _pauseHandler.Pause();
    }

    public void Exit()
    {
        Application.Quit();
    }
}

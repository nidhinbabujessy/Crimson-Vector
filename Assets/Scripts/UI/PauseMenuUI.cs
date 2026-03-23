namespace Game.UI
{
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using Game.Systems.SaveLoad;

    /// <summary>
    /// Handles Pausing the game, toggling the UI, and time scaling.
    /// </summary>
    public class PauseMenuUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _pausePanel;
        [SerializeField] private string _menuSceneName = "MainMenu";

        private bool _isPaused = false;

        private void Awake()
        {
            if (_pausePanel != null)
                _pausePanel.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_isPaused)
                    Resume();
                else
                    Pause();
            }
        }

        public void Resume()
        {
            _isPaused = false;
            if (_pausePanel != null)
                _pausePanel.SetActive(false);
            
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void Pause()
        {
            _isPaused = true;
            if (_pausePanel != null)
                _pausePanel.SetActive(true);
            
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void Restart()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void QuitToMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(_menuSceneName);
        }

        public void SaveGame()
        {
            if (SaveLoadManager.Instance != null)
            {
                SaveLoadManager.Instance.SaveGame();
            }
        }

        public void LoadGame()
        {
            if (SaveLoadManager.Instance != null)
            {
                SaveLoadManager.Instance.LoadGame();
                Resume(); // Resume game after loading to see results
            }
        }
    }
}

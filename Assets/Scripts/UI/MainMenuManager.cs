namespace Game.UI
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// Handles Main Menu actions, panel switching, and scene loading.
    /// </summary>
    public class MainMenuManager : MonoBehaviour
    {
        [Header("Menu Panels")]
       // [SerializeField] private GameObject _mainMenuPanel;
        [SerializeField] private GameObject _optionsPanel;

        [Header("Scene Settings")]
        [SerializeField] private string _gameSceneName = "GameScene";

        private void Start()
        {
            // Ensure the correct panel is visible on start
            ShowMainMenu();
            
            // Ensure time scale is reset and cursor is visible
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Cap Frame Rate to prevent high CPU/GPU usage in menus
            Application.targetFrameRate = 60;
        }

        public void PlayGame()
        {
            if (!string.IsNullOrEmpty(_gameSceneName))
            {
                SceneManager.LoadScene(_gameSceneName);
            }
            else
            {
                Debug.LogWarning("Game Scene Name is not set in MainMenuManager!");
            }
        }

        public void OpenOptions()
        {
           // if (_mainMenuPanel != null) _mainMenuPanel.SetActive(false);
            if (_optionsPanel != null) _optionsPanel.SetActive(true);
        }

        public void CloseOptions()
        {
            ShowMainMenu();
        }

        public void QuitGame()
        {
            Debug.Log("Quitting Game...");
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }

        private void ShowMainMenu()
        {
           // if (_mainMenuPanel != null) _mainMenuPanel.SetActive(true);
            if (_optionsPanel != null) _optionsPanel.SetActive(false);
        }
    }
}

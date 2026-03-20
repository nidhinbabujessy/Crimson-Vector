namespace Game.UI
{
    using UnityEngine;
    using Game.Core.Events;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// Handles showing the Game Over screen and scene restarting.
    /// </summary>
    public class GameOverUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _gameOverPanel;

        private void Awake()
        {
            if (_gameOverPanel != null)
                _gameOverPanel.SetActive(false);
        }

        private void OnEnable()
        {
            GameEvents.OnPlayerDied += ShowGameOver;
        }

        private void OnDisable()
        {
            GameEvents.OnPlayerDied -= ShowGameOver;
        }

        private void ShowGameOver()
        {
            if (_gameOverPanel != null)
            {
                _gameOverPanel.SetActive(true);
                // Set timescale to 0 or leave it for cinematic death?
                // Time.timeScale = 0f;
            }
        }

        /// <summary>
        /// Called from UI Button.
        /// </summary>
        public void RestartGame()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        /// <summary>
        /// Called from UI Button.
        /// </summary>
        public void QuitToMenu()
        {
            Time.timeScale = 1f;
            // SceneManager.LoadScene("MenuScene");
            Application.Quit();
        }
    }
}

namespace Game.UI
{
    using UnityEngine;
    using Game.Core.Events;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// Handles showing the Victory screen when the boss is defeated.
    /// </summary>
    public class VictoryUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _victoryPanel;
        [SerializeField] private string _menuSceneName = "MenuScene";

        private void Awake()
        {
            if (_victoryPanel != null)
                _victoryPanel.SetActive(false);
        }

        private void OnEnable()
        {
            GameEvents.OnBossDied += ShowVictory;
        }

        private void OnDisable()
        {
            GameEvents.OnBossDied -= ShowVictory;
        }

        private void ShowVictory()
        {
            if (_victoryPanel != null)
            {
                _victoryPanel.SetActive(true);
                // Pause game on victory
                Time.timeScale = 0f;
            }
        }

        public void RestartGame()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void QuitToMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(_menuSceneName);
        }
    }
}

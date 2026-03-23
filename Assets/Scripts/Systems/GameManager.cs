namespace Game.Systems
{
    using UnityEngine;
    using Game.Core.Events;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// Central manager for game state and victory/defeat logic.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public enum GameState { Playing, Won, Lost }
        public GameState CurrentState { get; private set; } = GameState.Playing;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnEnable()
        {
            GameEvents.OnPlayerDied += HandlePlayerDied;
            GameEvents.OnBossDied += HandleBossDied;
        }

        private void OnDisable()
        {
            GameEvents.OnPlayerDied -= HandlePlayerDied;
            GameEvents.OnBossDied -= HandleBossDied;
        }

        private void HandlePlayerDied()
        {
            if (CurrentState != GameState.Playing) return;
            CurrentState = GameState.Lost;
            Debug.Log("[GameManager] Player Died. Game Over.");
        }

        private void HandleBossDied()
        {
            if (CurrentState != GameState.Playing) return;
            CurrentState = GameState.Won;
            Debug.Log("[GameManager] Boss Died. Victory!");
        }

        public void RestartLevel()
        {
            CurrentState = GameState.Playing;
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

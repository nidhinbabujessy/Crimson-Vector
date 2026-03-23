namespace Game.Systems.Audio
{
    using UnityEngine;
    using Game.Core.Events;

    /// <summary>
    /// Updated SoundManager to handle BGM and Footsteps.
    /// </summary>
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }

        [Header("BGM")]
        public AudioClip bgmClip;
        [Range(0f, 1f)] public float bgmVolume = 0.3f;

        [Header("Player Sounds")]
        public AudioClip playerAttack;
        public AudioClip playerDash;
        public AudioClip playerDamaged;
        public AudioClip playerDeath;
        public AudioClip playerHealed;
        public AudioClip playerSpeedBuff;
        public AudioClip footstepClip;

        [Header("Enemy Sounds")]
        public AudioClip enemyAttack;
        public AudioClip enemyDeath;
        public AudioClip bossDied;
        public AudioClip bossDash;
        public AudioClip bossAreaAttack;
        public AudioClip bossPhaseTransition;

        [Header("UI & Items")]
        public AudioClip itemPickup;
        public AudioClip uiClick;

        private AudioSource _sfxSource;
        private AudioSource _bgmSource;
        private AudioSource _footstepSource;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                
                _sfxSource = gameObject.AddComponent<AudioSource>();
                
                _bgmSource = gameObject.AddComponent<AudioSource>();
                _bgmSource.loop = true;
                _bgmSource.playOnAwake = false;

                _footstepSource = gameObject.AddComponent<AudioSource>();
                _footstepSource.loop = true;
                _footstepSource.playOnAwake = false;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            if (bgmClip != null)
            {
                _bgmSource.clip = bgmClip;
                _bgmSource.volume = bgmVolume;
                _bgmSource.Play();
            }
        }

        private void OnEnable()
        {
            // Player
            GameEvents.OnPlayerAttack += () => PlayOneShot(playerAttack);
            GameEvents.OnPlayerDash += () => PlayOneShot(playerDash);
            GameEvents.OnPlayerDamaged += (c, m) => PlayOneShot(playerDamaged);
            GameEvents.OnPlayerDied += () => { PlayOneShot(playerDeath); ToggleFootsteps(false); };
            GameEvents.OnPlayerHealed += (a) => PlayOneShot(playerHealed);
            GameEvents.OnPlayerSpeedBuffed += (m, d) => PlayOneShot(playerSpeedBuff);
            GameEvents.OnPlayerMoveChange += ToggleFootsteps;

            // Enemy
            GameEvents.OnEnemyAttack += () => PlayOneShot(enemyAttack);
            GameEvents.OnEnemyDeath += () => PlayOneShot(enemyDeath);
            GameEvents.OnBossDied += () => PlayOneShot(bossDied);
            GameEvents.OnBossDash += () => PlayOneShot(bossDash);
            GameEvents.OnBossAreaAttack += () => PlayOneShot(bossAreaAttack);
            GameEvents.OnBossPhaseTransition += () => PlayOneShot(bossPhaseTransition);
            
            // Misc
            GameEvents.OnItemPickedUp += (i) => PlayOneShot(itemPickup);
            GameEvents.OnUIClick += () => PlayOneShot(uiClick);
        }

        private void PlayOneShot(AudioClip clip)
        {
            if (clip != null) _sfxSource.PlayOneShot(clip);
        }

        private void ToggleFootsteps(bool isMoving)
        {
            if (footstepClip == null) return;

            if (isMoving && !_footstepSource.isPlaying)
            {
                _footstepSource.clip = footstepClip;
                _footstepSource.Play();
            }
            else if (!isMoving && _footstepSource.isPlaying)
            {
                _footstepSource.Stop();
            }
        }

        public void PlayUIClick() => PlayOneShot(uiClick);
    }
}

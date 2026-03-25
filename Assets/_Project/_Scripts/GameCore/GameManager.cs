using TMPro;
using UnityEngine;

namespace GameCore
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public int CurrentScore { get; private set; } = 0;
        public int BestScore { get; private set; } = 0;
        public int CurrentDetails { get; private set; } = 0;
        public bool IsGameActive { get; private set; } = true;

        [SerializeField] private View.UI.UIScreen _winScreen;
        [SerializeField] private Misc.SceneManagment.SceneLoader _sceneLoader;
        [SerializeField] private Objects.ObstacleSpawner _spawner;
        [SerializeField] private PlayerController _player;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _detailsText;

        private readonly float ScorePerSecond = 1f;
        private float _scoreAccumulator = 0f;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                LoadData();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            Time.timeScale = 1.0f;
            ResetSession();
        }

        private void Update()
        {
            if (!IsGameActive)
                return;

            _scoreAccumulator += ScorePerSecond * Time.deltaTime;
            if (_scoreAccumulator >= 1f)
            {
                int pointsToAdd = Mathf.FloorToInt(_scoreAccumulator);
                CurrentScore += pointsToAdd;
                _scoreAccumulator -= pointsToAdd;
                UpdateScoreUI();
            }
        }

        public void AddDetails(int amount = 1)
        {
            CurrentDetails += amount;
            UpdateScoreUI();
        }

        public void RestartGame()
        {
            IsGameActive = false;
            Time.timeScale = 1.0f;
            _player.ResetState();

            foreach (var pool in _spawner.ObstaclePools)
                pool.ResetPool();
            foreach (var pool in _spawner.DetailPools)
                pool.ResetPool();

            _sceneLoader.ChangeScene(Misc.Data.SceneConstants.GAME_SCENE);
        }

        public void BackToMenu()
        {
            IsGameActive = false;
            Time.timeScale = 1.0f;
            _player.ResetState();

            foreach (var pool in _spawner.ObstaclePools)
                pool.ResetPool();
            foreach (var pool in _spawner.DetailPools)
                pool.ResetPool();

            _sceneLoader.ChangeScene(Misc.Data.SceneConstants.MENU_SCENE);
        }

        public void FinishGame()
        {
            if (!IsGameActive) return;

            IsGameActive = false;
            Time.timeScale = 0.0f;

            if (CurrentScore > BestScore)
                BestScore = CurrentScore;

            SaveData();

            _winScreen.StartScreen();
            UpdateScoreUI();
        }

        private void ResetSession()
        {
            CurrentScore = 0;
            _scoreAccumulator = 0f;
            IsGameActive = true;
            UpdateScoreUI();

            if (_player != null)
            {
                _player.StartSession();
            }
        }

        private void UpdateScoreUI()
        {
            _scoreText.text = $"Score: {CurrentScore}";
            _detailsText.text = $"{CurrentDetails}";
        }

        private void SaveData()
        {
            PlayerPrefs.SetInt(GameConstants.BEST_SCORE_KEY, BestScore);
            PlayerPrefs.SetInt(GameConstants.TOTAL_DETAILS_KEY, CurrentDetails);
            PlayerPrefs.Save();
        }

        private void LoadData()
        {
            BestScore = PlayerPrefs.GetInt(GameConstants.BEST_SCORE_KEY, 0);
            CurrentDetails = PlayerPrefs.GetInt(GameConstants.TOTAL_DETAILS_KEY, 0);
        }
    }
}
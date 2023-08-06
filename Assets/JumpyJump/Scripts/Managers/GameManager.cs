using UnityEngine;

namespace JumpyJump
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public static event System.Action OnScoreUp;

        // SCORE & BEST
        private int _score;
        private int _bestScore;


        #region Properties
        public int Score { get => _score; }
        public int BestScore { get => _bestScore; }
        #endregion
        private void Awake()
        {
            // Check if an instance already exists, and destroy the duplicate
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            // FPS
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            // Make the GameObject persist across scenes
            DontDestroyOnLoad(this.gameObject);
        }


        public void ScoreUp()
        {
            _score++;
            OnScoreUp?.Invoke();
        }

        public void ResetScore()
        {
            this._score = 0; 
        }

        public void SetBestScore(int score)
        {
            this._score = score;
            if (_bestScore < score)
            {
                _bestScore = score;
            }
        }
    }
}

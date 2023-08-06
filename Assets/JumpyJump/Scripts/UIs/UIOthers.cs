using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace JumpyJump
{
    public class UIOthers : CustomCanvas
    {
        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI _scoreText;

        private void OnEnable()
        {
            GameManager.OnScoreUp += UpdateScore;
        }

        private void OnDisable()
        {
            GameManager.OnScoreUp -= UpdateScore;
        }

        private void Start()
        {
            UpdateScore();
        }

        private void UpdateScore()
        {
            _scoreText.text = GameManager.Instance.Score.ToString();
        }
        
    }
}

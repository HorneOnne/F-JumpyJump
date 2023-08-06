using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace JumpyJump
{
    public class UIMainMenu : CustomCanvas
    {
        [Header("Buttons")]
        [SerializeField] private Button _playBtn;
        [SerializeField] private Button _musicBtn;
        [SerializeField] private Button _soundFXBtn;

        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI _scoreRecordText;

        [Header("Sprites")]
        [SerializeField] private Sprite _muteBtnSprite;
        [SerializeField] private Sprite _unmuteBtnSprite;


    
        private void Start()
        {
            LoadBest();
            UpdateMusicUI();
            UpdateSoundFXUI();

            _playBtn.onClick.AddListener(() =>
            {
                Loader.Load(Loader.Scene.GameplayScene);
                SoundManager.Instance.PlaySound(SoundType.Button, false);
            });

            _musicBtn.onClick.AddListener(() =>
            {
                ToggleMusic();
                SoundManager.Instance.PlaySound(SoundType.Button, false);
            });

            _soundFXBtn.onClick.AddListener(() =>
            {
                ToggleSFX();
                SoundManager.Instance.PlaySound(SoundType.Button, false);
            });
        }

        private void OnDestroy()
        {
            _playBtn.onClick.RemoveAllListeners();
            _musicBtn.onClick.RemoveAllListeners();
            _soundFXBtn.onClick.RemoveAllListeners();
        }

        private void LoadBest()
        {
            GameManager.Instance.SetBestScore(GameManager.Instance.Score);
            _scoreRecordText.text = $"{GameManager.Instance.BestScore}";
        }

        private void ToggleSFX()
        {

            SoundManager.Instance.MuteSoundFX(SoundManager.Instance.isSoundFXActive);
            SoundManager.Instance.isSoundFXActive = !SoundManager.Instance.isSoundFXActive;


            UpdateSoundFXUI();
        }

        private void UpdateSoundFXUI()
        {
            if (SoundManager.Instance.isSoundFXActive)
            {
                _soundFXBtn.image.sprite = _unmuteBtnSprite;
            }
            else
            {
                _soundFXBtn.image.sprite = _muteBtnSprite;
            }
        }

        private void ToggleMusic()
        {
            SoundManager.Instance.MuteBackground(SoundManager.Instance.isMusicActive);
            SoundManager.Instance.isMusicActive = !SoundManager.Instance.isMusicActive;


            UpdateMusicUI();
        }

        private void UpdateMusicUI()
        {
            if (SoundManager.Instance.isMusicActive)
            {
                _musicBtn.image.sprite = _unmuteBtnSprite;
            }
            else
            {
                _musicBtn.image.sprite = _muteBtnSprite;
            }
        }
    }
}

using TMPro;
using UnityEngine;

namespace JumpyJump
{
    public class TextWriter : MonoBehaviour
    {
        public static TextWriter Instance { get; private set; }
        public static event System.Action OnEntireTextDisplayed;

        private TextMeshPro _uiText;
        private string _textToWrite;
        private int _characterIndex;
        private float _timePerCharacter;
        private float _timer;


        private void Awake()
        {
            Instance = this;    
        }

        public void AddWriter(TextMeshPro uiText, string textToWrite, float timePerCharacter)
        {
            this._uiText = uiText;
            this._textToWrite= textToWrite;
            this._timePerCharacter= timePerCharacter;
            _characterIndex = 0;
        }

        private void Update()
        {
            if (GameplayManager.Instance.CurrentState != GameplayManager.GameState.GAMEOVER)
                return;

            if(_uiText != null)
            {
                _timer -= Time.deltaTime;
                if(_timer <= 0f)
                {
                    _timer += _timePerCharacter;
                    _characterIndex++;
                    _uiText.text = _textToWrite.Substring(0, _characterIndex);

                    if(_characterIndex >= _textToWrite.Length)
                    {
                        _uiText = null;
                        OnEntireTextDisplayed?.Invoke();
                        return;
                    }

                    SoundManager.Instance.PlaySound(SoundType.Button, false);
                }
            }
        }
    }
}

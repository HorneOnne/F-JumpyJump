using UnityEngine;
using UnityEngine.UI;

namespace JumpyJump
{
    public class UIGameover : CustomCanvas
    {
        [Header("Buttons")]
        [SerializeField] private Button _playBtn;
        [SerializeField] private Button _menuBtn;

        private void Start()
        {
            _playBtn.onClick.AddListener(() =>
            {
                Loader.Load(Loader.Scene.GameplayScene);
                SoundManager.Instance.PlaySound(SoundType.Button, false);
            });

            _menuBtn.onClick.AddListener(() =>
            {
                Loader.Load(Loader.Scene.MenuScene);
                SoundManager.Instance.PlaySound(SoundType.Button, false);
            });
        }

        private void OnDestroy()
        {
            _playBtn.onClick.RemoveAllListeners();
            _menuBtn.onClick.RemoveAllListeners();
        }
    }
}

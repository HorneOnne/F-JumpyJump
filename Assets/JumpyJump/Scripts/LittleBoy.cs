using TMPro;
using UnityEngine;

namespace JumpyJump
{
    public class LittleBoy : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private Animator _anim;
        private bool _facingLeft = true;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private LayerMask _wallLayer;
        [SerializeField] private LayerMask _characterLayer;

        // Cached
        private GameplayManager _gameplayManager;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _anim = GetComponent<Animator>();
        }


        private void Start()
        {
            _gameplayManager = GameplayManager.Instance;
        }
        private void OnEnable()
        {
            GameplayManager.OnGameOver += SetGameoverState;
        }

        private void OnDisable()
        {
            GameplayManager.OnGameOver -= SetGameoverState;
        }


        private void FixedUpdate()
        {
            if (_gameplayManager.CurrentState == GameplayManager.GameState.PLAYING)
            {
                if (_facingLeft)
                    _rb.velocity = new Vector2(-1.0f * _moveSpeed, _rb.velocity.y);
                else
                    _rb.velocity = new Vector2(1.0f * _moveSpeed, _rb.velocity.y);
            }
        }

        public void SetMoveSpeed(float speed)
        {
            this._moveSpeed = speed;
        }

        public void Flip()
        {
            _facingLeft = !_facingLeft;
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_gameplayManager.CurrentState != GameplayManager.GameState.PLAYING) return;

            if (_wallLayer == (_wallLayer | (1 << collision.gameObject.layer)))
            {
                Destroy(this.gameObject);
                GameController.Instance.ClearLittleBoy();
            }

            if (_characterLayer == (_characterLayer | (1 << collision.gameObject.layer)))
            {
                var forceDir = transform.position - collision.transform.position;
                _rb.AddForce(forceDir.normalized * 20f, ForceMode2D.Impulse);
            }
        }

        private void SetGameoverState()
        {
            _anim.SetTrigger("Gameover");
        }

    }
}

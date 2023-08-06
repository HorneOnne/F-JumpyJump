using System;
using UnityEngine;
using TMPro;

namespace JumpyJump
{
    public class Bigman : MonoBehaviour
    {
        private Animator _anim;
        private Rigidbody2D _rb;
        private bool _facingRight = true;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _jumpForce;
        [SerializeField] private LayerMask _characterLayer;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private LayerMask _wallLayer;
        [SerializeField] private LayerMask _scoreTriggerLayer;
        [SerializeField] private Transform _groundCheckPoint;

        [Space(10)]
        public TextMeshPro _messageText;

        // Cached
        private GameplayManager _gameplayManager;
        private bool _isScored = false;

        #region Properties
        public bool FacingRight { get => _facingRight; }
        #endregion
        private void OnEnable()
        {
            GameplayManager.OnGameOver += SetGameoverState;
        }

        private void OnDisable()
        {
            GameplayManager.OnGameOver -= SetGameoverState;
        }


        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _anim = GetComponent<Animator>();
            _gameplayManager = GameplayManager.Instance;         
        }

        private void FixedUpdate()
        {
            if(_gameplayManager.CurrentState == GameplayManager.GameState.PLAYING)
            {
                if (_facingRight)
                    _rb.velocity = new Vector2(1.0f * _moveSpeed, _rb.velocity.y);
                else
                    _rb.velocity = new Vector2(-1.0f * _moveSpeed, _rb.velocity.y);
            }        
        }

        public void SetMoveSpeed(float speed)
        {
            this._moveSpeed = speed;
        }    

        public void Flip()
        {
            _facingRight = !_facingRight;
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
        }

        public void Jump()
        {
            if(IsGrounded())
            {
                _anim.SetTrigger("Jump");
                _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);

                SoundManager.Instance.PlaySound(SoundType.Jump, false);
            }     
        }

        private bool IsGrounded()
        {
            // Check if the character is grounded
            return Physics2D.OverlapCircle(_groundCheckPoint.position, 0.3f, _groundLayer);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_gameplayManager.CurrentState != GameplayManager.GameState.PLAYING) return;

            if (_characterLayer == (_characterLayer | (1 << collision.gameObject.layer)))
            {
                GameplayManager.Instance.ChangeGameState(GameplayManager.GameState.GAMEOVER);
                SoundManager.Instance.PlaySound(SoundType.Collided, false);
            }

            if (_wallLayer == (_wallLayer | (1 << collision.gameObject.layer)))
            {
                Destroy(this.gameObject);
                GameController.Instance.ClearBigman();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {         
            if (_scoreTriggerLayer == (_scoreTriggerLayer | (1 << collision.gameObject.layer)))
            {
                if (_isScored) return;
                _isScored = true;
                StartCoroutine(Utilities.WaitAfter(0.25f, () =>
                {
                    if (_gameplayManager.CurrentState != GameplayManager.GameState.PLAYING) return;                    
                    GameManager.Instance.ScoreUp();
                }));             
            }
        }

        private void SetGameoverState()
        {
            _anim.SetTrigger("Gameover");
            StartCoroutine(Utilities.WaitAfter(0.5f, () =>
            {
                if(transform.localScale.x < 0)
                {
                    Vector3 newScale = _messageText.transform.localScale;
                    newScale.x *= -1;
                    _messageText.transform.localScale = newScale;
                }
                TextWriter.Instance.AddWriter(_messageText, "OOOPS..", 0.25f);             
            }));   
        }
    }
}

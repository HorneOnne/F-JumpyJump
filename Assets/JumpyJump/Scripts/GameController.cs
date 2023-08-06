using UnityEngine;

namespace JumpyJump
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }
        public static event System.Action OnNextRound;

        [Header("Spawn")]
        [SerializeField] private Bigman _bigmanPrefab;
        [SerializeField] private LittleBoy _littleBoyPrefab;
        [SerializeField] private float _xOffset;
        [SerializeField] private float _yOffset;

        [Header("Controller")]
        [SerializeField] private float _minSpeed;
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _currentSpeed;
        private float _timeToReachMaxSpeed = 600f;    // 10 minutes
        private float _elapsedTime;


        // Cached
        [SerializeField] private Bigman _bigman;
        [SerializeField] private LittleBoy _littleBoy;
        private Vector2 _centerPoint = Vector2.zero;
        private Vector2 _leftInitPosition;
        private Vector2 _rightInitPosition;



        private void Awake()
        {
            Instance = this;
            _currentSpeed = _minSpeed;
        }

        private void OnEnable()
        {
            OnNextRound += RandomSpawn;
            GameplayManager.OnGameOver += FacingTowardLittleBoy;
        }

        private void OnDisable()
        {
            OnNextRound -= RandomSpawn;
            GameplayManager.OnGameOver -= FacingTowardLittleBoy;
        }


        private void Start()
        {
            _leftInitPosition = _centerPoint + new Vector2(-_xOffset, _yOffset);
            _rightInitPosition = _centerPoint + new Vector2(_xOffset, _yOffset);

            RandomSpawn();
        }

        private void FixedUpdate()
        {
            _elapsedTime += Time.fixedDeltaTime;
            _currentSpeed = CalculateSpeedIncrease(_elapsedTime);
        }


        public void JumpLogicHandle()
        {
            if(GameplayManager.Instance.CurrentState == GameplayManager.GameState.PLAYING)
            {
                if (_bigman != null && _littleBoy != null)
                {
                    _bigman.Jump();
                }
            }          
        }

        private void RandomSpawn()
        {
            if (CanNextRound() == false) return;

            int rate = Random.Range(0, 2);
            switch (rate)
            {
                default: break;
                case 0:
                    SpawnBigman(_leftInitPosition).SetMoveSpeed(_currentSpeed);
                    SpawnLittleBoy(_rightInitPosition).SetMoveSpeed(_currentSpeed);
                    break;
                case 1:
                    SpawnBigman(_rightInitPosition).Flip();
                    SpawnLittleBoy(_leftInitPosition).Flip();

                    _bigman.SetMoveSpeed(_currentSpeed);
                    _littleBoy.SetMoveSpeed(_currentSpeed);
                    break;
            }
        }

        private bool CanNextRound()
        {
            if (_bigman != null || _littleBoy != null)
                return false;
            else
                return true;
        }

        private Bigman SpawnBigman(Vector2 position)
        {
            _bigman = Instantiate(_bigmanPrefab, position, Quaternion.identity);
            return _bigman;
        }

        private LittleBoy SpawnLittleBoy(Vector2 position)
        {
            _littleBoy = Instantiate(_littleBoyPrefab, position, Quaternion.identity);
            return _littleBoy;
        }


        public void ClearBigman()
        {
            _bigman = null;
            OnNextRound?.Invoke();
        }

        public void ClearLittleBoy()
        {
            _littleBoy = null;
            OnNextRound?.Invoke();
        }

        private void FacingTowardLittleBoy()
        {
            if (_bigman == null || _littleBoy == null) return;
            StartCoroutine(Utilities.WaitAfter(0.5f, () =>
            {              
                if (_bigman.transform.position.x > _littleBoy.transform.position.x)
                {
                    // Right
                    if (_bigman.FacingRight == true)
                        _bigman.Flip();
                }
                else
                {
                    // Left
                    if (_bigman.FacingRight == false)
                        _bigman.Flip();
                }
            }));
        }

        #region difficulty
        private float CalculateSpeedIncrease(float elapsedTime)
        {
            // Calculate the percentage of time elapsed
            float timePercentage = Mathf.Clamp01(elapsedTime / _timeToReachMaxSpeed);

            // Use the percentage to calculate the current speed within the range
            float currentSpeed = Mathf.Lerp(_minSpeed, _maxSpeed, timePercentage);

            return currentSpeed;
        }
        #endregion
    }
}

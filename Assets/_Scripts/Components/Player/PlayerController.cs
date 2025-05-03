using Unity.Cinemachine;
using UnityEngine;

namespace _Scripts.Components.Player
{
    /// <summary>
    /// Controller used for control Player model on the map
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        private float _baseSpeed = 1.8f;
        private Rigidbody2D _rbody;
        private Animator _animator;
        private Vector2 _movement;
        private CinemachineCamera _cinemaCamera;
  
        [SerializeField] private float runMultiply = 1.5f;
        private const string HorizontalMovingValue = "HorizontalValue";
        private const string VerticalMovingValue = "VerticleValue";
        private const string IsWalking = "IsWalking";
        private const string IsRunning = "IsRunning";

        private void Awake()
        {
            _cinemaCamera = FindFirstObjectByType<CinemachineCamera>();
            _cinemaCamera.Follow = transform;
        }
        private void Start()
        {
            _rbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }
        private void Update()
        {
            _movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        private void FixedUpdate()
        {
            MovementCheck();
        }

 

        private void MovementCheck()
        {
            bool isMoving = _movement != Vector2.zero;

            _animator.SetBool(IsWalking, isMoving);
            //movement check
            if (!isMoving)
            {
                _animator.SetBool(IsWalking, false);
                _animator.SetBool(IsRunning, false);
                if (_rbody.linearVelocity != Vector2.zero)
                    _rbody.linearVelocity = Vector2.zero;
                return;
            }
            //normalize vector
            if (_movement.magnitude > 1)
                _movement.Normalize();

            float speed = _baseSpeed;
            //run check
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _animator.SetBool(IsRunning, true);
                speed *= runMultiply;
            }
            else
            {
                _animator.SetBool(IsRunning, false);
            }
            //animations
            _animator.SetFloat(HorizontalMovingValue, _movement.x);
            _animator.SetFloat(VerticalMovingValue, _movement.y);

            _rbody.MovePosition(_rbody.position + speed * Time.fixedDeltaTime * _movement);
        }
    }
}



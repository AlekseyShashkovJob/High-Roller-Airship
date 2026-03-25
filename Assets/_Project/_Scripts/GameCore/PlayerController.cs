using UnityEngine;
using UnityEngine.InputSystem;

namespace GameCore
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float liftForce = 300f;
        [SerializeField] private float gravity = 400f;

        private Rigidbody2D _rb;
        private bool _isAlive = true;
        private bool _tap;

        private InputAction _tapAction;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();

            _tapAction = new InputAction(
                name: "Tap",
                type: InputActionType.Button,
                binding: "<Pointer>/press"
            );
        }

        private void OnEnable() => _tapAction.Enable();
        private void OnDisable() => _tapAction.Disable();

        private void Update()
        {
            _tap = _tapAction.IsPressed();
        }

        private void FixedUpdate()
        {
            if (!_isAlive || _rb.bodyType != RigidbodyType2D.Dynamic) return;

            if (_tap)
            {
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, liftForce);
            }
            else
            {
                _rb.linearVelocity += Vector2.down * gravity * Time.fixedDeltaTime;
            }
        }

        public void ResetState()
        {
            _isAlive = true;
            _rb.bodyType = RigidbodyType2D.Static;
            _rb.gravityScale = 0.0f;
            _rb.linearVelocity = Vector2.zero;
        }

        public void StartSession()
        {
            _isAlive = true;
            _rb.bodyType = RigidbodyType2D.Dynamic;
            _rb.gravityScale = 1.0f;
            _rb.linearVelocity = Vector2.zero;
        }

        public void Die()
        {
            if (!_isAlive) return;

            _isAlive = false;
            _rb.linearVelocity = Vector2.zero;
            _rb.gravityScale = 0.0f;
            _rb.bodyType = RigidbodyType2D.Static;

            GameManager.Instance.FinishGame();
        }
    }
}
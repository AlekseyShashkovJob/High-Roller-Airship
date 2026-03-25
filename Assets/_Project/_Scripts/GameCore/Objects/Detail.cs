using UnityEngine;

namespace GameCore.Objects
{
    public class Detail : MonoBehaviour
    {
        private const float Speed = 400f;

        private ObjectPool _pool;
        private RectTransform _rect;

        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
        }

        public void OnGetFromPool(ObjectPool poolRef)
        {
            _pool = poolRef;
        }

        private void Update()
        {
            if (!GameManager.Instance.IsGameActive) return;
            _rect.anchoredPosition += Vector2.left * Speed * Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                GameManager.Instance.AddDetails();
                if (_pool != null)
                {
                    Misc.Services.VibroManager.Vibrate();
                    _pool.ReturnObject(gameObject);
                }
                else
                    Destroy(gameObject);
            }
            else if (other.CompareTag("Ground"))
            {
                if (_pool != null)
                    _pool.ReturnObject(gameObject);
                else
                    Destroy(gameObject);
            }
        }
    }
}
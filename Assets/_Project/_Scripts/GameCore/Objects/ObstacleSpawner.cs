using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Objects
{
    public class ObstacleSpawner : MonoBehaviour
    {
        [SerializeField] private ObjectPool[] _obstaclePools;
        [SerializeField] private ObjectPool[] _detailPools;

        public ObjectPool[] ObstaclePools => _obstaclePools;
        public ObjectPool[] DetailPools => _detailPools;

        [SerializeField] private RectTransform _spawnArea;
        [SerializeField] private float ObstacleSpawnInterval = 2.0f;
        [SerializeField] private float DetailSpawnInterval = 4.5f;
        [SerializeField] private float MinDistanceBetweenSpawns = 150f;

        private float _obstacleTimer = 0f;
        private float _detailTimer = 0f;

        private List<Vector2> _recentSpawnPositions;

        private void Awake()
        {
            _recentSpawnPositions = new List<Vector2>();
        }

        private void Update()
        {
            if (!GameManager.Instance.IsGameActive)
                return;

            _obstacleTimer += Time.deltaTime;
            _detailTimer += Time.deltaTime;

            if (_obstacleTimer >= ObstacleSpawnInterval)
            {
                _obstacleTimer = 0f;

                Vector2 obstaclePos = GetValidSpawnPosition();

                int obsIndex = Random.Range(0, _obstaclePools.Length);
                GameObject obstacle = _obstaclePools[obsIndex].GetObject(_spawnArea);
                RectTransform obstacleRect = obstacle.GetComponent<RectTransform>();

                obstacleRect.SetParent(_spawnArea, false);
                obstacleRect.anchoredPosition = obstaclePos;

                _recentSpawnPositions.Add(obstaclePos);

                if (_recentSpawnPositions.Count > 20)
                    _recentSpawnPositions.RemoveAt(0);
            }

            if (_detailPools != null && _detailPools.Length > 0 && _detailTimer >= DetailSpawnInterval)
            {
                _detailTimer = 0f;

                Vector2 detailPos = GetValidSpawnPosition();

                int detailIndex = Random.Range(0, _detailPools.Length);
                GameObject detail = _detailPools[detailIndex].GetObject(_spawnArea);
                RectTransform detailRect = detail.GetComponent<RectTransform>();

                detailRect.SetParent(_spawnArea, false);
                detailRect.anchoredPosition = detailPos;

                _recentSpawnPositions.Add(detailPos);
                if (_recentSpawnPositions.Count > 20)
                    _recentSpawnPositions.RemoveAt(0);
            }
        }

        private Vector2 GetRandomSpawnPosition()
        {
            Vector2 size = _spawnArea.rect.size;
            Vector2 pivotOffset = new Vector2(
                _spawnArea.rect.width * _spawnArea.pivot.x,
                _spawnArea.rect.height * _spawnArea.pivot.y);

            float randomX = Random.Range(0f, size.x) - pivotOffset.x;
            float randomY = Random.Range(0f, size.y) - pivotOffset.y;

            return new Vector2(randomX, randomY);
        }

        private bool IsPositionValid(Vector2 pos)
        {
            foreach (var existingPos in _recentSpawnPositions)
            {
                if (Vector2.Distance(existingPos, pos) < MinDistanceBetweenSpawns)
                    return false;
            }
            return true;
        }

        private Vector2 GetValidSpawnPosition()
        {
            Vector2 pos;
            int attempts = 0;
            const int maxAttempts = 10;

            do
            {
                pos = GetRandomSpawnPosition();
                attempts++;
                if (attempts > maxAttempts) break;
            } while (!IsPositionValid(pos));

            return pos;
        }
    }
}
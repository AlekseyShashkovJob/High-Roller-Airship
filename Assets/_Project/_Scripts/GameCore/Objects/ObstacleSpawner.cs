using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Objects
{
    public class ObstacleSpawner : MonoBehaviour
    {
        [SerializeField] private ObjectPool[] _obstaclePools;
        [SerializeField] private ObjectPool[] _partsPools;

        public ObjectPool[] ObstaclePools => _obstaclePools;
        public ObjectPool[] PartPools => _partsPools;

        [SerializeField] private RectTransform _spawnArea;
        [SerializeField] private float ObstacleSpawnInterval = 2.0f;
        [SerializeField] private float PartSpawnInterval = 4.5f;
        [SerializeField] private float MinDistanceBetweenSpawns = 150f;

        private float _obstacleTimer = 0f;
        private float _partTimer = 0f;

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
            _partTimer += Time.deltaTime;

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

            if (_partsPools != null && _partsPools.Length > 0 && _partTimer >= PartSpawnInterval)
            {
                _partTimer = 0f;

                Vector2 partPos = GetValidSpawnPosition();

                int partIndex = Random.Range(0, _partsPools.Length);
                GameObject part = _partsPools[partIndex].GetObject(_spawnArea);
                RectTransform partRect = part.GetComponent<RectTransform>();

                partRect.SetParent(_spawnArea, false);
                partRect.anchoredPosition = partPos;

                _recentSpawnPositions.Add(partPos);
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
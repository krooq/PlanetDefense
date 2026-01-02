using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Krooq.Common;
using Krooq.Core;

namespace Krooq.PlanetDefense
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField, Sirenix.OdinInspector.ReadOnly] private bool _isWaveActive = false;
        private Camera _cam;
        protected GameManager GameManager => this.GetSingleton<GameManager>();

        public bool IsWaveActive => _isWaveActive;

        private void Start()
        {
            _cam = Camera.main;
        }

        public async void StartWave(int waveNumber)
        {
            _isWaveActive = true;
            int meteorCount = waveNumber * 5 + 5;
            float spawnRate = Mathf.Max(0.2f, 2f - (waveNumber * 0.1f));

            for (int i = 0; i < meteorCount; i++)
            {
                if (GameManager.State != GameState.Playing) break;

                SpawnMeteor();
                await UniTask.Delay((int)(spawnRate * 1000));
            }

            // Wait for all meteors to be gone
            while (GameManager.HasMeteors)
            {
                if (GameManager.State != GameState.Playing) break;
                await UniTask.Delay(500);
            }

            _isWaveActive = false;
            GameManager.EndWave();
        }

        void SpawnMeteor()
        {
            if (_cam == null) _cam = Camera.main;

            float height = 2f * _cam.orthographicSize;
            float width = height * _cam.aspect;
            float topEdge = _cam.transform.position.y + _cam.orthographicSize;
            float leftEdge = _cam.transform.position.x - width / 2f;
            float rightEdge = _cam.transform.position.x + width / 2f;

            // Spawn slightly above the top edge
            float spawnY = topEdge + 2f;
            float spawnX = Random.Range(leftEdge, rightEdge);
            var spawnPos = new Vector3(spawnX, spawnY, 0);

            // Target somewhere on the ground (y=0) within the screen width
            // Add some padding so they don't target the very edge
            float padding = 1f;
            float targetX = Random.Range(leftEdge + padding, rightEdge - padding);
            var targetPos = new Vector3(targetX, 0, 0);

            Vector3 direction = (targetPos - spawnPos).normalized;

            var m = GameManager.SpawnMeteor();
            m.transform.SetPositionAndRotation(spawnPos, Quaternion.identity);
            m.Initialize(GameManager.Data.MeteorBaseSpeed, GameManager.Data.MeteorBaseHealth, GameManager.Data.ResourcesPerMeteor, direction);
        }
    }
}

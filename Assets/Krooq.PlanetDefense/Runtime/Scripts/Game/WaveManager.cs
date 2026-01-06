using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Krooq.Common;
using Krooq.Core;
using Sirenix.OdinInspector;

namespace Krooq.PlanetDefense
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField, ReadOnly] private bool _isWaveActive = false;
        [SerializeField, ReadOnly] private Camera _cam;

        protected GameManager GameManager => this.GetSingleton<GameManager>();

        public bool IsWaveActive => _isWaveActive;

        protected void Start()
        {
            _cam = Camera.main;
        }

        public async void StartWave(int waveNumber)
        {
            var waveIndex = waveNumber - 1;
            _isWaveActive = true;
            var data = GameManager.Data;
            var threatCount = data.BaseWaveSize + (waveIndex * data.ThreatsPerWave);
            var spawnRate = Mathf.Max(data.MinSpawnRate, data.BaseSpawnRate - (waveIndex * data.SpawnRateDecreasePerWave));
            for (var i = 0; i < threatCount; i++)
            {
                if (GameManager.State != GameState.Playing) break;

                SpawnThreat();
                await UniTask.Delay((int)(spawnRate * 1000));
            }

            // Wait for all threats to be gone
            while (GameManager.HasThreats)
            {
                if (GameManager.State != GameState.Playing) break;
                await UniTask.Delay(500);
            }

            _isWaveActive = false;
            GameManager.EndWave();
        }

        protected void SpawnThreat()
        {
            if (_cam == null) _cam = Camera.main;

            var threats = GameManager.Data.Threats;
            if (threats == null || threats.Count == 0) return;

            var threatData = threats[Random.Range(0, threats.Count)];

            var height = 2f * _cam.orthographicSize;
            var width = height * _cam.aspect;
            var topEdge = _cam.transform.position.y + _cam.orthographicSize;
            var leftEdge = _cam.transform.position.x - width / 2f;
            var rightEdge = _cam.transform.position.x + width / 2f;

            Vector3 spawnPos;

            if (threatData.MovementType == ThreatMovementType.Ground)
            {
                // Spawn on left or right edge, within the configured Y range
                var spawnY = Random.Range(GameManager.Data.GroundUnitSpawnHeightMin, GameManager.Data.GroundUnitSpawnHeightMax);
                var leftSide = Random.value < 0.5f;
                var spawnX = leftSide ? leftEdge - 2f : rightEdge + 2f;
                spawnPos = new Vector3(spawnX, spawnY, 0);
            }
            else
            {
                // Air or Constant - Spawn above top edge
                var spawnY = topEdge + 2f;
                var spawnX = Random.Range(leftEdge, rightEdge);
                spawnPos = new Vector3(spawnX, spawnY, 0);
            }

            var threat = GameManager.SpawnThreat(GameManager.Data.ThreatPrefab);
            threat.transform.SetPositionAndRotation(spawnPos, Quaternion.identity);
            threat.Init(threatData);
        }
    }
}

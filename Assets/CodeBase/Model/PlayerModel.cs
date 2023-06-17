using System;
using UniRx;
using UnityEngine;

namespace CodeBase.Model
{
    [Serializable]
    public class PlayerConfig
    {
        [Tooltip("Number of attempts.")]
        public int attempts = 1;
        [Tooltip("Vertical speed in m/s.")]
        public float speed = 5.0f;
        [Tooltip("Position of the player at start.")]
        public Vector3 spawnPosition = new Vector3(7.5f, 0f, -1f);
    }

    [Serializable]
    public class LevelConfig
    {
        public Vector3 bounds = new Vector3(9f, 4.5f, 11f);

        public bool IsPosOutOfHorizontalBounds(Vector3 pos)
        {
            const float refAspectRatio = 16f / 9;
            var currentAspectRatio = Screen.width / (float)Screen.height;
            
            var boundX = bounds.x * (currentAspectRatio / refAspectRatio);

            return pos.x > boundX || pos.x < -boundX;
        }

        public bool IsPosOutOfVerticalBounds(Vector3 pos)
        {
            const float refAspectRatio = 9 / 16f;
            var currentAspectRatio = (float)Screen.height / Screen.width;
            
            var boundY = bounds.y * (currentAspectRatio / refAspectRatio);
            
            return pos.y > boundY || pos.y < -boundY;
        }
    }
    
    public class PlayerModel
    {
        public Vector3ReactiveProperty Position { get; private set; }
        
        private readonly PlayerConfig _playerConfig;
        private readonly LevelConfig _levelConfig;

        public PlayerModel(PlayerConfig playerConfig, LevelConfig levelConfig)
        {
            _playerConfig = playerConfig;
            _levelConfig = levelConfig;

            Position = new Vector3ReactiveProperty(_playerConfig.spawnPosition);
        }

        public void Reset() => 
            Position.Value = _playerConfig.spawnPosition;

        public void Move(float vertical, float deltaTime)
        {
            var deltaPos = vertical * _playerConfig.speed * Vector3.up * deltaTime;

            if (_levelConfig.IsPosOutOfVerticalBounds(Position.Value + deltaPos))
                return;

            Position.Value += deltaPos;
        }
    }
}

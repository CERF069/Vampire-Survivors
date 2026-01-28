using A_Code.Enemies.Interface;
using UnityEngine;
using Zenject;

namespace A_Code.Enemies.MoveDirection
{
    public sealed class FollowPlayerDirection : IEnemyTargetProvider
    {
        private readonly Transform _player;

        [Inject]
        public FollowPlayerDirection(
            Transform player)
        {
            _player = player;
        }

        public Vector2 GetDirection(Transform enemy)
        {
            return (_player.position - enemy.position).normalized;
        }
    }
}
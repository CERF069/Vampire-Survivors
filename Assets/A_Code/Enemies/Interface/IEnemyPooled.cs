using UnityEngine;

namespace A_Code.Enemies.Interface
{
    public interface IEnemyPooled
    {
        void OnSpawn(Vector2 position);
        void OnDespawn();
    }
}
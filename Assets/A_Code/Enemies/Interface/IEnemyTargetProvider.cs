using UnityEngine;

namespace A_Code.Enemies.Interface
{
    public interface IEnemyTargetProvider
    {
        Vector2 GetDirection(Transform enemy);
    }
}
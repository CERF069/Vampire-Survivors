using A_Code.Systems.Move;
using UnityEngine;

namespace A_Code.Player.Interface
{
    public interface IMoveble
    {
        void Move(Vector2 moveDirection, IMoveConfig moveConfig, Rigidbody2D rigidbody);
    }
    
    public interface IPlayerMoveble : IMoveble { }
    public interface IEnemyMoveble : IMoveble { }
}
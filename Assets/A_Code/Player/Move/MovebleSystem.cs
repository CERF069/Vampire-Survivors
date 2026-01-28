using UnityEngine;
using A_Code.Player.Interface;
using A_Code.Systems.Move;

namespace A_Code.Player.Move
{
    public class MovebleSystem : IMoveble
    {
        protected Vector2 _currentVelocity;

        public virtual void Move(Vector2 moveDirection, IMoveConfig config, Rigidbody2D rigidbody)
        {
            Vector2 targetVelocity = moveDirection * config.MoveSpeed;

            float accel = moveDirection.sqrMagnitude > 0f
                ? config.Acceleration
                : config.Deceleration;

            _currentVelocity = Vector2.MoveTowards(
                _currentVelocity,
                targetVelocity,
                accel * Time.fixedDeltaTime
            );

            rigidbody.linearVelocity = _currentVelocity;
        }
    }
}
using UnityEngine;

namespace A_Code.Player.Interface
{
    public interface IPlayerInput
    {
        Vector2 MoveDirection();
        PlayerInputActions Actions { get; set; }

        void Enable();
        void Disable();
    }
}
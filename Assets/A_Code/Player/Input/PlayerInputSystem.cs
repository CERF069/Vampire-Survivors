using UnityEngine;
using A_Code.Player.Interface;

namespace A_Code.Player.Input
{
    public sealed class PlayerInputSystem : IPlayerInput
    {
        public PlayerInputActions Actions { get; set; }

        public PlayerInputSystem()
        {
            Actions = new PlayerInputActions();
        }

        public Vector2 MoveDirection()
        {
            return Actions.Player.Move.ReadValue<Vector2>();
        }

        public void Enable()
        {
            Actions.Player.Enable();
        }

        public void Disable()
        {
            Actions.Player.Disable();
        }
    }
}
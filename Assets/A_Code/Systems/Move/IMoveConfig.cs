namespace A_Code.Systems.Move
{
    public interface IMoveConfig
    {
        public float MoveSpeed { get; set; }
        public float Acceleration { get; set; }
        public float Deceleration { get; set; }
    }
}
namespace Assets.Scripts.EFTB.Interface
{
    public interface ICatStateContrller
    {
        public void Initialize();
        public void Dispose();
        public void UpdateLogic(float deltaTime);
    }
}

namespace JumboJumps.EFTB.Interface
{
    public interface ICatStateController
    {
        public void Initialize();
        public void Dispose();
        public void UpdateLogic(float deltaTime);
    }
}

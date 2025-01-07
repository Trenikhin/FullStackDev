namespace Atomic.UI
{
    public interface IPresenterUpdate : IPresenter
    {
        void OnUpdate(float deltaTime);
    }

    public interface IPresenterFixedUpdate : IPresenter
    {
        void OnFixedUpdate(float deltaTime);
    }

    public interface IPresenterLateUpdate : IPresenter
    {
        void OnLateUpdate(float deltaTime);
    }
}
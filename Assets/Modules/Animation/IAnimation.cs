using Cysharp.Threading.Tasks;

namespace Modules.Animations
{
    public interface IAnimation
    {
        UniTask Execute();
    }
}
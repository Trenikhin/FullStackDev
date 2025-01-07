using Cysharp.Threading.Tasks;

namespace Modules.Animations
{
    public sealed class ParallelAnimation : IAnimation
    {
        private readonly IAnimation[] _animations;

        public ParallelAnimation(params IAnimation[] animations)
        {
            _animations = animations;
        }

        public async UniTask Execute()
        {
            int count = _animations.Length;
            UniTask[] tasks = new UniTask[count];
            for (int i = 0; i < count; i++)
            {
                IAnimation animation = _animations[i];
                tasks[i] = animation.Execute();
            }

            await UniTask.WhenAll(tasks);
        }
    }
}
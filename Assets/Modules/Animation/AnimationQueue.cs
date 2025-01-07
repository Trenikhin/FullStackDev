using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Modules.Animations
{
    public sealed class AnimationQueue 
    {
        private readonly Queue<IAnimation> _queue = new();

        public bool IsRunning => _running;

        private bool _running;

        public void Enqueue(IAnimation animation)
        {
            _queue.Enqueue(animation);
        }

        public async UniTask Execute()
        {
            if (_running)
                throw new Exception("Animator is already running!");
            
            _running = true;
            
            while (_queue.TryDequeue(out IAnimation animation))
                await animation.Execute();
            
            _running = false;
        }
    }
}
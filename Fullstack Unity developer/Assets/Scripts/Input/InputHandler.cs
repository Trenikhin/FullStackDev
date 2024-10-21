namespace ShootEmUp
{
    using System;
    using UnityEngine;

    public interface IInputHandler
    {
        event Action OnAttack;
        event Action<Vector2> OnMove;
    }
    
    
    public sealed class InputHandler : MonoBehaviour, IInputHandler
    {
        public event Action OnAttack;
        public event Action<Vector2> OnMove;
        
        bool    _fireRequired;
        Vector2 _moveDirection;

        void Update()
        {
            HandleMove();
            HandleFire();
        }

        void FixedUpdate()
        {
            // Try Fire
            if (_fireRequired)
            {
                OnAttack?.Invoke();
                _fireRequired = false;
            }
            
            OnMove?.Invoke( _moveDirection );
        }


        void HandleFire()
        {
            if ( Input.GetKeyDown( KeyCode.Space ) )
                _fireRequired = true;   
        }
        
        
        void HandleMove()
        {
            if (Input.GetKey(KeyCode.LeftArrow))
                _moveDirection = Vector2.left;
            else if (Input.GetKey(KeyCode.RightArrow))
                _moveDirection = Vector2.right;
            else
                _moveDirection = Vector2.zero;
        }
    }
}
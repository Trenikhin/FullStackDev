using UnityEngine;
using UnityEngine.Serialization;

namespace ShootEmUp
{
    public sealed class InputHandler : MonoBehaviour
    {
        [SerializeField] Player character;

        bool fireRequired;
        Vector2 moveDirection;

        void Update()
        {
            HandleMove();
            HandleFire();
        }

        void FixedUpdate()
        {
            // Try Fire
            if (fireRequired)
            {
                character.Fire();
                fireRequired = false;
            }
            
            character.Move( moveDirection );
        }


        void HandleFire()
        {
            if ( Input.GetKeyDown( KeyCode.Space ) )
            {
                fireRequired = true;   
            }
        }
        
        
        void HandleMove()
        {
            if (Input.GetKey(KeyCode.LeftArrow))
                this.moveDirection = Vector2.left;
            else if (Input.GetKey(KeyCode.RightArrow))
                this.moveDirection = Vector2.right;
            else
                this.moveDirection = Vector2.zero;
        }
    }
}
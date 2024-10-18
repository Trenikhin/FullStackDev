using UnityEngine;
using UnityEngine.Serialization;

namespace ShootEmUp
{
    public sealed class InputHandler : MonoBehaviour
    {
        [SerializeField] Player character;

        bool fireRequired;
        Vector2 moveDirection;

        void Awake()
        {
            this.character.OnHealthEmpty += _ => Time.timeScale = 0;
        }

        void Update()
        {
            HandleMove();
            
            if ( Input.GetKeyDown( KeyCode.Space ) )
            {
                fireRequired = true;   
            }
        }

        void FixedUpdate()
        {
            if (fireRequired)
            {
               character.Fire();
               fireRequired = false;
            }
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
            
            character.Move( moveDirection );
        }
    }
}
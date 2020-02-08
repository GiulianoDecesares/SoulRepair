using UnityEngine;

namespace Interfaces
{
    public interface ISoulBehavior
    {
        void Initialize();
        
        void OnFieldOfViewEnter(Collider[] others);
        
        void OnCollisionEnter(Collider[] others);
    }
}

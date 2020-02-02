using System;
using UnityEngine;

public class RangeTrigger : MonoBehaviour
{
    [SerializeField] private float range = 0.5f;
    [SerializeField] private bool userOverlapSphere;
    
    public System.Action<Collider[]> onDetection;
    public System.Action<Collider[]> onDetectionExit;

    private void Awake()
    {
        if (!this.userOverlapSphere)
        {
            SphereCollider collider = this.gameObject.AddComponent<SphereCollider>();
            
            if (collider != null)
            {
                collider.radius = this.range;
                collider.isTrigger = true;
            }
        }
    }

    private void Update()
    {
        if (this.userOverlapSphere)
        {
            Collider[] others = Physics.OverlapSphere(this.transform.position, this.range);
            
            if (others.Length > 0)
            {
                this.onDetection?.Invoke(others);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        this.onDetection?.Invoke(new [] { other });
    }

    private void OnTriggerExit(Collider other)
    {
        this.onDetection?.Invoke(new [] { other });
    }
}

using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Interactable : MonoBehaviour
    {
        public float radius = 3f;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
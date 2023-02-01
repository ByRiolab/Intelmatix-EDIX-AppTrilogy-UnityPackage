using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform pivot;
    [SerializeField] private Transform spotlight;


    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(pivot.position, spotlight.position);
    }
}

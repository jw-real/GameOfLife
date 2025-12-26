using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CellAnimationBridge : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private CellVisual cell;

    private void Awake()
    {
        cell = GetComponent<CellVisual>();
    }

    private void Update()
    {
        if (animator == null)
        {
            Debug.Log("No Animator Found");
            return;
        }
        cell.SetAlive(animator.GetBool("IsAlive"));
    }
}
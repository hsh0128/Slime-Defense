using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class TurretHeadAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    public void ShootAnimation()
    {
        animator.SetTrigger("OnShoot");
    }

    public void UpdateRotation(float angle)
    {
        transform.eulerAngles = Vector3.forward * angle;
    }
}

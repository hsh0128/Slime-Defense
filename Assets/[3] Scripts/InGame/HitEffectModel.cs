using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffectModel : MonoBehaviour
{
    public float lifetime;
    private float _remainTime;

    [HideInInspector]
    public Transform attachTransform;

    private void Awake()
    {
        _remainTime = lifetime;
    }

    private void Update()
    {
        _remainTime -= Time.deltaTime;

        if (attachTransform) transform.position = attachTransform.position;

        if (_remainTime <= 0) Destroy(gameObject);
    }
}

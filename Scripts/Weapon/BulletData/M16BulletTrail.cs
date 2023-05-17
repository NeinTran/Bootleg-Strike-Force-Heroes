using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M16BulletTrail : MonoBehaviour
{
    private Vector3 _startPosition;
    private Vector3 _targetPosition;
    private float _progress;
    public float knockbackForce = 3f;
    public float damage = 9;
    public float criticalDamage = 24;

    [SerializeField] private float _speed = 10f;

    void Start()
    {
        _startPosition = transform.position.WithAxis(Axis.Z, -1);
    }

    void Update()
    {
        _progress += Time.deltaTime * _speed;
        transform.position = Vector3.Lerp(_startPosition, _targetPosition, _progress);
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        _targetPosition = targetPosition.WithAxis(Axis.Z, -1);
    }
}

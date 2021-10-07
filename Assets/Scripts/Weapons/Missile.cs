using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField]
    private float _speed = 0.5f;
    [SerializeField]
    private float _rotationSpeed = 2f;
    [SerializeField]
    private GameObject _target;

    private Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetDirection = _target.transform.position - transform.position;
        float singleStep = _rotationSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
        _rb.velocity = transform.forward * _speed;
    }

    public void Speed_Update(float speed)
    {
        _speed = speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}

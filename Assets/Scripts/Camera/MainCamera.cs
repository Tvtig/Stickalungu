using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField]
    private Transform _targetTransform;
    [SerializeField]
    private float _followSpeed;
    [SerializeField]
    private float _maxFollowHeight = 5f;

    private float _followMoveThreshhold = 0.02f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 newPosition = Vector3.Lerp(transform.position, _targetTransform.position, _followSpeed * Time.deltaTime);
        transform.position = new Vector3(newPosition.x, transform.position.y, newPosition.z);
    }

  
}

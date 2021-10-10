using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField]
    private float _phaseStepPerFrame = 0.1f;
    [SerializeField]
    private bool _reset = false;
    [SerializeField]
    private float _shakeDuration = 1f;
    [SerializeField]
    private float _shakeAmount = 0.5f;
    [SerializeField]
    private GameObject _dustEffect;

    private MeshRenderer _meshRenderer;
    private Material _material;
    private Vector3 _originalPosition;

    // Start is called before the first frame update
    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _material = _meshRenderer.sharedMaterial;
        _material.SetFloat("Phase", 0);
        _originalPosition = transform.position;
        _dustEffect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            _material.SetFloat("Phase", 0);
        }
    }

    public void Damage_Take()
    {
        StartCoroutine(Cracks_Form());
        StartCoroutine(Shake());
    }

    private IEnumerator Cracks_Form()
    {
        float phase = 0.0f;

        while (phase <= 1)
        {
            phase += _phaseStepPerFrame;
            _material.SetFloat("Phase", phase);
            yield return new WaitForEndOfFrame();
        }

        _material.SetFloat("Phase", 1);
    }

    private IEnumerator Shake()
    {
        float currentShakeDuration = _shakeDuration;
        
        _dustEffect.SetActive(true);

        while (currentShakeDuration > 0)
        {
            transform.position = _originalPosition + Random.insideUnitSphere * _shakeAmount;
            currentShakeDuration -= Time.deltaTime * 1f;
            yield return new WaitForSeconds(0.1f);
        }

        _dustEffect.SetActive(false);

        yield return null;
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField]
    private float _phaseStepPerFrame = 0.1f;
    [SerializeField]
    private bool _reset = false;

    private MeshRenderer _meshRenderer;
    private Material _material;

    // Start is called before the first frame update
    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _material = _meshRenderer.sharedMaterial;
        _material.SetFloat("Phase", 0);
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
    }

    private IEnumerator Cracks_Form()
    {
        float phase = 0;

        while(phase <= 1)
        {
            phase += _phaseStepPerFrame;
            _material.SetFloat("Phase", phase);
            yield return new WaitForEndOfFrame();
        }

        _material.SetFloat("Phase", 1);


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMissileSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _missile;
    [SerializeField]
    private float _positionRangeMinimum = -2.0f;
    [SerializeField]
    private float _positionRangeMaximum = 2.0f;
    [SerializeField]
    private float _maximumSpeed = 5f;
    [SerializeField]
    private int _numCorourtines = 10;


    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < _numCorourtines; i++)
        {
            StartCoroutine(SpawnMissile());
        }
    }

    IEnumerator SpawnMissile()
    {
        while (true)
        {
            float delay = Random.Range(0.1f, 2.5f);
            yield return new WaitForSeconds(delay);

            Vector3 position = new Vector3(
                Random.Range(_positionRangeMinimum, _positionRangeMaximum), 
                Random.Range(_positionRangeMinimum, _positionRangeMaximum), 
                Random.Range(_positionRangeMinimum, _positionRangeMaximum));

            Quaternion rotation = Random.rotation;
            float speed = Random.Range(0, _maximumSpeed);
            
            GameObject m = Instantiate(_missile, position, rotation);
            
            m.GetComponent<Missile>().Speed_Update(speed);
        }
    }

}

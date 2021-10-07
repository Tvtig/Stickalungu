using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerSound
{
    Walk,
    Run,
    Punch01,
    Thud_01,
    Thud_02
}

public class AudioHandler : MonoBehaviour
{
    [SerializeField]
    private AudioSource _punch01;
    [SerializeField]
    private AudioSource _walk;
    [SerializeField]
    private AudioSource _run;
    [SerializeField]
    private AudioSource _thud_01;
    [SerializeField]
    private AudioSource _thud_02;

    public void Sound_Play(PlayerSound sound, float delay)
    {
        switch (sound)
        {
            case PlayerSound.Walk:
                StartCoroutine(Play(_walk, delay));
                break;
            case PlayerSound.Run:
                StartCoroutine(Play(_run, delay));
                break;
            case PlayerSound.Punch01:
                StartCoroutine(Play(_punch01, delay));
                break;
            case PlayerSound.Thud_01:
                StartCoroutine(Play(_thud_01, delay));
                break;
            case PlayerSound.Thud_02:
                StartCoroutine(Play(_thud_02, delay));
                break;

        }
    }

    private IEnumerator Play(AudioSource source, float delay)
    {
        yield return new WaitForSeconds(delay);
        source.Play();
    }
}

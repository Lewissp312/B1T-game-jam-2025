using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[ExecuteAlways]
public class AudioTester : MonoBehaviour{
    public AudioClip clip;
    public AudioSource source;

    public bool shouldPlay = true;

    void OnValidate()
    {
        source.Stop();

        if (shouldPlay){
            source.clip = clip;
            source.Play();
        }
    }
}
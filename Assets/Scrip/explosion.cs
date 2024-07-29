using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    public AudioSource src;
    public AudioClip Explo;
    // Start is called before the first frame update

    void Start()
    {
        src.clip = Explo;
        src.Play();
        Destroy(gameObject, 0.8f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

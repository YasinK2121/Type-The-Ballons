using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticleEffect : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(SpawnBallon());
    }

    IEnumerator SpawnBallon()
    {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}

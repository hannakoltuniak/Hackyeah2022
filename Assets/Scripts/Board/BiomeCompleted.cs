using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeCompleted : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector3.zero;
        LeanTween.scale(gameObject, Vector3.one, 0.3f)
            .setEaseInOutBounce();

        AudioManager.Instance.Play("rankUp", Assets.Scripts.SoundCategory.VFX);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

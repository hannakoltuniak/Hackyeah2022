using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconOrderInLayerKeeper : MonoBehaviour
{
    SpriteRenderer parentTileRenderer;
    SpriteRenderer iconRenderer;

    // Start is called before the first frame update
    void Start()
    {
        parentTileRenderer = transform.parent.GetComponent<SpriteRenderer>();
        iconRenderer = transform.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        iconRenderer.sortingOrder = parentTileRenderer.sortingOrder + 1;
    }
}

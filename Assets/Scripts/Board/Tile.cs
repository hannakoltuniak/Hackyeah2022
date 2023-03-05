using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Int32 TileKind;
    public SpriteRenderer TileIcon;
    public GameObject CircleMask;
    public Slot TargetSlot;

    private SpriteRenderer sprenTile;

    // Start is called before the first frame update
    void Start()
    {
        sprenTile = GetComponent<SpriteRenderer>();
        SustainableColors sc = new SustainableColors();
        if (TileKind == 0 || TileKind == 1)
        {
            sprenTile.color = sc.FirstTwoColor;
        }
        else if (TileKind == 2 || TileKind == 3)
        {
            sprenTile.color = sc.SecondTwoColor;
        }
        else if (TileKind == 4 || TileKind == 5)
        {
            sprenTile.color = sc.ThirdTwoColor;
        }
        
        CircleMask.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

    }
}

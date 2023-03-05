using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KnowledgeUnlockingController : MonoBehaviour
{
    public List<UnlockableKnowledge> objUnlockableKnowledges;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal Transform GetCollectionPointForTileKind(Tile sendant)
    {
        return objUnlockableKnowledges.First(uk => uk.tileKindsThatMakeProgress.Any(progKind => progKind == sendant.TileKind))
            .trPointCollectionPoint;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomTeleportDetector : SingletonMono<BottomTeleportDetector>
{
    public bool triggered { get; private set; }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMasks.PlatformLayerMask) return;
        triggered = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMasks.PlatformLayerMask) return;
        triggered = false;
    }
}

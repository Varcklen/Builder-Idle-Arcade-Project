using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceObject : MonoBehaviour
{
    public ResourceSO Resource;

    private bool _triggered;

    public event Action<ResourceObject> OnPickup;

    private void OnTriggerEnter(Collider collider)
    {
        if (_triggered || collider.tag != "Player" ) return;
        var playerBag = collider.GetComponent<PlayerBag>();
        playerBag.AddResource(this, out bool successfull);
        if (successfull == false) return;
        _triggered = true;
        OnPickup?.Invoke(this);
        Destroy(gameObject);
    }

}

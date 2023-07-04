using System;
using UnityEngine;

/// <summary>
/// Ресурс, який лежить на землi i може бути пiдiбраний гравцем
/// </summary>
public class ResourceObject : ResourceMover
{
    private bool _triggered;

    public event Action<ResourceObject> OnPickup;

    private void OnTriggerEnter(Collider collider)
    {
        if (_triggered || collider.tag != "Player" ) return;
        var playerBag = collider.GetComponent<PlayerBag>();
        playerBag.AddResource(this, out bool successfull);
        if (successfull == false) return;

        ResetMovement();
        _triggered = true;
        OnPickup?.Invoke(this);
        Destroy(gameObject);
    }

}

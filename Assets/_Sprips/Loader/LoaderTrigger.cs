using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Завантажує ресурси у Loader
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class LoaderTrigger : MonoBehaviour
{
    [SerializeField] private Loader _loader;

    private void OnTriggerEnter(Collider collider)
    {
        //Debug.Log($"Collide: {collider}");
        if (collider.tag != "Player") return;
        var playerBag = collider.GetComponent<PlayerBag>();

        int resourceAmountToLoad = playerBag.GetRecourseAmount(_loader.ResourceRequired);
        if (resourceAmountToLoad == 0) return;
        _loader.LoadResources(playerBag);
    }}
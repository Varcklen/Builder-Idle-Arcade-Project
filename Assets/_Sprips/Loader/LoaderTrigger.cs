using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class LoaderTrigger : MonoBehaviour
{
    [SerializeField] private Loader _loader;

    private void OnTriggerEnter(Collider collider)
    {
        //Debug.Log($"Collide: {collider}");
        if (collider.tag != "Player") return;
        _loader.LoadResources(collider.GetComponent<PlayerBag>());
    }}
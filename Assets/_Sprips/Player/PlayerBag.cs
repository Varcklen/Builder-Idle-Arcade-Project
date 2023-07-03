using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerBag : MonoBehaviour
{
    [SerializeField] private Transform _bagTransform;
    [SerializeField] private GameObject BagResourcePrefab;
    [SerializeField] private float _rangeBetweenItems = 1f;
    [SerializeField] private int _limitOfVisibleItems = 10;
    [SerializeField] private bool _accessToPickupDifferentItems;

    private readonly List<ResourceProjectile> _bagSlots = new ();
    private readonly List<ResourceAmount> _resourceAmountList = new ();

    private ResourceSO _mainVisibleResource;

    private class ResourceAmount
    {
        public ResourceSO Resource;
        public int amount;
    }

    public void AddResource(ResourceObject resourceObject, out bool successfull)
    {
        //Debug.Log($"_mainVisibleResource: {_mainVisibleResource?.Name}");
        if (_mainVisibleResource == null)
        {
            _mainVisibleResource = resourceObject.Resource;
        }
        if (_mainVisibleResource != resourceObject.Resource && _accessToPickupDifferentItems == false)
        {
            successfull = false;
            return;
        }

        ResourceSO resource = resourceObject.Resource;
        ResourceAmount resourceAmount = _resourceAmountList.Find(x => x.Resource == resource);
        if (resourceAmount == null)
        {
            
            _resourceAmountList.Add(new ResourceAmount() { 
                Resource = resource, 
                amount = 1
            });
        }
        else
        {
            resourceAmount.amount++;
        }

        ResourceProjectile bagResource;
        Vector3 newPosition;
        int itemsWithMainResource = GetRecourseAmount(_mainVisibleResource);
        if (itemsWithMainResource < _limitOfVisibleItems && resource == _mainVisibleResource)
        {
            bagResource = Instantiate(BagResourcePrefab, resourceObject.transform.position, Quaternion.identity).GetComponent<ResourceProjectile>();
            //Debug.Log($"bagResource: {bagResource}");
            //Debug.Log($"resourceObject: {resourceObject}");
            bagResource.Resource = resource;
            newPosition = _bagTransform.position + new Vector3(0, itemsWithMainResource * _rangeBetweenItems, 0);
            bagResource.MoveToPosition(newPosition, () =>
            {
                if (bagResource.IsMoveInterrupted == false)
                {
                    SetParent(bagResource);
                }
            }
            );
        }
        else
        {
            bagResource = Instantiate(BagResourcePrefab, _bagTransform.position, Quaternion.identity).GetComponent<ResourceProjectile>();
            newPosition = bagResource.transform.position;
            bagResource.Resource = resource;
            SetParent(bagResource);
            bagResource.gameObject.SetActive(false);
        }
        bagResource.SetModel(resourceObject);
        _bagSlots.Add(bagResource);
        successfull = true;
    }

    public void RemoveResource(ResourceSO resource, Vector3 moveTo, out List<ResourceProjectile> bagResources, int amount = 1, bool destroyAtEnd = false )
    {
        bagResources = new List<ResourceProjectile>();
        ResourceAmount resourceAmount = _resourceAmountList.Find(x => x.Resource == resource);
        if (resourceAmount == null) return;

        amount = Mathf.Min(amount, resourceAmount.amount);
        resourceAmount.amount -= amount;

        ResourceProjectile bagResource;
        for (int i = 0; i < amount; i++)
        {
            bagResource = _bagSlots.Find(x => x.Resource == resource);
            if (bagResource == null)
            {
                break;
            }
            bagResources.Add(bagResource);
            bagResource.ResetResource();
            //bagResource.transform.position = moveTo;
            if (destroyAtEnd)
            {
                bagResource.MoveToPosition(moveTo, () =>
                {
                    if (bagResource != null) Destroy(bagResource.gameObject);
                });
            }
            else
            {
                bagResource.MoveToPosition(moveTo);
            }
            
            _bagSlots.Remove(bagResource); 
        }
        if (GetRecourseAmount(_mainVisibleResource) <= 0)
        {
            _mainVisibleResource = null;
        }
    }

    private void SetParent(ResourceProjectile bagResource)
    {
        bagResource.transform.parent = _bagTransform;
        bagResource.transform.position = new Vector3(_bagTransform.transform.position.x, bagResource.transform.position.y, _bagTransform.transform.position.z);
        bagResource.transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    public int GetRecourseAmount(ResourceSO resource)
    {
        return _bagSlots.Where(x => x.Resource == resource).Count();
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Iнвентар гравц€.
/// </summary>
public class PlayerBag : MonoBehaviour
{
    [SerializeField] private Transform _bagTransform;
    [SerializeField] private GameObject ResourceProjectilePrefab;
    [SerializeField] private float _rangeBetweenItems = 1f;
    [SerializeField] private int _limitOfVisibleItems = 10;
    [SerializeField] private bool _canPickupDifferentItems;

    private readonly List<ResourceProjectile> _bagSlots = new ();

    private ResourceSO _mainVisibleResource;

    public void AddResource(ResourceObject resourceObject, out bool successfull)
    {
        if (_mainVisibleResource == null)
        {
            _mainVisibleResource = resourceObject.Resource;
        }
        if (_mainVisibleResource != resourceObject.Resource && _canPickupDifferentItems == false)
        {
            successfull = false;
            return;
        }

        ResourceSO resource = resourceObject.Resource;
        ResourceProjectile bagResource;
        int itemsWithMainResource = GetRecourseAmount(_mainVisibleResource);
        if (itemsWithMainResource < _limitOfVisibleItems && resource == _mainVisibleResource)
        {
            bagResource = Instantiate(ResourceProjectilePrefab, resourceObject.transform.position, Quaternion.identity).GetComponent<ResourceProjectile>();
            bagResource.Resource = resource;
            Vector3 newPosition = _bagTransform.position + new Vector3(0, itemsWithMainResource * _rangeBetweenItems, 0);
            bagResource.MoveToPosition(newPosition, actionAfterMove:() =>
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
            bagResource = Instantiate(ResourceProjectilePrefab, _bagTransform.position, Quaternion.identity).GetComponent<ResourceProjectile>();
            bagResource.Resource = resource;
            SetParent(bagResource);
            bagResource.gameObject.SetActive(false);
        }
        bagResource.SetModel(resourceObject);
        _bagSlots.Add(bagResource);
        successfull = true;
    }

    public void RemoveResource(ResourceSO resource, Vector3 moveTo, out List<ResourceProjectile> resourceList, int amount = 1, bool destroyAtEnd = false )
    {
        resourceList = new List<ResourceProjectile>();
        amount = Mathf.Min(amount, GetRecourseAmount(resource));
        ResourceProjectile resourceProjectile;
        for (int i = 0; i < amount; i++)
        {
            resourceProjectile = _bagSlots.Find(x => x.Resource == resource);
            if (resourceProjectile == null)
            {
                break;
            }
            resourceList.Add(resourceProjectile);
            resourceProjectile.ResetMovement();

            resourceProjectile.MoveToPosition(moveTo, destroyAtEnd);
            _bagSlots.Remove(resourceProjectile); 
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
    }

    public int GetRecourseAmount(ResourceSO resource)
    {
        return _bagSlots.Where(x => x.Resource == resource).Count();
    }
}

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Відвантажує вибрані ресурси у вказане місце
/// </summary>
public class Uploader : MonoBehaviour
{
    [SerializeField] private ResourceSO _resourceToCreate;
    [SerializeField] private GameObject _recourceObjectPrefab;
    [SerializeField] private Transform _uploadTransform;

    [SerializeField] private WorldInfoPanel _infoPanelEnd;

    [SerializeField] private float _rangeBetweenItemsY = 1;

    [SerializeField] private List<Transform> _pointsToPutProduct;

    private readonly List<ResourceObject> _itemsProduced = new();

    private void Start()
    {
        _infoPanelEnd.SetImage(_resourceToCreate.Sprite);
        _infoPanelEnd.SetValue(_itemsProduced.Count);
    }

    public void CreateResource()
    {
        ResourceObject resourceObject = Instantiate(_recourceObjectPrefab, _uploadTransform.position, Quaternion.identity).GetComponent<ResourceObject>();
        resourceObject.Resource = _resourceToCreate;
        resourceObject.MoveToPosition(GetPointToMove());
        _itemsProduced.Add(resourceObject);
        _infoPanelEnd.SetValue(_itemsProduced.Count);
        resourceObject.OnPickup += OnPickup;
    }

    private void OnPickup(ResourceObject resourceObject)
    {
        _itemsProduced.Remove(resourceObject);
        _infoPanelEnd.SetValue(_itemsProduced.Count);
        resourceObject.OnPickup -= OnPickup;
    }

    private Vector3 GetPointToMove()
    {
        int amount = _itemsProduced.Count;
        int pointsAmount = _pointsToPutProduct.Count;
        if (amount >= pointsAmount)
        {
            int newPosition = amount;
            while (newPosition >= pointsAmount)
            {
                newPosition -= pointsAmount;
            }
            return _pointsToPutProduct[newPosition].position + new Vector3(0, _rangeBetweenItemsY * (amount / pointsAmount), 0);
        }
        else
        {
            return _pointsToPutProduct[amount].position;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : Loader
{
    [SerializeField] private ResourceSO _resourceToCreate;
    [SerializeField] private Transform _loadTransform;
    [SerializeField] private Transform _uploadTransform;
    [SerializeField] private Transform _startProcessTransform;
    [SerializeField] private Transform _endProcessTransform;
    [SerializeField] private Transform _objectBuferTransform;

    [SerializeField] private WorldInfoPanel _infoPanelStart;
    [SerializeField] private WorldInfoPanel _infoPanelEnd;

    [SerializeField] private GameObject _recourceObjectPrefab;

    [SerializeField, Min(0.1f)] private float _processTime;
    [SerializeField, Min(1)] private float _loadRange;

    private readonly List<ResourceProjectile> _itemsInBufer = new ();
    private readonly List<ResourceObject> _itemsProduced = new ();

    private bool _inProcess;

    private void Start()
    {
        _infoPanelStart.SetImage(ResourceRequired.Sprite);
        _infoPanelStart.SetValue(_itemsInBufer.Count);

        _infoPanelEnd.SetImage(_resourceToCreate.Sprite);
        _infoPanelEnd.SetValue(_itemsProduced.Count);
    }

    public override void LoadResources(PlayerBag playerBag)
    {
        int resourceAmountToLoad = playerBag.GetRecourseAmount(ResourceRequired);

        if (resourceAmountToLoad == 0)
        {
            return;
        }
        playerBag.RemoveResource(ResourceRequired, _objectBuferTransform.position, out List<ResourceProjectile> bagResource, resourceAmountToLoad);
        _itemsInBufer.AddRange(bagResource);
        _infoPanelStart.SetValue(_itemsInBufer.Count);
        if (_inProcess == false)
        {
            StartCoroutine(Process());
        }
    }

    private IEnumerator Process()
    {
        _inProcess = true;
        var cooldown = new WaitForSeconds(_processTime);
        ResourceProjectile itemInProcess;
        while (_inProcess)
        {
            itemInProcess = _itemsInBufer[0];
            itemInProcess.ResetResource();
            StartCoroutine(Move(itemInProcess));
            yield return cooldown;
            _itemsInBufer.Remove(itemInProcess);
            _infoPanelStart.SetValue(_itemsInBufer.Count);
            Destroy(itemInProcess.gameObject);
            CreateResource();
            _inProcess = _itemsInBufer.Count > 0;
        }
    }

    private IEnumerator Move(ResourceProjectile itemInProcess)
    {
        Transform itemTransform;
        itemTransform = itemInProcess.transform;
        itemTransform.position = _startProcessTransform.position;

        var speed = Vector3.Distance(_endProcessTransform.position, _startProcessTransform.position) / _processTime;
        float time = 0;
        while (time < _processTime)
        {
            yield return null;
            itemTransform.position = Vector3.MoveTowards(itemTransform.position, _endProcessTransform.position, Time.deltaTime * speed);
            time += Time.deltaTime;
        }
    }

    private void CreateResource()
    {
        ResourceObject resourceObject = Instantiate(_recourceObjectPrefab, _uploadTransform.position, Quaternion.identity).GetComponent<ResourceObject>();
        resourceObject.Resource = _resourceToCreate;
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
}

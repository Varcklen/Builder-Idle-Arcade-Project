using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Тут проходе процесс переробки металу в мечi. Скрипт зв`язан з Loader и Uploader скриптами
/// </summary>
public class Factory : Loader
{

    [SerializeField] private Transform _startProcessTransform;
    [SerializeField] private Transform _endProcessTransform;
    [SerializeField] private Transform _objectBuferTransform;

    [SerializeField] private WorldInfoPanel _infoPanelStart;

    [SerializeField] private Uploader _uploader;

    [SerializeField, Min(0.1f)] private float _processTime;

    private readonly List<ResourceProjectile> _itemsInBufer = new ();

    private bool _inProcess;

    private void Start()
    {
        _infoPanelStart.SetImage(_resourceRequired.Sprite);
        _infoPanelStart.SetValue(_itemsInBufer.Count);
    }

    public override void LoadResources(PlayerBag playerBag)
    {
        playerBag.RemoveResource(_resourceRequired, _objectBuferTransform.position, out List<ResourceProjectile> bagResource, playerBag.GetRecourseAmount(_resourceRequired));
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
            itemInProcess.ResetMovement();
            StartCoroutine(Move(itemInProcess));
            yield return cooldown;
            _itemsInBufer.Remove(itemInProcess);
            _infoPanelStart.SetValue(_itemsInBufer.Count);
            Destroy(itemInProcess.gameObject);
            _uploader.CreateResource();
            _inProcess = _itemsInBufer.Count > 0;
        }
    }

    private IEnumerator Move(ResourceProjectile itemInProcess)
    {
        Transform itemTransform = itemInProcess.transform;
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
}

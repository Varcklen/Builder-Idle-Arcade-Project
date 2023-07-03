using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stockpile : Loader
{
    [SerializeField] private Transform _placeToPutItemTransform;
    [SerializeField] private WorldInfoPanel _infoPanel;

    private int _itemsInStock;

    private void Start()
    {
        SetStockAmount(_itemsInStock);
        _infoPanel.SetImage(ResourceRequired.Sprite);
    }

    public override void LoadResources(PlayerBag playerBag)
    {
        int resourceAmountToLoad = playerBag.GetRecourseAmount(ResourceRequired);

        if (resourceAmountToLoad == 0)
        {
            return;
        }
        playerBag.RemoveResource(ResourceRequired, _placeToPutItemTransform.position, out _, resourceAmountToLoad, true);

        AddStockAmount(resourceAmountToLoad);
    }

    public void SetStockAmount(int newValue)
    {
        _itemsInStock = newValue;
        _infoPanel.SetValue(newValue);
    }

    public void AddStockAmount(int valueToAdd)
    {
        SetStockAmount(_itemsInStock + valueToAdd);
    }
}

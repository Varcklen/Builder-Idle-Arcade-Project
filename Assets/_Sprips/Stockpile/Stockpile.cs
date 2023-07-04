using UnityEngine;

/// <summary>
/// Мiсце для відвантаження ресурсiв
/// </summary>
public class Stockpile : Loader
{
    [SerializeField] private Transform _placeToMoveItem;
    [SerializeField] private WorldInfoPanel _infoPanel;

    private int _itemsInStock;

    private void Start()
    {
        SetStockAmount(_itemsInStock);
        _infoPanel.SetImage(_resourceRequired.Sprite);
    }

    public override void LoadResources(PlayerBag playerBag)
    {
        int resourceAmountToLoad = playerBag.GetRecourseAmount(_resourceRequired);

        playerBag.RemoveResource(_resourceRequired, _placeToMoveItem.position, out _, resourceAmountToLoad, destroyAtEnd:true);

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

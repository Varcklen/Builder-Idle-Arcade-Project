using UnityEngine;

/// <summary>
/// "Dummy" ресурс, який використовується для вiзуального вiдображення (наприклад, в iнвентарi, або у переробцi на фабрицi)
/// </summary>
public class ResourceProjectile : ResourceMover
{
    public void SetModel(ResourceObject resourceObject)
    {
        GameObject model = resourceObject.transform.Find("Model").gameObject;
        Instantiate(model, transform);
    }
}

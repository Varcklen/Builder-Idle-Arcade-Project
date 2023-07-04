using UnityEngine;

/// <summary>
/// Класи, якi наслiдуются вiд класу Loader, можуть використовувати завантаження ресурсiв використовуючи LoaderTrigger
/// </summary>
public abstract class Loader : MonoBehaviour
{
    [SerializeField] protected ResourceSO _resourceRequired;
    public ResourceSO ResourceRequired => _resourceRequired;
    public abstract void LoadResources(PlayerBag bag);
}

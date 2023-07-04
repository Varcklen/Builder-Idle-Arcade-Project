using UnityEngine;

/// <summary>
/// �����, ��i ����i������ �i� ����� Loader, ������ ��������������� ������������ ������i� �������������� LoaderTrigger
/// </summary>
public abstract class Loader : MonoBehaviour
{
    [SerializeField] protected ResourceSO _resourceRequired;
    public ResourceSO ResourceRequired => _resourceRequired;
    public abstract void LoadResources(PlayerBag bag);
}

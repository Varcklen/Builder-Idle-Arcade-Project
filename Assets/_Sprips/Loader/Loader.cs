using UnityEngine;

public abstract class Loader : MonoBehaviour
{
    [SerializeField] protected ResourceSO ResourceRequired;
    public abstract void LoadResources(PlayerBag bag);
}

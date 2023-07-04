using System.Collections;
using UnityEngine;

/// <summary>
/// Скрипт дає можливiсть створювати ResourceObject, якi можуть бути пiдiбранi гравцем
/// </summary>
public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _resourceObjectPrefab;
    //[SerializeField] private ResourceSO _resourceType;
    [SerializeField, Min(1)] private float _maxSpawnRange;
    [SerializeField, Min(0.1f)] private float _spawnCooldown;
    [SerializeField] private bool _spawnOnStart;

    private void Start()
    {
        StartCoroutine(SpawnCooldown());
        if (_spawnOnStart)
        {
            Spawn();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _maxSpawnRange);
    }

    private IEnumerator SpawnCooldown()
    {
        var cooldown = new WaitForSeconds(_spawnCooldown);

        while (true)
        {
            yield return cooldown;
            Spawn();
        }
    }

    private void Spawn()
    {
        Vector3 spawnPosition = transform.position + (Vector3)Random.insideUnitCircle * (_maxSpawnRange);
        Vector3 result = new Vector3(spawnPosition.x, transform.position.y, spawnPosition.y);
        ResourceObject newObject = Instantiate(_resourceObjectPrefab, result, Quaternion.identity).GetComponent<ResourceObject>();
        //newObject.Resource = _resourceType;
    }
}

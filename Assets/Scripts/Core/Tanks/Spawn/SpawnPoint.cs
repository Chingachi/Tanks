using UnityEngine;
namespace Core.Tanks.Spawn
{
  public class SpawnPoint : MonoBehaviour
  {
    [SerializeField]
    private Transform _arenaCenter;

    private void Awake()
    {
      Vector3 dir = _arenaCenter.position - transform.position;
      dir.y = 0f;
      transform.rotation = Quaternion.LookRotation(dir);
    }
  }
}
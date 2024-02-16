using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SchoolManager : MonoBehaviour
{
  [SerializeField] GameObject _fishPrefab;

  [SerializeField] int _fishCount = 15;
  [SerializeField] bool _debugTankEnabled = false;

  [SerializeField] Vector3 _tankDimensions = new(10f, 8f, 4f);
  [Range(0, 2)] public float _tankMargin = 0.5f;

  Transform _debugTank;
  GameObject[] allFish;

  public Vector3 TankDimensions
  {
    get { return _tankDimensions; }

    set { _tankDimensions = value; }
  }

  public Vector3 SwimmingBounds
  {
    get { return _tankDimensions - _tankDimensions / 2 - new Vector3(_tankMargin, _tankMargin, _tankMargin); }
  }

  void Start()
  {
    _debugTank = transform.Find("Tank");
    SpawnFish();
  }

  void Update()
  {
    _debugTank.localScale = TankDimensions;
    _debugTank.gameObject.SetActive(_debugTankEnabled);
  }

  public GameObject[] GetAllFish()
  {
    return allFish;
  }

  private void SpawnFish()
  {
    allFish = new GameObject[_fishCount];
    for (int i = 0; i < _fishCount; i++)
    {
      Vector3 spawnPosition = new Vector3(Random.Range(-SwimmingBounds.x, SwimmingBounds.x),
                                      Random.Range(-SwimmingBounds.y, SwimmingBounds.y),
                                      Random.Range(-SwimmingBounds.z, SwimmingBounds.z));
      allFish[i] = GameObject.Instantiate(_fishPrefab, spawnPosition, Quaternion.identity, transform);
    }
  }
}

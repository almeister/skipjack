using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SchoolManager : MonoBehaviour
{
  [SerializeField] GameObject fishPrefab;
  [SerializeField] int fishCount = 15;
  [SerializeField] bool debugTank = false;

  public Vector3Int tankDimensions = new(5, 4, 2);
  [Range(0, 2)] public float tankMargin = 0.5f;
  public float tankAvoidanceFactor = 0.5f;
  public float turningFactor = 2f;
  [Range(0, 50)] public float visibilityRange = 1.5f;
  [Range(0, 50)] public float centeringFactor = 4.0f;

  public Vector3 targetPosition = Vector3.zero;

  GameObject[] allFish;

  // private Vector3 schoolCentre = Vector3.zero;

  void Start()
  {
    SpawnFish();
  }

  void Update()
  {
    if (Random.Range(0, 10000) < 50)
    {
      targetPosition = new Vector3(Random.Range(-tankDimensions.x, tankDimensions.x),
                                    Random.Range(-tankDimensions.y, tankDimensions.y),
                                    Random.Range(-tankDimensions.z, tankDimensions.z));
    }

    if (debugTank)
    {
      Transform tank = transform.Find("Tank");
      tank.gameObject.SetActive(debugTank);
      tank.localScale = tankDimensions * 2;
    }
  }

  public GameObject[] GetAllFish()
  {
    return allFish;
  }

  private void SpawnFish()
  {
    allFish = new GameObject[fishCount];
    for (int i = 0; i < fishCount; i++)
    {
      Vector3 position = new Vector3(Random.Range(-tankDimensions.x, tankDimensions.x),
                                      Random.Range(-tankDimensions.y, tankDimensions.y),
                                      Random.Range(-tankDimensions.z, tankDimensions.z));
      allFish[i] = GameObject.Instantiate(fishPrefab, position, Quaternion.identity, transform);
    }
  }

  // public Vector3 GetTankCentre() {
  //   return tankDimensions;
  // }

  // private void CalculateCentre()
  // {
  //   Vector3 sum = Vector3.zero;
  //   foreach (GameObject fish in allFish)
  //   {
  //     sum += fish.transform.position;
  //   }
  //   schoolCentre = sum / allFish.Length;
  // }

}

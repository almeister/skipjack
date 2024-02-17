using UnityEngine;

public class SchoolManager : MonoBehaviour
{
  [SerializeField] SchoolAttributesScriptableObject schoolAttributes;

  public GameObject[] AllFish { get; set; }

  Transform debugTankTransform;

  public Vector3 TankDimensions
  {
    get { return schoolAttributes.tankDimensions; }
  }

  public Vector3 SwimmingBounds
  {
    get { return schoolAttributes.tankDimensions - schoolAttributes.tankDimensions / 2 - new Vector3(schoolAttributes.tankMargin, schoolAttributes.tankMargin, schoolAttributes.tankMargin); }
  }

  void Start()
  {
    debugTankTransform = transform.Find("Tank");
    SpawnFish();
  }

  void Update()
  {
    ProcessDebugFeatures();
  }

  private void ProcessDebugFeatures()
  {
    debugTankTransform.localScale = TankDimensions;
    debugTankTransform.gameObject.SetActive(schoolAttributes.debugTank);
  }

  private void SpawnFish()
  {
    AllFish = new GameObject[schoolAttributes.fishCount];
    for (int i = 0; i < schoolAttributes.fishCount; i++)
    {
      Vector3 spawnPosition = new Vector3(Random.Range(-SwimmingBounds.x, SwimmingBounds.x),
                                      Random.Range(-SwimmingBounds.y, SwimmingBounds.y),
                                      Random.Range(-SwimmingBounds.z, SwimmingBounds.z));
      AllFish[i] = GameObject.Instantiate(schoolAttributes.fishPrefab, spawnPosition, Quaternion.identity, transform);
    }
  }
}

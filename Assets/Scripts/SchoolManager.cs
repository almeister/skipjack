using UnityEngine;

public class SchoolManager : MonoBehaviour
{
  [SerializeField] SchoolAttributesScriptableObject schoolAttributes;

  public GameObject[] AllFish { get; set; }

  public GameObject debugTank;
  public GameObject debugSwimmingBounds;

  public Bounds SwimmingBounds { get { return new Bounds(transform.position, TankDimensions - 2 * schoolAttributes.tankMargin * Vector3.one); } }

  Vector3 TankDimensions
  {
    get { return schoolAttributes.tankDimensions; }
  }

  void Start()
  {
    SpawnFish();
  }

  void Update()
  {
    ProcessDebugFeatures();
  }

  public void DisturbFish(Vector3 worldPosition)
  {
    foreach (GameObject fishGameObject in AllFish)
    {
      Fish fish = fishGameObject.GetComponent<Fish>();
      fish.Flee(worldPosition);
    }
  }

  private void ProcessDebugFeatures()
  {
    debugTank.transform.localScale = TankDimensions;
    debugTank.SetActive(schoolAttributes.showDebugTank);

    debugSwimmingBounds.transform.localScale = SwimmingBounds.size;
    debugSwimmingBounds.SetActive(schoolAttributes.showDebugTank);
  }

  private void SpawnFish()
  {
    AllFish = new GameObject[schoolAttributes.fishCount];
    for (int i = 0; i < schoolAttributes.fishCount; i++)
    {
      Vector3 spawnPosition = new Vector3(Random.Range(SwimmingBounds.min.x, SwimmingBounds.max.x),
                                      Random.Range(SwimmingBounds.min.y, SwimmingBounds.max.y),
                                      Random.Range(SwimmingBounds.min.z, SwimmingBounds.max.z));
      AllFish[i] = GameObject.Instantiate(schoolAttributes.fishPrefab, spawnPosition, Quaternion.identity, transform);
    }
  }
}

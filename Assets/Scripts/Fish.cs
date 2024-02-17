using UnityEngine;


public class Fish : MonoBehaviour
{
  public Vector3 Velocity { get; set; } = Vector3.zero;

  [SerializeField] FishAttributesScriptableObject fishAttributes;

  SchoolManager schoolManager;

  GameObject debugSeparationSphere;
  GameObject debugAlignmentSphere;

  void Start()
  {
    schoolManager = transform.parent.GetComponent<SchoolManager>();
    Velocity = new Vector3(Random.Range(fishAttributes.minSpeed, fishAttributes.maxSpeed), 0, 0);

    debugSeparationSphere = transform.Find("DebugSeparationSphere").gameObject;
    debugAlignmentSphere = transform.Find("DebugAlignmentSphere").gameObject;
  }

  void Update()
  {
    ShowDebugFeatures();

    AvoidOthers();
    AlignWithOthers();
    AvoidBounds();

    Velocity = Vector3.ClampMagnitude(Velocity, fishAttributes.maxSpeed);
    if (Velocity == Vector3.zero)
    {
      Debug.LogAssertion("Velocity is zero");
    }

    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Velocity), fishAttributes.turnFactor * Time.deltaTime);
    transform.Translate(Velocity * Time.deltaTime, Space.World);
  }

  private void AvoidBounds()
  {
    // Avoid tank walls
    if (transform.position.x > schoolManager.SwimmingBounds.x)
    {
      Velocity -= new Vector3(fishAttributes.wallAvoidanceFactor * Time.deltaTime, 0, 0);
    }

    if (transform.position.y > schoolManager.SwimmingBounds.y)
    {
      Velocity -= new Vector3(0, fishAttributes.wallAvoidanceFactor * Time.deltaTime, 0);
    }

    if (transform.position.z > schoolManager.SwimmingBounds.z)
    {
      Velocity -= new Vector3(0, 0, fishAttributes.wallAvoidanceFactor * Time.deltaTime);
    }

    if (transform.position.x < -schoolManager.SwimmingBounds.x)
    {
      Velocity += new Vector3(fishAttributes.wallAvoidanceFactor * Time.deltaTime, 0, 0);
    }

    if (transform.position.y < -schoolManager.SwimmingBounds.y)
    {
      Velocity += new Vector3(0, fishAttributes.wallAvoidanceFactor * Time.deltaTime, 0);
    }

    if (transform.position.z < -schoolManager.SwimmingBounds.z)
    {
      Velocity += new Vector3(0, 0, fishAttributes.wallAvoidanceFactor * Time.deltaTime);
    }
  }

  private void AlignWithOthers()
  {
    // Alignment
    int visibleFishCount = 0;
    Vector3 averageVelocity = Vector3.zero;
    foreach (GameObject fishGameObject in schoolManager.AllFish)
    {
      if ((transform.position - fishGameObject.transform.position).magnitude <= fishAttributes.alignmentRange)
      {
        visibleFishCount += 1;
        Fish fish = fishGameObject.GetComponent<Fish>();
        averageVelocity += fish.Velocity;
      }

    }

    if (visibleFishCount > 0)
    {
      averageVelocity /= visibleFishCount;
    }

    Velocity += (averageVelocity - Velocity) * fishAttributes.alignmentFactor;
  }

  private void AvoidOthers()
  {
    // Separation
    Vector3 distanceSum = Vector3.zero;
    foreach (GameObject fish in schoolManager.AllFish)
    {
      Vector3 distanceToFish = transform.position - fish.transform.position;
      if (distanceToFish.magnitude <= fishAttributes.separationRange)
      {
        distanceSum += distanceToFish;
      }
    }

    Velocity += distanceSum * fishAttributes.separationFactor;
  }

  private void ShowDebugFeatures()
  {
    if (fishAttributes.debugPointer)
    {
      Debug.DrawRay(transform.position, transform.forward, Color.yellow);
    }

    debugSeparationSphere.SetActive(fishAttributes.debugSeparation);
    debugSeparationSphere.transform.localScale = fishAttributes.separationRange * 2 * Vector3.one;

    debugAlignmentSphere.SetActive(fishAttributes.debugAlignment);
    debugAlignmentSphere.transform.localScale = fishAttributes.alignmentRange * 2 * Vector3.one;
  }
}

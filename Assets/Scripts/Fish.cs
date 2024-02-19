using UnityEngine;

public class Fish : MonoBehaviour
{
  public Vector3 Velocity { get; set; } = Vector3.zero;

  public enum BiasGroup
  {
    Lefties = 0,
    Righties = 1
  }
  public BiasGroup Group { get; set; }
  public float BiasFactor { get; set; }

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

    Group = Random.Range(0, 1f) < 0.5f ? BiasGroup.Lefties : BiasGroup.Righties;
    BiasFactor = Random.Range(fishAttributes.minBiasFactor, fishAttributes.maxBiasFactor);
  }

  void Update()
  {
    ShowDebugFeatures();

    AvoidOthers();
    AlignWithOthers();
    Cohere();
    MoveWithBiasGroup();
    AvoidBounds();

    Velocity = Vector3.ClampMagnitude(Velocity, fishAttributes.maxSpeed);
    if (Velocity.magnitude < fishAttributes.minSpeed)
    {
      Velocity *= fishAttributes.minSpeed;
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
    // Alignment - move in the same direction as visible fish
    int visibleFishCount = 0;
    Vector3 averageVelocity = Vector3.zero;
    foreach (GameObject fishGameObject in schoolManager.AllFish)
    {
      if ((transform.position - fishGameObject.transform.position).magnitude <= fishAttributes.visibleRange)
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

  private void Cohere()
  {
    // Cohesion - move gradually towards the centre of mass of visible fish
    int visibleFishCount = 0;
    Vector3 centreOfMass = Vector3.zero;
    foreach (GameObject fishGameObject in schoolManager.AllFish)
    {
      if ((transform.position - fishGameObject.transform.position).magnitude <= fishAttributes.visibleRange)
      {
        visibleFishCount += 1;
        centreOfMass += fishGameObject.transform.position;
      }
    }

    if (visibleFishCount > 0)
    {
      centreOfMass /= visibleFishCount;
    }

    Velocity += (centreOfMass - transform.position) * fishAttributes.centeringFactor;
  }

  private void MoveWithBiasGroup()
  {
    if (this.Group == BiasGroup.Lefties)
    {
      Velocity = new Vector3(Velocity.x - BiasFactor, Velocity.y, Velocity.z);
    }
    else if (this.Group == BiasGroup.Righties)
    {
      Velocity = new Vector3(Velocity.x + BiasFactor, Velocity.y, Velocity.z);
    }
  }

  private void AvoidOthers()
  {
    // Separation - avoid other fish
    Vector3 distanceSum = Vector3.zero;
    foreach (GameObject fish in schoolManager.AllFish)
    {
      Vector3 distanceToOther = transform.position - fish.transform.position;
      if (distanceToOther.magnitude <= fishAttributes.separationRange)
      {
        distanceSum += distanceToOther;
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
    debugAlignmentSphere.transform.localScale = fishAttributes.visibleRange * 2 * Vector3.one;
  }
}

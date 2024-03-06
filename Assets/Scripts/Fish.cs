using UnityEngine;

public class Fish : MonoBehaviour
{
  [SerializeField] FishAttributesScriptableObject fishAttributes;

  SchoolManager schoolManager;

  Vector3 velocity = Vector3.zero;

  GameObject debugSeparationSphere;
  GameObject debugAlignmentSphere;

  float coolDownTime = 0f;
  float coolDownPeriod = 0f;

  void Start()
  {
    schoolManager = transform.parent.GetComponent<SchoolManager>();
    velocity = new Vector3(-Random.Range(fishAttributes.minSpeed, fishAttributes.maxSpeed), 0, 0);

    debugSeparationSphere = transform.Find("PivotContainer/DebugSeparationSphere").gameObject;
    debugAlignmentSphere = transform.Find("PivotContainer/DebugAlignmentSphere").gameObject;

    transform.localScale = Vector3.one * Random.Range(fishAttributes.minScale, fishAttributes.maxScale);
  }

  void Update()
  {
    ShowDebugFeatures();

    ProcessFlockingAterCooldown();

    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(velocity), fishAttributes.turnFactor * Time.deltaTime);
    transform.Translate(velocity * Time.deltaTime, Space.World);
  }

  public void Flee(Vector3 pos)
  {
    Vector3 distanceToThreat = transform.position - pos;
    velocity += fishAttributes.fleeFactor * distanceToThreat.normalized;
  }

  private void ProcessFlockingAterCooldown()
  {
    coolDownTime += Time.deltaTime;
    if (coolDownTime >= coolDownPeriod)
    {
      AvoidOthers();
      AlignWithOthers();
      Cohere();
      AvoidBounds();
      ClampSpeed();

      coolDownPeriod = Random.Range(fishAttributes.minCoolDown, fishAttributes.maxCoolDown);
    }
  }

  private void ClampSpeed()
  {
    // Squared magnitude used for performance https://docs.unity3d.com/ScriptReference/Vector3-sqrMagnitude.html
    double squaredMag = velocity.sqrMagnitude;
    if (squaredMag > (double)fishAttributes.maxSpeed * (double)fishAttributes.maxSpeed)
    {
      velocity = velocity.normalized * fishAttributes.maxSpeed;
    }
    else if (squaredMag < (double)fishAttributes.minSpeed * (double)fishAttributes.minSpeed)
    {
      velocity = velocity.normalized * fishAttributes.minSpeed;
    }
  }

  private void AvoidBounds()
  {
    // Avoid tank walls
    if (transform.position.x > schoolManager.SwimmingBounds.max.x)
    {
      velocity -= new Vector3(fishAttributes.wallAvoidanceFactor * Time.deltaTime, 0, 0);
    }

    if (transform.position.y > schoolManager.SwimmingBounds.max.y)
    {
      velocity -= new Vector3(0, fishAttributes.wallAvoidanceFactor * Time.deltaTime, 0);
    }

    if (transform.position.z > schoolManager.SwimmingBounds.max.z)
    {
      velocity -= new Vector3(0, 0, fishAttributes.wallAvoidanceFactor * Time.deltaTime);
    }

    if (transform.position.x < schoolManager.SwimmingBounds.min.x)
    {
      velocity += new Vector3(fishAttributes.wallAvoidanceFactor * Time.deltaTime, 0, 0);
    }

    if (transform.position.y < schoolManager.SwimmingBounds.min.y)
    {
      velocity += new Vector3(0, fishAttributes.wallAvoidanceFactor * Time.deltaTime, 0);
    }

    if (transform.position.z < schoolManager.SwimmingBounds.min.z)
    {
      velocity += new Vector3(0, 0, fishAttributes.wallAvoidanceFactor * Time.deltaTime);
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
        averageVelocity += fish.velocity;
      }
    }

    if (visibleFishCount > 0)
    {
      averageVelocity /= visibleFishCount;
      velocity += (averageVelocity - velocity) * fishAttributes.alignmentFactor;
    }

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
      velocity += (centreOfMass - transform.position) * fishAttributes.centeringFactor;
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

    velocity += distanceSum * fishAttributes.separationFactor;
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

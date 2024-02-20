using UnityEngine;

public class Fish : MonoBehaviour
{
  public Vector3 velocity = Vector3.zero;
  public Vector3 Velocity { get { return this.velocity; } set { this.velocity = value; } }

  [SerializeField] FishAttributesScriptableObject fishAttributes;

  SchoolManager schoolManager;

  GameObject debugSeparationSphere;
  GameObject debugAlignmentSphere;

  float coolDownTime = 0f;
  float coolDownPeriod = 0f;

  void Start()
  {
    schoolManager = transform.parent.GetComponent<SchoolManager>();
    Velocity = new Vector3(-Random.Range(fishAttributes.minSpeed, fishAttributes.maxSpeed), 0, 0);

    debugSeparationSphere = transform.Find("PivotContainer/DebugSeparationSphere").gameObject;
    debugAlignmentSphere = transform.Find("PivotContainer/DebugAlignmentSphere").gameObject;
  }

  void Update()
  {
    ShowDebugFeatures();

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

    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Velocity), fishAttributes.turnFactor * Time.deltaTime);
    transform.Translate(Velocity * Time.deltaTime, Space.World);
  }

  private void ClampSpeed()
  {
    // Squared magnitude used for speed https://docs.unity3d.com/ScriptReference/Vector3-sqrMagnitude.html
    double squaredMag = Velocity.sqrMagnitude;
    if (squaredMag > (double)fishAttributes.maxSpeed * (double)fishAttributes.maxSpeed)
    {
      Velocity = Velocity.normalized * fishAttributes.maxSpeed;
    }
    else if (squaredMag < (double)fishAttributes.minSpeed * (double)fishAttributes.minSpeed)
    {
      Velocity = Velocity.normalized * fishAttributes.minSpeed;
    }
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
      Velocity += (averageVelocity - Velocity) * fishAttributes.alignmentFactor;
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
      Velocity += (centreOfMass - transform.position) * fishAttributes.centeringFactor;
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

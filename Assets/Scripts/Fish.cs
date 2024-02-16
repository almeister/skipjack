using TreeEditor;
using UnityEngine;


public class Fish : MonoBehaviour
{
  public Vector3 Velocity { get; set; } = Vector3.zero;

  [SerializeField] FishAttributesScriptableObject fishAttributes;

  SchoolManager schoolManager;

  void Start()
  {
    schoolManager = transform.parent.GetComponent<SchoolManager>();
    Velocity = new Vector3(0, 0, Random.Range(fishAttributes.minSpeed, fishAttributes.maxSpeed));
  }

  void Update()
  {
    // Separation
    Vector3 distanceToOtherFish = Vector3.zero;
    foreach (GameObject fish in schoolManager.GetAllFish())
    {
      Vector3 distanceToFish = fish.transform.position - transform.position;
      if (distanceToFish.magnitude <= fishAttributes.separationRange)
      {
        distanceToOtherFish += distanceToFish;
      }
    }

    Velocity += distanceToOtherFish * fishAttributes.separationFactor;
    // Debug.Log($"Separation velocity: {Velocity}");

    // Alignment
    int visibleFishCount = 0;
    Vector3 averageVelocity = Vector3.zero;
    foreach (GameObject fishGameObject in schoolManager.GetAllFish())
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
    // Debug.Log($"Alignment velocity: {Velocity}");

    // Avoid tank walls
    if (transform.position.x > schoolManager.SwimmingBounds.x)
    {
      Velocity -= new Vector3(0, 0, fishAttributes.turnFactor * Time.deltaTime);
    }
    if (transform.position.x < -schoolManager.SwimmingBounds.x)
    {
      Velocity += new Vector3(0, 0, fishAttributes.turnFactor * Time.deltaTime);
    }
    if (transform.position.y > schoolManager.SwimmingBounds.y)
    {
      Velocity -= new Vector3(0, 0, fishAttributes.turnFactor * Time.deltaTime);
    }
    if (transform.position.y < -schoolManager.SwimmingBounds.y)
    {
      Velocity += new Vector3(0, 0, fishAttributes.turnFactor * Time.deltaTime);
    }

    if (transform.position.z > schoolManager.SwimmingBounds.z)
    {
      Velocity -= new Vector3(0, 0, fishAttributes.turnFactor * Time.deltaTime);
    }
    if (transform.position.z < -schoolManager.SwimmingBounds.z)
    {
      Velocity += new Vector3(0, 0, fishAttributes.turnFactor * Time.deltaTime);
    }
    Debug.Log($"Wall Avoidance velocity: {Velocity}");

    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Velocity), fishAttributes.turnFactor * Time.deltaTime);
    transform.Translate(Velocity * Time.deltaTime, Space.World);
    Debug.DrawRay(transform.position, transform.forward, Color.yellow);
  }

}

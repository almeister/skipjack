using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class Fish : MonoBehaviour
{
  public SchoolManager schoolManager;
  public float speed = 0f;

  readonly float minSpeed = 2f;
  readonly float maxSpeed = 5f;
  readonly float turningTime = 0.5f;

  void Start()
  {
    schoolManager = transform.parent.GetComponent<SchoolManager>();
    speed = Random.Range(minSpeed, maxSpeed);
  }

  void Update()
  {
    if (Random.Range(0, 5) < 1)
    {
      ApplyRules();
    }

    AvoidTankWalls();

    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(schoolManager.targetPosition), schoolManager.turningFactor * Time.deltaTime);
    transform.Translate(transform.forward * Time.deltaTime * speed, Space.World);

    Debug.DrawRay(transform.position, transform.forward, Color.yellow);
  }

  private void ApplyRules()
  {
    // CohereToGroupCentre();

    // Turn towards heading of group

    // Avoid collisions with other fish
  }

  // Cohesion
  // Each bird moves towards the centre of mass of other birds
  // within it's range of visibility
  private void CohereToGroupCentre()
  {
    // Get in-range fish
    int visibleCount = 0;
    Vector3 positionSum = Vector3.zero;
    foreach (GameObject fish in schoolManager.GetAllFish())
    {
      if ((transform.position - fish.transform.position).magnitude < schoolManager.visibilityRange)
      {
        positionSum += fish.transform.position;
        visibleCount++;
      }
    }

    if (visibleCount > 0)
    {
      // Calculate centre of mass
      Vector3 centreOfMass = positionSum / visibleCount;
      // Move towards centre of mass
      transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(centreOfMass), Time.deltaTime * schoolManager.centeringFactor);
    }
  }

  private void AvoidTankWalls()
  {
    float step = schoolManager.tankAvoidanceFactor * Time.deltaTime;
    if (Mathf.Abs(transform.position.x) > schoolManager.tankDimensions.x - schoolManager.tankMargin)
    {
      transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-transform.position), schoolManager.tankAvoidanceFactor);
      // transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Inverse(transform.rotation), step);
    }

    if (Mathf.Abs(transform.position.y) > schoolManager.tankDimensions.y - schoolManager.tankMargin)
    {
      transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-transform.position), schoolManager.tankAvoidanceFactor);
      // transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Inverse(transform.rotation), step);
    }

    if (Mathf.Abs(transform.position.z) > schoolManager.tankDimensions.z - schoolManager.tankMargin)
    {
      transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-transform.position), schoolManager.tankAvoidanceFactor);
      // transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Inverse(transform.rotation), step);
    }
  }
}

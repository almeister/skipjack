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

  Vector3 currentTarget = Vector3.zero;
  float timeInTurn = 0f;

  void Start()
  {
    schoolManager = transform.parent.GetComponent<SchoolManager>();
    speed = Random.Range(minSpeed, maxSpeed);
  }

  void Update()
  {
    AvoidOtherFish();
    AlignWithOtherFish();
    // CohereToGroupCentre();
    AvoidTankWalls();

    transform.Translate(transform.forward * Time.deltaTime * speed, Space.World);
    Debug.DrawRay(transform.position, transform.forward, Color.yellow);
  }

  private void AvoidOtherFish()
  {
    Vector3 sumDeltaToOtherFish = Vector3.zero;
    foreach (GameObject fish in schoolManager.GetAllFish())
    {
      if ((transform.position - fish.transform.position).magnitude <= schoolManager.separationRange)
      {
        sumDeltaToOtherFish += fish.transform.position;
      }
    }

    float dotProduct = Vector3.Dot(transform.forward, sumDeltaToOtherFish.normalized);
    speed += dotProduct * schoolManager.separationFactor;
  }

  private void AlignWithOtherFish()
  {
    Vector3 sumDeltaToOtherFish = Vector3.zero;
    int neighbouringFish = 0;
    foreach (GameObject fish in schoolManager.GetAllFish())
    {
      if ((transform.position - fish.transform.position).magnitude <= schoolManager.visibilityRange)
      {
        sumDeltaToOtherFish += fish.transform.position;
        neighbouringFish += 1;
      }
    }

    if (neighbouringFish > 0)
    {
      float averageSpeed = sumDeltaToOtherFish.magnitude / neighbouringFish;
      speed += (averageSpeed - speed) * schoolManager.alignmentFactor;
    }

    // if (!schoolManager.targetPosition.Equals(currentTarget))
    // {
    //   timeInTurn = 0f;
    //   currentTarget = schoolManager.targetPosition;
    // }

    // timeInTurn += Time.deltaTime;
    // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(schoolManager.targetPosition), timeInTurn / schoolManager.timeToTurn);
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
      if ((transform.position - fish.transform.position).magnitude <= schoolManager.visibilityRange)
      {
        positionSum += fish.transform.position;
        visibleCount++;
      }
    }

    if (visibleCount > 0)
    {
      // Calculate centre of mass
      Vector3 centreOfMass = positionSum / visibleCount;
      speed += Vector3.Dot(transform.forward, centreOfMass.normalized) * schoolManager.centeringFactor;
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
    }

    if (Mathf.Abs(transform.position.y) > schoolManager.tankDimensions.y - schoolManager.tankMargin)
    {
      transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-transform.position), schoolManager.tankAvoidanceFactor);
    }

    if (Mathf.Abs(transform.position.z) > schoolManager.tankDimensions.z - schoolManager.tankMargin)
    {
      transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-transform.position), schoolManager.tankAvoidanceFactor);
    }
  }
}

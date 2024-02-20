using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/FishAttributesScriptableObject", order = 1)]
public class FishAttributesScriptableObject : ScriptableObject
{
  public bool debugPointer = false;
  public bool debugSeparation = false;
  public bool debugAlignment = false;

  [Range(0, 1f)] public float minCoolDown = 0.2f;
  [Range(0, 1f)] public float maxCoolDown = 0.6f;

  [Range(0, 5f)] public float minSpeed = 2f;
  [Range(0, 25f)] public float maxSpeed = 5f;

  [Range(0, 100f)] public float turnFactor = 50f;

  [Range(0, 5f)] public float separationRange = 2f;
  [Range(0, 100f)] public float separationFactor = 10f;

  [Range(0, 5f)] public float visibleRange = 2.5f;
  [Range(0, 0.5f)] public float alignmentFactor = 0.01f;
  [Range(0, 10f)] public float centeringFactor = 1f;

  [Range(0, 1000f)] public float wallAvoidanceFactor = 2f;

  [Range(1f, 1.2f)] public float leftRightBias = 1.01f;
}

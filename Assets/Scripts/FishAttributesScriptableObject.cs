using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/FishAttributesScriptableObject", order = 1)]
public class FishAttributesScriptableObject : ScriptableObject
{
  public bool debugPointer = false;
  public bool debugSeparation = false;
  public bool debugAlignment = false;

  [Range(0, 5f)] public float minSpeed = 2f;
  [Range(0, 25f)] public float maxSpeed = 5f;

  [Range(0, 100f)] public float turnFactor = 50f;

  [Range(0, 5f)] public float separationRange = 2f;
  [Range(0, 10f)] public float separationFactor = 1f;

  [Range(0, 5f)] public float visibleRange = 2.5f;
  [Range(0, 10f)] public float alignmentFactor = 1f;
  [Range(0, 10f)] public float centeringFactor = 1f;

  [Range(0, 1000f)] public float wallAvoidanceFactor = 2f;

  [Range(0f, 0.1f)] public float minBiasFactor = 0.001f;
  [Range(0f, 1f)] public float maxBiasFactor = 0.05f;
}

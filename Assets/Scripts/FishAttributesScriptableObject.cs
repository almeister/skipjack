using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/FishAttributesScriptableObject", order = 1)]
public class FishAttributesScriptableObject : ScriptableObject
{
  public bool debugPointer = false;
  public bool debugSeparation = false;
  public bool debugAlignment = false;

  [Range(0.1f, 1f)] public float minScale = 0.75f;
  [Range(1f, 5f)] public float maxScale = 1.8f;

  [Range(0, 1f)] public float minCoolDown = 0.2f;
  [Range(0, 1f)] public float maxCoolDown = 0.6f;

  [Range(0, 5f)] public float minSpeed = 2f;
  [Range(0, 25f)] public float maxSpeed = 5f;

  [Range(0, 100f)] public float turnFactor = 50f;

  [Range(0, 5f)] public float separationRange = 2f;
  [Range(0, 100f)] public float separationFactor = 10f;

  [Range(0, 5f)] public float visibleRange = 2.5f;
  [Range(0, 0.1f)] public float alignmentFactor = 0.01f;
  [Range(0, 0.1f)] public float centeringFactor = 0.01f;

  [Range(0, 1000f)] public float wallAvoidanceFactor = 2f;

  [Range(0, 100f)] public float fleeFactor = 10f;
}

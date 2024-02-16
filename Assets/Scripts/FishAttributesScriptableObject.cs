using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/FishAttributesScriptableObject", order = 1)]
public class FishAttributesScriptableObject : ScriptableObject
{
  public bool debugMode = false;

  [Range(0, 5f)] public float minSpeed = 2f;
  [Range(0, 5f)] public float maxSpeed = 5f;

  [Range(0, 100f)] public float turnFactor = 50f;

  [Range(0, 5f)] public float separationRange = 0.5f;
  [Range(0, 1f)] public float separationFactor = 0.25f;

  [Range(0, 5f)] public float alignmentRange = 1.0f;
  [Range(0, 1f)] public float alignmentFactor = 0.5f;

  [Range(0, 100f)] public float wallAvoidanceFactor = 2f;

}

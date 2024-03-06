using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SchoolAttributesScriptableObject", order = 1)]
public class SchoolAttributesScriptableObject : ScriptableObject
{
  public bool showDebugTank = false;

  public GameObject fishPrefab;
  [Range(1, 100)] public int fishCount = 15;

  public Vector3 tankDimensions = new(10f, 8f, 4f);
  [Range(0, 2)] public float tankMargin = 0.5f;

}

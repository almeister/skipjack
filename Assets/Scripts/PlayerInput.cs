using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInput : MonoBehaviour
{
  [SerializeField] GameObject school;
  [SerializeField] LayerMask layerMask;

  private SchoolManager schoolManager;
  private Ray ray;

  void Start()
  {
    schoolManager = school.GetComponent<SchoolManager>();
  }

  void OnMouseDown()
  {
    if (Input.GetMouseButtonDown((int)MouseButton.LeftMouse))
    {
      ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      if (Physics.Raycast(ray, out RaycastHit hitData, 1000f))
      {
        schoolManager.DisturbFish(hitData.point);
      }
    }
  }
}

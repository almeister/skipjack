using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Swimmer : MonoBehaviour
{
  [SerializeField] Transform leftBound;
  [SerializeField] Transform rightBound;

  Tweener tweener;

  // Start is called before the first frame update
  void Start()
  {
    SwimTo(leftBound.position);
  }

  private void SwimTo(Vector3 toPos)
  {
    tweener = transform.DOMove(toPos, 2);
    tweener.OnComplete(turnAround);
  }

  // Update is called once per frame
  void Update()
  {

  }

  private void turnAround()
  {
    Debug.Log("Here I come!");
    tweener = transform.DORotate(new Vector3(0, 180, 0), 0.5f);
    tweener.OnComplete(() => SwimTo(rightBound.position));
  }


}

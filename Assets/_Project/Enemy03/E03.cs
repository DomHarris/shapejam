using UnityEngine;

public class E03 : MonoBehaviour
{
    [SerializeField] private Transform lazerParent;

    [SerializeField] private float rotationSpeed;

    private bool _canMove = true;

    private void Update()
    {
        if (_canMove) RotateParent();
    }

    void RotateParent()
    {
        lazerParent.rotation = Quaternion.Euler(new Vector3(0, 0, rotationSpeed));
    }
}

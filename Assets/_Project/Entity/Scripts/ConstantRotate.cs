using UnityEngine;

namespace Entity
{
    public class ConstantRotate : MonoBehaviour
    {
        [SerializeField] private float speed = 100;

        // Update is called once per frame
        private void Update()
        {
            transform.Rotate(0,0,speed * Time.deltaTime);
        }
    }
}

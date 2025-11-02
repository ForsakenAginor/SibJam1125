using UnityEngine;

namespace Assets.Source.Scripts.Game
{

    public class PlayerTest : MonoBehaviour
	{
        [SerializeField] private Light light;

        Vector2 rotation = Vector2.zero;
        public float speed = 3.0f;

        private bool rotateEnabled = true;

        public float CurrentSpeed;


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                light.enabled = !light.enabled;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                rotateEnabled = !rotateEnabled;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                DirtPainter.Instance.StartClean();
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                DirtPainter.Instance.FillAll_Test();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                DirtPainter.Instance.FillAll();
            }

            RotateWithMouse();
            Movement();
        }

        private void Movement()
        {
            Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            direction = Vector3.ClampMagnitude(direction, 1);

            Vector3 rightMovement = transform.right * direction.x;
            Vector3 forwardMovement = transform.forward * direction.z;

            Vector3 movement = (rightMovement + forwardMovement).normalized * speed * Time.deltaTime;

            CurrentSpeed = movement.magnitude;

            transform.position += movement;
        }

        private void RotateWithMouse()
        {
            if (!rotateEnabled)
                return;

            rotation.x += Input.GetAxis("Mouse X") * speed;
            rotation.y += Input.GetAxis("Mouse Y") * speed;
            rotation.y = Mathf.Clamp(rotation.y, -90f, 90f); // Optional: Clamp vertical rotation

            transform.localRotation = Quaternion.Euler(-rotation.y, rotation.x, 0);
        }

    }


}
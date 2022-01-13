using UnityEngine;

namespace Assets.Code
{
    [RequireComponent(typeof(CharacterController))]
    public class Movement:MonoBehaviour
    {
        [SerializeField] private Joystick _joystick;
        [SerializeField] private float _speed;
        [SerializeField] private float _speedRotation;
        private CharacterController _characterController;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }
     
        private void Update()
        {
            Vector3 movementVector = Vector3.zero;

            if (_joystick.Direction.sqrMagnitude > 0.001f)
            {
                movementVector = Camera.main.transform.TransformDirection(_joystick.Direction);
                movementVector.y = 0;
                movementVector.Normalize();

                transform.forward = movementVector;
            }

            movementVector += Physics.gravity;

            _characterController.Move(_speed * movementVector * Time.deltaTime);
        }
    }
}
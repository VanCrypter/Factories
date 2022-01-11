using UnityEngine;

namespace Assets.Code
{
    [RequireComponent(typeof(CharacterController))]
    public class Movement:MonoBehaviour
    {
        [SerializeField] private Joystick _joystick;
        [SerializeField] private float _speed;

        private CharacterController _characterController;
        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }
        private void Update()
        {
            _characterController.Move(new Vector3(_joystick.Direction.x, 0, _joystick.Direction.y) * _speed * Time.deltaTime);

        }
    }
}
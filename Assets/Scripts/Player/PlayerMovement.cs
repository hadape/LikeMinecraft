using Assets.Scripts;
using Assets.Scripts.Player.Classes;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private CharacterController _controller;

    [SerializeField]
    private Transform _groundCheck;

    [SerializeField]
    private MovementSettings _movementSettings = new MovementSettings();

    Vector3 _velocity;
    bool _isGrounded;

    // Update is called once per frame
    void Update()
    {
        _isGrounded = Physics.CheckSphere(_groundCheck.position, _movementSettings.GroundDistance, _movementSettings.GroundMask);

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        float x = Input.GetAxis(Constants.HORIZONTAL);
        float z = Input.GetAxis(Constants.VERTICAL);

        Vector3 move = transform.right * x + transform.forward * z;

        _controller.Move(move * _movementSettings.Speed * Time.deltaTime);

        if (Input.GetButtonDown(Constants.JUMP) && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt(_movementSettings.JumpHeight * -2f * _movementSettings.Gravity);
        }

        _velocity.y += _movementSettings.Gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }
}

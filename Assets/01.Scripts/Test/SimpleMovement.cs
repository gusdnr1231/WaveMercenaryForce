using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SimpleMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // �̵� �ӵ�
    public float gravity = -9.81f; // �߷�

    private CharacterController _controller;
    private Vector3 _velocity; // ���� �ӵ� (�߷� ����)
    private bool _isGrounded;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // �ٴ� üũ: ĳ���Ͱ� �ٴڿ� �ִ��� Ȯ�� (CharacterController�� isGrounded ���)
        _isGrounded = _controller.isGrounded;

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f; // �ٴڿ� ���� �� Y �ӵ��� ���� (���� 0���� �ϸ� �Ҿ����� �� ����)
        }

        // �Է� ó��
        float moveX = Input.GetAxis("Horizontal"); // �¿� �̵� (A, D Ű)
        float moveZ = Input.GetAxis("Vertical"); // �յ� �̵� (W, S Ű)

        // ������ ���� ���� (���� ��ǥ�� ����)
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // ĳ���� �̵�
        _controller.Move(move * moveSpeed * Time.deltaTime);

        // �߷� ����
        _velocity.y += gravity * Time.deltaTime;

        // ĳ���Ϳ� �߷� ���� (Y�� �ӵ��� �ݿ�)
        _controller.Move(_velocity * Time.deltaTime);
    }
}

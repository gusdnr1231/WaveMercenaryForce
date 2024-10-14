using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SimpleMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // 이동 속도
    public float gravity = -9.81f; // 중력

    private CharacterController _controller;
    private Vector3 _velocity; // 현재 속도 (중력 포함)
    private bool _isGrounded;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 바닥 체크: 캐릭터가 바닥에 있는지 확인 (CharacterController의 isGrounded 사용)
        _isGrounded = _controller.isGrounded;

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f; // 바닥에 있을 때 Y 속도를 리셋 (완전 0으로 하면 불안정할 수 있음)
        }

        // 입력 처리
        float moveX = Input.GetAxis("Horizontal"); // 좌우 이동 (A, D 키)
        float moveZ = Input.GetAxis("Vertical"); // 앞뒤 이동 (W, S 키)

        // 움직일 방향 설정 (월드 좌표계 기준)
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // 캐릭터 이동
        _controller.Move(move * moveSpeed * Time.deltaTime);

        // 중력 적용
        _velocity.y += gravity * Time.deltaTime;

        // 캐릭터에 중력 적용 (Y축 속도만 반영)
        _controller.Move(_velocity * Time.deltaTime);
    }
}

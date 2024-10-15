using UnityEngine;

public class DragDrop : MonoBehaviour
{
    private bool isDragging = false;

    private CharacterController characterController;
    private Vector3 offset;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // 마우스 버튼이 눌리고 드래그 중일 때
        if (isDragging)
        {
            // 마우스의 월드 좌표를 가져온다
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.WorldToScreenPoint(transform.position).z;

            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

            // CharacterController의 이동
            characterController.enabled = false; // CharacterController 비활성화
            transform.position = new Vector3(worldPos.x, 0, worldPos.z);
            characterController.enabled = true; // CharacterController 활성화
        }
    }

    private void OnMouseDown()
    {
        isDragging = true;
        CameraManager.Instance.MoveCamera(1);

        // 오브젝트와의 거리 차이
        //offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        isDragging = false;
        CameraManager.Instance.MoveCamera(0);
    }
}

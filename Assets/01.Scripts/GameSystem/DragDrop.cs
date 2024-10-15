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
        // ���콺 ��ư�� ������ �巡�� ���� ��
        if (isDragging)
        {
            // ���콺�� ���� ��ǥ�� �����´�
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.WorldToScreenPoint(transform.position).z;

            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

            // CharacterController�� �̵�
            characterController.enabled = false; // CharacterController ��Ȱ��ȭ
            transform.position = new Vector3(worldPos.x, 0, worldPos.z);
            characterController.enabled = true; // CharacterController Ȱ��ȭ
        }
    }

    private void OnMouseDown()
    {
        isDragging = true;
        CameraManager.Instance.MoveCamera(1);

        // ������Ʈ���� �Ÿ� ����
        //offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        isDragging = false;
        CameraManager.Instance.MoveCamera(0);
    }
}

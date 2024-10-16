using UnityEngine;

public class DragDrop : MonoBehaviour
{
    [SerializeField] private LayerMask WhatIsGround;

    private bool isDragging = false;

    private MonoCharacter character;
    private CharacterController characterController;
    private Vector3 DefaultPosition;

    private RaycastHit hit;

    private void Awake()
    {
        character = GetComponent<MonoCharacter>();
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

            //Debug.Log(worldPos);
            Physics.Raycast(new Ray(worldPos, Vector3.down), out hit, 30f, WhatIsGround);
            Debug.Log(hit.collider);

            // CharacterController�� �̵�
            character.SetCharacterPosition(new Vector3(worldPos.x, hit.point.y, worldPos.z));
        }
    }

    private void OnMouseDown()
    {
        CameraManager.Instance.MoveCamera(1);
        DefaultPosition = transform.position;
        isDragging = true;

        characterController.enabled = false; // CharacterController ��Ȱ��ȭ
    }

    private void OnMouseUp()
    {
        if (hit.collider == null)
        {
            character.SetCharacterPosition(DefaultPosition);
        }
        isDragging = false;
        CameraManager.Instance.MoveCamera(0);

        characterController.enabled = true; // CharacterController Ȱ��ȭ
    }
}

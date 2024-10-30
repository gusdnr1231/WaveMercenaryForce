using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom")]
public class FindTargetAction : Action
{
    public SharedTransform TargetTrm; // Ÿ���� ����� ����
    public LayerMask TargetLayer; // Ÿ���� ���Ե� ���̾�
    public float searchRadius = 10000f; // Ž�� �ݰ��� �ſ� ũ�� ����

    // ĳ�̵� �迭 (�ִ� Ÿ�� �� 10���� ����, �ʿ信 ���� ����)
    private Collider[] cachedColliders = new Collider[10];

    public override TaskStatus OnUpdate()
    {
        int targetCount = Physics.OverlapSphereNonAlloc(transform.position, searchRadius, cachedColliders, TargetLayer);

        if (targetCount == 0)
        {
            // Ÿ���� ã�� ������ ��� ����
            return TaskStatus.Failure;
        }

        // ���� ����� Ÿ�� ã��
        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;

        for (int count = 0; count < targetCount; count++)
        {
            float distance = Vector3.Distance(transform.position, cachedColliders[count].transform.position);
            if (cachedColliders[count].TryGetComponent(out MonoCharacter character))
            {
                if (character._isAlive.Value == false) continue;
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = cachedColliders[count].transform;
                }
            }
        }

        TargetTrm.Value = closestTarget;

        // Ÿ���� ã���� ���� (TaskStatus.Success)
        return closestTarget != null ? TaskStatus.Success : TaskStatus.Failure;
    }
}

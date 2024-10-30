using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom")]
public class FindTargetAction : Action
{
    public SharedTransform TargetTrm; // 타겟이 저장될 변수
    public LayerMask TargetLayer; // 타겟이 포함된 레이어
    public float searchRadius = 10000f; // 탐색 반경을 매우 크게 설정

    // 캐싱된 배열 (최대 타겟 수 10개로 설정, 필요에 따라 조정)
    private Collider[] cachedColliders = new Collider[10];

    public override TaskStatus OnUpdate()
    {
        int targetCount = Physics.OverlapSphereNonAlloc(transform.position, searchRadius, cachedColliders, TargetLayer);

        if (targetCount == 0)
        {
            // 타겟을 찾지 못했을 경우 실패
            return TaskStatus.Failure;
        }

        // 가장 가까운 타겟 찾기
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

        // 타겟을 찾으면 성공 (TaskStatus.Success)
        return closestTarget != null ? TaskStatus.Success : TaskStatus.Failure;
    }
}

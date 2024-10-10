using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAttack : MonoBehaviour, IPlayerComponent
{
    public List<AttackData> attackDataList; // 에디터에서 할당할 공격 데이터 리스트
    private IAttack attack;
    private List<ISkill> skills = new List<ISkill>();

    private Transform AttackTarget;

    private PlayerCharacter _plc;

    public void Initilize(PlayerCharacter plc)
    {
        _plc = plc;
    }

    public void AfterInitilize()
    {
    }

    public void SetAttackTarget()
    {
        AttackTarget = _plc._target.Value;
    }

    public void UseAttack()
    {
        Debug.Log("공격 실행");
        /*if(AttackTarget == null) return;
        attack.ExecuteAttack(_plc, AttackTarget);*/
    }

    public void UseSkill(int useIndex)
    {
        if(AttackTarget == null) return;
        ISkill useSkill = skills[useIndex];
        useSkill.Execute(_plc, AttackTarget);
    }
}

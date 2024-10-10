using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAttack : MonoBehaviour, IPlayerComponent
{
    public List<AttackData> attackDataList; // �����Ϳ��� �Ҵ��� ���� ������ ����Ʈ
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
        Debug.Log("���� ����");
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

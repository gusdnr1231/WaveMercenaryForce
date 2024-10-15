using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Custom")]
public class WaitForAction : Action
{
    public enum WaitType
    {
        Animation, Cooldown
    }

    public WaitType waitType;
    public SharedFloat cooldown;
    public SharedBool isAnimationEnd;

    public override void OnStart()
    {
        if (waitType == WaitType.Animation) isAnimationEnd.Value = false;
    }

    public override TaskStatus OnUpdate()
    {
        switch (waitType)
        {
            case WaitType.Animation:
                if (isAnimationEnd.Value == false)
                    return TaskStatus.Running;
                break;
            case WaitType.Cooldown:
                if (cooldown.Value > 0)
                    return TaskStatus.Running;
                break;
        }

        return TaskStatus.Success;
    }
}

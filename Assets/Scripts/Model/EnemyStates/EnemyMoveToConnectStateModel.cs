using System.Collections;
using UnityEngine;
public class EnemyMoveToConnectStateModel : BaseEnemyStateModel, IEnemyState
{    
    public override void Execute(EnemyView enemy, EnemyController controller)
    {
        base.Execute(enemy, controller);
        enemy.Transform.position = Vector3.MoveTowards(enemy.Transform.position, controller.Player.Transform.position + enemy.SquadPosition, 5f* Time.deltaTime);
        if ((enemy.transform.position - (controller.Player.Transform.position + enemy.SquadPosition * 0.5f)).magnitude < 1f)
        {
            controller.SetEnemyState(enemy, EnemyStates.Connected);
        }
    }
}
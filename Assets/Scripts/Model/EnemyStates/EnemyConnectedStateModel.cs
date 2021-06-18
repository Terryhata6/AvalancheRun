using System.Collections;
using UnityEngine;

public class EnemyConnectedStateModel : BaseEnemyStateModel, IEnemyState
{
    public override void Execute(EnemyView enemy, EnemyController controller)
    {
        base.Execute(enemy, controller);
        enemy.Animator.SetFloat("VectorSpeedMagnitude", enemy.Magnitude);

    }
    

}


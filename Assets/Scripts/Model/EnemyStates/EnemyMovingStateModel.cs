using UnityEngine;

public class EnemyMovingStateModel : BaseEnemyStateModel, IEnemyState
{
    private Vector3 _temp;
    public override void Execute(EnemyView enemy, EnemyController controller)
    {
        if (_magnitude < 70)
        {
            _magnitude += 5;
        }
        base.Execute(enemy, controller);        
        _temp.x = enemy.FinishPoint.transform.position.x - enemy.transform.position.x;
        _temp.y = 0.0f;
        _temp.z = enemy.FinishPoint.transform.position.z - enemy.transform.position.z;
        enemy.Rotation = Quaternion.LookRotation(_temp, Vector3.up);
        enemy.Transform.Translate(Vector3.forward * _magnitude * 0.01f * enemy.MovementSpeed * Time.deltaTime);
        enemy.Animator.SetFloat("VectorSpeedMagnitude", _magnitude * 0.01f);
    }

}
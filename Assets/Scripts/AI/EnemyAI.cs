using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public IEnemyState CurrentState { get; private set; }
    [Tooltip("The target for this enemy.")]
    public Transform Target;

private void Start()
{
    if (Target == null)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Target = player.transform;
        }
    }
    // Begin in Patrol state.
    ChangeState(new PatrolState());
}


    private void Update()
    {
        CurrentState?.Update(this);
    }

    public void ChangeState(IEnemyState newState)
    {
        CurrentState?.Exit(this);
        CurrentState = newState;
        CurrentState?.Enter(this);
    }
}

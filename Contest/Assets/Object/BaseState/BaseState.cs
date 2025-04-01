using UnityEngine;

public class BaseState<T> : MonoBehaviour where T : BaseState<T>
{
    protected StateClass<T> currentState;

    void Update()
    {
        currentState.Change((T)this);
        currentState.Excute((T)this);
    }

    public void ChangeState(StateClass<T> newState)
    {
        currentState.Exit((T)this);
        currentState = newState;
        currentState.Enter((T)this);
    }
}

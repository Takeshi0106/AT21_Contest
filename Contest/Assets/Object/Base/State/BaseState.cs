using UnityEngine;

// ===================================
// ó‘Ô‚ğŠÇ—‚·‚éƒNƒ‰ƒX
// ===================================

public class BaseState<T> : MonoBehaviour where T : BaseState<T>
{
    protected StateClass<T> currentState;

    protected void StateUpdate()
    {
        currentState.Change((T)this);
        currentState.Excute((T)this);
    }

    public virtual void ChangeState(StateClass<T> newState)
    {
        currentState.Exit((T)this);
        currentState = newState;
        currentState.Enter((T)this);
    }
}

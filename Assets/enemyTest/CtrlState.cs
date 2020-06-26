using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlState
{
    private FSMState mState { get; set; }

    public void SetEnemyState(FSMState state)
    {
        if (mState != null)
        {
            state.End();
        }

        mState = state;
        if (mState != null)
        {
            mState.Start();
        }
    }

    public void UpdateEnemyState()
    {
        if (mState != null)
        {
            mState.Update();
        }
    }
 
}

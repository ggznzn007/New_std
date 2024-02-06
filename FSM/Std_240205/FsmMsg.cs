using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * FsmMsg
 * 2022-05-29
 * kij
 */
public class FsmMsg 
{
    protected int m_msgType;

    public int msgType { get { return m_msgType; } }

    public FsmMsg(int _msgType)
    {
        m_msgType = _msgType;
    }
}

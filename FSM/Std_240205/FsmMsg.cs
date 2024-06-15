using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FsmMsg 
{
    protected int m_msgType;

    public int msgType { get { return m_msgType; } }

    public FsmMsg(int _msgType)
    {
        m_msgType = _msgType;
    }
}

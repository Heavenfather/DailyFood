using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model : Notifier
{
    public virtual void Init(){}
    public virtual void Release(){}

    public void Raise(string eventName, params object[] args)
    {
        NotifireEvent(eventName,args);
    }
}

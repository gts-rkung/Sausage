using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBase
{
    public bool Damage(int pt);
}

public interface ITrap
{
    public void HurtSausageInRange();
}

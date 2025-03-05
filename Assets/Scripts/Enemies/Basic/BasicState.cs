using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicState : MonoBehaviour
{
    public abstract BasicState RunCurrentState();
}

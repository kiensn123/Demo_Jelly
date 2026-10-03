using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class A_Service<T> : SingletonKT<T>, I_InitSever
    where T : MonoBehaviour
{
    public abstract void InitSever();
}

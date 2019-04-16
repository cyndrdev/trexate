using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyState
{
    void Start(GameObject gameObject, MonoBehaviour script);
    void Update();
    void End();
}

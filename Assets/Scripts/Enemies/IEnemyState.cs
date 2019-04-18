using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyState
{
    void Start(GameObject gameObject, EnemyController controller);
    void Update();
    void End();
}

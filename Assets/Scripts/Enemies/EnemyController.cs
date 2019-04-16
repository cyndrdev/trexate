using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private EnemyData _data;
    private IEnemyState _currentState;

    private void Start()
    {
        SetState(_data.Stages[0].StageScript);
    }

    private void Update()
    {
        // TODO:
        // - remove `EnemyHeart` and manage health here
        // - update _currentState based on health triggers

        if (_currentState != null)
            _currentState.Update();
    }

    private void SetState(string stateName)
    {
        // first, end our current state cleanly
        if (_currentState != null)
            _currentState.End();

        Debug.Log(stateName);

        // to the unlucky soul reading this:
        // enemy behaviour is instantiated via reflection.
        // this is a terrible idea and you should never do it.
        // it was also the fastest way i could think of to get this working.
        // i hope you can forgive me.

        Type objType = Type.GetType(stateName);

        if (objType != null)
        {
            Debug.Log("found type");
            if (objType.GetInterfaces().Contains(typeof(IEnemyState)))
            {
                // we successfully reflected the type we want, not bad!
                // now let's create an instance
                _currentState = (IEnemyState)Activator.CreateInstance(objType);

                // and start it!
                _currentState.Start(gameObject, this);

                return;
            }
            throw new System.Exception("[EnemyController]: tried to instantiate state with class '" + stateName + "', however that class doesn't implement IEnemyState.");
        }

        throw new System.Exception("[EnemyController]: tried to instantiate state with class name '" + stateName + "', however no such class exists.");
    }
}

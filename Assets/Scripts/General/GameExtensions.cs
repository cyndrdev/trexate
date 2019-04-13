using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extensions
{
    public static class GameExtensions
    {
        public static int AddToState(this int value, string stateName)
        {
            Game.Instance.GlobalState.Counters[stateName] += value;
            return Game.Instance.GlobalState.Counters[stateName];
        }

        public static int AddToScore(this int value)
            => value.AddToState("score");
    }
}

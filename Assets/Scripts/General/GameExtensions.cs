using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extensions
{
    public static class GameExtensions
    {
        public static List<string> multipliers = new List<string>
        { "", "K", "M", "B", "T", "q", "Q" };

        public static int AddToState(this int value, string stateName)
        {
            var state = Game.GetPersistentComponent<GlobalState>();
            state.Counters[stateName] += value;
            return state.Counters[stateName];
        }

        public static int AddToScore(this int value)
            => value.AddToState("score");

        public static string ToYearString(this int value)
        {
            string era = " AD";
            if (value < 0)
            {
                value *= -1;
                era = " BC";
            }

            int unit = 0;
            while (value > 10000)
            {
                value /= 1000;
                unit++;
            }

            if (value > 1000 && unit > 0)
            {
                string yearString = value.ToString();
                unit++;
                return yearString[0] + "." + yearString[1] + multipliers[unit] + era;
            }
            else
            {
                return value.ToString() + multipliers[unit] + era;
            }
        }
    }
}

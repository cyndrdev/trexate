
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Extensions
{
    public static class Extensions
    {
        /// <summary>
        /// Find a GameObject with the given tag. Throws an exception if nothing
        /// is found.
        /// </summary>
        /// <param name="script">The current script</param>
        /// <param name="tag">The tag to look for</param>
        /// <returns>The found GameObject</returns>
        public static GameObject Find(this MonoBehaviour script, string tag)
        {
            var result = GameObject.FindGameObjectWithTag(tag);
            if (result == null)
                throw new Exception();

            return result;
        }

        /// <summary>
        /// Find a component on a GameObject with the given tag. Throws an exception
        /// if nothing is found.
        /// </summary>
        /// <typeparam name="T">The type of component to look for</typeparam>
        /// <param name="script">The current script</param>
        /// <param name="tag">The tag to look for</param>
        /// <returns>The found component</returns>
        public static T Find<T>(this MonoBehaviour script, string tag)
        {
            var result = Find(script, tag)
                .GetComponent<T>();

            if (result == null)
                throw new Exception();

            return result;
        }

        /// <summary>
        /// Find a target on a specific GameObject. Throws an exception if
        /// nothing is found.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static T Find<T>(this MonoBehaviour s, GameObject target)
        {
            var result = target.GetComponent<T>();
            if (result == null)
                throw new Exception();

            return result;
        }

        /// <summary>
        /// Find a script in the children of a specific GameObject. Throws an
        /// exception if nothing is found.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static T FindInChild<T>(this MonoBehaviour s, GameObject parent)
        {
            var result = parent.GetComponentInChildren<T>();
            if (result == null)
                throw new Exception();

            return result;
        }

        /// <summary>
        /// Find a component in the children of a GameObject with a given tag. Throws
        /// an exception if nothing is found.
        /// </summary>
        /// <typeparam name="T">The type of component to look for</typeparam>
        /// <param name="script">The current script</param>
        /// <param name="tag">The tag to look for</param>
        /// <returns>The found component</returns>
        public static T FindInChild<T>(this MonoBehaviour script, string tag)
        {
            var result = Find(script, tag)
                .GetComponentInChildren<T>();

            if (result == null)
                throw new Exception();

            return result;
        }

        /// <summary>
        /// Find all the components of a type T in the children of the parent
        /// GameObject. Throws an exception if none are found.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static T[] FindAllInChild<T>(this MonoBehaviour s, GameObject parent)
        {
            var results = parent.GetComponentsInChildren<T>();

            if (!results.Any())
                throw new Exception();

            return results;
        }

        /// <summary>
        /// Find all the components of a type T in the children of a tagged 
        /// GameObject. Throws an exception if none are found.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static T[] FindAllInChild<T>(this MonoBehaviour s, string tag) =>
            FindAllInChild<T>(s, s.Find(tag));

        public static float SignalToScale(this float f)
            => (f + 1.0f) / 2.0f;

        public static float ScaleToSignal(this float f)
            => (f - 0.5f) * 2.0f;
    }
}
using System;
using System.Collections;
using UnityEngine;

public static class Facade_Coroutine  {

     public static void Sequnce(MonoBehaviour go, params IEnumerator[] actions) {
		 go.StartCoroutine(Facade_Coroutine.Chain(go,actions));
     }	 
	 /**
      * Usage: StartCoroutine(CoroutineUtils.Chain(...))
      * For example:
      *     StartCoroutine(CoroutineUtils.Chain(
      *         CoroutineUtils.Do(() => Debug.Log("A")),
      *         CoroutineUtils.WaitForSeconds(2),
      *         CoroutineUtils.Do(() => Debug.Log("B"))));
      */
     public static IEnumerator Chain(MonoBehaviour go, params IEnumerator[] actions) {
         foreach (IEnumerator action in actions) {
             yield return go.StartCoroutine(action);
         }
     }
	 /**
      * Usage: StartCoroutine(CoroutineUtils.DelaySeconds(action, delay))
      * For example:
      *     StartCoroutine(CoroutineUtils.DelaySeconds(
      *         () => DebugUtils.Log("2 seconds past"),
      *         2);
      */
     public static void DelaySeconds(MonoBehaviour go, Action action, float delay) {
		 go.StartCoroutine(DelaySeconds(action,delay));
     }	 
     public static IEnumerator DelaySeconds(Action action, float delay) {
         yield return new WaitForSeconds(delay);
         action();
     }
  
     public static IEnumerator WaitForSeconds(float time) {
         yield return new WaitForSeconds(time);
     }
 
     public static void Do(MonoBehaviour go, Action action) {
		 go.StartCoroutine(Do(action));
     }
    public static void Run(MonoBehaviour go, IEnumerator routine)
    {
        go.StartCoroutine(routine);
    }
    public static IEnumerator Do(Action action) {
         action();
         yield return 0;
     }

    public static void While(MonoBehaviour go, System.Func<bool> condition) {
		 go.StartCoroutine(Wait(condition));
     }	 
    public static IEnumerator Wait(System.Func<bool> condition) {
        if (condition == null)
            yield break;
        while (condition())
            yield return null;
    }
}

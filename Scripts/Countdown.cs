using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ZnZUtil
{
    public class Countdown : IEnumerator<WaitForSeconds>
    {
        public WaitForSeconds Current => new WaitForSeconds(waitTimeStep / speed);
        object IEnumerator.Current => Current;

        private readonly float waitTimeStep;
        private readonly float totalTime;
        private float curTime;
        public float speed { get; set; } = 1f;

        public bool finished => curTime <= 0;

        public Countdown(float countdown, float timestep = 0.1f)
        {
            waitTimeStep = timestep;
            totalTime = countdown;
            curTime = countdown;
        }

        public void Dispose()
        {
            // NOOP nothing to dispose of
        }

        public bool MoveNext()
        {
            curTime -= waitTimeStep;

            if (curTime <= 0) curTime = 0; // just make sure  it is null and not negative, for display reasons

            return curTime > 0;
        }

        public float GetTime()
        {
            return curTime;
        }

        public void Reset()
        {
            curTime = totalTime;
        }

        /// <summary>
        /// Simply run the countdown as a coroutine
        /// </summary>
        /// <returns></returns>
        public IEnumerator SimpleRun()
        {
            while (MoveNext())
            {
                yield return Current;
            }
        }

        public IEnumerator PerformAfterRun(UnityAction action)
        {
            while (MoveNext())
            {
                yield return Current;
            }

            action();
        }
    }
}
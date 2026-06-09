using JumboJumps.EFTB.Utilities;
using System.Collections;
using UnityEngine;

namespace JumboJumps.EFTB.Utilities
{
    public class CoroutineHelper : MonoBehaviour
    {
        private void OnDestroy()
        {
            Dispose();
        }

        public void Initialize()
        {
            GameContext.Instance.Add(this);
        }

        public void Dispose()
        {
            GameContext.Instance.Remove(this);

            StopAllCoroutines();
        }

        /// Play coroutine
        /// </summary>
        /// <param name="coroutine">IEnumerator function</param>
        /// <param name="source">Coroutine player</param>
        /// <returns>Played Coroutine</returns>
        public Coroutine Play(IEnumerator coroutine, MonoBehaviour source = null)
        {
            if (this && coroutine != null)
            {
                ValidateSource(ref source);

                return source.StartCoroutine(coroutine);
            }

            return null;
        }

        /// <summary>
        /// Stop coroutine
        /// </summary>
        /// <param name="coroutine">IEnumerator function</param>
        /// <param name="source">Coroutine player</param>
        public void Stop(IEnumerator coroutine, MonoBehaviour source = null)
        {
            if (this && coroutine != null)
            {
                ValidateSource(ref source);

                source.StopCoroutine(coroutine);
            }
        }

        /// <summary>
        /// Stop coroutine
        /// </summary>
        /// <param name="coroutine">Played coroutine</param>
        /// <param name="source">Coroutine player</param>
        public void Stop(Coroutine coroutine, MonoBehaviour source = null)
        {
            if (this && coroutine != null)
            {
                ValidateSource(ref source);

                source.StopCoroutine(coroutine);
            }
        }

        /// <summary>
        /// Stop played coroutine and play new coroutine
        /// Note: Use to stop previous coroutine before play new one
        /// </summary>
        /// <param name="stopCoroutine">Coroutine that want to stop(previous coroutine)</param>
        /// <param name="playCoroutine">Coroutine that want to play(new coroutine)</param>
        /// <param name="source">Coroutine player</param>
        /// <returns></returns>
        public Coroutine Restart(Coroutine stopCoroutine, IEnumerator playCoroutine, MonoBehaviour source = null)
        {
            Stop(stopCoroutine, source);

            return Play(playCoroutine, source);
        }

        /// <summary>
        /// Stop all coroutine in specific MonoBehaviour
        /// </summary>
        /// <param name="source">Coroutine player</param>
        public void StopAll(MonoBehaviour source)
        {
            if (this)
            {
                //We don't need to use stop all for this MonoBehaviour
                //Because we can't handle everycases,
                //it possible to have system that need to use lifetime coroutine

                //Reject null instead of use this MonoBehaviour as source
                //ValidateSource(ref source);

                if (!source)
                {
                    DebugLogHelper.LogError($"[{GetType().Name}] {nameof(StopAll)}: Source should not be null");
                    return;
                }

                source.StopAllCoroutines();
            }
        }

        private void ValidateSource(ref MonoBehaviour source)
        {
            if (!source)
            {
                source = this;
            }
        }
    }
}

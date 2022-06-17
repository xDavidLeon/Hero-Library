using System.Collections.Generic;
using UnityEngine;

namespace HeroLib
{
    public static class MiscExt
    {
        /// <summary>
        /// Evaluate a non-normalized value (time) on a Normalized AnimationCurve given a min and max
        /// </summary>
        public static float EvaluateNormalized(this AnimationCurve ac, float currentValue, float min, float max)
        {
            float denom = (max - min);
            if (denom == 0) return 0f; //to avoid division by 0
            return ac.Evaluate((currentValue - min) / denom);
        }

        public static void PlayOneShotRandom(this AudioSource audioSource, List<AudioClip> clips, float volume = 1.0f)
        {
            if (clips == null || clips.Count == 0) return;
            audioSource.PlayOneShot(clips.GetRandom(), volume);
        }
    }
}
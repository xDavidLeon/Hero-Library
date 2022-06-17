using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public static class ProfilingUtils
{
    public static int samplesCounter = 0;
    public static Stack<string> samples = new Stack<string>();

    public static void BeginSample(string sample)
    {
        Profiler.BeginSample(sample);
        samplesCounter++;
        samples.Push(sample);
    }

    public static void EndSample(string sample, bool print = true)
    {
        Profiler.EndSample();
        samplesCounter--;

        string currentSample = samples.Pop();
        if (!string.Equals(currentSample, sample))
        {
            Debug.LogError("MiscExt: EndSample() Error, trying to remove \"" + sample + "\" but \"" +
                           currentSample + "\" found.");
        }
    }
}
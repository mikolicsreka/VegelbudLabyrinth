using System.Collections.Generic;

/// <summary>
/// Prioritásos sort megvalósító osztály az A*-hoz.
/// </summary>
/// <typeparam name="T"></typeparam>

//source: https://gist.github.com/keithcollins/307c3335308fea62db2731265ab44c06

// From Red Blob: I'm using an unsorted array for this example, but ideally this
// would be a binary heap. Find a binary heap class:
// * https://bitbucket.org/BlueRaja/high-speed-priority-queue-for-c/wiki/Home
// * http://visualstudiomagazine.com/articles/2012/11/01/priority-queues-with-c.aspx
// * http://xfleury.github.io/graphsearch.html
// * http://stackoverflow.com/questions/102398/priority-queue-in-net


public class PriorityQueue<T>
{
    private List<KeyValuePair<T, float>> elements = new List<KeyValuePair<T, float>>();

    public int Count
    {
        get { return elements.Count; }
    }

    public void Enqueue(T item, float priority)
    {
        elements.Add(new KeyValuePair<T, float>(item, priority));
    }

    // Returns the Location that has the lowest priority
    public T Dequeue()
    {
        int bestIndex = 0;

        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i].Value < elements[bestIndex].Value)
            {
                bestIndex = i;
            }
        }

        T bestItem = elements[bestIndex].Key;
        elements.RemoveAt(bestIndex);
        return bestItem;
    }
}
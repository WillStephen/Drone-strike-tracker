using System.Collections.Generic;
using Drone_strike_tracker.Models;

namespace Drone_strike_tracker
{
    public static class CompareLists
    {
        public static int Compare(List<Strike> list1, List<Strike> list2)
        {
            var list1Count = list1.Count;
            var list2Count = list2.Count;

            if (list1Count >= list2Count)
            {
                return 0;
            }

            var addedStrikesCount = list2Count - list1Count;

            var newStrikesStartIndex = (list2.Count) - addedStrikesCount;
            var newStrikesEndIndex = list2.Count;

            var addedStrikes = list2.GetRange(0, addedStrikesCount);//(newStrikesStartIndex, newStrikesEndIndex);

            list1.InsertRange(0, addedStrikes);

            return addedStrikesCount;
        }
    }
}
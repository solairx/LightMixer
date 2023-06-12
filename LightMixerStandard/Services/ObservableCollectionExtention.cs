using System.Collections.ObjectModel;
using System.Collections;

public static class ObservableCollectionExtention
{
    public static void AddRange<t>(this ObservableCollection<t> collection, IEnumerable<t> item)
    {
        foreach (var i in item)
        {
            collection.Add(i);
        }
    }

    
}
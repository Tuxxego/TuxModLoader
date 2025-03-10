using UnityEngine;
using UnityEngine.UI;

public class NewsBuilder
{
    public static void addNews(string news)
    {
        WorldLogMessage worldLogMessage = new WorldLogMessage { text = news };

        HistoryHud.instance.newHistory(ref worldLogMessage);
    }
}

using UnityEngine;
using UnityEngine.UI;

namespace TuxModLoader.Builders
{
    public class NewsBuilder
    {
        public static void addNews(string news)
        {
            WorldLogMessage worldLogMessage = new WorldLogMessage { text = news };

            HistoryHud.instance.newHistory(ref worldLogMessage);
        }
    }
}
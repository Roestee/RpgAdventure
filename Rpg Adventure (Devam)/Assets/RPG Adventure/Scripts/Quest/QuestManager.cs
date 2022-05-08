using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RpgAdventure
{
    public class JsonHelper
    {
        private class Wrapper<T>
        {
            public T[] array;
        }

        public static T[] GetJsonArray<T>(string json)
        {
            string newJson = "{\"array\":" + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.array;
        }
    }

    public class QuestManager : MonoBehaviour
    {
        public Quest[] quests;

        private void Awake()
        {
            LoadQuestFromDB();
            AssignQuest();
        }
        private void LoadQuestFromDB()
        {
            using (StreamReader reader = new StreamReader("Assets/RPG Adventure/DB/QuestDB.json"))
            {
                string json = reader.ReadToEnd();
                var loadedQuests = JsonHelper.GetJsonArray<Quest>(json);
                quests = new Quest[loadedQuests.Length];
                quests = loadedQuests;
            }
        }

        private void AssignQuest()
        {
            QuestGiver[] questGivers = FindObjectsOfType<QuestGiver>();

            if(questGivers != null && questGivers.Length > 0)
            {
                foreach (var questGiver in questGivers)
                {
                    AssignQuestTo(questGiver);
                }
            }     
        }

        private void AssignQuestTo(QuestGiver questGiver)
        {
            foreach (var quest in quests)
            {
                if(quest.questGiver == questGiver.GetComponent<UniqueID>().Uid)
                {
                    questGiver.quest = quest;
                }
            }
        }
    }
}


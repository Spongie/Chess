using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using Chess.Pieces;
using Newtonsoft.Json;

namespace Chess.AI
{
    public static class ConfigManager
    {
        public static IEnumerable<IBoardEvaluator> LoadAllBots()
        {
            var bots = new List<IBoardEvaluator> { new OnlyPieceCountMatterEvaluator() };

            var evaluatorTypes = new List<Type>();

            var targetType = typeof(IBoardEvaluator);

            foreach (var boardEvaluatorType in AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).Where(type => targetType.IsAssignableFrom(type)))
            {
                if (boardEvaluatorType == typeof(OnlyPieceCountMatterEvaluator))
                    continue;

                evaluatorTypes.Add(boardEvaluatorType);
            }

            foreach (var evaluatorType in evaluatorTypes)
            {
                string directory = $"BotConfigs\\{evaluatorType.Name}\\";

                CreateDirectoryIfNotExists(directory);

                foreach (var file in Directory.GetFiles(directory))
                {
                    var evaluator = Activator.CreateInstance(evaluatorType);

                    ((ISavableConfigation)evaluator).DeSerialize(File.ReadAllText(file));
                    ((IBoardEvaluator)evaluator).Name = new FileInfo(file).Name;

                    bots.Add((IBoardEvaluator) evaluator);
                }
            }

            return bots;
        }

        public static ConcurrentDictionary<EvalCacheKey, float> LoadCache(IBoardEvaluator boardEvaluator, int depth, Color color)
        {
            string directory = $"BotConfigs\\Cache\\";

            CreateDirectoryIfNotExists(directory);

            if (boardEvaluator == null || !File.Exists(directory + boardEvaluator.GetBotId(depth) + ".cache"))
                return new ConcurrentDictionary<EvalCacheKey, float>();

            var xmlSerializer = new XmlSerializer(typeof(List<CacheFileEntry>));
            var cacheList = (List<CacheFileEntry>)xmlSerializer.Deserialize(File.OpenRead(directory + boardEvaluator.GetBotId(depth) + ".cache"));

            var cache = new ConcurrentDictionary<EvalCacheKey, float>();

            foreach (var cacheFileEntry in cacheList)
            {
                cache.TryAdd(cacheFileEntry.CacheKey, cacheFileEntry.Score);
            }

            return cache;
            
            //return
            //    JsonConvert.DeserializeObject<ConcurrentDictionary<EvalCacheKey, float>>(
            //        File.ReadAllText(directory + boardEvaluator.GetBotId(depth) + ".cache"));
        }

        public static void SaveCache(IBoardEvaluator boardEvaluator, ConcurrentDictionary<EvalCacheKey, float> cache, int depth, Color color)
        {
            string directory = $"BotConfigs\\Cache\\";

            CreateDirectoryIfNotExists(directory);

            var cacheList = cache.Select(s => new CacheFileEntry { CacheKey = s.Key, Score = s.Value}).ToList();
            
            var xmlSerializer = new XmlSerializer(typeof(List<CacheFileEntry>));

            using (var stream = new FileStream(directory + boardEvaluator.GetBotId(depth) + ".cache", FileMode.Create))
            {
                xmlSerializer.Serialize(stream, cacheList);
            }
            //File.WriteAllText(directory + boardEvaluator.GetBotId(depth) + ".cache", JsonConvert.SerializeObject(cache));
        }

        public static void SaveBot(ISavableConfigation saveConfigation, string name)
        {
            string directory = $"BotConfigs\\{saveConfigation.GetType().Name}\\";

            CreateDirectoryIfNotExists(directory);

            File.WriteAllText($"{directory}{name}.bot",
                saveConfigation.Serialize());
        }

        private static void CreateDirectoryIfNotExists(string path)
        {
            var dir = new DirectoryInfo(path);

            if (!dir.Exists)
                dir.Create();
        }
    }
}

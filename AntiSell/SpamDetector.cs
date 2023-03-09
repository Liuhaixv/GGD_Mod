using MelonLoader;
using System;
using System.Collections.Generic;

namespace GGD_Hack.AntiSell
{    public class SpamDetector
    {
        public bool isSpamming = false;
        private int detectSize;
        int ignoreMessagesLessThan = 12;
        private double similarityThreshold;
        private Queue<string> messages;

        public SpamDetector(int detectSize,  int ignoreMessagesLessThan, double similarityThreshold)
        {
            this.detectSize = detectSize;
            this.ignoreMessagesLessThan = ignoreMessagesLessThan;
            this.similarityThreshold = similarityThreshold;
            this.messages = new Queue<string>();
        }

        public void AddMessage(string message)
        {
            if(message.Length < ignoreMessagesLessThan)
            {
#if Developer
                MelonLogger.Msg(System.ConsoleColor.Green,"发送的消息过短，忽略滥发屏蔽功能..");
#endif
                return;
            }

            if (messages.Count >= detectSize)
            {
                messages.Dequeue();
            }

            messages.Enqueue(message);
            isSpamming = IsSpamming();
        }

        private bool IsSpamming()
        {
            if (messages.Count < detectSize)
            {
                return false;
            }

            List<string> recentMessages = new List<string>(messages);
            int n = recentMessages.Count;

            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    string message1 = recentMessages[i];
                    string message2 = recentMessages[j];

                    double similarity = ComputeLevenshteinSimilarity(message1, message2);
#if Developer
                    MelonLogger.Msg(System.ConsoleColor.Green, "字符串相似度:" + similarity);
#endif
                    if (similarity > similarityThreshold)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private double ComputeJaccardSimilarity(string s, string t)
        {
            HashSet<string> sSet = new HashSet<string>(s.Split());
            HashSet<string> tSet = new HashSet<string>(t.Split());

            HashSet<string> union = new HashSet<string>(sSet);
            union.UnionWith(tSet);

            HashSet<string> intersection = new HashSet<string>(sSet);
            intersection.IntersectWith(tSet);

            return (double)intersection.Count / union.Count;
        }

        private double ComputeLevenshteinSimilarity(string s, string t)
        {
            int m = s.Length;
            int n = t.Length;
            int[,] d = new int[m + 1, n + 1];

            for (int i = 0; i <= m; i++)
            {
                d[i, 0] = i;
            }

            for (int j = 0; j <= n; j++)
            {
                d[0, j] = j;
            }

            for (int j = 1; j <= n; j++)
            {
                for (int i = 1; i <= m; i++)
                {
                    if (s[i - 1] == t[j - 1])
                    {
                        d[i, j] = d[i - 1, j - 1];
                    }
                    else
                    {
                        d[i, j] = Math.Min(d[i - 1, j], Math.Min(d[i, j - 1], d[i - 1, j - 1])) + 1;
                    }
                }
            }

            return 1.0 - (double)d[m, n] / Math.Max(m, n);
        }
    }
}

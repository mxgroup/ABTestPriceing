using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABTestPriceing
{
    class Program
    {
        private static int _i = int.MaxValue / 3 * 2;
        private static int _i2 = int.MaxValue / 3;

        static void Main(string[] args)
        {
            string fileName = "details3";
            var lines = File.ReadAllLines($@"C:\Users\shdp\Desktop\{fileName}.txt");

            List<List<long>> keys = lines.Select(line => line.Split('\t').Select(long.Parse).ToList()).ToList();

            var results = new Dictionary<int, int>();
            var sp = Stopwatch.StartNew();
            foreach (var key in keys)
            {
                var group = GetGroup(key);
                key.Add(group);
                if (!results.ContainsKey(group)) results.Add(group, 0);
                results[group] += 1;
            }
            sp.Stop();

            Console.WriteLine(
                $"elapsed:{sp.Elapsed.TotalMilliseconds}\r\n{string.Join("\r\n", results.Select(x => x.Value))}");
            Console.ReadKey();

            //File.AppendAllLines($@"C:\Users\shdp\Desktop\{fileName}_groups.txt", keys.Select(x=>$"{x[0]}\t{x[1]}\t{x[2]}\t{x[3]}"));
        }

        private static int GetGroup(List<long> key)
        {
            if (key.Count != 3)
            {
                throw new ArgumentException();
            }
            int hash = GetHash(key[0], key[1], key[2]);
            return Math.Abs(hash % 10);
        }

        private static int GetHash(long detailId, long priceId, long optovikId)
        {
            unchecked
            {
                // long.GetHashCode => return (unchecked((int)((long)m_value)) ^ (int)(m_value >> 32));
                return (detailId.GetHashCode() * 11173)
                       ^ (priceId.GetHashCode() * 31)
                       ^ (optovikId.GetHashCode() * 19);
            }
        }
    }
}

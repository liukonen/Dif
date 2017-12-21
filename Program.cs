using System;
using System.IO;
using System.Threading.Tasks;

namespace dif
{
    class Program
    {
        static void Main(string[] args)
        {

            if (args.Length != 2)
            {
                Console.WriteLine("Command Line usage. dif [source] [destination] - App uses GetLastWriteTime to compare files");

            }
            else
            {
                Console.WriteLine("dif - command line file difference viewer © 2017 Luke Liukonen");
                Console.WriteLine("--------------------------------------------------------------");

                string SourceDir = args[0];
                var DestDir = args[1];
                try
                {
                    var Sources = new System.Collections.Concurrent.ConcurrentDictionary<string, DateTime>();
                    var Destinations = new System.Collections.Concurrent.ConcurrentDictionary<string, DateTime>();

                    var input = Directory.EnumerateFiles(SourceDir, "*", SearchOption.AllDirectories);

                    Task GetSources = Task.Run(() =>
                    {
                        Parallel.ForEach(input, source =>
                        {
                            var DT = File.GetLastWriteTime(source);
                            Sources.TryAdd(source.Replace(SourceDir, string.Empty), DT);
                        });
                    });

                    var Output = Directory.EnumerateFiles(DestDir, "*", SearchOption.AllDirectories);
                    Task GetDestinations = Task.Run(() =>
                    {
                        Parallel.ForEach(Output, source =>
                        {
                            var DT = File.GetLastWriteTime(source);
                            Destinations.TryAdd(source.Replace(DestDir, string.Empty), DT);
                        });
                    });

                    GetDestinations.Wait(); GetSources.Wait();

                    foreach (var item in Sources.Keys)
                    {
                        if (Destinations.ContainsKey(item))
                        {
                            if (Destinations[item] != Sources[item]) /*files exist, but dont match*/
                            {
                                Console.WriteLine(string.Concat(item, " ", SourceDir, " is ", (Sources[item] > Destinations[item]) ? "newer" : "older", " than ", DestDir));
                            }


                        }
                        else { Console.WriteLine(string.Concat(item, " Exists in ", SourceDir, ", not in ", DestDir)); }
                    }
                    foreach (var item in Destinations.Keys)
                    {
                        if (!Sources.ContainsKey(item)) { Console.WriteLine(string.Concat(item, " Exists in ", DestDir, ", not in ", SourceDir)); }
                    }
                }
                catch (Exception x) { Console.WriteLine(x.Message); }
            }
        }
    }
}

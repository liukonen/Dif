using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace dif2
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
                Console.WriteLine("dif - command line file difference viewer © 2017-2020 Luke Liukonen");
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




                    foreach (var SourceItem in Sources)
                    {

                        if (Destinations.ContainsKey(SourceItem.Key))
                        {
                            var DestinationItem = Destinations[SourceItem.Key];
                            if (DestinationItem != SourceItem.Value) /*files exist, but dont match*/
                            {
                                var compareString = (SourceItem.Value > DestinationItem) ? "newer" : "older";
                                Console.WriteLine($"{SourceItem.Key}: {SourceDir} is {compareString} than {DestDir}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"{SourceItem.Key}: Exists in {SourceDir} not in {DestDir}");
                        }
                    }
                    foreach (var item in Destinations.Keys)
                    {
                        if (!Sources.ContainsKey(item))
                        {
                            Console.WriteLine(string.Concat($"{item}: Exists in {DestDir} not in {SourceDir}"));
                        }
                    }
                }
                catch (Exception x) { Console.WriteLine(x.Message); }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using TableauDataExtractor.DAL;
using TableauDataExtractor.Blob;
using TableauDataExtractor.TableauMetadata;
using System.Diagnostics;

namespace TableauDataExtractor
{
    class Program
    {
        static void Main(string[] args)
        {

            Stopwatch programWatch = Stopwatch.StartNew();
            Repository tr = new Repository();
            
            Console.WriteLine("Updating Staging");
            Stopwatch taskWatch = Stopwatch.StartNew();
            tr.LoadStgTable();
            taskWatch.Stop();
            Console.WriteLine("Elapsed:"+taskWatch.Elapsed);
            
            Console.WriteLine("Parsing Metadata");
            taskWatch.Restart();
            TableauMetadataFactory tm = new TableauMetadataFactory(tr.Blobs());
            taskWatch.Stop();
            Console.WriteLine("Elapsed:" + taskWatch.Elapsed);

            Console.WriteLine("Loading XrefWorkbookDatasource");
            taskWatch.Restart();
            tr.LoadXrefWorkbookDatasource(tm.Xmls);
            taskWatch.Stop();
            Console.WriteLine("Elapsed:" + taskWatch.Elapsed);

            Console.WriteLine("Loading Fields");
            taskWatch.Restart();
            tr.LoadFields(tm.Xmls);
            taskWatch.Stop();
            Console.WriteLine("Elapsed:" + taskWatch.Elapsed);
            
            Console.WriteLine("Swapping Tables");
            taskWatch.Restart();
            tr.SwapStgTable();
            taskWatch.Stop();
            Console.WriteLine("Elapsed:" + taskWatch.Elapsed);
            
            tr.Dispose();

            programWatch.Stop();

            Console.WriteLine("Total Elapsed:" + programWatch.Elapsed);

        }

    }
}

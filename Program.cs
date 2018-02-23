using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedComponentLabeling
{
    class Program
    {
        static void Main(string[] args)
        {
            var w = new Stopwatch();
            var detector = new CCLBlobDetector();

            //int nrItems = detector.Initialize(@"D:\Projects\ConnectedComponentLabeling\ConnectedComponentLabeling\singleblob.png");
            //Measure(nrItems, detector, w);

            int nrItems = detector.Initialize(@"D:\Projects\ConnectedComponentLabeling\ConnectedComponentLabeling\4.png");
            Measure(nrItems, detector, w);


            nrItems = detector.Initialize(@"D:\Projects\ConnectedComponentLabeling\ConnectedComponentLabeling\64.png");
            Measure(nrItems, detector, w);

            nrItems = detector.Initialize(@"D:\Projects\ConnectedComponentLabeling\ConnectedComponentLabeling\128.png");
            Measure(nrItems, detector, w);

            nrItems = detector.Initialize(@"D:\Projects\ConnectedComponentLabeling\ConnectedComponentLabeling\512.png");
            Measure(nrItems, detector, w);

            nrItems = detector.Initialize(@"D:\Projects\ConnectedComponentLabeling\ConnectedComponentLabeling\1024.png");
            Measure(nrItems, detector, w);

            //nrItems = detector.Initialize(@"D:\Projects\ConnectedComponentLabeling\ConnectedComponentLabeling\4000.png");
            //Measure(nrItems, detector, w);

            Console.WriteLine();
            Console.WriteLine("end");
            Console.Read();
        }

        private static void Measure(int nrItems, CCLBlobDetector d, Stopwatch w)
        {
            w.Start();
            d.GetBlobs();
            w.Stop();

            Console.WriteLine(string.Format("Pixels: {0}, Time: {1}ms, Relation: {2} Items/ms", nrItems, w.ElapsedMilliseconds, w.ElapsedMilliseconds == 0 ? -1 : nrItems/w.ElapsedMilliseconds));
            w.Reset();
        }
    }
}

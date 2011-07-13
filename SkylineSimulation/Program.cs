using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpatialIndex.GridRTree;
using IndexPercentageSimulation;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using DominanceTestCountSimulation;
using TuningTimeSimulation;

namespace SkylineSimulation
{
    class Program
    {
        public static PrefSpec[][] minPref =
        {
            null,null,
            new PrefSpec[] {PrefSpec.Min, PrefSpec.Min},
            new PrefSpec[] {PrefSpec.Min, PrefSpec.Min, PrefSpec.Min},
            new PrefSpec[] {PrefSpec.Min, PrefSpec.Min, PrefSpec.Min, PrefSpec.Min},
            new PrefSpec[] {PrefSpec.Min, PrefSpec.Min, PrefSpec.Min, PrefSpec.Min, PrefSpec.Min},
            new PrefSpec[] {PrefSpec.Min, PrefSpec.Min, PrefSpec.Min, PrefSpec.Min, PrefSpec.Min, PrefSpec.Min},
            new PrefSpec[] {PrefSpec.Min, PrefSpec.Min, PrefSpec.Min, PrefSpec.Min, PrefSpec.Min, PrefSpec.Min, PrefSpec.Min},
            new PrefSpec[] {PrefSpec.Min, PrefSpec.Min, PrefSpec.Min, PrefSpec.Min, PrefSpec.Min, PrefSpec.Min, PrefSpec.Min, PrefSpec.Min},
            new PrefSpec[] {PrefSpec.Min, PrefSpec.Min, PrefSpec.Min, PrefSpec.Min, PrefSpec.Min, PrefSpec.Min, PrefSpec.Min, PrefSpec.Min, PrefSpec.Min},
            new PrefSpec[] {PrefSpec.Min, PrefSpec.Min, PrefSpec.Min, PrefSpec.Min, PrefSpec.Min, PrefSpec.Min, PrefSpec.Min, PrefSpec.Min, PrefSpec.Min, PrefSpec.Min}
        };

        public static PrefSpec[][] maxPref =
        {
            null,null,
            new PrefSpec[] {PrefSpec.Max, PrefSpec.Max},
            new PrefSpec[] {PrefSpec.Max, PrefSpec.Max, PrefSpec.Max},
            new PrefSpec[] {PrefSpec.Max, PrefSpec.Max, PrefSpec.Max, PrefSpec.Max},
            new PrefSpec[] {PrefSpec.Max, PrefSpec.Max, PrefSpec.Max, PrefSpec.Max, PrefSpec.Max},
            new PrefSpec[] {PrefSpec.Max, PrefSpec.Max, PrefSpec.Max, PrefSpec.Max, PrefSpec.Max, PrefSpec.Max},
            new PrefSpec[] {PrefSpec.Max, PrefSpec.Max, PrefSpec.Max, PrefSpec.Max, PrefSpec.Max, PrefSpec.Max, PrefSpec.Max},
            new PrefSpec[] {PrefSpec.Max, PrefSpec.Max, PrefSpec.Max, PrefSpec.Max, PrefSpec.Max, PrefSpec.Max, PrefSpec.Max, PrefSpec.Max},
            new PrefSpec[] {PrefSpec.Max, PrefSpec.Max, PrefSpec.Max, PrefSpec.Max, PrefSpec.Max, PrefSpec.Max, PrefSpec.Max, PrefSpec.Max, PrefSpec.Max},
            new PrefSpec[] {PrefSpec.Max, PrefSpec.Max, PrefSpec.Max, PrefSpec.Max, PrefSpec.Max, PrefSpec.Max, PrefSpec.Max, PrefSpec.Max, PrefSpec.Max, PrefSpec.Max}
        };

        public static PrefSpec[] maxMinPref = { PrefSpec.Max, PrefSpec.Min };
        public static PrefSpec[] minMaxPref = { PrefSpec.Min, PrefSpec.Max };
        public static PrefSpec[] maxMaxPref = { PrefSpec.Max, PrefSpec.Max };
        
        static void Main(string[] args)
        {
            //GridRTree rtree = new GridRTree("data_inc_dim/skyline_5d_10000.txt", 10);
            //Console.WriteLine("Number of data points: " + 
            //    CountDataPoints(rtree.Root));
            //Console.WriteLine("Index size in bytes: " + 
            //    OneTimeIndexPercentage.GetIndexInBytes(rtree.Root));
            //Console.WriteLine("Data size in bytes: " + OneTimeIndexPercentage.GetDataPointsInBytes(rtree));
            //Console.WriteLine("Index percentage: " + OneTimeIndexPercentage.GetPercentage(rtree));

            //Console.WriteLine("(1, m) Index Perc:" + 
            //    OneMIndexPercentage.GetPercentage(rtree, 512));
            //Console.WriteLine("DFDI Index Perc:" + 
            //    DFDIIndexPercentage.GetPercentage(rtree, 2));

            //Test serializing entire GRTree object.
            //IntNode root = (IntNode)rtree.Root;
            //IntNode node = (IntNode)((IntNode)root.Children[0]).Children[0];
            //BinarySerializeToFile(node, "SerNode.bin");
            //GridRTree tree = (GridRTree)BinaryDeserializeFromFile("SerRTree.bin");

            //Console.WriteLine();
            //for (int i = 2; i <= 10; i++)
            //{
            //    GridRTree tree = new GridRTree("skyline_5d_1000.txt", i);
            //    Console.WriteLine("Number of data points: " + CountDataPoints(tree.Root));
            //    Console.WriteLine("Index size in bytes: " + OneTimeIndexPercentage.GetIndexInBytes(rtree));
            //}

            IncreaseRecordCount();
            //IncreaseDimension();
            //Console.WriteLine();
            //TuningSimSuite(3, 10);
            //DomTestSimSuite(2, 10);
            //DomTestSimIncDim(10000, 10);
            //DomTestSimIncBf(10000, 2);
            //DomTest_IncSize_Uniform_MinMaxMixed(2, 10);
            //DomTest_IncDim_Uniform_AllMinAllMax(10000, 10);
            //TuningSimIncDim(10000, 10);
            //TuningTime_IncSize_Uniform_MinMaxMixed(2, 10);
            //TuningTime_IncDim_Uniform_AllMinAllMax(10000, 10);
            //Console.WriteLine();
            //IndexPercIncBf();
            //IncRecordCountDomTestPB();
            //Console.WriteLine();
            //IncRecordCountDomTestIB();

            //GridRTree tree = new GridRTree(DataPath() + "\\d3_r10.txt", 3);
            //PointBasedTuneTime ttsim = new PointBasedTuneTime(tree, 1);
            //Console.WriteLine(ttsim.GetTuningTime());
            //IndexBasedTuneTime ibttsim = new IndexBasedTuneTime(tree, 1);
            //Console.WriteLine(ibttsim.GetTuningTime());
            Console.WriteLine("Press ENTER to exit");
            Console.ReadLine();
        }

        static void IncreaseRecordCount()
        {
            string dataPath = DataPath() + "\\uniform\\3d\\";
            int b = 10;
            int rep = 2;
            //int m = ??
            for (int i = 1; i <= 10; i++)
            {
                string file = "uniform_d3" + "_r" + i * 10000 + "_c" + i * 10000 + ".txt";
                GridRTree rtree = new GridRTree(dataPath + file, b);
                Console.Write(
                    OneTimeIndexPercentage.GetDataPointsInBytes(rtree) + "\t");
                Console.Write(
                    OneTimeIndexPercentage.GetIndexInBytes(rtree.Root) + "\t");
                //Console.Write(
                //    OneMIndexPercentage.GetIndexInBytes(rtree.Root) + "\t");
                Console.Write(
                    DFDIIndexPercentage.GetIndexInBytes(rtree.Root, 1, rep) + "\t");
                Console.Write(
                    OneTimeIndexPercentage.GetPercentage(rtree).ToString("f4") + "\t");
                Console.Write(
                    OneMIndexPercentage.GetPercentage(rtree, 2).ToString("f4") + "\t");
                Console.Write(
                    DFDIIndexPercentage.GetPercentage(rtree, rep).ToString("f4") + "\t");
                Console.WriteLine();
            }
        }

        static void IndexPercIncBf()
        {
            string dataPath = GetAllDataPath(DataType.Uniform, 3, 10000);
            int rep = 2;
            //int m = ??
            string outPath = "IncreaseBranchingFactor.txt";

            using (StreamWriter streamWriter = new StreamWriter(outPath))
            {
                for (int i = 2; i <= 15; i++)
                {
                    //string file = "uniform_d3_r20000_c20000";
                    GridRTree rtree = new GridRTree(dataPath, i);
                    streamWriter.Write(
                        OneTimeIndexPercentage.GetDataPointsInBytes(rtree).ToString() + "\t");
                    streamWriter.Write(
                        OneTimeIndexPercentage.GetIndexInBytes(rtree.Root).ToString() + "\t");
                    //Console.Write(
                    //    OneMIndexPercentage.GetIndexInBytes(rtree.Root).ToString() + "\t");
                    streamWriter.Write(
                        DFDIIndexPercentage.GetIndexInBytes(rtree.Root, 1, rep) + "\t");
                    streamWriter.Write(
                        OneTimeIndexPercentage.GetPercentage(rtree).ToString("f4") + "\t");
                    streamWriter.Write(
                        OneMIndexPercentage.GetPercentage(rtree, 2).ToString("f4") + "\t");
                    streamWriter.WriteLine(
                        DFDIIndexPercentage.GetPercentage(rtree, rep).ToString("f4") + "\t");     
                }
            }
        }

        public static void DomTestSimSuite(int d, int b)
        {
            Console.WriteLine("Begine DomTestSimSuite()");

            string outPath = "DomTest_d" + d + ".txt";

            using (StreamWriter streamWriter = new StreamWriter(outPath))
            {
                
                for (int dsSize = 10000; dsSize <= 100000; dsSize += 10000)
                {
                    Console.WriteLine("Size: " + dsSize);

                    streamWriter.Write(dsSize.ToString() + '\t');

                    //Uniformed
                    GridRTree rtree = new GridRTree(
                        GetAllDataPath(DataType.Uniform, d, dsSize),
                        b //+ ((dsSize / 10000) - 1)  //branching factor
                    );

                    streamWriter.Write(new PointBasedPruning(rtree,
                        minPref[d]).DominanceCount.ToString() + '\t');
                    streamWriter.Write(new IndexBasedPruning(rtree,
                        minPref[d]).DominanceCount.ToString() + '\t');

                    //Rising
                    rtree = new GridRTree(
                        GetAllDataPath(DataType.Rising, d, dsSize),
                        b //+ ((dsSize / 10000) - 1)   //branching factor
                    );

                    streamWriter.Write(new PointBasedPruning(rtree,
                        minPref[d]).DominanceCount.ToString() + '\t');
                    streamWriter.Write(new IndexBasedPruning(rtree,
                        minPref[d]).DominanceCount.ToString() + '\t');

                    //Falling
                    rtree = new GridRTree(
                        GetAllDataPath(DataType.Falling, d, dsSize),
                        b //+ ((dsSize / 10000) - 1)   //branching factor
                    );

                    streamWriter.Write(new PointBasedPruning(rtree,
                        minPref[d]).DominanceCount.ToString() + '\t');
                    streamWriter.WriteLine(new IndexBasedPruning(rtree,
                        minPref[d]).DominanceCount.ToString());
                }
            }

            Console.WriteLine("End DomTestSimSuite()");
        }

        public static void DomTest_IncSize_Uniform_MinMaxMixed(int d, int b)
        {
            Console.WriteLine("Begine DomTest_IncSize_Uniform_MinMaxMixed()");

            string outPath = "DomTest_IncSize_Uniform_MinMaxMixed_d" + d + ".txt";

            using (StreamWriter streamWriter = new StreamWriter(outPath))
            {
                streamWriter.WriteLine("Record Count\tP-B (Min, Min)\tI-B (Min, Min)" +
                    "\tP-B (Min, Max)\tI-B (Min, Max)" +
                    "\tP-B (Max, Min)\tI-B (Max, Min)" +
                    "\tP-B (Max, Max)\tI-B (Max, Max)");

                for (int dsSize = 10000; dsSize <= 100000; dsSize += 10000)
                {
                    Console.WriteLine("Size: " + dsSize);

                    streamWriter.Write(dsSize.ToString() + '\t');

                    //min, min
                    GridRTree rtree = new GridRTree(
                        GetAllDataPath(DataType.Uniform, d, dsSize),
                        b //+ ((dsSize / 10000) - 1)  //branching factor
                    );

                    streamWriter.Write(new PointBasedPruning(rtree,
                        minPref[d]).DominanceCount.ToString() + '\t');
                    streamWriter.Write(new IndexBasedPruning(rtree,
                        minPref[d]).DominanceCount.ToString() + '\t');

                    //min, max
                    rtree = new GridRTree(
                        GetAllDataPath(DataType.Uniform, d, dsSize),
                        b //+ ((dsSize / 10000) - 1)   //branching factor
                    );

                    streamWriter.Write(new PointBasedPruning(rtree,
                        minMaxPref).DominanceCount.ToString() + '\t');
                    streamWriter.Write(new IndexBasedPruning(rtree,
                        minMaxPref).DominanceCount.ToString() + '\t');

                    //max, min
                    rtree = new GridRTree(
                        GetAllDataPath(DataType.Uniform, d, dsSize),
                        b //+ ((dsSize / 10000) - 1)   //branching factor
                    );

                    streamWriter.Write(new PointBasedPruning(rtree,
                        maxMinPref).DominanceCount.ToString() + '\t');
                    streamWriter.Write(new IndexBasedPruning(rtree,
                        maxMinPref).DominanceCount.ToString() + '\t');

                    //max, max
                    rtree = new GridRTree(
                        GetAllDataPath(DataType.Uniform, d, dsSize),
                        b //+ ((dsSize / 10000) - 1)   //branching factor
                    );

                    streamWriter.Write(new PointBasedPruning(rtree,
                        maxMaxPref).DominanceCount.ToString() + '\t');
                    streamWriter.WriteLine(new IndexBasedPruning(rtree,
                        maxMaxPref).DominanceCount.ToString());
                }
            }

            Console.WriteLine("End DomTest_IncSize_Uniform_MinMaxMixed()");
        }

        public static void DomTest_IncDim_Uniform_AllMinAllMax(int dsSize, int b)
        {
            Console.WriteLine("Begine DomTest_IncDim_Uniform_AllMinAllMax()");

            string outPath = "DomTest_IncDim_Uniform_AllMinAllMax_r" + dsSize + ".txt";

            using (StreamWriter streamWriter = new StreamWriter(outPath))
            {
                streamWriter.WriteLine("Dimension\tP-B All Min\tI-B All Min" +
                    "\tP-B All Max\tI-B All Max");

                for (int d = 2; d <= 10; d++)
                {
                    Console.WriteLine("Dimension: " + d);

                    streamWriter.Write(d.ToString() + '\t');

                    //MIN
                    GridRTree rtree = new GridRTree(
                        GetAllDataPath(DataType.Uniform, d, dsSize),
                        b //+ ((dsSize / 10000) - 1)  //branching factor
                    );

                    streamWriter.Write(new PointBasedPruning(rtree,
                        minPref[d]).DominanceCount.ToString() + '\t');
                    streamWriter.Write(new IndexBasedPruning(rtree,
                        minPref[d]).DominanceCount.ToString() + '\t');

                    //Max
                    rtree = new GridRTree(
                        GetAllDataPath(DataType.Uniform, d, dsSize),
                        b //+ ((dsSize / 10000) - 1)   //branching factor
                    );

                    streamWriter.Write(new PointBasedPruning(rtree,
                        maxPref[d]).DominanceCount.ToString() + '\t');
                    streamWriter.WriteLine(new IndexBasedPruning(rtree,
                        maxPref[d]).DominanceCount.ToString());
                }
            }

            Console.WriteLine("End DomTest_IncDim_Uniform_AllMinAllMax()");
        }

        public static void DomTestSimIncDim(int size, int b)
        {
            Console.WriteLine("Begine DomTestSimSuite()");

            string outPath = "DomTestIncDim_r" + size + ".txt";

            using (StreamWriter streamWriter = new StreamWriter(outPath))
            {

                for (int d = 2; d <= 10; d++)
                {
                    Console.WriteLine("Dimension: " + d);

                    streamWriter.Write(d.ToString() + '\t');

                    //Uniformed
                    GridRTree rtree = new GridRTree(
                        UniformDataPath(d, size),
                        b   //branching factor
                    );

                    streamWriter.Write(new PointBasedPruning(rtree,
                        minPref[d]).DominanceCount.ToString() + '\t');
                    streamWriter.Write(new IndexBasedPruning(rtree,
                        minPref[d]).DominanceCount.ToString() + '\t');

                    //Rising
                    rtree = new GridRTree(
                        GetAllDataPath(DataType.Rising, d, size),
                        b   //branching factor
                    );

                    streamWriter.Write(new PointBasedPruning(rtree,
                        minPref[d]).DominanceCount.ToString() + '\t');
                    streamWriter.Write(new IndexBasedPruning(rtree,
                        minPref[d]).DominanceCount.ToString() + '\t');

                    //Falling
                    rtree = new GridRTree(
                        GetAllDataPath(DataType.Falling, d, size),
                        b   //branching factor
                    );

                    streamWriter.Write(new PointBasedPruning(rtree,
                        minPref[d]).DominanceCount.ToString() + '\t');
                    streamWriter.WriteLine(new IndexBasedPruning(rtree,
                        minPref[d]).DominanceCount.ToString());
                }
            }

            Console.WriteLine("End DomTestSimSuite()");
        }

        public static void DomTestSimIncBf(int size, int d)
        {
            Console.WriteLine("Begine DomTestSimIncBf()");

            string outPath = "DomTestIncBf_r" + size + ".txt";

            using (StreamWriter streamWriter = new StreamWriter(outPath))
            {

                for (int b = 2; b <= 50; b++)
                {
                    Console.WriteLine("Branching: " + b);

                    streamWriter.Write(b.ToString() + '\t');

                    //Uniformed
                    GridRTree rtree = new GridRTree(
                        UniformDataPath(d, size),
                        b   //branching factor
                    );

                    streamWriter.Write(new PointBasedPruning(rtree,
                        minPref[d]).DominanceCount.ToString() + '\t');
                    streamWriter.Write(new IndexBasedPruning(rtree,
                        minPref[d]).DominanceCount.ToString() + '\t');

                    //Rising
                    rtree = new GridRTree(
                        GetAllDataPath(DataType.Rising, d, size),
                        b   //branching factor
                    );

                    streamWriter.Write(new PointBasedPruning(rtree,
                        minPref[d]).DominanceCount.ToString() + '\t');
                    streamWriter.Write(new IndexBasedPruning(rtree,
                        minPref[d]).DominanceCount.ToString() + '\t');

                    //Falling
                    rtree = new GridRTree(
                        GetAllDataPath(DataType.Falling, d, size),
                        b   //branching factor
                    );

                    streamWriter.Write(new PointBasedPruning(rtree,
                        minPref[d]).DominanceCount.ToString() + '\t');
                    streamWriter.WriteLine(new IndexBasedPruning(rtree,
                        minPref[d]).DominanceCount.ToString());
                }
            }

            Console.WriteLine("End DomTestSimIncBf()");
        }

        public static void TuningSimSuite(int d, int b, PrefSpec[] prefs)
        {
            Console.WriteLine("Begine TuningSimSuite()");

            string outPath = "TuningSim_d" + d + ".txt";

            using (StreamWriter streamWriter = new StreamWriter(outPath))
            {

                for (int dsSize = 10000; dsSize <= 100000; dsSize += 10000)
                {
                    Console.WriteLine("Size: " + dsSize);

                    streamWriter.Write(dsSize.ToString() + '\t');

                    //Uniformed
                    GridRTree rtree = new GridRTree(
                        GetDataPath(DataType.Uniform, d, dsSize),
                        b   //branching factor
                    );

                    streamWriter.Write(new PointBasedTuneTime(rtree,
                        1, prefs).GetTuningTime().ToString() + '\t');
                    streamWriter.Write(new IndexBasedTuneTime(rtree,
                        1, prefs).GetTuningTime().ToString() + '\t');

                    //Rising
                    rtree = new GridRTree(
                        GetDataPath(DataType.Rising, d, dsSize),
                        b   //branching factor
                    );

                    streamWriter.Write(new PointBasedTuneTime(rtree,
                        1, prefs).GetTuningTime().ToString() + '\t');
                    streamWriter.Write(new IndexBasedTuneTime(rtree,
                        1, prefs).GetTuningTime().ToString() + '\t');

                    //Falling
                    rtree = new GridRTree(
                        GetDataPath(DataType.Falling, d, dsSize),
                        b   //branching factor
                    );

                    streamWriter.Write(new PointBasedTuneTime(rtree,
                        1, prefs).GetTuningTime().ToString() + '\t');
                    streamWriter.WriteLine(new IndexBasedTuneTime(rtree,
                       1, prefs).GetTuningTime().ToString());
                }
            }

            Console.WriteLine("End TuningSimSuite()");
        }

        public static void TuningSimIncDim(int size, int b, PrefSpec[] prefs)
        {
            Console.WriteLine("Begine TuningSimIncDim()");

            string outPath = "TuningSimIncDim_d" + size + ".txt";

            using (StreamWriter streamWriter = new StreamWriter(outPath))
            {

                for (int d = 2; d <= 10; d++)
                {
                    Console.WriteLine("Size: " + size);

                    streamWriter.Write(d.ToString() + '\t');

                    //Uniformed
                    GridRTree rtree = new GridRTree(
                        GetAllDataPath(DataType.Uniform, d, size),
                        b   //branching factor
                    );

                    streamWriter.Write(new PointBasedTuneTime(rtree,
                        1, prefs).GetTuningTime().ToString() + '\t');
                    streamWriter.Write(new IndexBasedTuneTime(rtree,
                        1, prefs).GetTuningTime().ToString() + '\t');

                    //Rising
                    rtree = new GridRTree(
                        GetAllDataPath(DataType.Rising, d, size),
                        b   //branching factor
                    );

                    streamWriter.Write(new PointBasedTuneTime(rtree,
                        1, prefs).GetTuningTime().ToString() + '\t');
                    streamWriter.Write(new IndexBasedTuneTime(rtree,
                        1, prefs).GetTuningTime().ToString() + '\t');

                    //Falling
                    rtree = new GridRTree(
                        GetAllDataPath(DataType.Falling, d, size),
                        b   //branching factor
                    );

                    streamWriter.Write(new PointBasedTuneTime(rtree,
                        1, prefs).GetTuningTime().ToString() + '\t');
                    streamWriter.WriteLine(new IndexBasedTuneTime(rtree,
                       1, prefs).GetTuningTime().ToString());
                }
            }

            Console.WriteLine("End TuningSimIncDim()");
        }

        public static void TuningTime_IncSize_Uniform_MinMaxMixed(int d, int b)
        {
            Console.WriteLine("Begine TuningTime_IncSize_Uniform_MinMaxMixed()");

            string outPath = "TuningTime_IncSize_Uniform_MinMaxMixed_d" + d + ".txt";

            using (StreamWriter streamWriter = new StreamWriter(outPath))
            {

                streamWriter.WriteLine("Record Count\tP-B (Min, Min)\tI-B (Min, Min)" +
                    "\tP-B (Min, Max)\tI-B (Min, Max)" +
                    "\tP-B (Max, Min)\tI-B (Max, Min)" +
                    "\tP-B (Max, Max)\tI-B (Max, Max)");

                for (int dsSize = 10000; dsSize <= 100000; dsSize += 10000)
                {
                    Console.WriteLine("Size: " + dsSize);

                    streamWriter.Write(dsSize.ToString() + '\t');

                    //min, min
                    GridRTree rtree = new GridRTree(
                        GetAllDataPath(DataType.Uniform, d, dsSize),
                        b //+ ((dsSize / 10000) - 1)  //branching factor
                    );

                    streamWriter.Write(new PointBasedTuneTime(rtree, 1,
                        minPref[d]).GetTuningTime().ToString() + '\t');
                    streamWriter.Write(new IndexBasedTuneTime(rtree, 1,
                        minPref[d]).GetTuningTime().ToString() + '\t');

                    //min, max
                    streamWriter.Write(new PointBasedTuneTime(rtree, 1,
                        minMaxPref).GetTuningTime().ToString() + '\t');
                    streamWriter.Write(new IndexBasedTuneTime(rtree, 1,
                        minMaxPref).GetTuningTime().ToString() + '\t');

                    //max, min
                    streamWriter.Write(new PointBasedTuneTime(rtree, 1,
                        maxMinPref).GetTuningTime().ToString() + '\t');
                    streamWriter.Write(new IndexBasedTuneTime(rtree, 1,
                        maxMinPref).GetTuningTime().ToString() + '\t');

                    //max, max
                    streamWriter.Write(new PointBasedTuneTime(rtree, 1,
                        maxPref[d]).GetTuningTime().ToString() + '\t');
                    streamWriter.WriteLine(new IndexBasedTuneTime(rtree, 1,
                        maxPref[d]).GetTuningTime().ToString());
                }
            }
            Console.WriteLine("End TuningTime_IncSize_Uniform_MinMaxMixed()");
        }

        public static void TuningTime_IncDim_Uniform_AllMinAllMax(int dsSize, int b)
        {
            Console.WriteLine("Begine TuningTime_IncDim_Uniform_AllMinAllMax()");

            string outPath = "TuningTime_IncDim_Uniform_AllMinAllMax_r" + dsSize + ".txt";

            using (StreamWriter streamWriter = new StreamWriter(outPath))
            {

                streamWriter.WriteLine("Dimension\tP-B All Min\tI-B All Min" +
                    "\tP-B All Max\tI-B All Max");

                for (int d = 2; d <= 10; d++)
                {
                    Console.WriteLine("Dimension: " + d);

                    streamWriter.Write(d.ToString() + '\t');

                    //min, min
                    GridRTree rtree = new GridRTree(
                        GetAllDataPath(DataType.Uniform, d, dsSize),
                        b //+ ((dsSize / 10000) - 1)  //branching factor
                    );

                    streamWriter.Write(new PointBasedTuneTime(rtree, 1,
                        minPref[d]).GetTuningTime().ToString() + '\t');
                    streamWriter.Write(new IndexBasedTuneTime(rtree, 1,
                        minPref[d]).GetTuningTime().ToString() + '\t');

                    //max, max
                    streamWriter.Write(new PointBasedTuneTime(rtree, 1,
                        maxPref[d]).GetTuningTime().ToString() + '\t');
                    streamWriter.WriteLine(new IndexBasedTuneTime(rtree, 1,
                        maxPref[d]).GetTuningTime().ToString());
                }
            }
            Console.WriteLine("End TuningTime_IncDim_Uniform_AllMinAllMax()");
        }

        static void IncRecordCountDomTestPB()
        {
            string dataPath = DataPath() + "\\inc_record\\";
            int b = 10;
            int rep = 2;
            PrefSpec[] pref = {PrefSpec.Min, PrefSpec.Min, PrefSpec.Min};

            //int m = ??
            for (int i = 1; i <= 10; i++)
            {
                string file = "uniform_d3" + "_r" + i * 10000 + "_c" + i * 10000;
                GridRTree rtree = new GridRTree(dataPath + file, b);
                PointBasedPruning pbp = new PointBasedPruning(rtree, pref);
                Console.WriteLine(pbp.DominanceCount);
            }
        }

        static void IncRecordCountDomTestIB()
        {
            string dataPath = DataPath() + "\\inc_record\\";
            int b = 10;
            int rep = 2;
            PrefSpec[] pref = { PrefSpec.Min, PrefSpec.Min, PrefSpec.Min };

            //int m = ??
            for (int i = 1; i <= 10; i++)
            {
                string file = "uniform_d3" + "_r" + i * 10000 + "_c" + i * 10000;
                GridRTree rtree = new GridRTree(dataPath + file, b);
                IndexBasedPruning pbp = new IndexBasedPruning(rtree, pref);
                Console.WriteLine(pbp.DominanceCount);
            }
        }

        static void IncreaseDimension()
        {
            string dataPath = DataPath() + "\\data_inc_dim\\";
            int b = 10;
            int rep = 2;
            //int m = ??
            GridRTree rtree = new GridRTree(dataPath + "skyline_2d_10000.txt", b);
            Console.Write(
                OneTimeIndexPercentage.GetDataPointsInBytes(rtree) + "\t");
            Console.Write(
                OneTimeIndexPercentage.GetIndexInBytes(rtree.Root) + "\t");
            //Console.Write(
            //    OneMIndexPercentage.GetIndexInBytes(rtree.Root) + "\t");
            Console.Write(
                DFDIIndexPercentage.GetIndexInBytes(rtree.Root, 1, rep) + "\t");
            Console.Write(
                OneTimeIndexPercentage.GetPercentage(rtree).ToString("f4") + "\t");
            Console.Write(
                OneMIndexPercentage.GetPercentage(rtree).ToString("f4") + "\t");
            Console.Write(
                DFDIIndexPercentage.GetPercentage(rtree, rep).ToString("f4") + "\t");
            Console.WriteLine();

            rtree = new GridRTree(dataPath + "skyline_3d_10000.txt", b);
            Console.Write(
                OneTimeIndexPercentage.GetDataPointsInBytes(rtree) + "\t");
            Console.Write(
                OneTimeIndexPercentage.GetIndexInBytes(rtree.Root) + "\t");
            //Console.Write(
            //    OneMIndexPercentage.GetIndexInBytes(rtree.Root) + "\t");
            Console.Write(
                DFDIIndexPercentage.GetIndexInBytes(rtree.Root, 1, rep) + "\t");
            Console.Write(
                OneTimeIndexPercentage.GetPercentage(rtree).ToString("f4") + "\t");
            Console.Write(
                OneMIndexPercentage.GetPercentage(rtree).ToString("f4") + "\t");
            Console.Write(
                DFDIIndexPercentage.GetPercentage(rtree, rep).ToString("f4") + "\t");
            Console.WriteLine();

            rtree = new GridRTree(dataPath + "skyline_4d_10000.txt", b);
            Console.Write(
                OneTimeIndexPercentage.GetDataPointsInBytes(rtree) + "\t");
            Console.Write(
                OneTimeIndexPercentage.GetIndexInBytes(rtree.Root) + "\t");
            //Console.Write(
            //    OneMIndexPercentage.GetIndexInBytes(rtree.Root) + "\t");
            Console.Write(
                DFDIIndexPercentage.GetIndexInBytes(rtree.Root, 1, rep) + "\t");
            Console.Write(
                OneTimeIndexPercentage.GetPercentage(rtree).ToString("f4") + "\t");
            Console.Write(
                OneMIndexPercentage.GetPercentage(rtree).ToString("f4") + "\t");
            Console.Write(
                DFDIIndexPercentage.GetPercentage(rtree, rep).ToString("f4") + "\t");
            Console.WriteLine();

            rtree = new GridRTree(dataPath + "skyline_5d_10000.txt", b);
            Console.Write(
                OneTimeIndexPercentage.GetDataPointsInBytes(rtree) + "\t");
            Console.Write(
                OneTimeIndexPercentage.GetIndexInBytes(rtree.Root) + "\t");
            //Console.Write(
            //    OneMIndexPercentage.GetIndexInBytes(rtree.Root) + "\t");
            Console.Write(
                DFDIIndexPercentage.GetIndexInBytes(rtree.Root, 1, rep) + "\t");
            Console.Write(
                OneTimeIndexPercentage.GetPercentage(rtree).ToString("f4") + "\t");
            Console.Write(
                OneMIndexPercentage.GetPercentage(rtree).ToString("f4") + "\t");
            Console.Write(
                DFDIIndexPercentage.GetPercentage(rtree, rep).ToString("f4") + "\t");
            Console.WriteLine();

            rtree = new GridRTree(dataPath + "skyline_6d_10000.txt", b);
            Console.Write(
                OneTimeIndexPercentage.GetDataPointsInBytes(rtree) + "\t");
            Console.Write(
                OneTimeIndexPercentage.GetIndexInBytes(rtree.Root) + "\t");
            //Console.Write(
            //    OneMIndexPercentage.GetIndexInBytes(rtree.Root) + "\t");
            Console.Write(
                DFDIIndexPercentage.GetIndexInBytes(rtree.Root, 1, rep) + "\t");
            Console.Write(
                OneTimeIndexPercentage.GetPercentage(rtree).ToString("f4") + "\t");
            Console.Write(
                OneMIndexPercentage.GetPercentage(rtree).ToString("f4") + "\t");
            Console.Write(
                DFDIIndexPercentage.GetPercentage(rtree, rep).ToString("f4") + "\t");
            Console.WriteLine();

            rtree = new GridRTree(dataPath + "skyline_7d_10000.txt", b);
            Console.Write(
                OneTimeIndexPercentage.GetDataPointsInBytes(rtree) + "\t");
            Console.Write(
                OneTimeIndexPercentage.GetIndexInBytes(rtree.Root) + "\t");
            //Console.Write(
            //    OneMIndexPercentage.GetIndexInBytes(rtree.Root) + "\t");
            Console.Write(
                DFDIIndexPercentage.GetIndexInBytes(rtree.Root, 1, rep) + "\t");
            Console.Write(
                OneTimeIndexPercentage.GetPercentage(rtree).ToString("f4") + "\t");
            Console.Write(
                OneMIndexPercentage.GetPercentage(rtree).ToString("f4") + "\t");
            Console.Write(
                DFDIIndexPercentage.GetPercentage(rtree, rep).ToString("f4") + "\t");
            Console.WriteLine();

            rtree = new GridRTree(dataPath + "skyline_8d_10000.txt", b);
            Console.Write(
                OneTimeIndexPercentage.GetDataPointsInBytes(rtree) + "\t");
            Console.Write(
                OneTimeIndexPercentage.GetIndexInBytes(rtree.Root) + "\t");
            //Console.Write(
            //    OneMIndexPercentage.GetIndexInBytes(rtree.Root) + "\t");
            Console.Write(
                DFDIIndexPercentage.GetIndexInBytes(rtree.Root, 1, rep) + "\t");
            Console.Write(
                OneTimeIndexPercentage.GetPercentage(rtree).ToString("f4") + "\t");
            Console.Write(
                OneMIndexPercentage.GetPercentage(rtree).ToString("f4") + "\t");
            Console.Write(
                DFDIIndexPercentage.GetPercentage(rtree, rep).ToString("f4") + "\t");
            Console.WriteLine();

            rtree = new GridRTree(dataPath + "skyline_9d_10000.txt", b);
            Console.Write(
                OneTimeIndexPercentage.GetDataPointsInBytes(rtree) + "\t");
            Console.Write(
                OneTimeIndexPercentage.GetIndexInBytes(rtree.Root) + "\t");
            //Console.Write(
            //    OneMIndexPercentage.GetIndexInBytes(rtree.Root) + "\t");
            Console.Write(
                DFDIIndexPercentage.GetIndexInBytes(rtree.Root, 1, rep) + "\t");
            Console.Write(
                OneTimeIndexPercentage.GetPercentage(rtree).ToString("f4") + "\t");
            Console.Write(
                OneMIndexPercentage.GetPercentage(rtree).ToString("f4") + "\t");
            Console.Write(
                DFDIIndexPercentage.GetPercentage(rtree, rep).ToString("f4") + "\t");
            Console.WriteLine();

            rtree = new GridRTree(dataPath + "skyline_10d_10000.txt", b);
            Console.Write(
                OneTimeIndexPercentage.GetDataPointsInBytes(rtree) + "\t");
            Console.Write(
                OneTimeIndexPercentage.GetIndexInBytes(rtree.Root) + "\t");
            //Console.Write(
            //    OneMIndexPercentage.GetIndexInBytes(rtree.Root) + "\t");
            Console.Write(
                DFDIIndexPercentage.GetIndexInBytes(rtree.Root, 1, rep) + "\t");
            Console.Write(
                OneTimeIndexPercentage.GetPercentage(rtree).ToString("f4") + "\t");
            Console.Write(
                OneMIndexPercentage.GetPercentage(rtree).ToString("f4") + "\t");
            Console.Write(
                DFDIIndexPercentage.GetPercentage(rtree, rep).ToString("f4") + "\t");
            Console.WriteLine();
        }

        static string DataPath()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(
                Environment.CurrentDirectory);
            dirInfo = dirInfo.Parent.Parent.Parent;
            return dirInfo.FullName + "\\data";
        }

        /// <summary>
        /// Gets data file path.
        /// </summary>
        /// <param name="dt">type of data</param>
        /// <param name="d">dimension</param>
        /// <param name="dsSize">data set size</param>
        /// <returns></returns>
        public static string GetDataPath(DataType dt, int d, int dsSize)
        {
            StringBuilder path = new StringBuilder(DataPath() + '\\');
            
            //Data type
            if(dt == DataType.Uniform)
                path.Append("uniform\\");
            else if(dt == DataType.Falling)
                path.Append("falling\\");
            else
                path.Append("rising\\");

            //Dimension
            path.Append(d + "d\\");

            //File name
            if(dt == DataType.Uniform)
                path.Append("uniform");
            else if(dt == DataType.Falling)
                path.Append("fall");
            else
                path.Append("rise");
            path.Append("_d" + d + "_r" + dsSize + "_c" + dsSize);
            if (dt == DataType.Falling ||
                dt == DataType.Rising)
                path.Append("_w20");
            path.Append(".txt");
            return path.ToString();
        }

        public static string UniformDataPath(int d, int size)
        {
            StringBuilder path = new StringBuilder(
                @"C:\Users\Jeff\Desktop\SkylineDataGenerator\SkylineDataGenerator\bin\Release\");
            path.Append("uniform_d" + d + "_r" + size + "_c" + size + ".txt");
            return path.ToString();
        }

        public static string GetAllDataPath(DataType dt, int d, int dsSize)
        {
            StringBuilder path = new StringBuilder(
                @"C:\Users\Jeff\Desktop\data\");

            //File name
            if (dt == DataType.Uniform)
                path.Append("uniform");
            else if (dt == DataType.Falling)
                path.Append("fall");
            else
                path.Append("rise");
            path.Append("_d" + d + "_r" + dsSize + "_c" + dsSize);
            if (dt == DataType.Falling ||
                dt == DataType.Rising)
                path.Append("_w20");
            path.Append(".txt");
            return path.ToString();
        }

        static int CountDataPoints(Node node)
        {
            if (node is LeafNode)
            {
                LeafNode leafNode = (LeafNode)node;
                return leafNode.Points.Count;
            }

            IntNode intNode = (IntNode)node;
            int count = 0;
            foreach (Node n in intNode.Children)
            {
                count += CountDataPoints(n);
            }

            return count;
        }

        static void BinarySerializeToFile(Object obj, String path)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(path, FileMode.Create,
                FileAccess.Write, FileShare.None))
            {
                formatter.Serialize(stream, obj);
            }
        }

        static Object BinaryDeserializeFromFile(String path)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(path, FileMode.Open,
                FileAccess.Read, FileShare.Read))
            {
                return formatter.Deserialize(stream);
            }
        }
    }
}

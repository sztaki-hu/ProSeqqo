using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LineLikeAnimation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Point3DCollection travelLines = new Point3DCollection();
        private Point3DCollection invisibleLines = new Point3DCollection();
        private Point3DCollection workLines = new Point3DCollection();
        private Point3DCollection points = new Point3DCollection();
        private readonly Stopwatch watch = new Stopwatch();
        private int numberOfPoints;
        public int maxNumberOfLines;

        public int MaxNumberOfLines
        {
            get
            {
                return this.maxNumberOfLines;
            }

            set
            {
                this.maxNumberOfLines = value;
                this.RaisePropertyChanged("MaxNumberOfLines");
            }
        }
        public int NumberOfPoints
        {
            get
            {
                return this.numberOfPoints;
            }

            set
            {
                this.numberOfPoints = value;
                Amination();
                this.RaisePropertyChanged("NumberOfPoints");
            }
        }
        public bool ShowTravelLines { get; set; }
        public bool ShowPoints { get; set; }
        public bool ShowWorkLines { get; set; }
        public bool ShowInvisibleLines { get; set; }
        public bool ShowGrid { get; set; }
        public bool Play { get; set; }

        public Point3DCollection TravelLines
        {
            get
            {
                return this.travelLines;
            }

            set
            {
                this.travelLines = value;
                this.RaisePropertyChanged("TravelLines");
            }
        }
        public Point3DCollection WorkLines
        {
            get
            {
                return this.workLines;
            }

            set
            {
                this.workLines = value;
                this.RaisePropertyChanged("WorkLines");
            }
        }
        public Point3DCollection InvisibleLines
        {
            get
            {
                return this.invisibleLines;
            }

            set
            {
                this.invisibleLines = value;
                this.RaisePropertyChanged("UnvisibleLines");
            }
        }
        public Point3DCollection Points
        {
            get
            {
                return this.points;
            }

            set
            {
                this.points = value;
                this.RaisePropertyChanged("Points");
            }
        }
        private List<Line> GTSPLines { get; set; }
        private List<Point> GTSPoints { get; set; }
        private Point Start { get; set; }
        private Point Finish { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            this.InitializeComponent();
            ShowWorkLines = true;
            ShowInvisibleLines = true;
            ShowTravelLines = true;
            GTSPLines = new List<Line>();
            GTSPoints = new List<Point>();
            string inputfile = Environment.GetCommandLineArgs()[0];
            this.watch.Start();
            this.NumberOfPoints = 0;
            this.DataContext = this;

            CompositionTarget.Rendering += this.OnCompositionTargetRendering;
        }

        public void Amination()
        {

          AnimationStepOne(numberOfPoints);


            //if (!ShowTravelLines)
            //{
            //    travelLines.Clear();
            //}
            //if (!ShowWorkLines)
            //{
            //    workLines.Clear();
            //}
            //if (!ShowUnvisibleLines)
            //{
            //    unvisibleLines.Clear();
            //}
            //if (!ShowPoints)
            //{
            //    points.Clear();
            //}

            travelLines.Clear();
            workLines.Clear();
            invisibleLines.Clear();
            points.Clear();

            MaxNumberOfLines = GTSPLines.Count;
            for (int i = 0; i < MaxNumberOfLines; i++)
            {
                if (i < numberOfPoints)
                {
                    GTSPLines[i].Visibility = true;
                }
                else
                {
                    GTSPLines[i].Visibility = false;
                }

                if (ShowInvisibleLines)
                {
                    if (GTSPLines[i].DrawType != DrawType.Travel)
                    {
                        invisibleLines.Add(GTSPLines[i].A.Config);
                        invisibleLines.Add(GTSPLines[i].B.Config);
                    }
                }

                if (GTSPLines[i].Visibility == true)
                {
                    if (GTSPLines[i].DrawType == DrawType.Work && ShowWorkLines)
                    {
                        workLines.Add(GTSPLines[i].A.Config);
                        workLines.Add(GTSPLines[i].B.Config);
                    }
                    if (GTSPLines[i].DrawType == DrawType.Work && ShowPoints)
                    {
                        Points.Add(GTSPLines[i].A.Config);
                        Points.Add(GTSPLines[i].B.Config);
                    }
                    if (GTSPLines[i].DrawType == DrawType.Travel && ShowTravelLines)
                    {
                        travelLines.Add(GTSPLines[i].A.Config);
                        travelLines.Add(GTSPLines[i].B.Config);
                    }
                }
                TravelLines = travelLines;
                WorkLines = workLines;
                InvisibleLines = invisibleLines;
            }
        }

        public void AnimationStepOne(int i)
        {
            if (GTSPLines.Count > i)
            {
                GTSPLines[i].Visibility = true;

                if (ShowInvisibleLines)
                {
                    if (GTSPLines[i].DrawType != DrawType.Travel)
                    {
                        invisibleLines.Add(GTSPLines[i].A.Config);
                        invisibleLines.Add(GTSPLines[i].B.Config);
                    }
                }

                if (GTSPLines[i].Visibility == true)
                {
                    if (GTSPLines[i].DrawType == DrawType.Work && ShowWorkLines)
                    {
                        workLines.Add(GTSPLines[i].A.Config);
                        workLines.Add(GTSPLines[i].B.Config);
                    }
                    if (GTSPLines[i].DrawType == DrawType.Work && ShowPoints)
                    {
                        Points.Add(GTSPLines[i].A.Config);
                        Points.Add(GTSPLines[i].B.Config);
                    }
                    if (GTSPLines[i].DrawType == DrawType.Travel && ShowTravelLines)
                    {
                        travelLines.Add(GTSPLines[i].A.Config);
                        travelLines.Add(GTSPLines[i].B.Config);
                    }
                }
                TravelLines = travelLines;
                WorkLines = workLines;
                InvisibleLines = invisibleLines;
            }
            
        }

        public void PhraseFile(string file)
        {
            List<string> linesRaw = new List<string>();
            GTSPLines.Clear();
            var lines = File.ReadAllLines(file);
            foreach (var item in lines)
            {

                if (item.Length > 0 && !item.Contains("Start") && !item.Contains("Finish") )
                    if (item[0] != '#')
                    {
                        linesRaw.Add(item);
                    }
                if (item.Contains("Start"))
                {

                }
                if (item.Contains("Finish"))
                {

                }
            }

            foreach (var item in linesRaw)
            {
                string[] conf = item.Split('[',']');
                string confA = conf[1];
                string confB = conf[3];
                int dim = confA.Split(';').Length;
                string[] line = item.Split(';');

                Line l = new Line()
                {
                    ID = int.Parse(line[0]),
                    ContourID = int.Parse(line[1]),
                    Name = line[2],
                    Length = double.Parse(line[3]),
                    DrawType = DrawType.Work,
                    A = new Point(line[4], line[5], confA),
                    B = new Point(line[6+dim], line[7+dim], confB)
                };
                GTSPLines.Add(l);
                GTSPoints.Add(l.A);
                GTSPoints.Add(l.B);
                Console.WriteLine(item);
            }

            var tmp = new List<Line>();
            for (int i = 0; i < linesRaw.Count-1; i++)
            {
                var str = linesRaw[i].Split(';');
                var cost = double.Parse(str[str.Length-1]);
                if(cost == 0)
                {
                    tmp.Add(GTSPLines[i]);
                }
                else
                {
                    tmp.Add(GTSPLines[i]);
                    Line l = new Line()
                    {
                            ID = -1,
                            ContourID = -1,
                            Name = "Travel",
                            Length = cost,
                            DrawType = DrawType.Travel,
                            A = GTSPLines[i].B,
                            B = GTSPLines[i + 1].A
                    };
                    tmp.Add(l);
                }
            }
            tmp.Add(GTSPLines[linesRaw.Count-1]);
            GTSPLines = tmp;
            MaxNumberOfLines = GTSPLines.Count;
            NumberOfPoints = GTSPLines.Count;
            //Amination();
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnCompositionTargetRendering(object sender, EventArgs e)
        {
            this.UpdatePoints();
        }

        public static IEnumerable<Point3D> GeneratePoints(int n, double time)
        {
            const double R = 2;
            const double Q = 0.5;
            for (int i = 0; i < n; i++)
            {
                double t = Math.PI * 2 * i / (n - 1);
                double u = (t * 24) + (time * 5);
                var pt = new Point3D(Math.Cos(t) * (R + (Q * Math.Cos(u))), Math.Sin(t) * (R + (Q * Math.Cos(u))), Q * Math.Sin(u));
                yield return pt;
                if (i > 0 && i < n - 1)
                {
                    yield return pt;
                }
            }
        }

        private void UpdatePoints()
        {
            if (Play && numberOfPoints < maxNumberOfLines)
                NumberOfPoints++;
            if (numberOfPoints == maxNumberOfLines)
                Play = false;
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                PhraseFile(openFileDialog.FileName);
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            Play = !Play;
            
        }
    }
}

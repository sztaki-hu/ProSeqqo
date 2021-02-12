using Microsoft.Win32;
using SequencePlanner.GTSPTask.Result;
using SequencePlanner.GTSPTask.Serialization.Result;
using SequencePlanner.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace LineAnimation
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
                this.RaisePropertyChanged("InvisibleLines");
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
            //if (!ShowInvisibleLines)
            //{
            //    invisibleLines.Clear();
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

        public void OpenFile(string file)
        {
            LineLikeResultSerializer serializer = new LineLikeResultSerializer();
            LineTaskResult result = null;
            if(file.Contains(".json"))
                result = serializer.ImportJSON(file);
            if (file.Contains(".xml"))
                result = serializer.ImportXML(file);
            if (file.Contains(".txt") || file.Contains(".seq"))
                result = serializer.ImportSEQ(file);
            if (result is null)
                throw new SeqException("File format not supported.");
            GTSPLines.Clear();
            GTSPoints.Clear();
            for (int i = 1; i < result.PositionResult.Count; i++)
            {
                Point3D point = new Point3D();
                if (result.PositionResult[i].Vector.Length > 0)
                    point.X = result.PositionResult[i].Vector[0];
                if (result.PositionResult[i].Vector.Length > 1)
                    point.Y = result.PositionResult[i].Vector[1];
                if (result.PositionResult[i].Vector.Length > 2)
                    point.Z = result.PositionResult[i].Vector[2];
                GTSPoints.Add(new Point(result.PositionResult[i].UserID.ToString(), result.PositionResult[i].Name, result.PositionResult[i].ToString()) { Config = point});
            }

            for (int i = 1; i < GTSPoints.Count-1; i+=2)
            {
                Line l = new Line()
                {
                    ID = 0,
                    ContourID = 0,
                    Name = "",
                    Length = 10,
                    DrawType = DrawType.Travel,
                    A = GTSPoints[i-1],
                    B = GTSPoints[i]
                };
                GTSPLines.Add(l);

                l = new Line()
                {
                    ID = 0,
                    ContourID = 0,
                    Name = "",
                    Length = 10,
                    DrawType = DrawType.Work,
                    A = GTSPoints[i],
                    B = GTSPoints[i+1]
                };
                GTSPLines.Add(l);
            }

            MaxNumberOfLines = GTSPLines.Count;
            NumberOfPoints = 0;
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
                OpenFile(openFileDialog.FileName);
              //PhraseFile(openFileDialog.FileName);
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            Play = !Play;
        }
    }
}


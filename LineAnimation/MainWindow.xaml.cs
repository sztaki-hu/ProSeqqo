using Microsoft.Win32;
using SequencePlanner.GTSPTask.Serialization.Result;
using SequencePlanner.Helper;
using SequencePlanner.Model.Hierarchy;
using SequencePlanner.Task;
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

        private int prevStep = 0;
        public void Amination()
        {
            //AnimationStepOne(numberOfPoints);
            if (prevStep + 1 == numberOfPoints)
                StepOne();
            else
                TakeAllStep();
            prevStep = numberOfPoints;
        }

        private void StepOne()
        {
            var i = numberOfPoints;
            if (i >= GTSPLines.Count)
                return;
            GTSPLines[i].Visibility = true;
            if (ShowInvisibleLines)
            {
                if (GTSPLines[i].DrawType == DrawType.Work)
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

        private void TakeAllStep()
        {
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
                    if (GTSPLines[i].DrawType == DrawType.Work)
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
            GeneralResultSerializer serializer = new GeneralResultSerializer();
            GeneralTaskResult result = null;
            if (file.Contains(".json"))
                result = serializer.ImportJSON(file);
            if (file.Contains(".xml"))
                result = serializer.ImportXML(file);
            if (file.Contains(".txt") || file.Contains(".seq"))
                result = serializer.ImportSEQ(file);
            if (result is null)
                throw new SeqException("File format not supported.");
            GTSPLines.Clear();
            GTSPoints.Clear();
         
            for (int i = 0; i < result.SolutionMotion.Count-1; i++)
            {
                var motion = result.SolutionMotion[i];
                if(!motion.Virtual)
                    GTSPLines.Add(new Line(motion));
                var nextMotion = result.SolutionMotion[i + 1];
                if(!motion.Virtual && !nextMotion.Virtual)
                    if(!isContinious(motion, nextMotion))
                        GTSPLines.Add(new Line(motion, nextMotion));
            }
            MaxNumberOfLines = GTSPLines.Count;
            NumberOfPoints = 0;
        }

        private bool isContinious(Motion motion, Motion nextMotion)
        {
            if (motion.LastConfig.Configuration.Count != nextMotion.FirstConfig.Configuration.Count)
                return false;
            for (int i = 0; i < motion.LastConfig.Configuration.Count; i++)
            {
                if (motion.LastConfig.Configuration[i] != nextMotion.FirstConfig.Configuration[i])
                    return false;
            }
            return true;
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
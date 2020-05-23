using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Options.Values
{
    public class PositionMatrixOptionValue
    {
        public List<int> ID { get; set; }
        public List<string> Name { get; set; }
        public int Dim { get; set; }
        public double[,] Matrix { get; set; }

        public PositionMatrixOptionValue()
        {
            ID = new List<int>();
            Dim = 0;
            Name = new List<string>();
        }

        public void fromString(List<string> input)
        {
            string[] tmp = input[1].Split(';');     //1;2;3;4--->[1][2][3][4]
            foreach (var item in tmp)
            {
                ID.Add(int.Parse(item));
            }
            Dim = ID.Count;
            Matrix = new double[Dim,Dim];
            for (int i = 2; i < Dim+2; i++)
            {
                tmp = input[i].Split(';');  //10;20;30;40;Name--->[10][20][30][40][Name]
                for (int j = 0; j < ID.Count; j++)
                {
                    Matrix[i - 2, j] = double.Parse(tmp[j]);
                }            
            }
            if (input.Count > Dim + 1)
            {
                tmp = input[input.Count-1].Split(';');
                for (int i = 0; i < tmp.Length; i++)
                {
                    Name.Add(tmp[i]);
                }
            }
        }
    }
}

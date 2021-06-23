using Executer.Core;
using System;
using System.Collections.Generic;

namespace Executer.Documents
{
    internal class LineFitData
    {
        public LineFitData(double[] x, double[] y)
        {
            this.X = x;
            this.Y = y;
        }

        public double[] X { get; }
        public double[] Y { get; }
    }
}

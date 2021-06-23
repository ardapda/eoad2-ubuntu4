using Executer.Core;
using System;
using System.Collections.Generic;

namespace Executer.Documents
{
    /// <summary>
    /// This is the a demo user-specific data structure which contains the data points
    /// and their uncertainties
    /// </summary>
    public class CustomUserVariable
    {
        public double[] X;
        public double[] Y;
        public double[] Ey;
    }
}

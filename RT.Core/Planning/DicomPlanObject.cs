﻿using Dicom;
using RT.Core.IO.Loaders;
using RT.Core.DICOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RT.Core.Planning
{
    public class DicomPlanObject:DicomObject
    {
        public List<Beam> Beams { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public double TotalMU { get { double total = 0; foreach (Beam beam in Beams) { total += beam.MU; } return total; } }
        public DicomPlanObject() { }
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading.Tasks;
using Valuechanges;

namespace Interceptions
{
    public class Valuechanges
    {
        public double[] _valuechange = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
        public double[] _ValueChange = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
        public double this[int index]
        {
            get { return _ValueChange[index]; }
            set
            {
                if (_valuechange[index] != value)
                    _ValueChange[index] = value - _valuechange[index];
                else
                    _ValueChange[index] = 0;
                _valuechange[index] = value;
            }
        }
    }
    public class SendInterception
    {
        [DllImport("user32.dll")]
        private static extern void SetPhysicalCursorPos(int X, int Y);
        [DllImport("user32.dll")]
        private static extern void SetCaretPos(int X, int Y);
        [DllImport("user32.dll")]
        private static extern void SetCursorPos(int X, int Y);
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        private Valuechanges ValueChanges = new Valuechanges();
        private Input input = new Input();
        private List<int> keyboard_ids = new List<int>(), mouse_ids = new List<int>();
        private bool keyboard_id_alreadyexist, mouse_id_alreadyexist;
        private bool formvisible;
        private Form1 form1 = new Form1();
        private Stopwatch PollingRate;
        private double pollingrateperm = 0, pollingratetemp = 0, pollingratedisplay = 0, pollingrate;
        private string inputdelaybutton = "", inputdelay = "";
        public Valuechange ValueChange;
        private double delay, elapseddown, elapsedup, elapsed;
        private bool getstate = false;
        private int[] wd = { 2 };
        private int[] wu = { 2 };
        public void valchanged(int n, bool val)
        {
            if (val)
            {
                if (wd[n] <= 1)
                {
                    wd[n] = wd[n] + 1;
                }
                wu[n] = 0;
            }
            else
            {
                if (wu[n] <= 1)
                {
                    wu[n] = wu[n] + 1;
                }
                wd[n] = 0;
            }
        }
        public SendInterception()
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
            input.KeyboardFilterMode = KeyboardFilterMode.All;
            input.MouseFilterMode = MouseFilterMode.All;
            input.Load();
        }
        public void ViewData(string inputdelaybutton = "")
        {
            if (!formvisible)
            {
                PollingRate = new Stopwatch();
                PollingRate.Start();
                ValueChange = new Valuechange();
                this.inputdelaybutton = inputdelaybutton;
                formvisible = true;
                Task.Run(() => form1.SetVisible());
            }
        }
        public void Connect(int number = 0)
        {
        }
        public void Disconnect()
        {
            foreach (int keyboard_id in keyboard_ids)
                foreach (int mouse_id in mouse_ids)
                    Set(keyboard_id, mouse_id, 0, 0, 0, 0, 0, 0, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false);
            input.Unload();
        }
        public void Set(int keyboard_id, int mouse_id, double MouseDesktopX, double MouseDesktopY, double deltaX, double deltaY, double x, double y, bool SendLeftClick, bool SendRightClick, bool SendMiddleClick, bool SendWheelUp, bool SendWheelDown, bool SendCANCEL, bool SendBACK, bool SendTAB, bool SendCLEAR, bool SendRETURN, bool SendSHIFT, bool SendCONTROL, bool SendMENU, bool SendCAPITAL, bool SendESCAPE, bool SendSPACE, bool SendPRIOR, bool SendNEXT, bool SendEND, bool SendHOME, bool SendLEFT, bool SendUP, bool SendRIGHT, bool SendDOWN, bool SendSNAPSHOT, bool SendINSERT, bool SendNUMPADDEL, bool SendNUMPADINSERT, bool SendHELP, bool SendAPOSTROPHE, bool SendBACKSPACE, bool SendPAGEDOWN, bool SendPAGEUP, bool SendFIN, bool SendMOUSE, bool SendA, bool SendB, bool SendC, bool SendD, bool SendE, bool SendF, bool SendG, bool SendH, bool SendI, bool SendJ, bool SendK, bool SendL, bool SendM, bool SendN, bool SendO, bool SendP, bool SendQ, bool SendR, bool SendS, bool SendT, bool SendU, bool SendV, bool SendW, bool SendX, bool SendY, bool SendZ, bool SendLWIN, bool SendRWIN, bool SendAPPS, bool SendDELETE, bool SendNUMPAD0, bool SendNUMPAD1, bool SendNUMPAD2, bool SendNUMPAD3, bool SendNUMPAD4, bool SendNUMPAD5, bool SendNUMPAD6, bool SendNUMPAD7, bool SendNUMPAD8, bool SendNUMPAD9, bool SendMULTIPLY, bool SendADD, bool SendSUBTRACT, bool SendDECIMAL, bool SendPRINTSCREEN, bool SendDIVIDE, bool SendF1, bool SendF2, bool SendF3, bool SendF4, bool SendF5, bool SendF6, bool SendF7, bool SendF8, bool SendF9, bool SendF10, bool SendF11, bool SendF12, bool SendNUMLOCK, bool SendSCROLLLOCK, bool SendLEFTSHIFT, bool SendRIGHTSHIFT, bool SendLEFTCONTROL, bool SendRIGHTCONTROL, bool SendLEFTALT, bool SendRIGHTALT, bool SendBROWSER_BACK, bool SendBROWSER_FORWARD, bool SendBROWSER_REFRESH, bool SendBROWSER_STOP, bool SendBROWSER_SEARCH, bool SendBROWSER_FAVORITES, bool SendBROWSER_HOME, bool SendVOLUME_MUTE, bool SendVOLUME_DOWN, bool SendVOLUME_UP, bool SendMEDIA_NEXT_TRACK, bool SendMEDIA_PREV_TRACK, bool SendMEDIA_STOP, bool SendMEDIA_PLAY_PAUSE, bool SendLAUNCH_MAIL, bool SendLAUNCH_MEDIA_SELECT, bool SendLAUNCH_APP1, bool SendLAUNCH_APP2, bool SendOEM_1, bool SendOEM_PLUS, bool SendOEM_COMMA, bool SendOEM_MINUS, bool SendOEM_PERIOD, bool SendOEM_2, bool SendOEM_3, bool SendOEM_4, bool SendOEM_5, bool SendOEM_6, bool SendOEM_7, bool SendOEM_8, bool SendOEM_102, bool SendEREOF, bool SendZOOM, bool SendEscape, bool SendOne, bool SendTwo, bool SendThree, bool SendFour, bool SendFive, bool SendSix, bool SendSeven, bool SendEight, bool SendNine, bool SendZero, bool SendDashUnderscore, bool SendPlusEquals, bool SendBackspace, bool SendTab, bool SendOpenBracketBrace, bool SendCloseBracketBrace, bool SendEnter, bool SendControl, bool SendSemicolonColon, bool SendSingleDoubleQuote, bool SendTilde, bool SendLeftShift, bool SendBackslashPipe, bool SendCommaLeftArrow, bool SendPeriodRightArrow, bool SendForwardSlashQuestionMark, bool SendRightShift, bool SendRightAlt, bool SendSpace, bool SendCapsLock, bool SendUp, bool SendDown, bool SendRight, bool SendLeft, bool SendHome, bool SendEnd, bool SendDelete, bool SendPageUp, bool SendPageDown, bool SendInsert, bool SendPrintScreen, bool SendNumLock, bool SendScrollLock, bool SendMenu, bool SendWindowsKey, bool SendNumpadDivide, bool SendNumpadAsterisk, bool SendNumpad7, bool SendNumpad8, bool SendNumpad9, bool SendNumpad4, bool SendNumpad5, bool SendNumpad6, bool SendNumpad1, bool SendNumpad2, bool SendNumpad3, bool SendNumpad0, bool SendNumpadDelete, bool SendNumpadEnter, bool SendNumpadPlus, bool SendNumpadMinus)
        {
            keyboard_id_alreadyexist = keyboard_ids.Contains(keyboard_id);
            if (!keyboard_id_alreadyexist)
                keyboard_ids.Add(keyboard_id);
            mouse_id_alreadyexist = mouse_ids.Contains(mouse_id);
            if (!mouse_id_alreadyexist)
                mouse_ids.Add(mouse_id);
            if (deltaX != 0f | deltaY != 0f)
                MoveMouseBy(input, (int)deltaX, (int)deltaY, mouse_id);
            if (x != 0f | y != 0f)
                MoveMouseTo(input, (int)x, (int)y, mouse_id);
            if (MouseDesktopX != 0f | MouseDesktopY != 0f)
            {
                System.Windows.Forms.Cursor.Position = new System.Drawing.Point((int)MouseDesktopX, (int)MouseDesktopY);
                SetPhysicalCursorPos((int)MouseDesktopX, (int)MouseDesktopY);
                SetCaretPos((int)MouseDesktopX, (int)MouseDesktopY);
                SetCursorPos((int)MouseDesktopX, (int)MouseDesktopY);
                Microsoft.Xna.Framework.Input.Mouse.SetPosition((int)MouseDesktopX, (int)MouseDesktopY);
            }
            ValueChanges[1] = SendLeftClick ? 1 : 0;
            if (ValueChanges._ValueChange[1] > 0f)
                mouseclickleft(input, mouse_id);
            if (ValueChanges._ValueChange[1] < 0f)
                mouseclickleftF(input, mouse_id);
            ValueChanges[2] = SendRightClick ? 1 : 0;
            if (ValueChanges._ValueChange[2] > 0f)
                mouseclickright(input, mouse_id);
            if (ValueChanges._ValueChange[2] < 0f)
                mouseclickrightF(input, mouse_id);
            ValueChanges[3] = SendMiddleClick ? 1 : 0;
            if (ValueChanges._ValueChange[3] > 0f)
                mouseclickmiddle(input, mouse_id);
            if (ValueChanges._ValueChange[3] < 0f)
                mouseclickmiddleF(input, mouse_id);
            ValueChanges[4] = SendWheelUp ? 1 : 0;
            if (ValueChanges._ValueChange[4] > 0f)
                mousewheelup(input, mouse_id);
            ValueChanges[5] = SendWheelDown ? 1 : 0;
            if (ValueChanges._ValueChange[5] > 0f)
                mousewheeldown(input, mouse_id);
            ValueChanges[6] = SendCANCEL ? 1 : 0;
            if (ValueChanges._ValueChange[6] > 0f)
                keyboardkey(input, Keys.CANCEL, keyboard_id);
            if (ValueChanges._ValueChange[6] < 0f)
                keyboardkeyF(input, Keys.CANCEL, keyboard_id);
            ValueChanges[7] = SendBACK ? 1 : 0;
            if (ValueChanges._ValueChange[7] > 0f)
                keyboardkey(input, Keys.BACK, keyboard_id);
            if (ValueChanges._ValueChange[7] < 0f)
                keyboardkeyF(input, Keys.BACK, keyboard_id);
            ValueChanges[8] = SendTAB ? 1 : 0;
            if (ValueChanges._ValueChange[8] > 0f)
                keyboardkey(input, Keys.TAB, keyboard_id);
            if (ValueChanges._ValueChange[8] < 0f)
                keyboardkeyF(input, Keys.TAB, keyboard_id);
            ValueChanges[9] = SendCLEAR ? 1 : 0;
            if (ValueChanges._ValueChange[9] > 0f)
                keyboardkey(input, Keys.CLEAR, keyboard_id);
            if (ValueChanges._ValueChange[9] < 0f)
                keyboardkeyF(input, Keys.CLEAR, keyboard_id);
            ValueChanges[10] = SendRETURN ? 1 : 0;
            if (ValueChanges._ValueChange[10] > 0f)
                keyboardkey(input, Keys.RETURN, keyboard_id);
            if (ValueChanges._ValueChange[10] < 0f)
                keyboardkeyF(input, Keys.RETURN, keyboard_id);
            ValueChanges[11] = SendSHIFT ? 1 : 0;
            if (ValueChanges._ValueChange[11] > 0f)
                keyboardkey(input, Keys.SHIFT, keyboard_id);
            if (ValueChanges._ValueChange[11] < 0f)
                keyboardkeyF(input, Keys.SHIFT, keyboard_id);
            ValueChanges[12] = SendCONTROL ? 1 : 0;
            if (ValueChanges._ValueChange[12] > 0f)
                keyboardkey(input, Keys.CONTROL, keyboard_id);
            if (ValueChanges._ValueChange[12] < 0f)
                keyboardkeyF(input, Keys.CONTROL, keyboard_id);
            ValueChanges[13] = SendMENU ? 1 : 0;
            if (ValueChanges._ValueChange[13] > 0f)
                keyboardkey(input, Keys.MENU, keyboard_id);
            if (ValueChanges._ValueChange[13] < 0f)
                keyboardkeyF(input, Keys.MENU, keyboard_id);
            ValueChanges[14] = SendCAPITAL ? 1 : 0;
            if (ValueChanges._ValueChange[14] > 0f)
                keyboardkey(input, Keys.CAPITAL, keyboard_id);
            if (ValueChanges._ValueChange[14] < 0f)
                keyboardkeyF(input, Keys.CAPITAL, keyboard_id);
            ValueChanges[15] = SendESCAPE ? 1 : 0;
            if (ValueChanges._ValueChange[15] > 0f)
                keyboardkey(input, Keys.ESCAPE, keyboard_id);
            if (ValueChanges._ValueChange[15] < 0f)
                keyboardkeyF(input, Keys.ESCAPE, keyboard_id);
            ValueChanges[16] = SendSPACE ? 1 : 0;
            if (ValueChanges._ValueChange[16] > 0f)
                keyboardkey(input, Keys.SPACE, keyboard_id);
            if (ValueChanges._ValueChange[16] < 0f)
                keyboardkeyF(input, Keys.SPACE, keyboard_id);
            ValueChanges[17] = SendPRIOR ? 1 : 0;
            if (ValueChanges._ValueChange[17] > 0f)
                keyboardkey(input, Keys.PRIOR, keyboard_id);
            if (ValueChanges._ValueChange[17] < 0f)
                keyboardkeyF(input, Keys.PRIOR, keyboard_id);
            ValueChanges[18] = SendNEXT ? 1 : 0;
            if (ValueChanges._ValueChange[18] > 0f)
                keyboardkey(input, Keys.NEXT, keyboard_id);
            if (ValueChanges._ValueChange[18] < 0f)
                keyboardkeyF(input, Keys.NEXT, keyboard_id);
            ValueChanges[19] = SendEND ? 1 : 0;
            if (ValueChanges._ValueChange[19] > 0f)
                keyboardkey(input, Keys.END, keyboard_id);
            if (ValueChanges._ValueChange[19] < 0f)
                keyboardkeyF(input, Keys.END, keyboard_id);
            ValueChanges[20] = SendHOME ? 1 : 0;
            if (ValueChanges._ValueChange[20] > 0f)
                keyboardkey(input, Keys.HOME, keyboard_id);
            if (ValueChanges._ValueChange[20] < 0f)
                keyboardkeyF(input, Keys.HOME, keyboard_id);
            ValueChanges[21] = SendLEFT ? 1 : 0;
            if (ValueChanges._ValueChange[21] > 0f)
                keyboardkey(input, Keys.LEFT, keyboard_id);
            if (ValueChanges._ValueChange[21] < 0f)
                keyboardkeyF(input, Keys.LEFT, keyboard_id);
            ValueChanges[22] = SendUP ? 1 : 0;
            if (ValueChanges._ValueChange[22] > 0f)
                keyboardkey(input, Keys.UP, keyboard_id);
            if (ValueChanges._ValueChange[22] < 0f)
                keyboardkeyF(input, Keys.UP, keyboard_id);
            ValueChanges[23] = SendRIGHT ? 1 : 0;
            if (ValueChanges._ValueChange[23] > 0f)
                keyboardkey(input, Keys.RIGHT, keyboard_id);
            if (ValueChanges._ValueChange[23] < 0f)
                keyboardkeyF(input, Keys.RIGHT, keyboard_id);
            ValueChanges[24] = SendDOWN ? 1 : 0;
            if (ValueChanges._ValueChange[24] > 0f)
                keyboardkey(input, Keys.DOWN, keyboard_id);
            if (ValueChanges._ValueChange[24] < 0f)
                keyboardkeyF(input, Keys.DOWN, keyboard_id);
            ValueChanges[25] = SendSNAPSHOT ? 1 : 0;
            if (ValueChanges._ValueChange[25] > 0f)
                keyboardkey(input, Keys.SNAPSHOT, keyboard_id);
            if (ValueChanges._ValueChange[25] < 0f)
                keyboardkeyF(input, Keys.SNAPSHOT, keyboard_id);
            ValueChanges[26] = SendINSERT ? 1 : 0;
            if (ValueChanges._ValueChange[26] > 0f)
                keyboardkey(input, Keys.INSERT, keyboard_id);
            if (ValueChanges._ValueChange[26] < 0f)
                keyboardkeyF(input, Keys.INSERT, keyboard_id);
            ValueChanges[27] = SendNUMPADDEL ? 1 : 0;
            if (ValueChanges._ValueChange[27] > 0f)
                keyboardkey(input, Keys.NUMPADDEL, keyboard_id);
            if (ValueChanges._ValueChange[27] < 0f)
                keyboardkeyF(input, Keys.NUMPADDEL, keyboard_id);
            ValueChanges[28] = SendNUMPADINSERT ? 1 : 0;
            if (ValueChanges._ValueChange[28] > 0f)
                keyboardkey(input, Keys.NUMPADINSERT, keyboard_id);
            if (ValueChanges._ValueChange[28] < 0f)
                keyboardkeyF(input, Keys.NUMPADINSERT, keyboard_id);
            ValueChanges[29] = SendHELP ? 1 : 0;
            if (ValueChanges._ValueChange[29] > 0f)
                keyboardkey(input, Keys.HELP, keyboard_id);
            if (ValueChanges._ValueChange[29] < 0f)
                keyboardkeyF(input, Keys.HELP, keyboard_id);
            ValueChanges[30] = SendAPOSTROPHE ? 1 : 0;
            if (ValueChanges._ValueChange[30] > 0f)
                keyboardkey(input, Keys.APOSTROPHE, keyboard_id);
            if (ValueChanges._ValueChange[30] < 0f)
                keyboardkeyF(input, Keys.APOSTROPHE, keyboard_id);
            ValueChanges[31] = SendBACKSPACE ? 1 : 0;
            if (ValueChanges._ValueChange[31] > 0f)
                keyboardkey(input, Keys.BACKSPACE, keyboard_id);
            if (ValueChanges._ValueChange[31] < 0f)
                keyboardkeyF(input, Keys.BACKSPACE, keyboard_id);
            ValueChanges[32] = SendPAGEDOWN ? 1 : 0;
            if (ValueChanges._ValueChange[32] > 0f)
                keyboardkey(input, Keys.PAGEDOWN, keyboard_id);
            if (ValueChanges._ValueChange[32] < 0f)
                keyboardkeyF(input, Keys.PAGEDOWN, keyboard_id);
            ValueChanges[33] = SendPAGEUP ? 1 : 0;
            if (ValueChanges._ValueChange[33] > 0f)
                keyboardkey(input, Keys.PAGEUP, keyboard_id);
            if (ValueChanges._ValueChange[33] < 0f)
                keyboardkeyF(input, Keys.PAGEUP, keyboard_id);
            ValueChanges[34] = SendFIN ? 1 : 0;
            if (ValueChanges._ValueChange[34] > 0f)
                keyboardkey(input, Keys.FIN, keyboard_id);
            if (ValueChanges._ValueChange[34] < 0f)
                keyboardkeyF(input, Keys.FIN, keyboard_id);
            ValueChanges[35] = SendMOUSE ? 1 : 0;
            if (ValueChanges._ValueChange[35] > 0f)
                keyboardkey(input, Keys.MOUSE, keyboard_id);
            if (ValueChanges._ValueChange[35] < 0f)
                keyboardkeyF(input, Keys.MOUSE, keyboard_id);
            ValueChanges[36] = SendA ? 1 : 0;
            if (ValueChanges._ValueChange[36] > 0f)
                keyboardkey(input, Keys.A, keyboard_id);
            if (ValueChanges._ValueChange[36] < 0f)
                keyboardkeyF(input, Keys.A, keyboard_id);
            ValueChanges[37] = SendB ? 1 : 0;
            if (ValueChanges._ValueChange[37] > 0f)
                keyboardkey(input, Keys.B, keyboard_id);
            if (ValueChanges._ValueChange[37] < 0f)
                keyboardkeyF(input, Keys.B, keyboard_id);
            ValueChanges[38] = SendC ? 1 : 0;
            if (ValueChanges._ValueChange[38] > 0f)
                keyboardkey(input, Keys.C, keyboard_id);
            if (ValueChanges._ValueChange[38] < 0f)
                keyboardkeyF(input, Keys.C, keyboard_id);
            ValueChanges[39] = SendD ? 1 : 0;
            if (ValueChanges._ValueChange[39] > 0f)
                keyboardkey(input, Keys.D, keyboard_id);
            if (ValueChanges._ValueChange[39] < 0f)
                keyboardkeyF(input, Keys.D, keyboard_id);
            ValueChanges[40] = SendE ? 1 : 0;
            if (ValueChanges._ValueChange[40] > 0f)
                keyboardkey(input, Keys.E, keyboard_id);
            if (ValueChanges._ValueChange[40] < 0f)
                keyboardkeyF(input, Keys.E, keyboard_id);
            ValueChanges[41] = SendF ? 1 : 0;
            if (ValueChanges._ValueChange[41] > 0f)
                keyboardkey(input, Keys.F, keyboard_id);
            if (ValueChanges._ValueChange[41] < 0f)
                keyboardkeyF(input, Keys.F, keyboard_id);
            ValueChanges[42] = SendG ? 1 : 0;
            if (ValueChanges._ValueChange[42] > 0f)
                keyboardkey(input, Keys.G, keyboard_id);
            if (ValueChanges._ValueChange[42] < 0f)
                keyboardkeyF(input, Keys.G, keyboard_id);
            ValueChanges[43] = SendH ? 1 : 0;
            if (ValueChanges._ValueChange[43] > 0f)
                keyboardkey(input, Keys.H, keyboard_id);
            if (ValueChanges._ValueChange[43] < 0f)
                keyboardkeyF(input, Keys.H, keyboard_id);
            ValueChanges[44] = SendI ? 1 : 0;
            if (ValueChanges._ValueChange[44] > 0f)
                keyboardkey(input, Keys.I, keyboard_id);
            if (ValueChanges._ValueChange[44] < 0f)
                keyboardkeyF(input, Keys.I, keyboard_id);
            ValueChanges[45] = SendJ ? 1 : 0;
            if (ValueChanges._ValueChange[45] > 0f)
                keyboardkey(input, Keys.J, keyboard_id);
            if (ValueChanges._ValueChange[45] < 0f)
                keyboardkeyF(input, Keys.J, keyboard_id);
            ValueChanges[46] = SendK ? 1 : 0;
            if (ValueChanges._ValueChange[46] > 0f)
                keyboardkey(input, Keys.K, keyboard_id);
            if (ValueChanges._ValueChange[46] < 0f)
                keyboardkeyF(input, Keys.K, keyboard_id);
            ValueChanges[47] = SendL ? 1 : 0;
            if (ValueChanges._ValueChange[47] > 0f)
                keyboardkey(input, Keys.L, keyboard_id);
            if (ValueChanges._ValueChange[47] < 0f)
                keyboardkeyF(input, Keys.L, keyboard_id);
            ValueChanges[48] = SendM ? 1 : 0;
            if (ValueChanges._ValueChange[48] > 0f)
                keyboardkey(input, Keys.M, keyboard_id);
            if (ValueChanges._ValueChange[48] < 0f)
                keyboardkeyF(input, Keys.M, keyboard_id);
            ValueChanges[49] = SendN ? 1 : 0;
            if (ValueChanges._ValueChange[49] > 0f)
                keyboardkey(input, Keys.N, keyboard_id);
            if (ValueChanges._ValueChange[49] < 0f)
                keyboardkeyF(input, Keys.N, keyboard_id);
            ValueChanges[50] = SendO ? 1 : 0;
            if (ValueChanges._ValueChange[50] > 0f)
                keyboardkey(input, Keys.O, keyboard_id);
            if (ValueChanges._ValueChange[50] < 0f)
                keyboardkeyF(input, Keys.O, keyboard_id);
            ValueChanges[51] = SendP ? 1 : 0;
            if (ValueChanges._ValueChange[51] > 0f)
                keyboardkey(input, Keys.P, keyboard_id);
            if (ValueChanges._ValueChange[51] < 0f)
                keyboardkeyF(input, Keys.P, keyboard_id);
            ValueChanges[52] = SendQ ? 1 : 0;
            if (ValueChanges._ValueChange[52] > 0f)
                keyboardkey(input, Keys.Q, keyboard_id);
            if (ValueChanges._ValueChange[52] < 0f)
                keyboardkeyF(input, Keys.Q, keyboard_id);
            ValueChanges[53] = SendR ? 1 : 0;
            if (ValueChanges._ValueChange[53] > 0f)
                keyboardkey(input, Keys.R, keyboard_id);
            if (ValueChanges._ValueChange[53] < 0f)
                keyboardkeyF(input, Keys.R, keyboard_id);
            ValueChanges[54] = SendS ? 1 : 0;
            if (ValueChanges._ValueChange[54] > 0f)
                keyboardkey(input, Keys.S, keyboard_id);
            if (ValueChanges._ValueChange[54] < 0f)
                keyboardkeyF(input, Keys.S, keyboard_id);
            ValueChanges[55] = SendT ? 1 : 0;
            if (ValueChanges._ValueChange[55] > 0f)
                keyboardkey(input, Keys.T, keyboard_id);
            if (ValueChanges._ValueChange[55] < 0f)
                keyboardkeyF(input, Keys.T, keyboard_id);
            ValueChanges[56] = SendU ? 1 : 0;
            if (ValueChanges._ValueChange[56] > 0f)
                keyboardkey(input, Keys.U, keyboard_id);
            if (ValueChanges._ValueChange[56] < 0f)
                keyboardkeyF(input, Keys.U, keyboard_id);
            ValueChanges[57] = SendV ? 1 : 0;
            if (ValueChanges._ValueChange[57] > 0f)
                keyboardkey(input, Keys.V, keyboard_id);
            if (ValueChanges._ValueChange[57] < 0f)
                keyboardkeyF(input, Keys.V, keyboard_id);
            ValueChanges[58] = SendW ? 1 : 0;
            if (ValueChanges._ValueChange[58] > 0f)
                keyboardkey(input, Keys.W, keyboard_id);
            if (ValueChanges._ValueChange[58] < 0f)
                keyboardkeyF(input, Keys.W, keyboard_id);
            ValueChanges[59] = SendX ? 1 : 0;
            if (ValueChanges._ValueChange[59] > 0f)
                keyboardkey(input, Keys.X, keyboard_id);
            if (ValueChanges._ValueChange[59] < 0f)
                keyboardkeyF(input, Keys.X, keyboard_id);
            ValueChanges[60] = SendY ? 1 : 0;
            if (ValueChanges._ValueChange[60] > 0f)
                keyboardkey(input, Keys.Y, keyboard_id);
            if (ValueChanges._ValueChange[60] < 0f)
                keyboardkeyF(input, Keys.Y, keyboard_id);
            ValueChanges[61] = SendZ ? 1 : 0;
            if (ValueChanges._ValueChange[61] > 0f)
                keyboardkey(input, Keys.Z, keyboard_id);
            if (ValueChanges._ValueChange[61] < 0f)
                keyboardkeyF(input, Keys.Z, keyboard_id);
            ValueChanges[62] = SendLWIN ? 1 : 0;
            if (ValueChanges._ValueChange[62] > 0f)
                keyboardkey(input, Keys.LWIN, keyboard_id);
            if (ValueChanges._ValueChange[62] < 0f)
                keyboardkeyF(input, Keys.LWIN, keyboard_id);
            ValueChanges[63] = SendRWIN ? 1 : 0;
            if (ValueChanges._ValueChange[63] > 0f)
                keyboardkey(input, Keys.RWIN, keyboard_id);
            if (ValueChanges._ValueChange[63] < 0f)
                keyboardkeyF(input, Keys.RWIN, keyboard_id);
            ValueChanges[64] = SendAPPS ? 1 : 0;
            if (ValueChanges._ValueChange[64] > 0f)
                keyboardkey(input, Keys.APPS, keyboard_id);
            if (ValueChanges._ValueChange[64] < 0f)
                keyboardkeyF(input, Keys.APPS, keyboard_id);
            ValueChanges[65] = SendDELETE ? 1 : 0;
            if (ValueChanges._ValueChange[65] > 0f)
                keyboardkey(input, Keys.DELETE, keyboard_id);
            if (ValueChanges._ValueChange[65] < 0f)
                keyboardkeyF(input, Keys.DELETE, keyboard_id);
            ValueChanges[66] = SendNUMPAD0 ? 1 : 0;
            if (ValueChanges._ValueChange[66] > 0f)
                keyboardkey(input, Keys.NUMPAD0, keyboard_id);
            if (ValueChanges._ValueChange[66] < 0f)
                keyboardkeyF(input, Keys.NUMPAD0, keyboard_id);
            ValueChanges[67] = SendNUMPAD1 ? 1 : 0;
            if (ValueChanges._ValueChange[67] > 0f)
                keyboardkey(input, Keys.NUMPAD1, keyboard_id);
            if (ValueChanges._ValueChange[67] < 0f)
                keyboardkeyF(input, Keys.NUMPAD1, keyboard_id);
            ValueChanges[68] = SendNUMPAD2 ? 1 : 0;
            if (ValueChanges._ValueChange[68] > 0f)
                keyboardkey(input, Keys.NUMPAD2, keyboard_id);
            if (ValueChanges._ValueChange[68] < 0f)
                keyboardkeyF(input, Keys.NUMPAD2, keyboard_id);
            ValueChanges[69] = SendNUMPAD3 ? 1 : 0;
            if (ValueChanges._ValueChange[69] > 0f)
                keyboardkey(input, Keys.NUMPAD3, keyboard_id);
            if (ValueChanges._ValueChange[69] < 0f)
                keyboardkeyF(input, Keys.NUMPAD3, keyboard_id);
            ValueChanges[70] = SendNUMPAD4 ? 1 : 0;
            if (ValueChanges._ValueChange[70] > 0f)
                keyboardkey(input, Keys.NUMPAD4, keyboard_id);
            if (ValueChanges._ValueChange[70] < 0f)
                keyboardkeyF(input, Keys.NUMPAD4, keyboard_id);
            ValueChanges[71] = SendNUMPAD5 ? 1 : 0;
            if (ValueChanges._ValueChange[71] > 0f)
                keyboardkey(input, Keys.NUMPAD5, keyboard_id);
            if (ValueChanges._ValueChange[71] < 0f)
                keyboardkeyF(input, Keys.NUMPAD5, keyboard_id);
            ValueChanges[72] = SendNUMPAD6 ? 1 : 0;
            if (ValueChanges._ValueChange[72] > 0f)
                keyboardkey(input, Keys.NUMPAD6, keyboard_id);
            if (ValueChanges._ValueChange[72] < 0f)
                keyboardkeyF(input, Keys.NUMPAD6, keyboard_id);
            ValueChanges[73] = SendNUMPAD7 ? 1 : 0;
            if (ValueChanges._ValueChange[73] > 0f)
                keyboardkey(input, Keys.NUMPAD7, keyboard_id);
            if (ValueChanges._ValueChange[73] < 0f)
                keyboardkeyF(input, Keys.NUMPAD7, keyboard_id);
            ValueChanges[74] = SendNUMPAD8 ? 1 : 0;
            if (ValueChanges._ValueChange[74] > 0f)
                keyboardkey(input, Keys.NUMPAD8, keyboard_id);
            if (ValueChanges._ValueChange[74] < 0f)
                keyboardkeyF(input, Keys.NUMPAD8, keyboard_id);
            ValueChanges[75] = SendNUMPAD9 ? 1 : 0;
            if (ValueChanges._ValueChange[75] > 0f)
                keyboardkey(input, Keys.NUMPAD9, keyboard_id);
            if (ValueChanges._ValueChange[75] < 0f)
                keyboardkeyF(input, Keys.NUMPAD9, keyboard_id);
            ValueChanges[76] = SendMULTIPLY ? 1 : 0;
            if (ValueChanges._ValueChange[76] > 0f)
                keyboardkey(input, Keys.MULTIPLY, keyboard_id);
            if (ValueChanges._ValueChange[76] < 0f)
                keyboardkeyF(input, Keys.MULTIPLY, keyboard_id);
            ValueChanges[77] = SendADD ? 1 : 0;
            if (ValueChanges._ValueChange[77] > 0f)
                keyboardkey(input, Keys.ADD, keyboard_id);
            if (ValueChanges._ValueChange[77] < 0f)
                keyboardkeyF(input, Keys.ADD, keyboard_id);
            ValueChanges[78] = SendSUBTRACT ? 1 : 0;
            if (ValueChanges._ValueChange[78] > 0f)
                keyboardkey(input, Keys.SUBTRACT, keyboard_id);
            if (ValueChanges._ValueChange[78] < 0f)
                keyboardkeyF(input, Keys.SUBTRACT, keyboard_id);
            ValueChanges[79] = SendDECIMAL ? 1 : 0;
            if (ValueChanges._ValueChange[79] > 0f)
                keyboardkey(input, Keys.DECIMAL, keyboard_id);
            if (ValueChanges._ValueChange[79] < 0f)
                keyboardkeyF(input, Keys.DECIMAL, keyboard_id);
            ValueChanges[80] = SendPRINTSCREEN ? 1 : 0;
            if (ValueChanges._ValueChange[80] > 0f)
                keyboardkey(input, Keys.PRINTSCREEN, keyboard_id);
            if (ValueChanges._ValueChange[80] < 0f)
                keyboardkeyF(input, Keys.PRINTSCREEN, keyboard_id);
            ValueChanges[81] = SendDIVIDE ? 1 : 0;
            if (ValueChanges._ValueChange[81] > 0f)
                keyboardkey(input, Keys.DIVIDE, keyboard_id);
            if (ValueChanges._ValueChange[81] < 0f)
                keyboardkeyF(input, Keys.DIVIDE, keyboard_id);
            ValueChanges[82] = SendF1 ? 1 : 0;
            if (ValueChanges._ValueChange[82] > 0f)
                keyboardkey(input, Keys.F1, keyboard_id);
            if (ValueChanges._ValueChange[82] < 0f)
                keyboardkeyF(input, Keys.F1, keyboard_id);
            ValueChanges[83] = SendF2 ? 1 : 0;
            if (ValueChanges._ValueChange[83] > 0f)
                keyboardkey(input, Keys.F2, keyboard_id);
            if (ValueChanges._ValueChange[83] < 0f)
                keyboardkeyF(input, Keys.F2, keyboard_id);
            ValueChanges[84] = SendF3 ? 1 : 0;
            if (ValueChanges._ValueChange[84] > 0f)
                keyboardkey(input, Keys.F3, keyboard_id);
            if (ValueChanges._ValueChange[84] < 0f)
                keyboardkeyF(input, Keys.F3, keyboard_id);
            ValueChanges[85] = SendF4 ? 1 : 0;
            if (ValueChanges._ValueChange[85] > 0f)
                keyboardkey(input, Keys.F4, keyboard_id);
            if (ValueChanges._ValueChange[85] < 0f)
                keyboardkeyF(input, Keys.F4, keyboard_id);
            ValueChanges[86] = SendF5 ? 1 : 0;
            if (ValueChanges._ValueChange[86] > 0f)
                keyboardkey(input, Keys.F5, keyboard_id);
            if (ValueChanges._ValueChange[86] < 0f)
                keyboardkeyF(input, Keys.F5, keyboard_id);
            ValueChanges[87] = SendF6 ? 1 : 0;
            if (ValueChanges._ValueChange[87] > 0f)
                keyboardkey(input, Keys.F6, keyboard_id);
            if (ValueChanges._ValueChange[87] < 0f)
                keyboardkeyF(input, Keys.F6, keyboard_id);
            ValueChanges[88] = SendF7 ? 1 : 0;
            if (ValueChanges._ValueChange[88] > 0f)
                keyboardkey(input, Keys.F7, keyboard_id);
            if (ValueChanges._ValueChange[88] < 0f)
                keyboardkeyF(input, Keys.F7, keyboard_id);
            ValueChanges[89] = SendF8 ? 1 : 0;
            if (ValueChanges._ValueChange[89] > 0f)
                keyboardkey(input, Keys.F8, keyboard_id);
            if (ValueChanges._ValueChange[89] < 0f)
                keyboardkeyF(input, Keys.F8, keyboard_id);
            ValueChanges[90] = SendF9 ? 1 : 0;
            if (ValueChanges._ValueChange[90] > 0f)
                keyboardkey(input, Keys.F9, keyboard_id);
            if (ValueChanges._ValueChange[90] < 0f)
                keyboardkeyF(input, Keys.F9, keyboard_id);
            ValueChanges[91] = SendF10 ? 1 : 0;
            if (ValueChanges._ValueChange[91] > 0f)
                keyboardkey(input, Keys.F10, keyboard_id);
            if (ValueChanges._ValueChange[91] < 0f)
                keyboardkeyF(input, Keys.F10, keyboard_id);
            ValueChanges[92] = SendF11 ? 1 : 0;
            if (ValueChanges._ValueChange[92] > 0f)
                keyboardkey(input, Keys.F11, keyboard_id);
            if (ValueChanges._ValueChange[92] < 0f)
                keyboardkeyF(input, Keys.F11, keyboard_id);
            ValueChanges[93] = SendF12 ? 1 : 0;
            if (ValueChanges._ValueChange[93] > 0f)
                keyboardkey(input, Keys.F12, keyboard_id);
            if (ValueChanges._ValueChange[93] < 0f)
                keyboardkeyF(input, Keys.F12, keyboard_id);
            ValueChanges[94] = SendNUMLOCK ? 1 : 0;
            if (ValueChanges._ValueChange[94] > 0f)
                keyboardkey(input, Keys.NUMLOCK, keyboard_id);
            if (ValueChanges._ValueChange[94] < 0f)
                keyboardkeyF(input, Keys.NUMLOCK, keyboard_id);
            ValueChanges[95] = SendSCROLLLOCK ? 1 : 0;
            if (ValueChanges._ValueChange[95] > 0f)
                keyboardkey(input, Keys.SCROLLLOCK, keyboard_id);
            if (ValueChanges._ValueChange[95] < 0f)
                keyboardkeyF(input, Keys.SCROLLLOCK, keyboard_id);
            ValueChanges[96] = SendLEFTSHIFT ? 1 : 0;
            if (ValueChanges._ValueChange[96] > 0f)
                keyboardkey(input, Keys.LEFTSHIFT, keyboard_id);
            if (ValueChanges._ValueChange[96] < 0f)
                keyboardkeyF(input, Keys.LEFTSHIFT, keyboard_id);
            ValueChanges[97] = SendRIGHTSHIFT ? 1 : 0;
            if (ValueChanges._ValueChange[97] > 0f)
                keyboardkey(input, Keys.RIGHTSHIFT, keyboard_id);
            if (ValueChanges._ValueChange[97] < 0f)
                keyboardkeyF(input, Keys.RIGHTSHIFT, keyboard_id);
            ValueChanges[98] = SendLEFTCONTROL ? 1 : 0;
            if (ValueChanges._ValueChange[98] > 0f)
                keyboardkey(input, Keys.LEFTCONTROL, keyboard_id);
            if (ValueChanges._ValueChange[98] < 0f)
                keyboardkeyF(input, Keys.LEFTCONTROL, keyboard_id);
            ValueChanges[99] = SendRIGHTCONTROL ? 1 : 0;
            if (ValueChanges._ValueChange[99] > 0f)
                keyboardkey(input, Keys.RIGHTCONTROL, keyboard_id);
            if (ValueChanges._ValueChange[99] < 0f)
                keyboardkeyF(input, Keys.RIGHTCONTROL, keyboard_id);
            ValueChanges[100] = SendLEFTALT ? 1 : 0;
            if (ValueChanges._ValueChange[100] > 0f)
                keyboardkey(input, Keys.LEFTALT, keyboard_id);
            if (ValueChanges._ValueChange[100] < 0f)
                keyboardkeyF(input, Keys.LEFTALT, keyboard_id);
            ValueChanges[101] = SendRIGHTALT ? 1 : 0;
            if (ValueChanges._ValueChange[101] > 0f)
                keyboardkey(input, Keys.RIGHTALT, keyboard_id);
            if (ValueChanges._ValueChange[101] < 0f)
                keyboardkeyF(input, Keys.RIGHTALT, keyboard_id);
            ValueChanges[102] = SendBROWSER_BACK ? 1 : 0;
            if (ValueChanges._ValueChange[102] > 0f)
                keyboardkey(input, Keys.BROWSER_BACK, keyboard_id);
            if (ValueChanges._ValueChange[102] < 0f)
                keyboardkeyF(input, Keys.BROWSER_BACK, keyboard_id);
            ValueChanges[103] = SendBROWSER_FORWARD ? 1 : 0;
            if (ValueChanges._ValueChange[103] > 0f)
                keyboardkey(input, Keys.BROWSER_FORWARD, keyboard_id);
            if (ValueChanges._ValueChange[103] < 0f)
                keyboardkeyF(input, Keys.BROWSER_FORWARD, keyboard_id);
            ValueChanges[104] = SendBROWSER_REFRESH ? 1 : 0;
            if (ValueChanges._ValueChange[104] > 0f)
                keyboardkey(input, Keys.BROWSER_REFRESH, keyboard_id);
            if (ValueChanges._ValueChange[104] < 0f)
                keyboardkeyF(input, Keys.BROWSER_REFRESH, keyboard_id);
            ValueChanges[105] = SendBROWSER_STOP ? 1 : 0;
            if (ValueChanges._ValueChange[105] > 0f)
                keyboardkey(input, Keys.BROWSER_STOP, keyboard_id);
            if (ValueChanges._ValueChange[105] < 0f)
                keyboardkeyF(input, Keys.BROWSER_STOP, keyboard_id);
            ValueChanges[106] = SendBROWSER_SEARCH ? 1 : 0;
            if (ValueChanges._ValueChange[106] > 0f)
                keyboardkey(input, Keys.BROWSER_SEARCH, keyboard_id);
            if (ValueChanges._ValueChange[106] < 0f)
                keyboardkeyF(input, Keys.BROWSER_SEARCH, keyboard_id);
            ValueChanges[107] = SendBROWSER_FAVORITES ? 1 : 0;
            if (ValueChanges._ValueChange[107] > 0f)
                keyboardkey(input, Keys.BROWSER_FAVORITES, keyboard_id);
            if (ValueChanges._ValueChange[107] < 0f)
                keyboardkeyF(input, Keys.BROWSER_FAVORITES, keyboard_id);
            ValueChanges[108] = SendBROWSER_HOME ? 1 : 0;
            if (ValueChanges._ValueChange[108] > 0f)
                keyboardkey(input, Keys.BROWSER_HOME, keyboard_id);
            if (ValueChanges._ValueChange[108] < 0f)
                keyboardkeyF(input, Keys.BROWSER_HOME, keyboard_id);
            ValueChanges[109] = SendVOLUME_MUTE ? 1 : 0;
            if (ValueChanges._ValueChange[109] > 0f)
                keyboardkey(input, Keys.VOLUME_MUTE, keyboard_id);
            if (ValueChanges._ValueChange[109] < 0f)
                keyboardkeyF(input, Keys.VOLUME_MUTE, keyboard_id);
            ValueChanges[110] = SendVOLUME_DOWN ? 1 : 0;
            if (ValueChanges._ValueChange[110] > 0f)
                keyboardkey(input, Keys.VOLUME_DOWN, keyboard_id);
            if (ValueChanges._ValueChange[110] < 0f)
                keyboardkeyF(input, Keys.VOLUME_DOWN, keyboard_id);
            ValueChanges[111] = SendVOLUME_UP ? 1 : 0;
            if (ValueChanges._ValueChange[111] > 0f)
                keyboardkey(input, Keys.VOLUME_UP, keyboard_id);
            if (ValueChanges._ValueChange[111] < 0f)
                keyboardkeyF(input, Keys.VOLUME_UP, keyboard_id);
            ValueChanges[112] = SendMEDIA_NEXT_TRACK ? 1 : 0;
            if (ValueChanges._ValueChange[112] > 0f)
                keyboardkey(input, Keys.MEDIA_NEXT_TRACK, keyboard_id);
            if (ValueChanges._ValueChange[112] < 0f)
                keyboardkeyF(input, Keys.MEDIA_NEXT_TRACK, keyboard_id);
            ValueChanges[113] = SendMEDIA_PREV_TRACK ? 1 : 0;
            if (ValueChanges._ValueChange[113] > 0f)
                keyboardkey(input, Keys.MEDIA_PREV_TRACK, keyboard_id);
            if (ValueChanges._ValueChange[113] < 0f)
                keyboardkeyF(input, Keys.MEDIA_PREV_TRACK, keyboard_id);
            ValueChanges[114] = SendMEDIA_STOP ? 1 : 0;
            if (ValueChanges._ValueChange[114] > 0f)
                keyboardkey(input, Keys.MEDIA_STOP, keyboard_id);
            if (ValueChanges._ValueChange[114] < 0f)
                keyboardkeyF(input, Keys.MEDIA_STOP, keyboard_id);
            ValueChanges[115] = SendMEDIA_PLAY_PAUSE ? 1 : 0;
            if (ValueChanges._ValueChange[115] > 0f)
                keyboardkey(input, Keys.MEDIA_PLAY_PAUSE, keyboard_id);
            if (ValueChanges._ValueChange[115] < 0f)
                keyboardkeyF(input, Keys.MEDIA_PLAY_PAUSE, keyboard_id);
            ValueChanges[116] = SendLAUNCH_MAIL ? 1 : 0;
            if (ValueChanges._ValueChange[116] > 0f)
                keyboardkey(input, Keys.LAUNCH_MAIL, keyboard_id);
            if (ValueChanges._ValueChange[116] < 0f)
                keyboardkeyF(input, Keys.LAUNCH_MAIL, keyboard_id);
            ValueChanges[117] = SendLAUNCH_MEDIA_SELECT ? 1 : 0;
            if (ValueChanges._ValueChange[117] > 0f)
                keyboardkey(input, Keys.LAUNCH_MEDIA_SELECT, keyboard_id);
            if (ValueChanges._ValueChange[117] < 0f)
                keyboardkeyF(input, Keys.LAUNCH_MEDIA_SELECT, keyboard_id);
            ValueChanges[118] = SendLAUNCH_APP1 ? 1 : 0;
            if (ValueChanges._ValueChange[118] > 0f)
                keyboardkey(input, Keys.LAUNCH_APP1, keyboard_id);
            if (ValueChanges._ValueChange[118] < 0f)
                keyboardkeyF(input, Keys.LAUNCH_APP1, keyboard_id);
            ValueChanges[119] = SendLAUNCH_APP2 ? 1 : 0;
            if (ValueChanges._ValueChange[119] > 0f)
                keyboardkey(input, Keys.LAUNCH_APP2, keyboard_id);
            if (ValueChanges._ValueChange[119] < 0f)
                keyboardkeyF(input, Keys.LAUNCH_APP2, keyboard_id);
            ValueChanges[120] = SendOEM_1 ? 1 : 0;
            if (ValueChanges._ValueChange[120] > 0f)
                keyboardkey(input, Keys.OEM_1, keyboard_id);
            if (ValueChanges._ValueChange[120] < 0f)
                keyboardkeyF(input, Keys.OEM_1, keyboard_id);
            ValueChanges[121] = SendOEM_PLUS ? 1 : 0;
            if (ValueChanges._ValueChange[121] > 0f)
                keyboardkey(input, Keys.OEM_PLUS, keyboard_id);
            if (ValueChanges._ValueChange[121] < 0f)
                keyboardkeyF(input, Keys.OEM_PLUS, keyboard_id);
            ValueChanges[122] = SendOEM_COMMA ? 1 : 0;
            if (ValueChanges._ValueChange[122] > 0f)
                keyboardkey(input, Keys.OEM_COMMA, keyboard_id);
            if (ValueChanges._ValueChange[122] < 0f)
                keyboardkeyF(input, Keys.OEM_COMMA, keyboard_id);
            ValueChanges[123] = SendOEM_MINUS ? 1 : 0;
            if (ValueChanges._ValueChange[123] > 0f)
                keyboardkey(input, Keys.OEM_MINUS, keyboard_id);
            if (ValueChanges._ValueChange[123] < 0f)
                keyboardkeyF(input, Keys.OEM_MINUS, keyboard_id);
            ValueChanges[124] = SendOEM_PERIOD ? 1 : 0;
            if (ValueChanges._ValueChange[124] > 0f)
                keyboardkey(input, Keys.OEM_PERIOD, keyboard_id);
            if (ValueChanges._ValueChange[124] < 0f)
                keyboardkeyF(input, Keys.OEM_PERIOD, keyboard_id);
            ValueChanges[125] = SendOEM_2 ? 1 : 0;
            if (ValueChanges._ValueChange[125] > 0f)
                keyboardkey(input, Keys.OEM_2, keyboard_id);
            if (ValueChanges._ValueChange[125] < 0f)
                keyboardkeyF(input, Keys.OEM_2, keyboard_id);
            ValueChanges[126] = SendOEM_3 ? 1 : 0;
            if (ValueChanges._ValueChange[126] > 0f)
                keyboardkey(input, Keys.OEM_3, keyboard_id);
            if (ValueChanges._ValueChange[126] < 0f)
                keyboardkeyF(input, Keys.OEM_3, keyboard_id);
            ValueChanges[127] = SendOEM_4 ? 1 : 0;
            if (ValueChanges._ValueChange[127] > 0f)
                keyboardkey(input, Keys.OEM_4, keyboard_id);
            if (ValueChanges._ValueChange[127] < 0f)
                keyboardkeyF(input, Keys.OEM_4, keyboard_id);
            ValueChanges[128] = SendOEM_5 ? 1 : 0;
            if (ValueChanges._ValueChange[128] > 0f)
                keyboardkey(input, Keys.OEM_5, keyboard_id);
            if (ValueChanges._ValueChange[128] < 0f)
                keyboardkeyF(input, Keys.OEM_5, keyboard_id);
            ValueChanges[129] = SendOEM_6 ? 1 : 0;
            if (ValueChanges._ValueChange[129] > 0f)
                keyboardkey(input, Keys.OEM_6, keyboard_id);
            if (ValueChanges._ValueChange[129] < 0f)
                keyboardkeyF(input, Keys.OEM_6, keyboard_id);
            ValueChanges[130] = SendOEM_7 ? 1 : 0;
            if (ValueChanges._ValueChange[130] > 0f)
                keyboardkey(input, Keys.OEM_7, keyboard_id);
            if (ValueChanges._ValueChange[130] < 0f)
                keyboardkeyF(input, Keys.OEM_7, keyboard_id);
            ValueChanges[131] = SendOEM_8 ? 1 : 0;
            if (ValueChanges._ValueChange[131] > 0f)
                keyboardkey(input, Keys.OEM_8, keyboard_id);
            if (ValueChanges._ValueChange[131] < 0f)
                keyboardkeyF(input, Keys.OEM_8, keyboard_id);
            ValueChanges[132] = SendOEM_102 ? 1 : 0;
            if (ValueChanges._ValueChange[132] > 0f)
                keyboardkey(input, Keys.OEM_102, keyboard_id);
            if (ValueChanges._ValueChange[132] < 0f)
                keyboardkeyF(input, Keys.OEM_102, keyboard_id);
            ValueChanges[133] = SendEREOF ? 1 : 0;
            if (ValueChanges._ValueChange[133] > 0f)
                keyboardkey(input, Keys.EREOF, keyboard_id);
            if (ValueChanges._ValueChange[133] < 0f)
                keyboardkeyF(input, Keys.EREOF, keyboard_id);
            ValueChanges[134] = SendZOOM ? 1 : 0;
            if (ValueChanges._ValueChange[134] > 0f)
                keyboardkey(input, Keys.ZOOM, keyboard_id);
            if (ValueChanges._ValueChange[134] < 0f)
                keyboardkeyF(input, Keys.ZOOM, keyboard_id);
            ValueChanges[135] = SendEscape ? 1 : 0;
            if (ValueChanges._ValueChange[135] > 0f)
                keyboardkey(input, Keys.Escape, keyboard_id);
            if (ValueChanges._ValueChange[135] < 0f)
                keyboardkeyF(input, Keys.Escape, keyboard_id);
            ValueChanges[136] = SendOne ? 1 : 0;
            if (ValueChanges._ValueChange[136] > 0f)
                keyboardkey(input, Keys.One, keyboard_id);
            if (ValueChanges._ValueChange[136] < 0f)
                keyboardkeyF(input, Keys.One, keyboard_id);
            ValueChanges[137] = SendTwo ? 1 : 0;
            if (ValueChanges._ValueChange[137] > 0f)
                keyboardkey(input, Keys.Two, keyboard_id);
            if (ValueChanges._ValueChange[137] < 0f)
                keyboardkeyF(input, Keys.Two, keyboard_id);
            ValueChanges[138] = SendThree ? 1 : 0;
            if (ValueChanges._ValueChange[138] > 0f)
                keyboardkey(input, Keys.Three, keyboard_id);
            if (ValueChanges._ValueChange[138] < 0f)
                keyboardkeyF(input, Keys.Three, keyboard_id);
            ValueChanges[139] = SendFour ? 1 : 0;
            if (ValueChanges._ValueChange[139] > 0f)
                keyboardkey(input, Keys.Four, keyboard_id);
            if (ValueChanges._ValueChange[139] < 0f)
                keyboardkeyF(input, Keys.Four, keyboard_id);
            ValueChanges[140] = SendFive ? 1 : 0;
            if (ValueChanges._ValueChange[140] > 0f)
                keyboardkey(input, Keys.Five, keyboard_id);
            if (ValueChanges._ValueChange[140] < 0f)
                keyboardkeyF(input, Keys.Five, keyboard_id);
            ValueChanges[141] = SendSix ? 1 : 0;
            if (ValueChanges._ValueChange[141] > 0f)
                keyboardkey(input, Keys.Six, keyboard_id);
            if (ValueChanges._ValueChange[141] < 0f)
                keyboardkeyF(input, Keys.Six, keyboard_id);
            ValueChanges[142] = SendSeven ? 1 : 0;
            if (ValueChanges._ValueChange[142] > 0f)
                keyboardkey(input, Keys.Seven, keyboard_id);
            if (ValueChanges._ValueChange[142] < 0f)
                keyboardkeyF(input, Keys.Seven, keyboard_id);
            ValueChanges[143] = SendEight ? 1 : 0;
            if (ValueChanges._ValueChange[143] > 0f)
                keyboardkey(input, Keys.Eight, keyboard_id);
            if (ValueChanges._ValueChange[143] < 0f)
                keyboardkeyF(input, Keys.Eight, keyboard_id);
            ValueChanges[144] = SendNine ? 1 : 0;
            if (ValueChanges._ValueChange[144] > 0f)
                keyboardkey(input, Keys.Nine, keyboard_id);
            if (ValueChanges._ValueChange[144] < 0f)
                keyboardkeyF(input, Keys.Nine, keyboard_id);
            ValueChanges[145] = SendZero ? 1 : 0;
            if (ValueChanges._ValueChange[145] > 0f)
                keyboardkey(input, Keys.Zero, keyboard_id);
            if (ValueChanges._ValueChange[145] < 0f)
                keyboardkeyF(input, Keys.Zero, keyboard_id);
            ValueChanges[146] = SendDashUnderscore ? 1 : 0;
            if (ValueChanges._ValueChange[146] > 0f)
                keyboardkey(input, Keys.DashUnderscore, keyboard_id);
            if (ValueChanges._ValueChange[146] < 0f)
                keyboardkeyF(input, Keys.DashUnderscore, keyboard_id);
            ValueChanges[147] = SendPlusEquals ? 1 : 0;
            if (ValueChanges._ValueChange[147] > 0f)
                keyboardkey(input, Keys.PlusEquals, keyboard_id);
            if (ValueChanges._ValueChange[147] < 0f)
                keyboardkeyF(input, Keys.PlusEquals, keyboard_id);
            ValueChanges[148] = SendBackspace ? 1 : 0;
            if (ValueChanges._ValueChange[148] > 0f)
                keyboardkey(input, Keys.Backspace, keyboard_id);
            if (ValueChanges._ValueChange[148] < 0f)
                keyboardkeyF(input, Keys.Backspace, keyboard_id);
            ValueChanges[149] = SendTab ? 1 : 0;
            if (ValueChanges._ValueChange[149] > 0f)
                keyboardkey(input, Keys.Tab, keyboard_id);
            if (ValueChanges._ValueChange[149] < 0f)
                keyboardkeyF(input, Keys.Tab, keyboard_id);
            ValueChanges[150] = SendOpenBracketBrace ? 1 : 0;
            if (ValueChanges._ValueChange[150] > 0f)
                keyboardkey(input, Keys.OpenBracketBrace, keyboard_id);
            if (ValueChanges._ValueChange[150] < 0f)
                keyboardkeyF(input, Keys.OpenBracketBrace, keyboard_id);
            ValueChanges[151] = SendCloseBracketBrace ? 1 : 0;
            if (ValueChanges._ValueChange[151] > 0f)
                keyboardkey(input, Keys.CloseBracketBrace, keyboard_id);
            if (ValueChanges._ValueChange[151] < 0f)
                keyboardkeyF(input, Keys.CloseBracketBrace, keyboard_id);
            ValueChanges[152] = SendEnter ? 1 : 0;
            if (ValueChanges._ValueChange[152] > 0f)
                keyboardkey(input, Keys.Enter, keyboard_id);
            if (ValueChanges._ValueChange[152] < 0f)
                keyboardkeyF(input, Keys.Enter, keyboard_id);
            ValueChanges[153] = SendSemicolonColon ? 1 : 0;
            if (ValueChanges._ValueChange[153] > 0f)
                keyboardkey(input, Keys.SemicolonColon, keyboard_id);
            if (ValueChanges._ValueChange[153] < 0f)
                keyboardkeyF(input, Keys.SemicolonColon, keyboard_id);
            ValueChanges[154] = SendSingleDoubleQuote ? 1 : 0;
            if (ValueChanges._ValueChange[154] > 0f)
                keyboardkey(input, Keys.SingleDoubleQuote, keyboard_id);
            if (ValueChanges._ValueChange[154] < 0f)
                keyboardkeyF(input, Keys.SingleDoubleQuote, keyboard_id);
            ValueChanges[155] = SendTilde ? 1 : 0;
            if (ValueChanges._ValueChange[155] > 0f)
                keyboardkey(input, Keys.Tilde, keyboard_id);
            if (ValueChanges._ValueChange[155] < 0f)
                keyboardkeyF(input, Keys.Tilde, keyboard_id);
            ValueChanges[156] = SendLeftShift ? 1 : 0;
            if (ValueChanges._ValueChange[156] > 0f)
                keyboardkey(input, Keys.LeftShift, keyboard_id);
            if (ValueChanges._ValueChange[156] < 0f)
                keyboardkeyF(input, Keys.LeftShift, keyboard_id);
            ValueChanges[157] = SendBackslashPipe ? 1 : 0;
            if (ValueChanges._ValueChange[157] > 0f)
                keyboardkey(input, Keys.BackslashPipe, keyboard_id);
            if (ValueChanges._ValueChange[157] < 0f)
                keyboardkeyF(input, Keys.BackslashPipe, keyboard_id);
            ValueChanges[158] = SendCommaLeftArrow ? 1 : 0;
            if (ValueChanges._ValueChange[158] > 0f)
                keyboardkey(input, Keys.CommaLeftArrow, keyboard_id);
            if (ValueChanges._ValueChange[158] < 0f)
                keyboardkeyF(input, Keys.CommaLeftArrow, keyboard_id);
            ValueChanges[159] = SendPeriodRightArrow ? 1 : 0;
            if (ValueChanges._ValueChange[159] > 0f)
                keyboardkey(input, Keys.PeriodRightArrow, keyboard_id);
            if (ValueChanges._ValueChange[159] < 0f)
                keyboardkeyF(input, Keys.PeriodRightArrow, keyboard_id);
            ValueChanges[160] = SendForwardSlashQuestionMark ? 1 : 0;
            if (ValueChanges._ValueChange[160] > 0f)
                keyboardkey(input, Keys.ForwardSlashQuestionMark, keyboard_id);
            if (ValueChanges._ValueChange[160] < 0f)
                keyboardkeyF(input, Keys.ForwardSlashQuestionMark, keyboard_id);
            ValueChanges[161] = SendRightShift ? 1 : 0;
            if (ValueChanges._ValueChange[161] > 0f)
                keyboardkey(input, Keys.RightShift, keyboard_id);
            if (ValueChanges._ValueChange[161] < 0f)
                keyboardkeyF(input, Keys.RightShift, keyboard_id);
            ValueChanges[162] = SendRightAlt ? 1 : 0;
            if (ValueChanges._ValueChange[162] > 0f)
                keyboardkey(input, Keys.RightAlt, keyboard_id);
            if (ValueChanges._ValueChange[162] < 0f)
                keyboardkeyF(input, Keys.RightAlt, keyboard_id);
            ValueChanges[163] = SendSpace ? 1 : 0;
            if (ValueChanges._ValueChange[163] > 0f)
                keyboardkey(input, Keys.Space, keyboard_id);
            if (ValueChanges._ValueChange[163] < 0f)
                keyboardkeyF(input, Keys.Space, keyboard_id);
            ValueChanges[164] = SendCapsLock ? 1 : 0;
            if (ValueChanges._ValueChange[164] > 0f)
                keyboardkey(input, Keys.CapsLock, keyboard_id);
            if (ValueChanges._ValueChange[164] < 0f)
                keyboardkeyF(input, Keys.CapsLock, keyboard_id);
            ValueChanges[165] = SendUp ? 1 : 0;
            if (ValueChanges._ValueChange[165] > 0f)
                keyboardkey(input, Keys.Up, keyboard_id);
            if (ValueChanges._ValueChange[165] < 0f)
                keyboardkeyF(input, Keys.Up, keyboard_id);
            ValueChanges[166] = SendDown ? 1 : 0;
            if (ValueChanges._ValueChange[166] > 0f)
                keyboardkey(input, Keys.Down, keyboard_id);
            if (ValueChanges._ValueChange[166] < 0f)
                keyboardkeyF(input, Keys.Down, keyboard_id);
            ValueChanges[167] = SendRight ? 1 : 0;
            if (ValueChanges._ValueChange[167] > 0f)
                keyboardkey(input, Keys.Right, keyboard_id);
            if (ValueChanges._ValueChange[167] < 0f)
                keyboardkeyF(input, Keys.Right, keyboard_id);
            ValueChanges[168] = SendLeft ? 1 : 0;
            if (ValueChanges._ValueChange[168] > 0f)
                keyboardkey(input, Keys.Left, keyboard_id);
            if (ValueChanges._ValueChange[168] < 0f)
                keyboardkeyF(input, Keys.Left, keyboard_id);
            ValueChanges[169] = SendHome ? 1 : 0;
            if (ValueChanges._ValueChange[169] > 0f)
                keyboardkey(input, Keys.Home, keyboard_id);
            if (ValueChanges._ValueChange[169] < 0f)
                keyboardkeyF(input, Keys.Home, keyboard_id);
            ValueChanges[170] = SendEnd ? 1 : 0;
            if (ValueChanges._ValueChange[170] > 0f)
                keyboardkey(input, Keys.End, keyboard_id);
            if (ValueChanges._ValueChange[170] < 0f)
                keyboardkeyF(input, Keys.End, keyboard_id);
            ValueChanges[171] = SendDelete ? 1 : 0;
            if (ValueChanges._ValueChange[171] > 0f)
                keyboardkey(input, Keys.Delete, keyboard_id);
            if (ValueChanges._ValueChange[171] < 0f)
                keyboardkeyF(input, Keys.Delete, keyboard_id);
            ValueChanges[172] = SendPageUp ? 1 : 0;
            if (ValueChanges._ValueChange[172] > 0f)
                keyboardkey(input, Keys.PageUp, keyboard_id);
            if (ValueChanges._ValueChange[172] < 0f)
                keyboardkeyF(input, Keys.PageUp, keyboard_id);
            ValueChanges[173] = SendPageDown ? 1 : 0;
            if (ValueChanges._ValueChange[173] > 0f)
                keyboardkey(input, Keys.PageDown, keyboard_id);
            if (ValueChanges._ValueChange[173] < 0f)
                keyboardkeyF(input, Keys.PageDown, keyboard_id);
            ValueChanges[174] = SendInsert ? 1 : 0;
            if (ValueChanges._ValueChange[174] > 0f)
                keyboardkey(input, Keys.Insert, keyboard_id);
            if (ValueChanges._ValueChange[174] < 0f)
                keyboardkeyF(input, Keys.Insert, keyboard_id);
            ValueChanges[175] = SendPrintScreen ? 1 : 0;
            if (ValueChanges._ValueChange[175] > 0f)
                keyboardkey(input, Keys.PrintScreen, keyboard_id);
            if (ValueChanges._ValueChange[175] < 0f)
                keyboardkeyF(input, Keys.PrintScreen, keyboard_id);
            ValueChanges[176] = SendNumLock ? 1 : 0;
            if (ValueChanges._ValueChange[176] > 0f)
                keyboardkey(input, Keys.NumLock, keyboard_id);
            if (ValueChanges._ValueChange[176] < 0f)
                keyboardkeyF(input, Keys.NumLock, keyboard_id);
            ValueChanges[177] = SendScrollLock ? 1 : 0;
            if (ValueChanges._ValueChange[177] > 0f)
                keyboardkey(input, Keys.ScrollLock, keyboard_id);
            if (ValueChanges._ValueChange[177] < 0f)
                keyboardkeyF(input, Keys.ScrollLock, keyboard_id);
            ValueChanges[178] = SendMenu ? 1 : 0;
            if (ValueChanges._ValueChange[178] > 0f)
                keyboardkey(input, Keys.Menu, keyboard_id);
            if (ValueChanges._ValueChange[178] < 0f)
                keyboardkeyF(input, Keys.Menu, keyboard_id);
            ValueChanges[179] = SendWindowsKey ? 1 : 0;
            if (ValueChanges._ValueChange[179] > 0f)
                keyboardkey(input, Keys.WindowsKey, keyboard_id);
            if (ValueChanges._ValueChange[179] < 0f)
                keyboardkeyF(input, Keys.WindowsKey, keyboard_id);
            ValueChanges[180] = SendNumpadDivide ? 1 : 0;
            if (ValueChanges._ValueChange[180] > 0f)
                keyboardkey(input, Keys.NumpadDivide, keyboard_id);
            if (ValueChanges._ValueChange[180] < 0f)
                keyboardkeyF(input, Keys.NumpadDivide, keyboard_id);
            ValueChanges[181] = SendNumpadAsterisk ? 1 : 0;
            if (ValueChanges._ValueChange[181] > 0f)
                keyboardkey(input, Keys.NumpadAsterisk, keyboard_id);
            if (ValueChanges._ValueChange[181] < 0f)
                keyboardkeyF(input, Keys.NumpadAsterisk, keyboard_id);
            ValueChanges[182] = SendNumpad7 ? 1 : 0;
            if (ValueChanges._ValueChange[182] > 0f)
                keyboardkey(input, Keys.Numpad7, keyboard_id);
            if (ValueChanges._ValueChange[182] < 0f)
                keyboardkeyF(input, Keys.Numpad7, keyboard_id);
            ValueChanges[183] = SendNumpad8 ? 1 : 0;
            if (ValueChanges._ValueChange[183] > 0f)
                keyboardkey(input, Keys.Numpad8, keyboard_id);
            if (ValueChanges._ValueChange[183] < 0f)
                keyboardkeyF(input, Keys.Numpad8, keyboard_id);
            ValueChanges[184] = SendNumpad9 ? 1 : 0;
            if (ValueChanges._ValueChange[184] > 0f)
                keyboardkey(input, Keys.Numpad9, keyboard_id);
            if (ValueChanges._ValueChange[184] < 0f)
                keyboardkeyF(input, Keys.Numpad9, keyboard_id);
            ValueChanges[185] = SendNumpad4 ? 1 : 0;
            if (ValueChanges._ValueChange[185] > 0f)
                keyboardkey(input, Keys.Numpad4, keyboard_id);
            if (ValueChanges._ValueChange[185] < 0f)
                keyboardkeyF(input, Keys.Numpad4, keyboard_id);
            ValueChanges[186] = SendNumpad5 ? 1 : 0;
            if (ValueChanges._ValueChange[186] > 0f)
                keyboardkey(input, Keys.Numpad5, keyboard_id);
            if (ValueChanges._ValueChange[186] < 0f)
                keyboardkeyF(input, Keys.Numpad5, keyboard_id);
            ValueChanges[187] = SendNumpad6 ? 1 : 0;
            if (ValueChanges._ValueChange[187] > 0f)
                keyboardkey(input, Keys.Numpad6, keyboard_id);
            if (ValueChanges._ValueChange[187] < 0f)
                keyboardkeyF(input, Keys.Numpad6, keyboard_id);
            ValueChanges[188] = SendNumpad1 ? 1 : 0;
            if (ValueChanges._ValueChange[188] > 0f)
                keyboardkey(input, Keys.Numpad1, keyboard_id);
            if (ValueChanges._ValueChange[188] < 0f)
                keyboardkeyF(input, Keys.Numpad1, keyboard_id);
            ValueChanges[189] = SendNumpad2 ? 1 : 0;
            if (ValueChanges._ValueChange[189] > 0f)
                keyboardkey(input, Keys.Numpad2, keyboard_id);
            if (ValueChanges._ValueChange[189] < 0f)
                keyboardkeyF(input, Keys.Numpad2, keyboard_id);
            ValueChanges[190] = SendNumpad3 ? 1 : 0;
            if (ValueChanges._ValueChange[190] > 0f)
                keyboardkey(input, Keys.Numpad3, keyboard_id);
            if (ValueChanges._ValueChange[190] < 0f)
                keyboardkeyF(input, Keys.Numpad3, keyboard_id);
            ValueChanges[191] = SendNumpad0 ? 1 : 0;
            if (ValueChanges._ValueChange[191] > 0f)
                keyboardkey(input, Keys.Numpad0, keyboard_id);
            if (ValueChanges._ValueChange[191] < 0f)
                keyboardkeyF(input, Keys.Numpad0, keyboard_id);
            ValueChanges[192] = SendNumpadDelete ? 1 : 0;
            if (ValueChanges._ValueChange[192] > 0f)
                keyboardkey(input, Keys.NumpadDelete, keyboard_id);
            if (ValueChanges._ValueChange[192] < 0f)
                keyboardkeyF(input, Keys.NumpadDelete, keyboard_id);
            ValueChanges[193] = SendNumpadEnter ? 1 : 0;
            if (ValueChanges._ValueChange[193] > 0f)
                keyboardkey(input, Keys.NumpadEnter, keyboard_id);
            if (ValueChanges._ValueChange[193] < 0f)
                keyboardkeyF(input, Keys.NumpadEnter, keyboard_id);
            ValueChanges[194] = SendNumpadPlus ? 1 : 0;
            if (ValueChanges._ValueChange[194] > 0f)
                keyboardkey(input, Keys.NumpadPlus, keyboard_id);
            if (ValueChanges._ValueChange[194] < 0f)
                keyboardkeyF(input, Keys.NumpadPlus, keyboard_id);
            ValueChanges[195] = SendNumpadMinus ? 1 : 0;
            if (ValueChanges._ValueChange[195] > 0f)
                keyboardkey(input, Keys.NumpadMinus, keyboard_id);
            if (ValueChanges._ValueChange[195] < 0f)
                keyboardkeyF(input, Keys.NumpadMinus, keyboard_id);
            if (formvisible)
            {
                pollingratedisplay++;
                pollingratetemp = pollingrateperm;
                pollingrateperm = (double)PollingRate.ElapsedTicks / (Stopwatch.Frequency / 1000L);
                if (pollingratedisplay > 300)
                {
                    pollingrate = pollingrateperm - pollingratetemp;
                    pollingratedisplay = 0;
                }
                string str = "keyboard_id : " + keyboard_id + Environment.NewLine;
                str += "mouse_id : " + mouse_id + Environment.NewLine;
                str += "MouseDesktopX : " + MouseDesktopX + Environment.NewLine;
                str += "MouseDesktopY : " + MouseDesktopY + Environment.NewLine;
                str += "deltaX : " + deltaX + Environment.NewLine;
                str += "deltaY : " + deltaY + Environment.NewLine;
                str += "x : " + x + Environment.NewLine;
                str += "y : " + y + Environment.NewLine;
                str += "SendLeftClick : " + SendLeftClick + Environment.NewLine;
                str += "SendRightClick : " + SendRightClick + Environment.NewLine;
                str += "SendMiddleClick : " + SendMiddleClick + Environment.NewLine;
                str += "SendWheelUp : " + SendWheelUp + Environment.NewLine;
                str += "SendWheelDown : " + SendWheelDown + Environment.NewLine;
                str += "SendCANCEL : " + SendCANCEL + Environment.NewLine;
                str += "SendBACK : " + SendBACK + Environment.NewLine;
                str += "SendTAB : " + SendTAB + Environment.NewLine;
                str += "SendCLEAR : " + SendCLEAR + Environment.NewLine;
                str += "SendRETURN : " + SendRETURN + Environment.NewLine;
                str += "SendSHIFT : " + SendSHIFT + Environment.NewLine;
                str += "SendCONTROL : " + SendCONTROL + Environment.NewLine;
                str += "SendMENU : " + SendMENU + Environment.NewLine;
                str += "SendCAPITAL : " + SendCAPITAL + Environment.NewLine;
                str += "SendESCAPE : " + SendESCAPE + Environment.NewLine;
                str += "SendSPACE : " + SendSPACE + Environment.NewLine;
                str += "SendPRIOR : " + SendPRIOR + Environment.NewLine;
                str += "SendNEXT : " + SendNEXT + Environment.NewLine;
                str += "SendEND : " + SendEND + Environment.NewLine;
                str += "SendHOME : " + SendHOME + Environment.NewLine;
                str += "SendLEFT : " + SendLEFT + Environment.NewLine;
                str += "SendUP : " + SendUP + Environment.NewLine;
                str += "SendRIGHT : " + SendRIGHT + Environment.NewLine;
                str += "SendDOWN : " + SendDOWN + Environment.NewLine;
                str += "SendSNAPSHOT : " + SendSNAPSHOT + Environment.NewLine;
                str += "SendINSERT : " + SendINSERT + Environment.NewLine;
                str += "SendNUMPADDEL : " + SendNUMPADDEL + Environment.NewLine;
                str += "SendNUMPADINSERT : " + SendNUMPADINSERT + Environment.NewLine;
                str += "SendHELP : " + SendHELP + Environment.NewLine;
                str += "SendAPOSTROPHE : " + SendAPOSTROPHE + Environment.NewLine;
                str += "SendBACKSPACE : " + SendBACKSPACE + Environment.NewLine;
                str += "SendPAGEDOWN : " + SendPAGEDOWN + Environment.NewLine;
                str += "SendPAGEUP : " + SendPAGEUP + Environment.NewLine;
                str += "SendFIN : " + SendFIN + Environment.NewLine;
                str += "SendMOUSE : " + SendMOUSE + Environment.NewLine;
                str += "SendA : " + SendA + Environment.NewLine;
                str += "SendB : " + SendB + Environment.NewLine;
                str += "SendC : " + SendC + Environment.NewLine;
                str += "SendD : " + SendD + Environment.NewLine;
                str += "SendE : " + SendE + Environment.NewLine;
                str += "SendF : " + SendF + Environment.NewLine;
                str += "SendG : " + SendG + Environment.NewLine;
                str += "SendH : " + SendH + Environment.NewLine;
                str += "SendI : " + SendI + Environment.NewLine;
                str += "SendJ : " + SendJ + Environment.NewLine;
                str += "SendK : " + SendK + Environment.NewLine;
                str += "SendL : " + SendL + Environment.NewLine;
                str += "SendM : " + SendM + Environment.NewLine;
                str += "SendN : " + SendN + Environment.NewLine;
                str += "SendO : " + SendO + Environment.NewLine;
                str += "SendP : " + SendP + Environment.NewLine;
                str += "SendQ : " + SendQ + Environment.NewLine;
                str += "SendR : " + SendR + Environment.NewLine;
                str += "SendS : " + SendS + Environment.NewLine;
                str += "SendT : " + SendT + Environment.NewLine;
                str += "SendU : " + SendU + Environment.NewLine;
                str += "SendV : " + SendV + Environment.NewLine;
                str += "SendX : " + SendX + Environment.NewLine;
                str += "SendY : " + SendY + Environment.NewLine;
                str += "SendZ : " + SendZ + Environment.NewLine;
                str += "SendLWIN : " + SendLWIN + Environment.NewLine;
                str += "SendRWIN : " + SendRWIN + Environment.NewLine;
                str += "SendAPPS : " + SendAPPS + Environment.NewLine;
                str += "SendDELETE : " + SendDELETE + Environment.NewLine;
                str += "SendNUMPAD0 : " + SendNUMPAD0 + Environment.NewLine;
                str += "SendNUMPAD1 : " + SendNUMPAD1 + Environment.NewLine;
                str += "SendNUMPAD2 : " + SendNUMPAD2 + Environment.NewLine;
                str += "SendNUMPAD3 : " + SendNUMPAD3 + Environment.NewLine;
                str += "SendNUMPAD4 : " + SendNUMPAD4 + Environment.NewLine;
                str += "SendNUMPAD5 : " + SendNUMPAD5 + Environment.NewLine;
                str += "SendNUMPAD6 : " + SendNUMPAD6 + Environment.NewLine;
                str += "SendNUMPAD7 : " + SendNUMPAD7 + Environment.NewLine;
                str += "SendNUMPAD8 : " + SendNUMPAD8 + Environment.NewLine;
                str += "SendNUMPAD9 : " + SendNUMPAD9 + Environment.NewLine;
                str += "SendMULTIPLY : " + SendMULTIPLY + Environment.NewLine;
                str += "SendADD : " + SendADD + Environment.NewLine;
                str += "SendSUBTRACT : " + SendSUBTRACT + Environment.NewLine;
                str += "SendDECIMAL : " + SendDECIMAL + Environment.NewLine;
                str += "SendPRINTSCREEN : " + SendPRINTSCREEN + Environment.NewLine;
                str += "SendDIVIDE : " + SendDIVIDE + Environment.NewLine;
                str += "SendF1 : " + SendF1 + Environment.NewLine;
                str += "SendF2 : " + SendF2 + Environment.NewLine;
                str += "SendF3 : " + SendF3 + Environment.NewLine;
                str += "SendF4 : " + SendF4 + Environment.NewLine;
                str += "SendF5 : " + SendF5 + Environment.NewLine;
                str += "SendF6 : " + SendF6 + Environment.NewLine;
                str += "SendF7 : " + SendF7 + Environment.NewLine;
                str += "SendF8 : " + SendF8 + Environment.NewLine;
                str += "SendF9 : " + SendF9 + Environment.NewLine;
                str += "SendF10 : " + SendF10 + Environment.NewLine;
                str += "SendF11 : " + SendF11 + Environment.NewLine;
                str += "SendF12 : " + SendF12 + Environment.NewLine;
                str += "SendNUMLOCK : " + SendNUMLOCK + Environment.NewLine;
                str += "SendSCROLLLOCK : " + SendSCROLLLOCK + Environment.NewLine;
                str += "SendLEFTSHIFT : " + SendLEFTSHIFT + Environment.NewLine;
                str += "SendRIGHTSHIFT : " + SendRIGHTSHIFT + Environment.NewLine;
                str += "SendLEFTCONTROL : " + SendLEFTCONTROL + Environment.NewLine;
                str += "SendRIGHTCONTROL : " + SendRIGHTCONTROL + Environment.NewLine;
                str += "SendLEFTALT : " + SendLEFTALT + Environment.NewLine;
                str += "SendRIGHTALT : " + SendRIGHTALT + Environment.NewLine;
                str += "SendBROWSER_BACK : " + SendBROWSER_BACK + Environment.NewLine;
                str += "SendBROWSER_FORWARD : " + SendBROWSER_FORWARD + Environment.NewLine;
                str += "SendBROWSER_REFRESH : " + SendBROWSER_REFRESH + Environment.NewLine;
                str += "SendBROWSER_STOP : " + SendBROWSER_STOP + Environment.NewLine;
                str += "SendBROWSER_SEARCH : " + SendBROWSER_SEARCH + Environment.NewLine;
                str += "SendBROWSER_FAVORITES : " + SendBROWSER_FAVORITES + Environment.NewLine;
                str += "SendBROWSER_HOME : " + SendBROWSER_HOME + Environment.NewLine;
                str += "SendVOLUME_MUTE : " + SendVOLUME_MUTE + Environment.NewLine;
                str += "SendVOLUME_DOWN : " + SendVOLUME_DOWN + Environment.NewLine;
                str += "SendVOLUME_UP : " + SendVOLUME_UP + Environment.NewLine;
                str += "SendMEDIA_NEXT_TRACK : " + SendMEDIA_NEXT_TRACK + Environment.NewLine;
                str += "SendMEDIA_PREV_TRACK : " + SendMEDIA_PREV_TRACK + Environment.NewLine;
                str += "SendMEDIA_STOP : " + SendMEDIA_STOP + Environment.NewLine;
                str += "SendMEDIA_PLAY_PAUSE : " + SendMEDIA_PLAY_PAUSE + Environment.NewLine;
                str += "SendLAUNCH_MAIL : " + SendLAUNCH_MAIL + Environment.NewLine;
                str += "SendLAUNCH_MEDIA_SELECT : " + SendLAUNCH_MEDIA_SELECT + Environment.NewLine;
                str += "SendLAUNCH_APP1 : " + SendLAUNCH_APP1 + Environment.NewLine;
                str += "SendLAUNCH_APP2 : " + SendLAUNCH_APP2 + Environment.NewLine;
                str += "SendOEM_1 : " + SendOEM_1 + Environment.NewLine;
                str += "SendOEM_PLUS : " + SendOEM_PLUS + Environment.NewLine;
                str += "SendOEM_COMMA : " + SendOEM_COMMA + Environment.NewLine;
                str += "SendOEM_MINUS : " + SendOEM_MINUS + Environment.NewLine;
                str += "SendOEM_PERIOD : " + SendOEM_PERIOD + Environment.NewLine;
                str += "SendOEM_2 : " + SendOEM_2 + Environment.NewLine;
                str += "SendOEM_3 : " + SendOEM_3 + Environment.NewLine;
                str += "SendOEM_4 : " + SendOEM_4 + Environment.NewLine;
                str += "SendOEM_5 : " + SendOEM_5 + Environment.NewLine;
                str += "SendOEM_6 : " + SendOEM_6 + Environment.NewLine;
                str += "SendOEM_7 : " + SendOEM_7 + Environment.NewLine;
                str += "SendOEM_8 : " + SendOEM_8 + Environment.NewLine;
                str += "SendOEM_102 : " + SendOEM_102 + Environment.NewLine;
                str += "SendEREOF : " + SendEREOF + Environment.NewLine;
                str += "SendZOOM : " + SendZOOM + Environment.NewLine;
                str += "SendEscape : " + SendEscape + Environment.NewLine;
                str += "SendOne : " + SendOne + Environment.NewLine;
                str += "SendTwo : " + SendTwo + Environment.NewLine;
                str += "SendThree : " + SendThree + Environment.NewLine;
                str += "SendFour : " + SendFour + Environment.NewLine;
                str += "SendFive : " + SendFive + Environment.NewLine;
                str += "SendSix : " + SendSix + Environment.NewLine;
                str += "SendSeven : " + SendSeven + Environment.NewLine;
                str += "SendEight : " + SendEight + Environment.NewLine;
                str += "SendNine : " + SendNine + Environment.NewLine;
                str += "SendZero : " + SendZero + Environment.NewLine;
                str += "SendDashUnderscore : " + SendDashUnderscore + Environment.NewLine;
                str += "SendPlusEquals : " + SendPlusEquals + Environment.NewLine;
                str += "SendBackspace : " + SendBackspace + Environment.NewLine;
                str += "SendTab : " + SendTab + Environment.NewLine;
                str += "SendOpenBracketBrace : " + SendOpenBracketBrace + Environment.NewLine;
                str += "SendCloseBracketBrace : " + SendCloseBracketBrace + Environment.NewLine;
                str += "SendEnter : " + SendEnter + Environment.NewLine;
                str += "SendControl : " + SendControl + Environment.NewLine;
                str += "SendSemicolonColon : " + SendSemicolonColon + Environment.NewLine;
                str += "SendSingleDoubleQuote : " + SendSingleDoubleQuote + Environment.NewLine;
                str += "SendTilde : " + SendTilde + Environment.NewLine;
                str += "SendLeftShift : " + SendLeftShift + Environment.NewLine;
                str += "SendBackslashPipe : " + SendBackslashPipe + Environment.NewLine;
                str += "SendCommaLeftArrow : " + SendCommaLeftArrow + Environment.NewLine;
                str += "SendPeriodRightArrow : " + SendPeriodRightArrow + Environment.NewLine;
                str += "SendForwardSlashQuestionMark : " + SendForwardSlashQuestionMark + Environment.NewLine;
                str += "SendRightShift : " + SendRightShift + Environment.NewLine;
                str += "SendRightAlt : " + SendRightAlt + Environment.NewLine;
                str += "SendSpace : " + SendSpace + Environment.NewLine;
                str += "SendCapsLock : " + SendCapsLock + Environment.NewLine;
                str += "SendUp : " + SendUp + Environment.NewLine;
                str += "SendDown : " + SendDown + Environment.NewLine;
                str += "SendRight : " + SendRight + Environment.NewLine;
                str += "SendLeft : " + SendLeft + Environment.NewLine;
                str += "SendHome : " + SendHome + Environment.NewLine;
                str += "SendEnd : " + SendEnd + Environment.NewLine;
                str += "SendDelete : " + SendDelete + Environment.NewLine;
                str += "SendPageUp : " + SendPageUp + Environment.NewLine;
                str += "SendPageDown : " + SendPageDown + Environment.NewLine;
                str += "SendInsert : " + SendInsert + Environment.NewLine;
                str += "SendPrintScreen : " + SendPrintScreen + Environment.NewLine;
                str += "SendNumLock : " + SendNumLock + Environment.NewLine;
                str += "SendScrollLock : " + SendScrollLock + Environment.NewLine;
                str += "SendMenu : " + SendMenu + Environment.NewLine;
                str += "SendWindowsKey : " + SendWindowsKey + Environment.NewLine;
                str += "SendNumpadDivide : " + SendNumpadDivide + Environment.NewLine;
                str += "SendNumpadAsterisk : " + SendNumpadAsterisk + Environment.NewLine;
                str += "SendNumpad7 : " + SendNumpad7 + Environment.NewLine;
                str += "SendNumpad8 : " + SendNumpad8 + Environment.NewLine;
                str += "SendNumpad9 : " + SendNumpad9 + Environment.NewLine;
                str += "SendNumpad4 : " + SendNumpad4 + Environment.NewLine;
                str += "SendNumpad5 : " + SendNumpad5 + Environment.NewLine;
                str += "SendNumpad6 : " + SendNumpad6 + Environment.NewLine;
                str += "SendNumpad1 : " + SendNumpad1 + Environment.NewLine;
                str += "SendNumpad2 : " + SendNumpad2 + Environment.NewLine;
                str += "SendNumpad3 : " + SendNumpad3 + Environment.NewLine;
                str += "SendNumpad0 : " + SendNumpad0 + Environment.NewLine;
                str += "SendNumpadDelete : " + SendNumpadDelete + Environment.NewLine;
                str += "SendNumpadEnter : " + SendNumpadEnter + Environment.NewLine;
                str += "SendNumpadPlus : " + SendNumpadPlus + Environment.NewLine;
                str += "SendNumpadMinus : " + SendNumpadMinus + Environment.NewLine;
                str += "PollingRate : " + pollingrate + " ms" + Environment.NewLine;
                string txt = str;
                string[] lines = txt.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                foreach (string line in lines)
                    if (line.Contains(inputdelaybutton + " : "))
                        inputdelay = line;
                valchanged(0, inputdelay.Contains("True"));
                if (wd[0] == 1)
                {
                    getstate = true;
                }
                if (inputdelay.Contains("False"))
                    getstate = false;
                if (getstate)
                {
                    elapseddown = (double)PollingRate.ElapsedTicks / (Stopwatch.Frequency / 1000L);
                    elapsed = 0;
                }
                if (wu[0] == 1)
                {
                    elapsedup = (double)PollingRate.ElapsedTicks / (Stopwatch.Frequency / 1000L);
                    elapsed = elapsedup - elapseddown;
                }
                ValueChange[0] = inputdelay.Contains("False") ? elapsed : 0;
                if (ValueChange._ValueChange[0] > 0)
                {
                    delay = ValueChange._ValueChange[0];
                }
                str += "InputDelay : " + delay + " ms" + Environment.NewLine;
                str += Environment.NewLine;
                form1.SetLabel1(str);
            }
        }
        private void mouseclickleft(Input input, int mouse_id)
        {
            Task.Run(() => input.SendLeftClick(mouse_id));
        }
        private void mouseclickleftF(Input input, int mouse_id)
        {
            Task.Run(() => input.SendLeftClickF(mouse_id));
        }
        private void mouseclickright(Input input, int mouse_id)
        {
            Task.Run(() => input.SendRightClick(mouse_id));
        }
        private void mouseclickrightF(Input input, int mouse_id)
        {
            Task.Run(() => input.SendRightClickF(mouse_id));
        }
        private void mouseclickmiddle(Input input, int mouse_id)
        {
            Task.Run(() => input.SendMiddleClick(mouse_id));
        }
        private void mouseclickmiddleF(Input input, int mouse_id)
        {
            Task.Run(() => input.SendMiddleClickF(mouse_id));
        }
        private void mousewheelup(Input input, int mouse_id)
        {
            Task.Run(() => input.SendWheelUp(mouse_id));
        }
        private void mousewheeldown(Input input, int mouse_id)
        {
            Task.Run(() => input.SendWheelDown(mouse_id));
        }
        private void keyboardkey(Input input, Keys key, int keyboard_id)
        {
            Task.Run(() => input.SendKey(key, keyboard_id));
        }
        private void keyboardkeyF(Input input, Keys key, int keyboard_id)
        {
            Task.Run(() => input.SendKeyF(key, keyboard_id));
        }
        public void MoveMouseBy(Input input, int deltaX, int deltaY, int mouseId)
        {
            Task.Run(() => input.MoveMouseBy(deltaX, deltaY, mouseId));
        }
        private void MoveMouseTo(Input input, int x, int y, int mouseId)
        {
            Task.Run(() => input.MoveMouseTo(x, y, mouseId));
        }
    }
    public class Input
    {
        private IntPtr context;
        public KeyboardFilterMode KeyboardFilterMode { get; set; }
        public MouseFilterMode MouseFilterMode { get; set; }
        public bool IsLoaded { get; set; }
        public Input()
        {
            context = IntPtr.Zero;
            KeyboardFilterMode = KeyboardFilterMode.None;
            MouseFilterMode = MouseFilterMode.None;
        }
        public bool Load()
        {
            context = InterceptionDriver.CreateContext();
            return true;
        }
        public void Unload()
        {
            InterceptionDriver.DestroyContext(context);
        }
        public void SendKey(Keys key, KeyState state, int keyboardId)
        {
            Stroke stroke = new Stroke();
            KeyStroke keyStroke = new KeyStroke();
            keyStroke.Code = key;
            keyStroke.State = state;
            stroke.Key = keyStroke;
            InterceptionDriver.Send(context, keyboardId, ref stroke, 1);
        }
        public void SendKey(Keys key, int keyboardId)
        {
            SendKey(key, KeyState.Down, keyboardId);
        }
        public void SendKeyF(Keys key, int keyboardId)
        {
            SendKey(key, KeyState.Up, keyboardId);
        }
        public void SendMouseEvent(MouseState state, int mouseId)
        {
            Stroke stroke = new Stroke();
            MouseStroke mouseStroke = new MouseStroke();
            mouseStroke.State = state;
            if (state == MouseState.ScrollUp)
            {
                mouseStroke.Rolling = 120;
            }
            else if (state == MouseState.ScrollDown)
            {
                mouseStroke.Rolling = -120;
            }
            stroke.Mouse = mouseStroke;
            InterceptionDriver.Send(context, mouseId, ref stroke, 1);
        }
        public void SendLeftClick(int mouseId)
        {
            SendMouseEvent(MouseState.LeftDown, mouseId);
        }
        public void SendRightClick(int mouseId)
        {
            SendMouseEvent(MouseState.RightDown, mouseId);
        }
        public void SendLeftClickF(int mouseId)
        {
            SendMouseEvent(MouseState.LeftUp, mouseId);
        }
        public void SendRightClickF(int mouseId)
        {
            SendMouseEvent(MouseState.RightUp, mouseId);
        }
        public void SendMiddleClick(int mouseId)
        {
            SendMouseEvent(MouseState.MiddleDown, mouseId);
        }
        public void SendMiddleClickF(int mouseId)
        {
            SendMouseEvent(MouseState.MiddleUp, mouseId);
        }
        public void SendWheelUp(int mouseId)
        {
            SendMouseEvent(MouseState.ScrollUp, mouseId);
        }
        public void SendWheelDown(int mouseId)
        {
            SendMouseEvent(MouseState.ScrollDown, mouseId);
        }
        public void MoveMouseBy(int deltaX, int deltaY, int mouseId)
        {
            if (deltaX != 0 | deltaY != 0)
            {
                Stroke stroke = new Stroke();
                MouseStroke mouseStroke = new MouseStroke();
                mouseStroke.X = deltaX;
                mouseStroke.Y = deltaY;
                stroke.Mouse = mouseStroke;
                stroke.Mouse.Flags = MouseFlags.MoveRelative;
                InterceptionDriver.Send(context, mouseId, ref stroke, 1);
            }
        }
        public void MoveMouseTo(int x, int y, int mouseId)
        {
            if (x != 0 | y != 0)
            {
                Stroke stroke = new Stroke();
                MouseStroke mouseStroke = new MouseStroke();
                mouseStroke.X = x;
                mouseStroke.Y = y;
                stroke.Mouse = mouseStroke;
                stroke.Mouse.Flags = MouseFlags.MoveAbsolute;
                InterceptionDriver.Send(context, mouseId, ref stroke, 1);
            }
        }
    }
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int Predicate(int device);
    [Flags]
    public enum KeyState : ushort
    {
        Down = 0x00,
        Up = 0x01,
        E0 = 0x02,
        E1 = 0x04,
        TermsrvSetLED = 0x08,
        TermsrvShadow = 0x10,
        TermsrvVKPacket = 0x20
    }
    [Flags]
    public enum KeyboardFilterMode : ushort
    {
        None = 0x0000,
        All = 0xFFFF,
        KeyDown = KeyState.Up,
        KeyUp = KeyState.Up << 1,
        KeyE0 = KeyState.E0 << 1,
        KeyE1 = KeyState.E1 << 1,
        KeyTermsrvSetLED = KeyState.TermsrvSetLED << 1,
        KeyTermsrvShadow = KeyState.TermsrvShadow << 1,
        KeyTermsrvVKPacket = KeyState.TermsrvVKPacket << 1
    }
    [Flags]
    public enum MouseState : ushort
    {
        LeftDown = 0x01,
        LeftUp = 0x02,
        RightDown = 0x04,
        RightUp = 0x08,
        MiddleDown = 0x10,
        MiddleUp = 0x20,
        LeftExtraDown = 0x40,
        LeftExtraUp = 0x80,
        RightExtraDown = 0x100,
        RightExtraUp = 0x200,
        ScrollVertical = 0x400,
        ScrollUp = 0x400,
        ScrollDown = 0x400,
        ScrollHorizontal = 0x800,
        ScrollLeft = 0x800,
        ScrollRight = 0x800,
    }
    [Flags]
    public enum MouseFilterMode : ushort
    {
        None = 0x0000,
        All = 0xFFFF,
        LeftDown = 0x01,
        LeftUp = 0x02,
        RightDown = 0x04,
        RightUp = 0x08,
        MiddleDown = 0x10,
        MiddleUp = 0x20,
        LeftExtraDown = 0x40,
        LeftExtraUp = 0x80,
        RightExtraDown = 0x100,
        RightExtraUp = 0x200,
        MouseWheelVertical = 0x400,
        MouseWheelHorizontal = 0x800,
        MouseMove = 0x1000,
    }
    [Flags]
    public enum MouseFlags : ushort
    {
        MoveRelative = 0x000,
        MoveAbsolute = 0x001,
        VirtualDesktop = 0x002,
        AttributesChanged = 0x004,
        MoveWithoutCoalescing = 0x008,
        TerminalServicesSourceShadow = 0x100
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct MouseStroke
    {
        public MouseState State;
        public MouseFlags Flags;
        public Int16 Rolling;
        public Int32 X;
        public Int32 Y;
        public UInt16 Information;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct KeyStroke
    {
        public Keys Code;
        public KeyState State;
        public UInt32 Information;
    }
    [StructLayout(LayoutKind.Explicit)]
    public struct Stroke
    {
        [FieldOffset(0)]
        public MouseStroke Mouse;
        [FieldOffset(0)]
        public KeyStroke Key;
    }
    public class InterceptionDriver
    {
        [DllImport("interception.dll", EntryPoint = "interception_create_context", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateContext();
        [DllImport("interception.dll", EntryPoint = "interception_destroy_context", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DestroyContext(IntPtr context);
        [DllImport("interception.dll", EntryPoint = "interception_get_precedence", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetPrecedence(IntPtr context, Int32 device);
        [DllImport("interception.dll", EntryPoint = "interception_set_precedence", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetPrecedence(IntPtr context, Int32 device, Int32 Precedence);
        [DllImport("interception.dll", EntryPoint = "interception_get_filter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetFilter(IntPtr context, Int32 device);
        [DllImport("interception.dll", EntryPoint = "interception_set_filter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetFilter(IntPtr context, Predicate predicate, Int32 keyboardFilterMode);
        [DllImport("interception.dll", EntryPoint = "interception_wait", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 Wait(IntPtr context);
        [DllImport("interception.dll", EntryPoint = "interception_wait_with_timeout", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 WaitWithTimeout(IntPtr context, UInt64 milliseconds);
        [DllImport("interception.dll", EntryPoint = "interception_send", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 Send(IntPtr context, Int32 device, ref Stroke stroke, UInt32 numStrokes);
        [DllImport("interception.dll", EntryPoint = "interception_receive", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 Receive(IntPtr context, Int32 device, ref Stroke stroke, UInt32 numStrokes);
        [DllImport("interception.dll", EntryPoint = "interception_get_hardware_id", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 GetHardwareId(IntPtr context, Int32 device, String hardwareIdentifier, UInt32 sizeOfString);
        [DllImport("interception.dll", EntryPoint = "interception_is_invalid", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 IsInvalid(Int32 device);
        [DllImport("interception.dll", EntryPoint = "interception_is_keyboard", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 IsKeyboard(Int32 device);
        [DllImport("interception.dll", EntryPoint = "interception_is_mouse", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 IsMouse(Int32 device);
    }
    public class KeyPressedEventArgs : EventArgs
    {
        public Keys Key { get; set; }
        public KeyState State { get; set; }
        public bool Handled { get; set; }
    }
    public enum Keys : ushort
    {
        CANCEL = 70,
        BACK = 14,
        TAB = 15,
        CLEAR = 76,
        RETURN = 28,
        SHIFT = 42,
        CONTROL = 29,
        MENU = 56,
        CAPITAL = 58,
        ESCAPE = 1,
        SPACE = 57,
        PRIOR = 73,
        NEXT = 81,
        END = 79,
        HOME = 71,
        LEFT = 101,
        UP = 100,
        RIGHT = 103,
        DOWN = 102,
        SNAPSHOT = 84,
        INSERT = 91,
        NUMPADDEL = 83,
        NUMPADINSERT = 82,
        HELP = 99,
        APOSTROPHE = 41,
        BACKSPACE = 14,
        PAGEDOWN = 97,
        PAGEUP = 93,
        FIN = 96,
        MOUSE = 105,
        A = 16,
        B = 48,
        C = 46,
        D = 32,
        E = 18,
        F = 33,
        G = 34,
        H = 35,
        I = 23,
        J = 36,
        K = 37,
        L = 38,
        M = 39,
        N = 49,
        O = 24,
        P = 25,
        Q = 30,
        R = 19,
        S = 31,
        T = 20,
        U = 22,
        V = 47,
        W = 44,
        X = 45,
        Y = 21,
        Z = 17,
        LWIN = 91,
        RWIN = 92,
        APPS = 93,
        DELETE = 95,
        NUMPAD0 = 82,
        NUMPAD1 = 79,
        NUMPAD2 = 80,
        NUMPAD3 = 81,
        NUMPAD4 = 75,
        NUMPAD5 = 76,
        NUMPAD6 = 77,
        NUMPAD7 = 71,
        NUMPAD8 = 72,
        NUMPAD9 = 73,
        MULTIPLY = 55,
        ADD = 78,
        SUBTRACT = 74,
        DECIMAL = 83,
        PRINTSCREEN = 84,
        DIVIDE = 53,
        F1 = 59,
        F2 = 60,
        F3 = 61,
        F4 = 62,
        F5 = 63,
        F6 = 64,
        F7 = 65,
        F8 = 66,
        F9 = 67,
        F10 = 68,
        F11 = 87,
        F12 = 88,
        NUMLOCK = 69,
        SCROLLLOCK = 70,
        LEFTSHIFT = 42,
        RIGHTSHIFT = 54,
        LEFTCONTROL = 29,
        RIGHTCONTROL = 99,
        LEFTALT = 56,
        RIGHTALT = 98,
        BROWSER_BACK = 106,
        BROWSER_FORWARD = 105,
        BROWSER_REFRESH = 103,
        BROWSER_STOP = 104,
        BROWSER_SEARCH = 101,
        BROWSER_FAVORITES = 102,
        BROWSER_HOME = 50,
        VOLUME_MUTE = 32,
        VOLUME_DOWN = 46,
        VOLUME_UP = 48,
        MEDIA_NEXT_TRACK = 25,
        MEDIA_PREV_TRACK = 16,
        MEDIA_STOP = 36,
        MEDIA_PLAY_PAUSE = 34,
        LAUNCH_MAIL = 108,
        LAUNCH_MEDIA_SELECT = 109,
        LAUNCH_APP1 = 107,
        LAUNCH_APP2 = 33,
        OEM_1 = 27,
        OEM_PLUS = 13,
        OEM_COMMA = 50,
        OEM_MINUS = 0,
        OEM_PERIOD = 51,
        OEM_2 = 52,
        OEM_3 = 40,
        OEM_4 = 12,
        OEM_5 = 43,
        OEM_6 = 26,
        OEM_7 = 41,
        OEM_8 = 53,
        OEM_102 = 86,
        EREOF = 93,
        ZOOM = 98,
        Escape = 1,
        One = 2,
        Two = 3,
        Three = 4,
        Four = 5,
        Five = 6,
        Six = 7,
        Seven = 8,
        Eight = 9,
        Nine = 10,
        Zero = 11,
        DashUnderscore = 12,
        PlusEquals = 13,
        Backspace = 14,
        Tab = 15,
        OpenBracketBrace = 26,
        CloseBracketBrace = 27,
        Enter = 28,
        Control = 29,
        SemicolonColon = 39,
        SingleDoubleQuote = 40,
        Tilde = 41,
        LeftShift = 42,
        BackslashPipe = 43,
        CommaLeftArrow = 51,
        PeriodRightArrow = 52,
        ForwardSlashQuestionMark = 53,
        RightShift = 54,
        RightAlt = 56,
        Space = 57,
        CapsLock = 58,
        Up = 72,
        Down = 80,
        Right = 77,
        Left = 75,
        Home = 71,
        End = 79,
        Delete = 83,
        PageUp = 73,
        PageDown = 81,
        Insert = 82,
        PrintScreen = 55,
        NumLock = 69,
        ScrollLock = 70,
        Menu = 93,
        WindowsKey = 91,
        NumpadDivide = 53,
        NumpadAsterisk = 55,
        Numpad7 = 71,
        Numpad8 = 72,
        Numpad9 = 73,
        Numpad4 = 75,
        Numpad5 = 76,
        Numpad6 = 77,
        Numpad1 = 79,
        Numpad2 = 80,
        Numpad3 = 81,
        Numpad0 = 82,
        NumpadDelete = 83,
        NumpadEnter = 28,
        NumpadPlus = 78,
        NumpadMinus = 74,
    }
    public class MousePressedEventArgs : EventArgs
    {
        public MouseState State { get; set; }
        public bool Handled { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public short Rolling { get; set; }
    }
    public enum ScrollDirection
    {
        Down,
        Up
    }
}
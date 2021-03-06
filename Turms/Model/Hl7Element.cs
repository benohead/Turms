﻿using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Turms.UnitTest")]

namespace Turms.Model
{
    public abstract class Hl7Element
    {
        protected internal Hl7Encoding Encoding { get; protected set; }
        internal bool IsParsed { get; set; }
        protected string Value { get; set; }

        public void EnsureFullyParsed()
        {
            if (!IsParsed)
            {
                FullyParse();
            }
        }

        protected abstract void FullyParse();
        public abstract Hl7Element this[int i] { get; set; }
    }
}
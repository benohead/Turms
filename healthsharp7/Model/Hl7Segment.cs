using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("HealthSharp7.UnitTest")]

namespace healthsharp7.Model
{
    public class Hl7Segment
    {
        private static readonly char FieldSeparator = '|';

        private Hl7Segment(string name)
        {
            Name = name;
            Fields = new List<Hl7Field>();
        }

        private Hl7Segment(string name, string segment) : this(name)
        {
            Value = segment;
        }

        private string Value { get; }
        public string Name { get; }
        internal bool IsParsed { get; private set; }

        public Hl7Field this[int i]
        {
            get
            {
                EnsureFieldsParsed();
                return Fields.Count > i ? Fields[i] : new Hl7Field("");
            }
        }

        private void EnsureFieldsParsed()
        {
            if (!IsParsed)
                ParseFields();
        }

        private List<Hl7Field> Fields { get; }

        private void ParseFields()
        {
            var fields = Value.TrimEnd(FieldSeparator).Split(FieldSeparator).ToList();
            Fields.Clear();
            foreach (var field in fields)
            {
                Fields.Add(Hl7Field.Parse(field));                
            }
        }

        public override string ToString()
        {
            EnsureFieldsParsed();
            return String.Join(FieldSeparator, Fields.Select(f => f.ToString()));
        }

        public static Hl7Segment Parse(string segment)
        {
            if (segment == null) throw new ArgumentNullException(nameof(segment));
            if (segment.Length < 3
                || segment.Length > 3 && segment[3] != '|'
                || segment.Substring(0, 3).Contains("|"))
                throw new ArgumentException("Segment names should be 3 character long", segment);

            var segmentName = segment.Substring(0, 3);
            var hl7Segment = new Hl7Segment(segmentName, segment);
            if (segment.Length <= 4)
                hl7Segment.IsParsed = true;
            return hl7Segment;
        }
    }
}
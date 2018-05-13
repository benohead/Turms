using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using HealthSharp7.Model;

[assembly: InternalsVisibleTo("HealthSharp7.UnitTest")]

namespace healthsharp7.Model
{
    public class Hl7Segment
    {
        private Hl7Segment(Hl7Encoding encoding)
        {
            Encoding = encoding;
        }

        private Hl7Segment(string name, Hl7Encoding encoding) : this(encoding)
        {
            Name = name;
            Fields = new List<Hl7Field>();
        }

        private Hl7Segment(string name, string segment, Hl7Encoding encoding) : this(name, encoding)
        {
            Value = segment;
        }

        private Hl7Encoding Encoding { get; }
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

        private List<Hl7Field> Fields { get; }

        private void EnsureFieldsParsed()
        {
            if (!IsParsed)
                ParseFields();
        }

        private void ParseFields()
        {
            var fields = Value.TrimEnd(Encoding.FieldSeparator).Split(Encoding.FieldSeparator).ToList();
            Fields.Clear();
            foreach (var field in fields)
                Fields.Add(Hl7Field.Parse(field));
        }

        public override string ToString()
        {
            EnsureFieldsParsed();
            return string.Join(Encoding.FieldSeparator, Fields.Select(f => f.ToString()));
        }

        public static Hl7Segment Parse(string segment)
        {
            return Parse(segment, new Hl7Encoding());
        }

        public static Hl7Segment Parse(string segment, Hl7Encoding encoding)
        {
            if (segment == null) throw new ArgumentNullException(nameof(segment));
            if (segment.Length < 3
                || segment.Length > 3 && segment[3] != encoding.FieldSeparator
                || segment.Substring(0, 3).Contains(encoding.FieldSeparator))
                throw new ArgumentException("Segment names should be 3 character long", segment);

            var segmentName = segment.Substring(0, 3);
            var hl7Segment = new Hl7Segment(segmentName, segment, encoding);
            if (segment.Length <= 4)
                hl7Segment.IsParsed = true;
            return hl7Segment;
        }
    }
}
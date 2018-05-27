using System;
using System.Collections.Generic;
using System.Linq;

namespace Turms.Model
{
    public class Hl7Segment : Hl7Element
    {
        public string Name { get; }
        private List<Hl7Field> Fields { get; }

        public override string ToString()
        {
            EnsureFullyParsed();
            return string.Join(Encoding.FieldSeparator.ToString(), Fields.Select(f => f.ToString()))
                .TrimEnd(Encoding.FieldSeparator);
        }

        #region constructors

        public Hl7Segment() : this(new Hl7Encoding())
        {
        }

        private Hl7Segment(Hl7Encoding encoding)
        {
            Encoding = encoding;
            Value = string.Empty;
            Fields = new List<Hl7Field>();
        }

        private Hl7Segment(string name, Hl7Encoding encoding) : this(encoding)
        {
            Name = name;
        }

        private Hl7Segment(string name, string segment, Hl7Encoding encoding) : this(name, encoding)
        {
            Value = segment;
        }

        public Hl7Segment(string value) : this(value.Substring(0, 3), value, new Hl7Encoding())
        {
        }

        #endregion constructors

        #region operators

        public override Hl7Element this[int i]
        {
            get
            {
                EnsureFullyParsed();
                if (Fields.Count > i)
                {
                    return Fields[i];
                }
                var field = Hl7Field.Parse("", Encoding);
                this[i] = field;
                return field;
            }
            set
            {
                EnsureFullyParsed();
                for (int j = Fields.Count; j <= i; j++)
                {
                    Fields.Add(new Hl7Field());
                }
                Fields[i] = (Hl7Field) value;
            }
        }

        public static Hl7Segment operator +(Hl7Segment segment, Hl7Field field)
        {
            segment.EnsureFullyParsed();
            segment.Fields.Add(field);
            return segment;
        }

        public static Hl7Segment operator +(Hl7Segment segment, string field)
        {
            segment += Hl7Field.Parse(field, segment.Encoding);
            return segment;
        }

        #endregion

        #region parsing

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
            {
                hl7Segment.EnsureFullyParsed();
            }
            return hl7Segment;
        }

        protected override void FullyParse()
        {
            if (!string.IsNullOrEmpty(Value))
            {
                var fields = Value.TrimEnd(Encoding.FieldSeparator).Split(Encoding.FieldSeparator).ToList();
                Fields.Clear();
                foreach (var field in fields)
                    Fields.Add(Hl7Field.Parse(field, Encoding));
                IsParsed = true;
                Value = null;
            }
        }

        #endregion parsing
    }
}
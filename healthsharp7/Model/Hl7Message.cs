using System;
using System.Collections.Generic;
using System.Linq;
using HealthSharp7.Model;

namespace healthsharp7.Model
{
    public class Hl7Message: Hl7Element
    {
        private readonly List<Hl7Segment> segmentsInternal;

        public List<Hl7Segment> Segments
        {
            get
            {
                EnsureFullyParsed();
                return segmentsInternal;
            }
        }

        public Hl7Message() : this(new Hl7Encoding())
        {
        }

        private Hl7Message(Hl7Encoding encoding)
        {
            Encoding = encoding;
            Value = String.Empty;
            segmentsInternal = new List<Hl7Segment>();
        }

        private Hl7Message(string message, Hl7Encoding encoding): this(encoding)
        {
            Value = message;
        }

        public override string ToString()
        {
            return string.Join(Encoding.SegmentSeparator[0], Segments.Select(f => f.ToString()));
        }

        public static Hl7Message operator +(Hl7Message message, Hl7Segment segment)
        {
            message.Segments.Add(segment);
            return message;
        }

        public static Hl7Message Parse(string message)
        {
            return Parse(message, new Hl7Encoding());
        }

        public static Hl7Message Parse(string message, Hl7Encoding encoding)
        {
            var hl7Message = new Hl7Message(message, encoding);

            return hl7Message;
        }

        protected override void FullyParse()
        {
            if (!String.IsNullOrEmpty(Value)) {
                var segments = Value.Split(Encoding.SegmentSeparator, StringSplitOptions.None).ToList();
                segmentsInternal.Clear();
                foreach (var segment in segments)
                    segmentsInternal.Add(Hl7Segment.Parse(segment));
            }
            IsParsed = true;
        }
    }
}
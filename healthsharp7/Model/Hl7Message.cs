using System;
using System.Collections.Generic;
using System.Linq;
using HealthSharp7.Model;

namespace healthsharp7.Model
{
    public class Hl7Message
    {
        private Hl7Encoding Encoding { get; }

        public List<Hl7Segment> Segments { get; }

        public Hl7Message() : this(new Hl7Encoding())
        {
        }

        private Hl7Message(Hl7Encoding encoding)
        {
            Encoding = encoding;
            Segments = new List<Hl7Segment>();
        }

        private Hl7Message(List<string> segments, Hl7Encoding encoding) : this(encoding)
        {
            Segments.Clear();
            foreach (var segment in segments)
                Segments.Add(Hl7Segment.Parse(segment));
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
            var segments = message.Split(encoding.SegmentSeparator, StringSplitOptions.None).ToList();
            var hl7Message = new Hl7Message(segments, encoding);

            return hl7Message;
        }
    }
}
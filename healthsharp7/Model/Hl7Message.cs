using System;
using System.Collections.Generic;
using System.Linq;

namespace healthsharp7.Model
{
    public class Hl7Message
    {
        private static readonly string[] LineSeparators = {"\r\n", "\n\r", "\r", "\n"};

        private Hl7Message()
        {
            Segments = new List<Hl7Segment>();
        }

        private Hl7Message(List<string> segments) : this()
        {
            Segments.Clear();
            foreach (var segment in segments)
                Segments.Add(Hl7Segment.Parse(segment));
        }

        public List<Hl7Segment> Segments { get; }

        public static Hl7Message Parse(string message)
        {
            var segments = message.Split(LineSeparators, StringSplitOptions.None).ToList();
            var hl7Message = new Hl7Message(segments);

            return hl7Message;
        }
    }
}
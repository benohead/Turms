using System;

namespace healthsharp7.Model
{
    public class Hl7Segment
    {
        public string Name { get; }

        private Hl7Segment(string name)
        {
            Name = name;
        }

        public static Hl7Segment Parse(string segment)
        {
            if (segment == null) throw new ArgumentNullException(nameof(segment));
            if (segment.Length < 3 || (segment.Length > 3 && segment[3] != '|')) throw new ArgumentException("Segment names should be 3 character long", segment);

            var segmentName = segment.Substring(0, 3);
            return new Hl7Segment(segmentName);
        }
    }
}
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("healthsharp7.utest")]
namespace healthsharp7.Model
{
    public class Hl7Segment
    {
        private Hl7Segment(string name)
        {
            Name = name;
        }

        public string Name { get; }
        internal bool IsParsed { get; private set; }

        public static Hl7Segment Parse(string segment)
        {
            if (segment == null) throw new ArgumentNullException(nameof(segment));
            if (segment.Length < 3 || segment.Length > 3 && segment[3] != '|')
                throw new ArgumentException("Segment names should be 3 character long", segment);

            var segmentName = segment.Substring(0, 3);
            var hl7Segment = new Hl7Segment(segmentName);
            if (segment.Length <= 4)
            {
                hl7Segment.IsParsed = true;
            }
            return hl7Segment;
        }
    }
}
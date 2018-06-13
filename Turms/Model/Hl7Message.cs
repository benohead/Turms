using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Turms.Model
{
    public class Hl7Message : Hl7Element
    {
        private readonly List<Hl7Segment> segmentsInternal;

        public string MessageType => this["MSH.8"].ToString();
        public string MessageControlId => this["MSH.9"].ToString();
        public string ProcessingId => this["MSH.10"].ToString();
        public string MessageVersion => this["MSH.11"].ToString();

        public List<Hl7Segment> Segments
        {
            get
            {
                EnsureFullyParsed();
                return segmentsInternal;
            }
        }

        #region constructors

        public Hl7Message() : this(new Hl7Encoding())
        {
        }

        private Hl7Message(Hl7Encoding encoding)
        {
            Encoding = encoding;
            Value =
                $"MSH{encoding.FieldSeparator}{encoding.ComponentSeparator}{encoding.RepetitionSeparator}{encoding.EscapeCharacter}{encoding.SubcomponentSeparator}{encoding.FieldSeparator}";
            segmentsInternal = new List<Hl7Segment>();
        }

        private Hl7Message(string message, Hl7Encoding encoding) : this(encoding)
        {
            Value = CheckMessage(message);
        }

        private Hl7Message(string message) : this(new Hl7Encoding())
        {
            Value = CheckMessage(message);
            Encoding.FieldSeparator = Value[3];
        }

        private static string CheckMessage(string message)
        {
            message = message.TrimStart(' ');
            if (!message.StartsWith("MSH"))
            {
                throw new ArgumentException("HL7 messages should start with an MSH segment", nameof(message));
            }
            if (message[3] != message[8])
            {
                throw new ArgumentException("There should be exactly 4 separators defined in the MSH segment",
                    nameof(message));
            }
            if (message.Substring(3, 5).Any(Char.IsLetterOrDigit))
            {
                throw new ArgumentException("Separators should not be alphanumeric characters", nameof(message));
            }
            if (message.Substring(3, 5).Distinct().Count() != 5)
            {
                throw new ArgumentException("All separators should not be distinct characters", nameof(message));
            }
            return message;
        }

        #endregion

        public override string ToString()
        {
            EnsureFullyParsed();
            return string.Join(Encoding.SegmentSeparator[0], Segments.Select(segment => segment.ToEscapedString()));
        }

        #region operators

        public Hl7Element this[string query]
        {
            get
            {
                EnsureFullyParsed();
                var queryParts = query.Split('.').ToList();
                var segmentName = queryParts[0];
                var segment = segmentsInternal.FirstOrDefault(s => s.Name == segmentName);
                if (queryParts.Count == 1)
                {
                    return segment;
                }

                var index = int.Parse(queryParts[1]);
                return segment?[index];
            }
        }

        public override Hl7Element this[int i]
        {
            get
            {
                EnsureFullyParsed();
                return Segments.Count >= i ? Segments[i - 1] : null;
            }
            set
            {
                EnsureFullyParsed();
                for (int j = Segments.Count; j <= i - 1; j++)
                {
                    Segments.Add(new Hl7Segment());
                }
                Segments[i - 1] = (Hl7Segment) value;
            }
        }

        public static Hl7Message operator +(Hl7Message message, Hl7Segment segment)
        {
            message.EnsureFullyParsed();
            if (message.Segments.Count == 1 && message.Segments[0].Name == "MSH" && segment.Name == "MSH")
            {
                message.Segments.Clear();
            }
            message.Segments.Add(segment);
            return message;
        }

        public static Hl7Message operator +(Hl7Message message, string segment)
        {
            message += Hl7Segment.Parse(segment, message.Encoding);
            return message;
        }

        #endregion

        #region parsing

        public static Hl7Message Parse(string message)
        {
            return new Hl7Message(message);
        }

        public static Hl7Message Parse(string message, Hl7Encoding encoding)
        {
            return new Hl7Message(message, encoding);
        }

        protected override void FullyParse()
        {
            if (!string.IsNullOrEmpty(Value))
            {
                var segments = Value.Split(Encoding.SegmentSeparator, StringSplitOptions.None).ToList();
                segmentsInternal.Clear();
                foreach (var segment in segments)
                {
                    segmentsInternal.Add(Hl7Segment.Parse(segment, Encoding));
                }
            }
            Value = null;
            IsParsed = true;
        }

        #endregion

        public static string Fix(string message)
        {
            return Fix(message, new Hl7Encoding());
        }

        private static string Fix(string message, Hl7Encoding encoding)
        {
            message = FixNewlinesInSegment(message, encoding);
            return message;
        }

        private static string FixNewlinesInSegment(string message, Hl7Encoding encoding)
        {
            var segments = message.Split(encoding.SegmentSeparator, StringSplitOptions.None).ToList();
            StringBuilder builder = new StringBuilder();
            builder.Append(segments[0]);
            for (int i = 1; i < segments.Count; i++)
            {
                var segment = segments[i];
                if (segment.Length > 3 && segment[3] != encoding.FieldSeparator)
                {
                    builder.Append(encoding.EscapeAllCharacters(encoding.SegmentSeparator[0]));
                }
                else
                {
                    builder.Append(encoding.SegmentSeparator[0]);
                }
                builder.Append(segment);
            }
            return builder.ToString();
        }
    }
}
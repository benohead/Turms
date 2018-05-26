using System;
using System.Collections.Generic;
using System.Linq;

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
            Value = string.Empty;
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
                throw new ArgumentException("There should be exactly 4 separators defined in the MSH segment", nameof(message));
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
            return string.Join(Encoding.SegmentSeparator[0], Segments.Select(f => f.ToString()));
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
                    return segment;
                var index = int.Parse(queryParts[1]);
                return segment?[index];
            }
        }

        public static Hl7Message operator +(Hl7Message message, Hl7Segment segment)
        {
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
                    segmentsInternal.Add(Hl7Segment.Parse(segment, Encoding));
            }
            IsParsed = true;
        }

        #endregion
    }
}
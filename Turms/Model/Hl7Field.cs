using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Turms.Model
{
    public class Hl7Field: Hl7Element
    {
        private List<Hl7FieldRepetition> Repetitions { get; set; }

        private Hl7Field(string value, Hl7Encoding encoding): this(encoding)
        {
            Value = value.TrimEnd(encoding.ComponentSeparator);
        }

        public Hl7Field() : this(new Hl7Encoding())
        {
        }

        private Hl7Field(Hl7Encoding encoding)
        {
            Encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));
            Value = string.Empty;
            Repetitions = new List<Hl7FieldRepetition>();
        }

        public override string ToString()
        {
            EnsureFullyParsed();
            return string.Join(Encoding.RepetitionSeparator.ToString(), Repetitions.Select(f => f.ToString())).TrimEnd(Encoding.RepetitionSeparator);
        }

        public static Hl7Field Parse(string field)
        {
            return Parse(field, new Hl7Encoding());
        }
        public static Hl7Field Parse(string field, Hl7Encoding encoding)
        {
            return new Hl7Field(field, encoding);
        }

        protected override void FullyParse()
        {
            if (!string.IsNullOrEmpty(Value))
            {
                var repetitions = Value.TrimEnd(Encoding.RepetitionSeparator).Split(Encoding.RepetitionSeparator).ToList();
                Repetitions.Clear();
                foreach (var repetition in repetitions)
                    Repetitions.Add(Hl7FieldRepetition.Parse(repetition, Encoding));
                IsParsed = true;
            }
        }

        public Hl7FieldRepetition this[int i]
        {
            get
            {
                EnsureFullyParsed();
                return Repetitions.Count >= i ? Repetitions[i-1] : Hl7FieldRepetition.Parse("", Encoding);
            }
        }

        public static Hl7Field operator +(Hl7Field field, Hl7FieldRepetition repetition)
        {
            field.Repetitions.Add(repetition);
            return field;
        }

        public static Hl7Field operator +(Hl7Field field, string repetition)
        {
            field += Hl7FieldRepetition.Parse(repetition, field.Encoding);
            return field;
        }
    }
}
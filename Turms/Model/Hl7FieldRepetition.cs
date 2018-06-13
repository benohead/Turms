using System;
using System.Collections.Generic;
using System.Linq;

namespace Turms.Model
{
    public class Hl7FieldRepetition : Hl7Element
    {
        private readonly bool doNotEscape;

        private Hl7FieldRepetition(string value, Hl7Encoding encoding)
        {
            Value = value;
            Encoding = encoding;
            Components = new List<Hl7Component>();
        }

        private Hl7FieldRepetition(string value, Hl7Encoding encoding, bool doNotEscape) : this(value, encoding)
        {
            this.doNotEscape = doNotEscape;
        }

        public Hl7FieldRepetition(string value) : this(value, new Hl7Encoding())
        {
        }

        public Hl7FieldRepetition() : this("")
        {
        }

        private List<Hl7Component> Components { get; }

        public override Hl7Element this[int i]
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public static Hl7FieldRepetition Parse(string repetition, Hl7Encoding encoding, bool doNotEscape = false)
        {
            return new Hl7FieldRepetition(repetition, encoding, doNotEscape);
        }

        protected override void FullyParse()
        {
            if (Value == null || Value == "" && !doNotEscape)
                return;

            var trimmedValue = doNotEscape ? Value : Value.TrimEnd(Encoding.ComponentSeparator);
            var components = trimmedValue.Split(Encoding.ComponentSeparator)
                .ToList();
            Components.Clear();
            foreach (var component in components)
                Components.Add(Hl7Component.Parse(component, Encoding));

            IsParsed = true;
            Value = null;
        }

        public override string ToString()
        {
            EnsureFullyParsed();
            return string.Join(Encoding.ComponentSeparator.ToString(), Components.Select(c => c.ToString()))
                .TrimEnd(Encoding.RepetitionSeparator);
        }

        public string ToEscapedString()
        {
            EnsureFullyParsed();
            return string.Join(Encoding.ComponentSeparator.ToString(), Components.Select(c => c.ToEscapedString()))
                .TrimEnd(Encoding.ComponentSeparator);
        }
    }
}
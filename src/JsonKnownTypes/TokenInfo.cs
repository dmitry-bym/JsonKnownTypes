using System;
using System.Globalization;
using Newtonsoft.Json;

namespace JsonKnownTypes
{
    public class TokenInfo
    {
        public TokenInfo(JsonReader reader)
        {
            CloseInput = reader.CloseInput;
            SupportMultipleContent = reader.SupportMultipleContent;
            QuoteChar = reader.QuoteChar;
            DateTimeZoneHandling = reader.DateTimeZoneHandling;
            DateParseHandling = reader.DateParseHandling;
            FloatParseHandling = reader.FloatParseHandling;
            DateFormatString = reader.DateFormatString;
            MaxDepth = reader.MaxDepth;
            TokenType = reader.TokenType;
            Value = reader.Value;
            ValueType = reader.ValueType;
            Depth = reader.Depth;
            Path = reader.Path;
            Culture = reader.Culture;
        }
        
        public bool CloseInput { get; set; }

        public bool SupportMultipleContent { get; set; }

        public char QuoteChar { get; set; }

        public DateTimeZoneHandling DateTimeZoneHandling { get; set; }
        
        public DateParseHandling DateParseHandling { get; set; }

        public FloatParseHandling FloatParseHandling { get; set; }
        
        public string? DateFormatString { get; set; }

        public int? MaxDepth { get; set; }

        public JsonToken TokenType { get; set; }

        public object? Value { get; set; }

        public Type? ValueType { get; set; }

        public int Depth { get; set; }

        public string Path { get; set; }

        public CultureInfo Culture { get; set; }
    }
}

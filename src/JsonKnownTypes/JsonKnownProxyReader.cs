﻿using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JsonKnownTypes
{
    public class JsonKnownProxyReader : JsonReader
    {
        public readonly JsonReader JsonReader;
        public readonly CircuitQueue<TokenInfo> Buffer;
        public TokenInfo? Current { get; private set; }
        private bool _magikFlag;

        public JsonKnownProxyReader(JsonReader jsonReader)
        {
            JsonReader = jsonReader;
            Current = null;
            Buffer = new CircuitQueue<TokenInfo>();
        }

        public override void Close()
        {
            Current = null;
            JsonReader.Close();
        }

        public override async Task<bool> ReadAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            if (Buffer.Count == 0)
            {
                if (_magikFlag)
                    return await JsonReader.ReadAsync(cancellationToken);
                
                _magikFlag = true;
                Current = null;
                return true;
            }

            _magikFlag = false;
            Current = Buffer.Dequeue();
            return true;
        }

        public override bool Read()
        {
            if (Buffer.Count == 0)
            {
                if (_magikFlag)
                    return JsonReader.Read();
                
                Current = null;
                _magikFlag = true;
                return true;
            }
        
            _magikFlag = false;
            Current = Buffer.Dequeue();
            return true;
        }

        public void SetCursorToFirstInBuffer()
        {
            if(Current == null && Buffer.Count > 0)
                Read();
        }
        
        public bool ReadAndBuffer()
        {
            var t = new TokenInfo(JsonReader);
            Buffer.Enqueue(t);
            return JsonReader.Read();
        }

        public new bool CloseInput
        {
            get => Current?.CloseInput ?? JsonReader.CloseInput;
            set
            {
                if (Current == null)
                {
                    JsonReader.CloseInput = value;
                    return;
                }

                Current.CloseInput = value;
            }
        }

        public new bool SupportMultipleContent
        {
            get => Current?.SupportMultipleContent ?? JsonReader.SupportMultipleContent;
            set 
            {
                if (Current == null)
                {
                    JsonReader.SupportMultipleContent = value;
                    return;
                }

                Current.SupportMultipleContent = value;
            }
        }

        public override char QuoteChar
        {
            get => Current?.QuoteChar ?? JsonReader.QuoteChar;
        }

        public new DateTimeZoneHandling DateTimeZoneHandling
        {
            get => Current?.DateTimeZoneHandling ?? JsonReader.DateTimeZoneHandling;
            set 
            {
                if (Current == null)
                {
                    JsonReader.DateTimeZoneHandling = value;
                    return;
                }

                Current.DateTimeZoneHandling = value;
            }
        }

        public new DateParseHandling DateParseHandling
        {
            get => Current?.DateParseHandling ?? JsonReader.DateParseHandling;
            set 
            {
                if (Current == null)
                {
                    JsonReader.DateParseHandling = value;
                    return;
                }

                Current.DateParseHandling = value;
            }
        }

        public new FloatParseHandling FloatParseHandling
        {
            get => Current?.FloatParseHandling ?? JsonReader.FloatParseHandling;
            set 
            {
                if (Current == null)
                {
                    JsonReader.FloatParseHandling = value;
                    return;
                }

                Current.FloatParseHandling = value;
            }
        }

        public new string? DateFormatString
        {
            get => Current?.DateFormatString ?? JsonReader.DateFormatString;
            set 
            {
                if (Current == null)
                {
                    JsonReader.DateFormatString = value;
                    return;
                }

                Current.DateFormatString = value;
            }
        }

        public new int? MaxDepth
        {
            get => Current?.MaxDepth ?? JsonReader.MaxDepth;
            set 
            {
                if (Current == null)
                {
                    JsonReader.MaxDepth = value;
                    return;
                }

                Current.MaxDepth = value;
            }
        }

        public override JsonToken TokenType => Current?.TokenType ?? JsonReader.TokenType;

        public override object? Value => Current?.Value ??JsonReader.Value;

        public override Type? ValueType => Current?.ValueType ??JsonReader.ValueType;

        public override int Depth => Current?.Depth ??JsonReader.Depth;

        public override string Path => Current?.Path ??JsonReader.Path;

        public new CultureInfo Culture
        {
            get => Current?.Culture ?? JsonReader.Culture;
            set 
            {
                if (Current == null)
                {
                    JsonReader.Culture = value;
                    return;
                }

                Current.Culture = value;
            }
        }
    }
}

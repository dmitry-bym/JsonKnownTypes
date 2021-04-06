using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JsonKnownTypes
{
    public class TokenInfo
    {
        public TokenInfo()
        { }

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

    public class JsonKnownProxyReader : JsonReader
    {
        public readonly JsonReader JsonReader;
        private readonly Queue<TokenInfo> _tokenInfos;

        public TokenInfo Current;
        private bool flag1;

        public JsonKnownProxyReader(JsonReader jsonReader)
        {
            JsonReader = jsonReader;
            _tokenInfos = new Queue<TokenInfo>();
        }

        public void Dispose() => ((System.IDisposable) JsonReader).Dispose();

        public override async Task<bool> ReadAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            if (_tokenInfos.Count == 0)
            {
                if (flag1)
                    return await JsonReader.ReadAsync(cancellationToken);


                flag1 = true;
                Current = null;
                return true;
            }

            flag1 = false;
            Current = _tokenInfos.Dequeue();
            return true;
        }

        public new async Task SkipAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            if (_tokenInfos.Count == 0)
            {
                if (flag1)
                    await JsonReader.SkipAsync(cancellationToken);
                else
                {
                    flag1 = true;
                    Current = null;
                }
                return;
            }
            
            flag1 = false;
            Current = _tokenInfos.Dequeue();
        }

        public override Task<bool?> ReadAsBooleanAsync(CancellationToken cancellationToken = new CancellationToken()) =>
            JsonReader.ReadAsBooleanAsync(cancellationToken);

        public override Task<byte[]?> ReadAsBytesAsync(CancellationToken cancellationToken = new CancellationToken()) =>
            JsonReader.ReadAsBytesAsync(cancellationToken);

        public override Task<DateTime?> ReadAsDateTimeAsync(CancellationToken cancellationToken = new CancellationToken()) =>
            JsonReader.ReadAsDateTimeAsync(cancellationToken);

        public override Task<DateTimeOffset?> ReadAsDateTimeOffsetAsync(
            CancellationToken cancellationToken = new CancellationToken()) =>
            JsonReader.ReadAsDateTimeOffsetAsync(cancellationToken);

        public override Task<decimal?> ReadAsDecimalAsync(CancellationToken cancellationToken = new CancellationToken()) =>
            JsonReader.ReadAsDecimalAsync(cancellationToken);

        public override Task<double?> ReadAsDoubleAsync(CancellationToken cancellationToken = new CancellationToken()) =>
            JsonReader.ReadAsDoubleAsync(cancellationToken);

        public override Task<int?> ReadAsInt32Async(CancellationToken cancellationToken = new CancellationToken()) =>
            JsonReader.ReadAsInt32Async(cancellationToken);

        public override Task<string?> ReadAsStringAsync(CancellationToken cancellationToken = new CancellationToken()) =>
            JsonReader.ReadAsStringAsync(cancellationToken);

        public override bool Read()
        {
            if (_tokenInfos.Count == 0)
            {
                if (flag1)
                    return JsonReader.Read();
                
                Current = null;
                flag1 = true;
                return true;
            }

            flag1 = false;
            Current = _tokenInfos.Dequeue();
            return true;
        }

        public override int? ReadAsInt32() => JsonReader.ReadAsInt32();

        public override string? ReadAsString() => JsonReader.ReadAsString();

        public override byte[]? ReadAsBytes() => JsonReader.ReadAsBytes();

        public override double? ReadAsDouble() => JsonReader.ReadAsDouble();

        public override bool? ReadAsBoolean() => JsonReader.ReadAsBoolean();

        public override decimal? ReadAsDecimal() => JsonReader.ReadAsDecimal();

        public override DateTime? ReadAsDateTime() => JsonReader.ReadAsDateTime();

        public override DateTimeOffset? ReadAsDateTimeOffset() => JsonReader.ReadAsDateTimeOffset();
        
        private static bool IsStartToken(JsonToken token)
        {
            switch (token)
            {
                case JsonToken.StartObject:
                case JsonToken.StartArray:
                case JsonToken.StartConstructor:
                    return true;
                default:
                    return false;
            }
        }


        public new void Skip()
        {
            if (TokenType == JsonToken.PropertyName)
            {
                Read();
            }

            if (IsStartToken(TokenType))
            {
                int depth = Depth;

                while (Read() && (depth < Depth))
                {
                }
            }
        }

        public bool ReadAndBuffer()
        {
            var t = new TokenInfo(this);
            _tokenInfos.Enqueue(t);
            return JsonReader.Read();
        }

        public void SkipAndBuffer()
        {
            var t = new TokenInfo(this);
            _tokenInfos.Enqueue(t);
            JsonReader.Skip();
        }

        public override void Close() => JsonReader.Close();

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

        public bool ToBoolean(IFormatProvider provider) => ((IConvertible) CurrentState).ToBoolean(provider);

        public byte ToByte(IFormatProvider provider) => ((IConvertible) CurrentState).ToByte(provider);

        public char ToChar(IFormatProvider provider) => ((IConvertible) CurrentState).ToChar(provider);

        public DateTime ToDateTime(IFormatProvider provider) => ((IConvertible) CurrentState).ToDateTime(provider);

        public decimal ToDecimal(IFormatProvider provider) => ((IConvertible) CurrentState).ToDecimal(provider);

        public double ToDouble(IFormatProvider provider) => ((IConvertible) CurrentState).ToDouble(provider);

        public short ToInt16(IFormatProvider provider) => ((IConvertible) CurrentState).ToInt16(provider);

        public int ToInt32(IFormatProvider provider) => ((IConvertible) CurrentState).ToInt32(provider);

        public long ToInt64(IFormatProvider provider) => ((IConvertible) CurrentState).ToInt64(provider);

        public sbyte ToSByte(IFormatProvider provider) => ((IConvertible) CurrentState).ToSByte(provider);

        public float ToSingle(IFormatProvider provider) => ((IConvertible) CurrentState).ToSingle(provider);

        public object ToType(Type conversionType, IFormatProvider provider) =>
            ((IConvertible) CurrentState).ToType(conversionType, provider);

        public ushort ToUInt16(IFormatProvider provider) => ((IConvertible) CurrentState).ToUInt16(provider);

        public uint ToUInt32(IFormatProvider provider) => ((IConvertible) CurrentState).ToUInt32(provider);

        public ulong ToUInt64(IFormatProvider provider) => ((IConvertible) CurrentState).ToUInt64(provider);

        public int CompareTo(object target) => CurrentState.CompareTo(target);

        public TypeCode GetTypeCode() => CurrentState.GetTypeCode();

        public bool HasFlag(Enum flag) => CurrentState.HasFlag(flag);

        public string ToString(IFormatProvider provider) => CurrentState.ToString(provider);

        public string ToString(string format) => CurrentState.ToString(format);

        public string ToString(string format, IFormatProvider provider) => CurrentState.ToString(format, provider);
    }
}

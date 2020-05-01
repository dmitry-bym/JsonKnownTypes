using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JsonKnownTypes
{
    internal class JsonKnownProxyWriter : JsonWriter
    {
        private readonly string _fieldName;
        private readonly string _disc;
        private bool _discriminatorWritten;
        private readonly JsonWriter _writer;

        public JsonKnownProxyWriter(string fieldName, string disc, JsonWriter writer)
        {
            _discriminatorWritten = false;
            _fieldName = fieldName;
            _disc = disc;
            _writer = writer;
        }

        public override void WriteStartObject()
        {
            _writer.WriteStartObject();
            
            if (_discriminatorWritten == false)
            {
                _writer.WritePropertyName(_fieldName);
                _writer.WriteValue(_disc);
                _discriminatorWritten = true;
            }
        }

        public void Dispose() => ((IDisposable) _writer).Dispose();
        
        public override Task CloseAsync(CancellationToken cancellationToken = new CancellationToken()) => _writer.CloseAsync(cancellationToken);

        public override Task FlushAsync(CancellationToken cancellationToken = new CancellationToken()) => _writer.FlushAsync(cancellationToken);

        public override Task WriteRawAsync(string json, CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteRawAsync(json, cancellationToken);

        public override Task WriteEndAsync(CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteEndAsync(cancellationToken);

        public override Task WriteEndArrayAsync(CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteEndArrayAsync(cancellationToken);

        public override Task WriteEndConstructorAsync(CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteEndConstructorAsync(cancellationToken);

        public override Task WriteEndObjectAsync(CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteEndObjectAsync(cancellationToken);

        public override Task WriteNullAsync(CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteNullAsync(cancellationToken);

        public override Task WritePropertyNameAsync(string name, CancellationToken cancellationToken = new CancellationToken()) => _writer.WritePropertyNameAsync(name, cancellationToken);

        public override Task WritePropertyNameAsync(string name, bool escape, CancellationToken cancellationToken = new CancellationToken()) => _writer.WritePropertyNameAsync(name, escape, cancellationToken);

        public override Task WriteStartArrayAsync(CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteStartArrayAsync(cancellationToken);

        public override Task WriteCommentAsync(string text, CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteCommentAsync(text, cancellationToken);

        public override Task WriteRawValueAsync(string json, CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteRawValueAsync(json, cancellationToken);

        public override Task WriteStartConstructorAsync(string name, CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteStartConstructorAsync(name, cancellationToken);

        public override Task WriteStartObjectAsync(CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteStartObjectAsync(cancellationToken);

        public override Task WriteValueAsync(bool value, CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteValueAsync(value, cancellationToken);

        public override Task WriteValueAsync(bool? value, CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteValueAsync(value, cancellationToken);

        public override Task WriteValueAsync(byte value, CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteValueAsync(value, cancellationToken);

        public override Task WriteValueAsync(byte? value, CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteValueAsync(value, cancellationToken);

        public override Task WriteValueAsync(byte[] value, CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteValueAsync(value, cancellationToken);

        public override Task WriteValueAsync(char value, CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteValueAsync(value, cancellationToken);

        public override Task WriteValueAsync(char? value, CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteValueAsync(value, cancellationToken);

        public override Task WriteValueAsync(DateTime value, CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteValueAsync(value, cancellationToken);

        public override Task WriteValueAsync(DateTime? value, CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteValueAsync(value, cancellationToken);

        public override Task WriteValueAsync(DateTimeOffset value, CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteValueAsync(value, cancellationToken);

        public override Task WriteValueAsync(DateTimeOffset? value, CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteValueAsync(value, cancellationToken);

        public override Task WriteValueAsync(decimal value, CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteValueAsync(value, cancellationToken);

        public override Task WriteValueAsync(decimal? value, CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteValueAsync(value, cancellationToken);

        public override Task WriteValueAsync(double value, CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteValueAsync(value, cancellationToken);

        public override Task WriteValueAsync(double? value, CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteValueAsync(value, cancellationToken);

        public override Task WriteValueAsync(float value, CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteValueAsync(value, cancellationToken);

        public override Task WriteValueAsync(float? value, CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteValueAsync(value, cancellationToken);

        public override Task WriteValueAsync(Guid value, CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteValueAsync(value, cancellationToken);

        public override Task WriteValueAsync(Guid? value, CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteValueAsync(value, cancellationToken);

        public override Task WriteValueAsync(int value, CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteValueAsync(value, cancellationToken);

        public override Task WriteValueAsync(int? value, CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteValueAsync(value, cancellationToken);

        public override Task WriteValueAsync(long value, CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteValueAsync(value, cancellationToken);

        public override Task WriteValueAsync(long? value, CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteValueAsync(value, cancellationToken);

        public override Task WriteValueAsync(object value, CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteValueAsync(value, cancellationToken);

        public override Task WriteValueAsync(sbyte value, CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteValueAsync(value, cancellationToken);

        public override Task WriteValueAsync(sbyte? value, CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteValueAsync(value, cancellationToken);

        public override Task WriteValueAsync(short value, CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteValueAsync(value, cancellationToken);

        public override Task WriteValueAsync(short? value, CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteValueAsync(value, cancellationToken);

        public override Task WriteValueAsync(string value, CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteValueAsync(value, cancellationToken);

        public override Task WriteValueAsync(TimeSpan value, CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteValueAsync(value, cancellationToken);

        public override Task WriteValueAsync(TimeSpan? value, CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteValueAsync(value, cancellationToken);

        public override Task WriteValueAsync(uint value, CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteValueAsync(value, cancellationToken);

        public override Task WriteValueAsync(uint? value, CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteValueAsync(value, cancellationToken);

        public override Task WriteValueAsync(ulong value, CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteValueAsync(value, cancellationToken);

        public override Task WriteValueAsync(ulong? value, CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteValueAsync(value, cancellationToken);

        public override Task WriteValueAsync(Uri value, CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteValueAsync(value, cancellationToken);

        public override Task WriteValueAsync(ushort value, CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteValueAsync(value, cancellationToken);

        public override Task WriteValueAsync(ushort? value, CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteValueAsync(value, cancellationToken);

        public override Task WriteUndefinedAsync(CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteUndefinedAsync(cancellationToken);

        public override Task WriteWhitespaceAsync(string ws, CancellationToken cancellationToken = new CancellationToken()) => _writer.WriteWhitespaceAsync(ws, cancellationToken);

        public override void Flush() => _writer.Flush();

        public override void Close() => _writer.Close();
        
        public override void WriteEndObject() => _writer.WriteEndObject();

        public override void WriteStartArray() => _writer.WriteStartArray();

        public override void WriteEndArray() => _writer.WriteEndArray();

        public override void WriteStartConstructor(string name) => _writer.WriteStartConstructor(name);

        public override void WriteEndConstructor() => _writer.WriteEndConstructor();

        public override void WritePropertyName(string name) => _writer.WritePropertyName(name);

        public override void WritePropertyName(string name, bool escape) => _writer.WritePropertyName(name, escape);

        public override void WriteEnd() => _writer.WriteEnd();

        public override void WriteNull() => _writer.WriteNull();

        public override void WriteUndefined() => _writer.WriteUndefined();

        public override void WriteRaw(string json) => _writer.WriteRaw(json);

        public override void WriteRawValue(string json) => _writer.WriteRawValue(json);

        public override void WriteValue(string value) => _writer.WriteValue(value);

        public override void WriteValue(int value) => _writer.WriteValue(value);

        public override void WriteValue(uint value) => _writer.WriteValue(value);

        public override void WriteValue(long value) => _writer.WriteValue(value);

        public override void WriteValue(ulong value) => _writer.WriteValue(value);

        public override void WriteValue(float value) => _writer.WriteValue(value);

        public override void WriteValue(double value) => _writer.WriteValue(value);

        public override void WriteValue(bool value) => _writer.WriteValue(value);

        public override void WriteValue(short value) => _writer.WriteValue(value);

        public override void WriteValue(ushort value) => _writer.WriteValue(value);

        public override void WriteValue(char value) => _writer.WriteValue(value);

        public override void WriteValue(byte value) => _writer.WriteValue(value);

        public override void WriteValue(sbyte value) => _writer.WriteValue(value);

        public override void WriteValue(decimal value) => _writer.WriteValue(value);

        public override void WriteValue(DateTime value) => _writer.WriteValue(value);

        public override void WriteValue(DateTimeOffset value) => _writer.WriteValue(value);

        public override void WriteValue(Guid value) => _writer.WriteValue(value);

        public override void WriteValue(TimeSpan value) => _writer.WriteValue(value);

        public override void WriteValue(int? value) => _writer.WriteValue(value);

        public override void WriteValue(uint? value) => _writer.WriteValue(value);

        public override void WriteValue(long? value) => _writer.WriteValue(value);

        public override void WriteValue(ulong? value) => _writer.WriteValue(value);

        public override void WriteValue(float? value) => _writer.WriteValue(value);

        public override void WriteValue(double? value) => _writer.WriteValue(value);

        public override void WriteValue(bool? value) => _writer.WriteValue(value);

        public override void WriteValue(short? value) => _writer.WriteValue(value);

        public override void WriteValue(ushort? value) => _writer.WriteValue(value);

        public override void WriteValue(char? value) => _writer.WriteValue(value);

        public override void WriteValue(byte? value) => _writer.WriteValue(value);

        public override void WriteValue(sbyte? value) => _writer.WriteValue(value);

        public override void WriteValue(decimal? value) => _writer.WriteValue(value);

        public override void WriteValue(DateTime? value) => _writer.WriteValue(value);

        public override void WriteValue(DateTimeOffset? value) => _writer.WriteValue(value);

        public override void WriteValue(Guid? value) => _writer.WriteValue(value);

        public override void WriteValue(TimeSpan? value) => _writer.WriteValue(value);

        public override void WriteValue(byte[] value) => _writer.WriteValue(value);

        public override void WriteValue(Uri value) => _writer.WriteValue(value);

        public override void WriteValue(object value) => _writer.WriteValue(value);

        public override void WriteComment(string text) => _writer.WriteComment(text);

        public override void WriteWhitespace(string ws) => _writer.WriteWhitespace(ws);
    }
}

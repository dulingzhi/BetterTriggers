
using System;
using System.Globalization;
using System.IO;
using System.Text;
using War3Net.IO.Slk;

namespace BetterTriggers.Utility
{
    public sealed class SylkParser1
    {
        private SylkTable _table;
        private int? _lastY;

        public SylkParser1()
        {
            _lastY = null;
        }

        public SylkTable Parse(Stream input, bool leaveOpen = false)
        {
            using var reader = new StreamReader(input, Encoding.UTF8, true, 1024, leaveOpen);

            var isOnFirstLine = true;
            while (true)
            {
                var line = reader.ReadLine();
                var fields = line.Split(';');
                var recordType = fields[0];

                string GetField(string fieldName, bool mandatory)
                {
                    foreach (var field in fields)
                    {
                        if (field.StartsWith(fieldName))
                        {
                            return field.Substring(fieldName.Length);
                        }
                    }

                    if (mandatory)
                    {
                        throw new InvalidDataException($"Record does not contain mandatory field of type '{fieldName}'.");
                    }

                    return null;
                }

                if (isOnFirstLine)
                {
                    isOnFirstLine = false;
                    if (recordType != "ID")
                    {
                        throw new InvalidDataException("SYLK file must start with 'ID'.");
                    }

                    GetField("P", true);
                }
                else
                {
                    switch (recordType)
                    {
                        case "ID":
                            throw new InvalidDataException("Record type 'ID' can only occur on the first line.");

                        case "B":
                            if (_table != null)
                            {
                                throw new InvalidDataException("Only one record of type 'B' may be present.");
                            }

                            _table = new SylkTable(int.Parse(GetField("X", true)), int.Parse(GetField("Y", true)));
                            break;

                        case "C":
                            if (_table == null)
                            {
                                throw new InvalidDataException("Unable to parse record of type 'C' before encountering a record of type 'B'.");
                            }

                            var value = GetField("K", false);
                            if (value == null)
                            {
                                continue;
                            }

                            var x = GetField("X", false);
                            if (x == null)
                                continue;

                            SetCellContent(x, GetField("Y", false), value);
                            break;

                        case "E":
                            return _table;

                        default:
                            //throw new NotSupportedException($"Support for record type '{recordType}' is not implemented. Only records of type 'ID', 'B', 'C', and 'E' are supported.");
                            break;
                    }
                }
            }
        }

        /// <param name="x">The cell's 1-indexed X position.</param>
        /// <param name="y">The cell's 1-indexed Y position.</param>
        private void SetCellContent(string x, string? y, string value)
        {
            if (y == null && _lastY == null)
            {
                //throw new InvalidDataException("Row for cell is not defined.");
                return;
            }

            var xi = int.Parse(x, NumberStyles.Integer, CultureInfo.InvariantCulture) - 1;
            var yi = y == null ? _lastY.Value : (int.Parse(y, NumberStyles.Integer, CultureInfo.InvariantCulture) - 1);

            if (value.StartsWith('"') && value.EndsWith('"'))
            {
                _table[xi, yi] = value[1..^1];
            }
            else if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var @int))
            {
                _table[xi, yi] = @int;
            }
            else if (float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var @float))
            {
                _table[xi, yi] = @float;
            }
            else if (string.Equals(value, bool.TrueString, StringComparison.OrdinalIgnoreCase))
            {
                _table[xi, yi] = true;
            }
            else if (string.Equals(value, bool.FalseString, StringComparison.OrdinalIgnoreCase))
            {
                _table[xi, yi] = false;
            }
            else if (string.Equals(value, "#VALUE!", StringComparison.Ordinal) || string.Equals(value, "#REF!", StringComparison.Ordinal))
            {
                _table[xi, yi] = 0;
            }
            else
            {
                throw new NotSupportedException($"Unable to parse value '{value}'. Can only parse strings, integers, floats, and booleans.");
            }

            _lastY = yi;
        }
    }
}
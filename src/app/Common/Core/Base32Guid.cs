using System;
using System.Text;

namespace Clinic.Common.Core
{
    /// <summary>
    /// Class used for conversion between byte array and Base32Guid notation
    /// </summary>
    public struct Base32Guid
    {
        internal const string CODE_CHARS = "12345678abcdefghijklmnpqrstuvwxy";
        internal const byte BIT5 = 0x5;
        internal const byte BIT8 = 0x8;
        private static char[] char_table;
          
         #region Static

        /// <summary>
        /// A read-only instance of the Base32Guid class whose value
        /// is guaranteed to be all zeroes.
        /// </summary>
        public static readonly Base32Guid Empty = new Base32Guid(Guid.Empty);

        #endregion

        #region Fields

        Guid _guid;
        string _value;

        #endregion

        #region Contructors

        /// <summary>
        /// Creates a Base32Guid from a base64 encoded string
        /// </summary>
        /// <param name="value">The encoded guid as a
        /// base64 string</param>
        public Base32Guid(string value)
        {
            _value = value;
            _guid = Decode(value);
        }

        /// <summary>
        /// Creates a Base32Guid from a Guid
        /// </summary>
        /// <param name="guid">The Guid to encode</param>
        public Base32Guid(Guid guid)
        {
            _value = Encode(guid);
            _guid = guid;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets/sets the underlying Guid
        /// </summary>
        public Guid Guid
        {
            get { return _guid; }
            set
            {
                if (value != _guid)
                {
                    _guid = value;
                    _value = Encode(value);
                }
            }
        }

        /// <summary>
        /// Gets/sets the underlying base64 encoded string
        /// </summary>
        public string Value
        {
            get { return _value; }
            set
            {
                if (value != _value)
                {
                    _value = value;
                    _guid = Decode(value);
                }
            }
        }

        #endregion

        #region ToString

        /// <summary>
        /// Returns the base64 encoded guid as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _value;
        }

        #endregion

        #region Equals

        /// <summary>
        /// Returns a value indicating whether this instance and a
        /// specified Object represent the same type and value.
        /// </summary>
        /// <param name="obj">The object to compare</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is Base32Guid)
                return _guid.Equals(((Base32Guid)obj)._guid);
            if (obj is Guid)
                return _guid.Equals((Guid)obj);
            if (obj is string)
                return _guid.Equals(((Base32Guid)obj)._guid);
            return false;
        }

        #endregion

        #region GetHashCode

        /// <summary>
        /// Returns the HashCode for underlying Guid.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return _guid.GetHashCode();
        }

        #endregion

        #region NewGuid

        /// <summary>
        /// Initialises a new instance of the Base32Guid class
        /// </summary>
        /// <returns></returns>
        public static Base32Guid NewGuid()
        {
            return new Base32Guid(Guid.NewGuid());
        }

        #endregion

        #region Encode

        /// <summary>
        /// Creates a new instance of a Guid using the string value,
        /// then returns the base64 encoded version of the Guid.
        /// </summary>
        /// <param name="value">An actual Guid string (i.e. not a Base32Guid)</param>
        /// <returns></returns>
        public static string Encode(string value)
        {
            Guid guid = new Guid(value);
            return Encode(guid);
        }

        /// <summary>
        /// Encodes the given Guid as a base64 string that is 22
        /// characters long.
        /// </summary>
        /// <param name="guid">The Guid to encode</param>
        /// <returns></returns>
        public static string Encode(Guid guid)
        {
            //string encoded = Convert.ToBase64String(guid.ToByteArray());
            //encoded = encoded
            //  .Replace("/", "_")
            //  .Replace("+", ",");
            //return encoded.Substring(0, 22);
            return Encode(guid.ToString(), Encoding.ASCII, true);
        }

        public static string Encode(string str, Encoding encoding, bool ignoreCase)
        {
            if (String.IsNullOrEmpty(str))
            {
                return "";
            }
            if (ignoreCase)
            {
                str = str.ToLower();
            }
            StringBuilder binaryString = new StringBuilder();
            byte[] strBytes = encoding.GetBytes(str);
            foreach (byte b in strBytes)
            {
                binaryString.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            }
            int blocks = binaryString.Length % BIT5 == 0 ? binaryString.Length / BIT5 : binaryString.Length / BIT5 + 1;
            string[] bit5Array = new string[blocks];
            int readSize = 0;
            for (int i = 0; i < blocks; i++)
            {
                readSize = BIT5;
                if (i * BIT5 + BIT5 > binaryString.Length)
                {
                    readSize = binaryString.Length - i * BIT5;
                }
                bit5Array[i] = binaryString.ToString(i * BIT5, readSize);
                bit5Array[i] = bit5Array[i].PadRight(BIT5, '0');
                bit5Array[i] = bit5Array[i].PadLeft(BIT8, '0');
            }
            byte[] encodedBytes = new byte[bit5Array.Length];
            for (int i = 0; i < bit5Array.Length; i++)
            {
                encodedBytes[i] = Convert.ToByte(bit5Array[i], 2);
            }
            if (char_table == null)
            {
                char_table = CODE_CHARS.ToCharArray();
            }
            StringBuilder encodedString = new StringBuilder();
            foreach (byte b in encodedBytes)
            {
                encodedString.Append(char_table[b]);
            }
            return encodedString.ToString();
        }

        #endregion

        #region Decode

        /// <summary>
        /// Decodes the given base64 string
        /// </summary>
        /// <param name="value">The base64 encoded string of a Guid</param>
        /// <returns>A new Guid</returns>
        public static Guid Decode(string value)
        {
            if (string.IsNullOrEmpty(value)) return Guid.Empty;

            //value = value.Replace("_", "/").Replace(",", "+");

            try
            {
                //byte[] buffer = Convert.FromBase64String(value + "==");
                var buffer = Decode(value, Encoding.ASCII);
                return new Guid(buffer);
            }
            catch (Exception)
            { }
            return Guid.Empty;
        }

        public static string Decode(string str, Encoding encoding)
        {
            if (String.IsNullOrEmpty(str))
            {
                return "";
            }
            str = str.ToLower();
            StringBuilder binaryString = new StringBuilder();
            foreach (char c in str.ToCharArray())
            {
                binaryString.Append(Convert.ToString(CODE_CHARS.IndexOf(c), 2).PadLeft(BIT8, '0'));
            }
            int n = binaryString.Length / BIT8;
            string[] bit8Array = new string[n];
            for (int i = 0; i < n; i++)
            {
                bit8Array[i] = binaryString.ToString(i * BIT8, BIT8).Substring(BIT8 - BIT5);
            }
            string bit8String = String.Join("", bit8Array);
            bit8Array = new string[bit8String.Length / BIT8];
            for (int i = 0; i < bit8Array.Length; i++)
            {
                bit8Array[i] = bit8String.Substring(i * BIT8, BIT8);
            }
            byte[] decodedBytes = new byte[bit8Array.Length];
            for (int i = 0; i < decodedBytes.Length; i++)
            {
                decodedBytes[i] = Convert.ToByte(bit8Array[i], 2);
            }
            return encoding.GetString(decodedBytes);
        }

        #endregion

        #region Operators

        /// <summary>
        /// Determines if both Base32Guids have the same underlying
        /// Guid value.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator ==(Base32Guid x, Base32Guid y)
        {
            if ((object)x == null) return (object)y == null;
            return x._guid == y._guid;
        }

        /// <summary>
        /// Determines if both Base32Guids do not have the
        /// same underlying Guid value.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator !=(Base32Guid x, Base32Guid y)
        {
            return !(x == y);
        }

        /// <summary>
        /// Implicitly converts the Base32Guid to it's string equivilent
        /// </summary>
        /// <param name="Base32Guid"></param>
        /// <returns></returns>
        public static implicit operator string(Base32Guid Base32Guid)
        {
            return Base32Guid._value;
        }

        /// <summary>
        /// Implicitly converts the Base32Guid to it's Guid equivilent
        /// </summary>
        /// <param name="Base32Guid"></param>
        /// <returns></returns>
        public static implicit operator Guid(Base32Guid Base32Guid)
        {
            return Base32Guid._guid;
        }

        /// <summary>
        /// Implicitly converts the string to a Base32Guid
        /// </summary>
        /// <param name="Base32Guid"></param>
        /// <returns></returns>
        public static implicit operator Base32Guid(string Base32Guid)
        {
            return new Base32Guid(Base32Guid);
        }

        /// <summary>
        /// Implicitly converts the Guid to a Base32Guid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static implicit operator Base32Guid(Guid guid)
        {
            return new Base32Guid(guid);
        }

        #endregion
    }
}

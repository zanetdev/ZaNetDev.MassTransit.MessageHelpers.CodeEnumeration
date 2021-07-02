using System;

namespace ZaNetDev.MassTransit.MessageHelpers.CodeEnumeration
{
    public abstract partial class CodeEnumeration<TEnum> : IComparable
    {
        protected CodeEnumeration()
        {
        }

        private int? _requestedHashCode;
        private string _code;

        public string Code
        {
            get => _code;
            protected set => _code = value;
        }

        public override bool Equals(object obj)
        {
            var otherValue = obj as CodeEnumeration<TEnum>;
            if (otherValue == null) return false;
            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Code.Equals(otherValue.Code);
            return typeMatches && valueMatches;
        }
        
        public override int GetHashCode()
        {
            if (!_requestedHashCode.HasValue)
                _requestedHashCode = this.Code.GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

            return _requestedHashCode.Value;
        }

        public static bool operator == (CodeEnumeration<TEnum> left, CodeEnumeration<TEnum> right)
        {
            return 
                Equals(left, null) 
                    ? Equals(right, null) 
                    : left.Equals(right);
        }

        public static bool operator !=(CodeEnumeration<TEnum> left, CodeEnumeration<TEnum> right)
        {
            return !(left == right);
        }
        
        public int CompareTo(object other) => Code.CompareTo(((CodeEnumeration<TEnum>)other).Code);
    }
}
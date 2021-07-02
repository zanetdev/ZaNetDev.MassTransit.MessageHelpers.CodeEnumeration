using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ZaNetDev.MassTransit.MessageHelpers.CodeEnumeration
{
    public abstract partial class CodeEnumeration<TEnum>  where TEnum : CodeEnumeration<TEnum>
    {   
        public static CodeEnumeration<TEnum> Type { get; }
        
        protected CodeEnumeration(string code)
        {
            Code = code?.ToLower();
        }

        public override string ToString() => Code;
        
        public static IEnumerable<string> GetAllCodes()
        {
            return GetAll().Select(c => c.Code.ToLower()).ToList();
        }
        
        public static IEnumerable<TEnum> GetAll()
        {
            var fields = typeof(TEnum)
                .GetFields(
                    BindingFlags.Public | 
                    BindingFlags.Static | 
                    BindingFlags.DeclaredOnly
                )
                .ToList() ;
            var matchingTypeFields = fields.Where(f => f.FieldType == typeof(TEnum)).ToList();
            
            return matchingTypeFields.Select(f => f.GetValue(null)).Cast<TEnum>();
        }
        
        public static bool TryGetFromCode(string code, out TEnum item) 
        {
            try
            {
                item = FromCode(code);
                return true;
            }
            catch (InvalidOperationException e)
            {
                item = null;
                return false;
            }
        }
        
        public static TEnum FromCode(string code) 
        {
            var matchingItem = Parse<string>(code, "code", item => item.Code.ToLower() == code.ToLower());
            return matchingItem;
        }
        
        public static TEnum FromCodeOrDefault(string code)
        {
            if (code == null) return null;
            
            try
            {
                return FromCode(code);
            }
            catch (InvalidOperationException e)
            {
                return null;
            }
        }

        private static TEnum Parse<K>(K value, string description, Func<TEnum, bool> predicate)
        {
            var matchingItem = GetAll().FirstOrDefault(predicate);

            if (matchingItem == null)
                throw new InvalidOperationException($"'{value}' is not a valid {description} in {typeof(TEnum)}");

            return matchingItem;
        }

        
    }
}
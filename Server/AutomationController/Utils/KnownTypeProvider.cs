// ----------------------------------------------------------------------
// <copyright file="KnownTypeProvider.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WindowsPhoneTestFramework.AutomationController.Utils
{
    // KnownTypeProvider based on http://davybrion.com/blog/2009/11/requestresponse-service-layer-exposing-the-service-layer-through-wcf/
    public static class KnownTypeProvider
    {
        private static HashSet<Type> _knownTypes = new HashSet<Type>();

        public static void ClearAllKnownTypes()
        {
            _knownTypes = new HashSet<Type>();
        }

        public static void Register<T>()
        {
            Register(typeof(T));
        }

        public static void Register(Type type)
        {
            _knownTypes.Add(type);
        }

        public static void RegisterDerivedTypesOf<T>(Assembly assembly)
        {
            RegisterDerivedTypesOf(typeof(T), assembly);
        }

        public static void RegisterDerivedTypesOf<T>(IEnumerable<Type> types)
        {
            RegisterDerivedTypesOf(typeof(T), types);
        }

        public static void RegisterDerivedTypesOf(Type type, Assembly assembly)
        {
            RegisterDerivedTypesOf(type, assembly.GetTypes());
        }

        public static void RegisterDerivedTypesOf(Type type, IEnumerable<Type> types)
        {
            _knownTypes.UnionWith(GetDerivedTypesOf(type, types));
        }

        public static IEnumerable<Type> GetKnownTypes(ICustomAttributeProvider provider)
        {
            return _knownTypes;
        }

        private static IEnumerable<Type> GetDerivedTypesOf(Type baseType, IEnumerable<Type> types)
        {
            return types.Where(t => !t.IsAbstract && t.IsSubclassOf(baseType));
        }
    }    
}
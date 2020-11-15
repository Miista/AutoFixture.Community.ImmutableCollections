using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;

// ReSharper disable once CheckNamespace
namespace AutoFixture
{
    internal class ImmutableCollectionRelay : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var type = request as Type;
            if (type == null)
                return new NoSpecimen();

            var typeArguments = type.GetTypeInfo().GetGenericArguments();
            if (typeArguments.Length < 1)
                return new NoSpecimen();

            var innerType = typeArguments.FirstOrDefault();
            if (innerType == null)
                return new NoSpecimen();

            var typeDefinition = type.GetGenericTypeDefinition();

            if (typeDefinition == typeof(ImmutableList<>)) return CreateImmutableList(innerType, context);
            if (typeDefinition == typeof(IImmutableList<>)) return CreateImmutableList(innerType, context);
            if (typeDefinition == typeof(ImmutableQueue<>)) return CreateImmutableQueue(innerType, context);
            if (typeDefinition == typeof(IImmutableQueue<>)) return CreateImmutableQueue(innerType, context);
            if (typeDefinition == typeof(ImmutableArray<>)) return CreateImmutableArray(innerType, context);
            if (typeDefinition == typeof(ImmutableStack<>)) return CreateImmutableStack(innerType, context);
            if (typeDefinition == typeof(IImmutableStack<>)) return CreateImmutableStack(innerType, context);
            if (typeDefinition == typeof(ImmutableHashSet<>)) return CreateImmutableHashSet(innerType, context);
            if (typeDefinition == typeof(IImmutableSet<>)) return CreateImmutableHashSet(innerType, context);
            if (typeDefinition == typeof(ImmutableSortedSet<>)) return CreateImmutableSortedSet(innerType, context);

            var valueType = typeArguments.ElementAtOrDefault(1);
            if (valueType == null)
                return new NoSpecimen();
            
            if (typeDefinition == typeof(IImmutableDictionary<,>)) return CreateImmutableDictionary(innerType, valueType, context);
            if (typeDefinition == typeof(ImmutableDictionary<,>)) return CreateImmutableDictionary(innerType, valueType, context);
            if (typeDefinition == typeof(ImmutableSortedDictionary<,>)) return CreateImmutableSortedDictionary(innerType, valueType, context);

            return new NoSpecimen();
        }

        private static object CreateImmutableList(Type resultType, ISpecimenContext context)
            => CreateImmutableCollection(resultType, context, 
                collectionType: typeof(ImmutableList),
                createMethodName: nameof(ImmutableList.CreateRange));
        
        private static object CreateImmutableQueue(Type resultType, ISpecimenContext context)
            => CreateImmutableCollection(resultType, context, 
                collectionType: typeof(ImmutableQueue),
                createMethodName: nameof(ImmutableQueue.CreateRange));

        private static object CreateImmutableArray(Type resultType, ISpecimenContext context)
            => CreateImmutableCollection(resultType, context, 
                collectionType: typeof(ImmutableArray),
                createMethodName: nameof(ImmutableArray.CreateRange));

        private static object CreateImmutableStack(Type resultType, ISpecimenContext context)
            => CreateImmutableCollection(resultType, context, 
                collectionType: typeof(ImmutableStack),
                createMethodName: nameof(ImmutableStack.CreateRange));
        
        private static object CreateImmutableHashSet(Type resultType, ISpecimenContext context)
            => CreateImmutableCollection(resultType, context, 
                collectionType: typeof(ImmutableHashSet),
                createMethodName: nameof(ImmutableHashSet.CreateRange));
        
        private static object CreateImmutableSortedSet(Type resultType, ISpecimenContext context)
            => CreateImmutableCollection(resultType, context, 
                collectionType: typeof(ImmutableSortedSet),
                createMethodName: nameof(ImmutableSortedSet.CreateRange));
        
        private static object CreateImmutableDictionary(Type keyType, Type valueType, ISpecimenContext context)
            => CreateImmutableDictionary(keyType, valueType, context,
                collectionType: typeof(ImmutableDictionary),
                createMethodName: nameof(ImmutableDictionary.CreateRange));
        
        private static object CreateImmutableSortedDictionary(Type keyType, Type valueType, ISpecimenContext context)
            => CreateImmutableDictionary(keyType, valueType, context,
                collectionType: typeof(ImmutableSortedDictionary),
                createMethodName: nameof(ImmutableSortedDictionary.CreateRange));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyType"></param>
        /// <param name="resultType"></param>
        /// <param name="context"></param>
        /// <param name="collectionType"></param>
        /// <param name="createMethodName">The method must take either a params or a list of Ts</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private static object CreateImmutableDictionary(
            Type keyType,
            Type resultType,
            ISpecimenContext context,
            Type collectionType,
            string createMethodName)
        {
            if (keyType == null) throw new ArgumentNullException(nameof(keyType));
            if (resultType == null) throw new ArgumentNullException(nameof(resultType));
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (collectionType == null) throw new ArgumentNullException(nameof(collectionType));
            if (createMethodName == null) throw new ArgumentNullException(nameof(createMethodName));

            // Because we are constructing a dictionary, it must be a list of KeyValuePairs.
            var targetType = typeof(KeyValuePair<,>).MakeGenericType(keyType, resultType);

            var creationMethod = GetCreateMethod(createMethodName, collectionType, targetType)
                .MakeGenericMethod(keyType, resultType);

            var elements = context.Resolve(typeof(List<>).MakeGenericType(targetType));
            
            return creationMethod.Invoke(null, new[] {elements});            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resultType"></param>
        /// <param name="context"></param>
        /// <param name="collectionType"></param>
        /// <param name="createMethodName">The method must take either a params or a list of Ts</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private static object CreateImmutableCollection(
            Type resultType,
            ISpecimenContext context,
            Type collectionType,
            string createMethodName)
        {
            if (resultType == null) throw new ArgumentNullException(nameof(resultType));
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (collectionType == null) throw new ArgumentNullException(nameof(collectionType));
            if (createMethodName == null) throw new ArgumentNullException(nameof(createMethodName));

            var creationMethod = GetCreateMethod(createMethodName, collectionType, resultType)
                .MakeGenericMethod(resultType);

            var elements = context.Resolve(typeof(List<>).MakeGenericType(resultType));
            
            return creationMethod.Invoke(null, new[] {elements});            
        }

        private static MethodInfo GetCreateMethod(
            string createMethodName,
            Type collectionType,
            Type resultType)
        {
            return collectionType
                       .GetMethods(BindingFlags.Public | BindingFlags.Static)
                       .Where(m => m.Name == createMethodName)
                       .Where(m => m.GetParameters().Length == 1)
                       .Where(m => FirstParameterIsEnumerableOfType(resultType, m))
                       .ToList()
                       .FirstOrDefault()
                   ?? throw new InvalidOperationException(
                       $"Could not find method '{createMethodName}' on type '{collectionType}'");
        }

        private static bool FirstParameterIsEnumerableOfType(Type type, MethodInfo methodInfo)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));

            var genericTargetType = typeof(IEnumerable<>).GetGenericTypeDefinition().MakeGenericType(type);

            var genericTypeDefinition = methodInfo
                                            .GetParameters()
                                            .SingleOrDefault()
                                            ?.ParameterType
                                            ?.GetGenericTypeDefinition()
                                        ?? throw new InvalidOperationException(
                                            "Could not get generic type definition for method.");

            // We can only handle generic types with 1 type argument
            if (genericTypeDefinition.GetGenericArguments().Length != 1) return false;

            var genericParameterType = genericTypeDefinition.MakeGenericType(type);

            return genericParameterType == genericTargetType;
        }
    }
}
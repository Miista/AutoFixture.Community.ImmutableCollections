using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;

namespace AutoFixture.Community.ImmutableCollections
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

        /// <summary>Creates an immutable dictionary of the specified type.</summary>
        /// <param name="keyType">The type for the dictionary keys.</param>
        /// <param name="resultType">The type for the dictionary values.</param>
        /// <param name="context">The specimen context.</param>
        /// <param name="collectionType">The type of the dictionary.</param>
        /// <param name="createMethodName">The name of the method used to create the collection. The method must take either a params or a list of Ts</param>
        /// <returns>A collection of type <paramref name="collectionType"/>.</returns>
        /// <remarks>The create method for the collection (parameter: <paramref name="createMethodName"/>) must take either:
        ///     <list type="bullet">
        ///         <item>
        ///             <description>A params of <c>T</c>s, or</description>
        ///         </item>
        ///         <item>
        ///             <description>A list of <c>T</c>s</description>
        ///         </item>
        ///     </list>
        ///     where <c>T</c> is <paramref name="resultType"/>.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Any of the arguments are null.</exception>
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
        /// Creates an immutable collection of the specified type.
        /// The elements in the collection are of type <paramref name="resultType"/>.
        /// </summary>
        /// <param name="resultType">The type of elements that the collection should contain.</param>
        /// <param name="context">The specimen context.</param>
        /// <param name="collectionType">The type of the collection.</param>
        /// <param name="createMethodName">The name of the method used to create the collection. The method must take either a params or a list of Ts.</param>
        /// <returns>A collection of type <paramref name="collectionType"/> containing elements of type <paramref name="resultType"/>.</returns>
        /// <remarks>The create method for the collection (parameter: <paramref name="createMethodName"/>) must take either:
        ///     <list type="bullet">
        ///         <item>
        ///             <description>A params of <c>T</c>s, or</description>
        ///         </item>
        ///         <item>
        ///             <description>A list of <c>T</c>s</description>
        ///         </item>
        ///     </list>
        ///     where <c>T</c> is <paramref name="resultType"/>.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Any of the arguments are null.</exception>
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

        /// <summary>
        /// Determines if the first parameter to <paramref name="methodInfo"/> is an <see cref="IEnumerable{T}">IEnumerable</see> specialized to type <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type that the <see cref="IEnumerable{T}">IEnumerable</see> should be specialized to.</param>
        /// <param name="methodInfo">The method whose first parameter to inspect.</param>
        /// <returns>
        /// <see langword="true">true</see> if the first parameter of <paramref name="methodInfo"/> is an enumerable of type <paramref name="type"/>; otherwise, <see langword="false">false</see>.</returns>
        /// <exception cref="ArgumentNullException">Any of the arguments are null.</exception>
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
using System;
using System.Linq;
using System.Reflection;
using AutoTrade.Tests.Properties;

namespace AutoTrade.Tests
{
    public static class TestingExtensions
    {
        #region Methods

        /// <summary>
        /// Invokes a static method with void return type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="methodName"></param>
        /// <param name="parameters"></param>
        public static void InvokeStaticMethod(this Type type, string methodName, params object[] parameters)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            Invoke(() => GetMethod(type, methodName, true, typeof(void), parameters).Invoke(null, parameters));
        }

        /// <summary>
        /// Invokes a private static method with return type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="methodName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static T InvokeStaticMethod<T>(this Type type, string methodName, params object[] parameters)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            return Invoke(() => (T)GetMethod(type, methodName, true, typeof(T), parameters).Invoke(null, parameters));
        }

        /// <summary>
        /// Invokes a private instance method on a object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="methodName"></param>
        /// <param name="parameters"></param>
        public static void InvokeMethod(this object obj, string methodName, params object[] parameters)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            Invoke(() => GetMethod(obj.GetType(), methodName, false, typeof(void), parameters).Invoke(obj, parameters));
        }

        /// <summary>
        /// Invokes a private instance method on a object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="methodName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static T InvokeMethod<T>(this object obj, string methodName, params object[] parameters)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            return Invoke(() => (T)GetMethod(obj.GetType(), methodName, false, typeof(T), parameters).Invoke(obj, parameters));
        }

        /// <summary>
        /// Gets a method on a type that matches based on name, return type, and parameters
        /// </summary>
        /// <param name="type"></param>
        /// <param name="methodName"></param>
        /// <param name="isStatic"></param>
        /// <param name="returnType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static MethodInfo GetMethod(Type type, string methodName, bool isStatic, Type returnType, params object[] parameters)
        {
            // get flag for instance or static
            var instanceOrStaticFlag = isStatic ? BindingFlags.Static : BindingFlags.Instance;

            // get methods that match based on the method name, parameters, and return type
            var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | instanceOrStaticFlag)
                              .Where(m =>
                                  {
                                      // check that the method name matches
                                      if (m.Name != methodName)
                                          return false;

                                      // check that the return type matches
                                      if (returnType != m.ReturnType)
                                          return false;

                                      // get the list of parameters and check that the number matches
                                      var methodParams = m.GetParameters();
                                      if (methodParams.Length != parameters.Length)
                                          return false;
                                      
                                      // check that parameters match on type
                                      if (parameters.Where((t, i) => (t == null && methodParams[i].ParameterType.IsValueType) ||
                                                                     (t != null && t.GetType() != methodParams[i].ParameterType)).Any())
                                          return false;

                                      // all criteria passed - return true
                                      return true;
                                  } ).ToList();

            // check that at least one method was found
            if (methods.Count == 0)
                throw new MissingMethodException(type.FullName, methodName);

            // check that no more than 1 method was found
            if (methods.Count > 1)
                throw new AmbiguousMatchException(string.Format(Resources.AmbiguousMethodExceptionFormat, type.FullName, methodName));

            return methods[0];
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the value of a static property of a type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static T GetStaticPropertyValue<T>(this Type type, string propertyName)
        {
            // check that the type is set
            if (type == null)
                throw new ArgumentNullException("type");

            return Invoke(() => (T)GetPropertyForRead(type, propertyName, typeof(T)).GetValue(null, null));
        }

        /// <summary>
        /// Gets the value of a static property of a type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void SetStaticPropertyValue(this Type type, string propertyName, object value)
        {
            // check that the type is set
            if (type == null)
                throw new ArgumentNullException("type");

            // set the value
            Invoke(() => GetPropertyForWrite(type, propertyName, true, value).SetValue(null, value, null));
        }

        /// <summary>
        /// Gets the value of a property on an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static T GetPropertyValue<T>(this object obj, string propertyName)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            return Invoke(() => (T)GetPropertyForRead(obj.GetType(), propertyName, typeof(T)).GetValue(obj, null));
        }

        /// <summary>
        /// Sets the value of a property on an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public static void SetPropertyValue<T>(this object obj, string propertyName, T value)
        {
            // check that the type is set
            if (obj == null)
                throw new ArgumentNullException("obj");

            // set the value
            Invoke(() => GetPropertyForWrite(obj.GetType(), propertyName, true, value).SetValue(obj, value, null));
        }

        /// <summary>
        /// Gets a property and validates that it is readable
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        private static PropertyInfo GetPropertyForRead(Type type, string propertyName, Type propertyType)
        {
            // get the property
            var property = GetProperty(type, propertyName, true);

            // check that the property supports reading (i.e. has a getter)
            if (!property.CanRead)
                throw new InvalidOperationException(string.Format(Resources.PropertyCannotBeReadFormat, type.FullName, propertyName));

            // check that types match
            if (!propertyType.IsAssignableFrom(property.PropertyType))
                throw new InvalidOperationException(string.Format(Resources.PropertyCannotBeCastFormat,
                    type.FullName,
                    propertyName,
                    propertyType));

            return property;
        }

        /// <summary>
        /// Gets a property and validates that it is writable
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyName"></param>
        /// <param name="isStatic"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static PropertyInfo GetPropertyForWrite(Type type, string propertyName, bool isStatic, object value)
        {
            // get the property
            var property = GetProperty(type, propertyName, isStatic);

            // check that the property supports reading (i.e. has a getter)
            if (!property.CanWrite)
                throw new InvalidOperationException(string.Format(Resources.PropertyCannotBeReadFormat, type.FullName, propertyName));

            // if the value is null, ensure property accepts nulls 
            if (value == null && property.PropertyType.IsValueType)
                throw new InvalidOperationException(string.Format(Resources.PropertyCannotBeSetToNullFormat,
                    type.FullName,
                    propertyName));

            // if the value is set, check that the property can be set from its type
            if (value != null && !property.PropertyType.IsInstanceOfType(value))
                throw new InvalidOperationException(string.Format(Resources.PropertyCannotBeSetFormat,
                    type.FullName,
                    propertyName,
                    value.GetType().FullName));

            return property;
        }

        /// <summary>
        /// Gets a property for a type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyName"></param>
        /// <param name="isStatic"></param>
        /// <returns></returns>
        private static PropertyInfo GetProperty(Type type, string propertyName, bool isStatic)
        {
            // get flag indicating whether instance or static
            var instanceOrStaticFlag = isStatic ? BindingFlags.Static : BindingFlags.Instance;

            // get property based on name
            var property =
                type.GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Public | instanceOrStaticFlag);

            // check that property was found
            if (property == null)
                throw new MissingMemberException(type.FullName, propertyName);

            // return the property found
            return property;
        }

        #endregion

        #region Fields

        /// <summary>
        /// Gets a static field's value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static T GetStaticFieldValue<T>(this Type type, string fieldName)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (string.IsNullOrWhiteSpace(fieldName))
                throw new ArgumentNullException("fieldName");

            return (T)GetFieldForRead(type, fieldName, true, typeof(T)).GetValue(null);
        }

        /// <summary>
        /// Sets a static field's value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public static void SetStaticFieldValue<T>(this Type type, string fieldName, T value)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (string.IsNullOrWhiteSpace(fieldName))
                throw new ArgumentNullException("fieldName");

            GetFieldForWrite(type, fieldName, true, value).SetValue(null, value);
        }

        /// <summary>
        /// Gets a field's value for an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static T GetFieldValue<T>(this object obj, string fieldName)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            if (string.IsNullOrWhiteSpace(fieldName))
                throw new ArgumentNullException("fieldName");

            return (T)GetFieldForRead(obj.GetType(), fieldName, false, typeof(T)).GetValue(obj);
        }

        /// <summary>
        /// Sets a field's value for an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public static void SetFieldValue<T>(this object obj, string fieldName, T value)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            if (string.IsNullOrWhiteSpace(fieldName))
                throw new ArgumentNullException("fieldName");

            GetFieldForWrite(obj.GetType(), fieldName, false, value).SetValue(obj, value);
        }

        /// <summary>
        /// Gets a field and validates that it returns the right type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldType"></param>
        /// <param name="isStatic"></param>
        /// <returns></returns>
        private static FieldInfo GetFieldForRead(Type type, string fieldName, bool isStatic, Type fieldType)
        {
            // get the field
            var field = GetField(type, fieldName, isStatic);

            // check that types match
            if (!fieldType.IsAssignableFrom(field.FieldType))
                throw new InvalidOperationException(string.Format(Resources.FieldCannotBeCastFormat,
                    fieldName,
                    field.FieldType,
                    type.FullName,
                    fieldType));

            return field;
        }

        /// <summary>
        /// Gets a field and validates it for writing a given value
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fieldName"></param>
        /// <param name="isStatic"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static FieldInfo GetFieldForWrite(Type type, string fieldName, bool isStatic, object value)
        {
            // get the field
            var field = GetField(type, fieldName, isStatic);

            // if the value is null, ensure field accepts nulls 
            if (value == null && field.FieldType.IsValueType)
                throw new InvalidOperationException(string.Format(Resources.FieldCannotBeSetToNullFormat,
                    type.FullName,
                    fieldName));

            // if the value is set, check that the field can be set from its type
            if (value != null && !field.FieldType.IsInstanceOfType(value))
                throw new InvalidOperationException(string.Format(Resources.FieldCannotBeSetFormat,
                    type.FullName,
                    fieldName,
                    value.GetType().FullName));

            return field;
        }

        /// <summary>
        /// Gets a field for a type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fieldName"></param>
        /// <param name="isStatic"></param>
        /// <returns></returns>
        private static FieldInfo GetField(Type type, string fieldName, bool isStatic)
        {
            // get flag indicating whether instance or static
            var instanceOrStaticFlag = isStatic ? BindingFlags.Static : BindingFlags.Instance;

            // get property based on name
            var field =
                type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Public | instanceOrStaticFlag);

            // check that property was found
            if (field == null)
                throw new MissingFieldException(type.FullName, fieldName);

            // return the property found
            return field;
        }

        #endregion

        #region Invocation

        /// <summary>
        /// Invokes a delegate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        private static T Invoke<T>(Func<T> func)
        {
            try
            {
                return func();
            }
            catch (TargetInvocationException exception)
            {
                throw exception.InnerException;
            }
        }

        /// <summary>
        /// Invokes a delegate
        /// </summary>
        /// <param name="action"></param>
        private static void Invoke(Action action)
        {
            try
            {
                action();
            }
            catch (TargetInvocationException exception)
            {
                throw exception.InnerException;
            }
        }

        #endregion
    }
}

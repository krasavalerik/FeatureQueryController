using System;
using System.ComponentModel.Design;

namespace WebApp.DI
{
    /// <summary>
    /// Содержит в себе методы работы с LightInject контейнером. Хранит в себе словарь "сборка - контейнер".
    /// </summary>
    [Obsolete("Убрать.")]
    [MyAttribute]
    public static class LightInjectCore
    {
        private static ServiceContainer MainContainer { get; set; } = new ServiceContainer();

        //[MyAttribute]
        //public static T GetService<T>(string name)
        //{
        //    return MainContainer.TryGetInstance<T>(name);
        //}
    }

    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method)]
    public class MyAttribute : Attribute
    {

    }
}

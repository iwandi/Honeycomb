using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using DryIoc;

namespace Honeycomb
{
    public interface ISystem
    {
        void Init();
    }

    public class System : ISystem
    {
        [Import]
        protected Context context;

        public virtual void Init()
        {

        }
    }

    public class Context
    {
        Container container = new Container(Rules.Default.With(SelectPropertiesAndFieldsWithImportAttribute));

        public static readonly PropertiesAndFieldsSelector SelectPropertiesAndFieldsWithImportAttribute =
            PropertiesAndFields.All(withInfo: GetImportedPropertiesAndFields);

        private static PropertyOrFieldServiceInfo GetImportedPropertiesAndFields(MemberInfo m, Request req)
        {
            var import = (ImportAttribute)m.GetAttributes(typeof(ImportAttribute)).FirstOrDefault();
            return import == null ? null : PropertyOrFieldServiceInfo.Of(m)
                .WithDetails(ServiceDetails.Of(import.ContractType, import.ContractName), null);
        }

        public Context()
        {
            container.RegisterInstance<Context>(this);
        }

        public void Init()
        {
            foreach (ISystem system in container.ResolveMany<ISystem>())
            {
                container.InjectPropertiesAndFields(system);
                system.Init();
            }
        }

        public T Add<T>() where T : ISystem, new()
        {
            T system = new T();
            container.RegisterInstance<T>(system);
            container.RegisterInstance<ISystem>(system);
            return system;
        }

        public IEnumerable<T> ListSystems<T>()
        {
            foreach(ISystem system in container.ResolveMany<ISystem>())
            {
                if(system is T)
                {
                    yield return (T)system;
                }
            }
        }
    }
}

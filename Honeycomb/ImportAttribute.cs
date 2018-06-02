using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Honeycomb
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class ImportAttribute : Attribute
    {
        public ImportAttribute() { }

        public ImportAttribute(string contractName, Type contractType = null)
        {
            ContractName = contractName;
            ContractType = contractType;
        }

        public Type ContractType { get; set; }
        public string ContractName { get; set; }
    }
}

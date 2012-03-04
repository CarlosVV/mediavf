using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace MediaVF.Services.Core.Components
{
    public interface IServiceComponent : IModule
    {
        int ID { get; set; }

        IUnityContainer ComponentContainer { get; set; }

        void InitializeComponent();

        void Run();
    }
}

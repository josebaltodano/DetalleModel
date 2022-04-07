using AppCore.IServices;
using AppCore.Services;
using Autofac;
using Domain.Interfaces;
using Infraestructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace practicaDepreciacion
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var builder = new ContainerBuilder();
            var builder1 = new ContainerBuilder();
            builder1.RegisterType<BinaryActivoRepository>().As<IActivoModel>();
            builder1.RegisterType<ActivoServices>().As<IActivoServices>();
            builder.RegisterType<BinaryEmpleadoRepository>().As<IEmpleadoModel>();
            builder.RegisterType<EmpleadoServices>().As<IEmpleadoServices>();
            var container = builder.Build();
            var container1 = builder1.Build();
            Application.Run(new Form1(container1.Resolve<IActivoServices>()));
                /*(new EmpleadoForm(container.Resolve<IEmpleadoServices>(), container1.Resolve<IActivoServices>()));*/

        }
    }
}

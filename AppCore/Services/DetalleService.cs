using AppCore.IServices;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Services
{
    public class DetalleService : BaseServices<DetalleAE> ,IDetalleServices
    {
        IdetalleModel idetalleModel;
        public DetalleService(IdetalleModel model): base(model)
        {
            this.idetalleModel = model;
        }

        public List<DetalleAE> detalles(Expression<Func<DetalleAE, bool>> detalle)
        {
            return idetalleModel.detalles(detalle);
        }

        public void Mostrar(DetalleAE detalleAE)
        {
            idetalleModel.Mostrar(detalleAE);
        }
    }
}

using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.IServices
{
   public interface IDetalleServices: IServices<DetalleAE>
    {

        void Mostrar(DetalleAE detalleAE);
        List<DetalleAE> detalles(Expression<Func<DetalleAE, bool>> detalle);
    }
}

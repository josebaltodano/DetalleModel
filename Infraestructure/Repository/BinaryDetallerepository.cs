using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repository
{
   public  class BinaryDetallerepository : IdetalleModel
    {
       public  RAFContext<DetalleAE> context;
        private const int SIZE = 30;
        public BinaryDetallerepository()
        {
            context = new RAFContext<DetalleAE>("Detalle", SIZE);
        }

        public void Add(DetalleAE t)
        {
            context.Create<DetalleAE>(t);
        }

        public void Delete(DetalleAE t)
        {
            throw new NotImplementedException();
        }

        public List<DetalleAE> detalles(Expression<Func<DetalleAE, bool>> detalle)
        {
            return context.Find<DetalleAE>(detalle);
        }

        public List<DetalleAE> Find()
        {
            throw new NotImplementedException();
        }

        public void Mostrar(DetalleAE detalleAE)
        {
            this.context.Update<DetalleAE>(detalleAE);
        }

        public List<DetalleAE> Read()
        {
            throw new NotImplementedException();
        }

        public int Update(DetalleAE t)
        {
            throw new NotImplementedException();
        }
    }
}

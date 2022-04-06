using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repository
{
    public class RAFContext<T>
    {
        private string fileName;
        private int size;
        private const string directoryName = "DATA";
        private string DirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), directoryName);

        public RAFContext(string fileName, int size)
        {
            this.fileName = fileName;
            this.size = size;
        }

        public Stream HeaderStream
        {
            get => File.Open($"{fileName}.hd", FileMode.OpenOrCreate, FileAccess.ReadWrite);
        }
        public Stream TemporalStream
        {
            get => File.Open($"{fileName}.tp", FileMode.OpenOrCreate, FileAccess.ReadWrite);
        }

        public Stream DataStream
        {
            get => File.Open($"{fileName}.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite);
        }
        private object GetClass(object obj,BinaryReader Data)
        {
            object newValue = Activator.CreateInstance(obj.GetType());
            PropertyInfo[] infos = obj.GetType().GetProperties();
            
            foreach (PropertyInfo pinfo in infos)
            {
                Type type = pinfo.PropertyType;
                if(!type.IsPrimitive && type.IsClass && type != Type.GetType("System.String"))
                {
                    object j = pinfo.PropertyType;
                    pinfo.SetValue(newValue,GetClass(j,Data));
                }
                if (type.IsGenericType)
                {
                    continue;
                }

                if (type == typeof(int))
                {
                    pinfo.SetValue(newValue, Data.GetValue<int>(TypeCode.Int32));
                }
                else if (type.IsEnum)
                {
                    pinfo.SetValue(newValue, Data.GetValue<int>(TypeCode.Int32));
                }
                else if (type == typeof(long))
                {
                    pinfo.SetValue(newValue, Data.GetValue<long>(TypeCode.Int64));
                }
                else if (type == typeof(float))
                {
                    pinfo.SetValue(newValue, Data.GetValue<float>(TypeCode.Single));
                }
                else if (type == typeof(double))
                {
                    pinfo.SetValue(newValue, Data.GetValue<double>(TypeCode.Double));
                }
                else if (type == typeof(decimal))
                {
                    pinfo.SetValue(newValue, Data.GetValue<decimal>(TypeCode.Decimal));
                }
                else if (type == typeof(char))
                {
                    pinfo.SetValue(newValue, Data.GetValue<char>(TypeCode.Char));
                }
                else if (type == typeof(bool))
                {
                    pinfo.SetValue(newValue, Data.GetValue<bool>(TypeCode.Boolean));
                }
                else if (type == typeof(string))
                {
                    pinfo.SetValue(newValue, Data.GetValue<string>(TypeCode.String));
                }
                
            }
            return newValue;

        }
        private object GetClass(object obj, BinaryWriter Data)
        {
            object newValue = Activator.CreateInstance(obj.GetType());
            PropertyInfo[] infos = obj.GetType().GetProperties();

            foreach (PropertyInfo pinfo in infos)
            {
                Type type = pinfo.PropertyType;
                object obj1 = pinfo.GetValue(obj, null);
                if (!type.IsPrimitive && type.IsClass && type != Type.GetType("System.String"))
                {
                    object j = pinfo.PropertyType;
                    pinfo.SetValue(newValue, GetClass(j, Data));
                }
                if (type.IsGenericType)
                {
                    continue;
                }

                if (type == typeof(int))
                {
                    Data.Write((int)obj1);
                }
                else if (type.IsEnum)
                {
                    Data.Write((int)obj1);
                }
                else if (type == typeof(long))
                {
                    Data.Write((long)obj1);
                }
                else if (type == typeof(float))
                {
                    Data.Write((float)obj1);
                }
                else if (type == typeof(double))
                {
                    Data.Write((double)obj1);
                }
                else if (type == typeof(decimal))
                {
                    Data.Write((decimal)obj);
                }
                else if (type == typeof(char))
                {
                    Data.Write((char)obj1);
                }
                else if (type == typeof(bool))
                {
                    Data.Write((bool)obj1);
                }
                else if (type == typeof(string))
                {
                    Data.Write((string)obj1);
                }

            }
            return newValue;

        }
        public void Create<T>(T t)
        {
            try
            {
                using (BinaryWriter bwHeader = new BinaryWriter(HeaderStream),
                                 bwData = new BinaryWriter(DataStream))
                {
                    int n, k;
                    using (BinaryReader brHeader = new BinaryReader(bwHeader.BaseStream))
                    {
                        if (brHeader.BaseStream.Length == 0)
                        {
                            n = 0;
                            k = 0;
                        }
                        else
                        {
                            brHeader.BaseStream.Seek(0, SeekOrigin.Begin);
                            n = brHeader.ReadInt32();
                            k = brHeader.ReadInt32();
                        }
                        //calculamos la posicion en Data
                        long pos = k * size;
                        bwData.BaseStream.Seek(pos, SeekOrigin.Begin);

                        //PropertyInfo[] info = t.GetType().GetProperties();
                        //foreach (PropertyInfo pinfo in info)
                        //{
                        //    Type type = pinfo.PropertyType;
                        //    object obj = pinfo.GetValue(t, null);

                        //    if (type.IsGenericType)
                        //    {
                        //        continue;
                        //    }

                        //    if (pinfo.Name.Equals("Id", StringComparison.CurrentCultureIgnoreCase))
                        //    {
                        //        bwData.Write(++k);
                        //        continue;
                        //    }

                        //    if (type == typeof(int))
                        //    {
                        //        bwData.Write((int)obj);
                        //    }
                        //    else if (type.IsEnum)
                        //    {
                        //        bwData.Write((int)obj);
                        //    }
                        //    else if (type == typeof(long))
                        //    {
                        //        bwData.Write((long)obj);
                        //    }
                        //    else if (type == typeof(float))
                        //    {
                        //        bwData.Write((float)obj);
                        //    }
                        //    else if (type == typeof(double))
                        //    {
                        //        bwData.Write((double)obj);
                        //    }
                        //    else if (type == typeof(decimal))
                        //    {
                        //        bwData.Write((decimal)obj);
                        //    }
                        //    else if (type == typeof(char))
                        //    {
                        //        bwData.Write((char)obj);
                        //    }
                        //    else if (type == typeof(bool))
                        //    {
                        //        bwData.Write((bool)obj);
                        //    }
                        //    else if (type == typeof(string))
                        //    {
                        //        bwData.Write((string)obj);
                        //    }else if (type.IsClass)
                        //    {
                        //        //object obj = ;
                        //        //int id1 = (int)obj.GetType().GetProperty("Id").GetValue(obj);
                        //        GetClass(obj, bwData);
                        //        //bwData.Write(id1);
                        //    }
                        //}
                        PropertyInfo[] info = t.GetType().GetProperties();
                        foreach (PropertyInfo pinfo in info)
                        {


                            Type type = pinfo.PropertyType;
                            object obj = pinfo.GetValue(t, null);

                            if (!type.IsPrimitive && type.IsClass && type != Type.GetType("System.String"))
                            {
                                PropertyInfo[] infoClass = obj.GetType().GetProperties();
                                object objectClass = Activator.CreateInstance(obj.GetType());
                                foreach (PropertyInfo PInfoClass in infoClass)
                                {
                                    if (PInfoClass.Name.Equals("Id", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        bwData.Write(0);
                                        break;
                                    }
                                }
                                continue;
                                //WriteObject(obj, bwData);
                            }
                            if (type.IsGenericType)
                            {

                                continue;
                            }

                            if (pinfo.Name.Equals("Id", StringComparison.CurrentCultureIgnoreCase))
                            {
                                bwData.Write(++k);
                                continue;
                            }

                            if (type == typeof(int))
                            {
                                bwData.Write((int)obj);
                                continue;
                            }
                            else if (type == typeof(long))
                            {

                                bwData.Write((long)obj);
                                continue;
                            }
                            else if (type == typeof(float))
                            {
                                bwData.Write((float)obj);
                                continue;
                            }
                            else if (type == typeof(double))
                            {
                                bwData.Write((double)obj);
                                continue;
                            }
                            else if (type == typeof(decimal))
                            {
                                bwData.Write((decimal)obj);
                                continue;
                            }
                            else if (type == typeof(char))
                            {
                                bwData.Write((char)obj);
                                continue;
                            }
                            else if (type == typeof(bool))
                            {
                                bwData.Write((bool)obj);
                                continue;
                            }
                            else if (type == typeof(string))
                            {
                                bwData.Write((string)obj);
                                continue;
                            }
                            if (type.IsEnum)
                            {
                                bwData.Write((int)obj);
                                continue;
                            }
                        }

                            long posh = 8 + n * 4;
                        bwHeader.BaseStream.Seek(posh, SeekOrigin.Begin);
                        bwHeader.Write(k);

                        bwHeader.BaseStream.Seek(0, SeekOrigin.Begin);
                        bwHeader.Write(++n);
                        bwHeader.Write(k);
                    }

                }
            }
            catch (IOException)
            {
                throw;
            }

        }

        public T Get<T>(int id)
        {
            try
            {
                T newValue = (T)Activator.CreateInstance(typeof(T));
                using (BinaryReader brHeader = new BinaryReader(HeaderStream),
                                    brData = new BinaryReader(DataStream))
                {
                    //TODO Validar como en l metodo create
                    brHeader.BaseStream.Seek(0, SeekOrigin.Begin);
                    int n = brHeader.ReadInt32();
                    int k = brHeader.ReadInt32();

                    if (id <= 0 || id > k)
                    {
                        return default(T);
                    }

                    PropertyInfo[] properties = newValue.GetType().GetProperties();
                    long posh = 8 + (id - 1) * 4;
                    //TODO Add Binary search to find the id
                    brHeader.BaseStream.Seek(posh, SeekOrigin.Begin);
                    int index = brHeader.ReadInt32();
                    if (index == 0)
                    {
                        return newValue;
                    }
                    //TO-DO VALIDATE INDEX
                    long posd = (index - 1) * size;
                    brData.BaseStream.Seek(posd, SeekOrigin.Begin);
                    foreach (PropertyInfo pinfo in properties)
                    {
                        Type type = pinfo.PropertyType;

                        if (type.IsGenericType)
                        {
                            continue;
                        }

                        if (type == typeof(int))
                        {
                            pinfo.SetValue(newValue, brData.GetValue<int>(TypeCode.Int32));
                        }else if (type.IsEnum)
                        {
                            pinfo.SetValue(newValue,brData.GetValue<int>(TypeCode.Int32));
                        }
                        else if (type == typeof(long))
                        {
                            pinfo.SetValue(newValue, brData.GetValue<long>(TypeCode.Int64));
                        }
                        else if (type == typeof(float))
                        {
                            pinfo.SetValue(newValue, brData.GetValue<float>(TypeCode.Single));
                        }
                        else if (type == typeof(double))
                        {
                            pinfo.SetValue(newValue, brData.GetValue<double>(TypeCode.Double));
                        }
                        else if (type == typeof(decimal))
                        {
                            pinfo.SetValue(newValue, brData.GetValue<decimal>(TypeCode.Decimal));
                        }
                        else if (type == typeof(char))
                        {
                            pinfo.SetValue(newValue, brData.GetValue<char>(TypeCode.Char));
                        }
                        else if (type == typeof(bool))
                        {
                            pinfo.SetValue(newValue, brData.GetValue<bool>(TypeCode.Boolean));
                        }
                        else if (type == typeof(string))
                        {
                            pinfo.SetValue(newValue, brData.GetValue<string>(TypeCode.String));
                        }else if (type.IsClass)
                        {
                            GetClass(brData.GetValue<object>(TypeCode.Object), brData);
                            /*int id1 = (int)brData.GetValue<int>(TypeCode.Int32);
                            /*object obj = brData.GetValue<object>(TypeCode.Object);
                            int id1 = (int)obj.GetType().GetProperty("Id").GetValue(obj);*/
                            //pinfo.SetValue(newValue, Get<object>(id1));*/
                        }
                    }


                }
                return newValue;
            }
            catch (Exception)
            {
                throw;
            }


        }

        public List<T> GetAll<T>()
        {
            List<T> listT = new List<T>();
            int k = 0, n = 0;
            try
            {
            using (BinaryReader brHeader = new BinaryReader(HeaderStream))
            {
                if (brHeader.BaseStream.Length > 0)
                {
                    brHeader.BaseStream.Seek(0, SeekOrigin.Begin);
                    n = brHeader.ReadInt32();
                }

            }
            if (n == 0)
            {
                return listT;
            }
            for (int i = 0; i < n; i++)
            {
                int index;
                using (BinaryReader brHeader = new BinaryReader(HeaderStream))
                {
                    long posh = 8 + i * 4;
                    brHeader.BaseStream.Seek(posh, SeekOrigin.Begin);
                    index = brHeader.ReadInt32();
                }
                if (index == 0)
                {
                    continue;
                }
                else
                {
                    T t = Get<T>(index);
                    listT.Add(t);
                }
            }
            }
            catch (Exception)
            {
                throw;
            }


            return listT;
        }

        public List<T> Find<T>(Expression<Func<T, bool>> where)
        {
            List<T> listT = new List<T>();
            int n, k;
            Func<T, bool> comparator = where.Compile();
            try
            {
                using (BinaryReader brHeader = new BinaryReader(HeaderStream))
                {
                    brHeader.BaseStream.Seek(0, SeekOrigin.Begin);
                    n = brHeader.ReadInt32();
                    k = brHeader.ReadInt32();
                }

                for (int i = 0; i < n; i++)
                {
                    int index;
                    using (BinaryReader brHeader = new BinaryReader(HeaderStream))
                    {
                        long posh = 8 + i * 4;
                        brHeader.BaseStream.Seek(posh, SeekOrigin.Begin);
                        index = brHeader.ReadInt32();
                    }

                    T t = Get<T>(index);
                    if (comparator(t))
                    {
                        listT.Add(t);
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }

            return listT;
        }
        public int Update<T>(T t)
        {
            int id;
            try
            {

                id = (int)t.GetType().GetProperty("Id").GetValue(t);
                int index = BinarySearch(id);
                if (index < 0)
                {
                    throw new ArgumentException($"No se encontro un objeto con el Id: {id}");
                }
                using (BinaryWriter bwData = new BinaryWriter(DataStream))
                {
                    long posd = index * size;
                    bwData.BaseStream.Seek(posd, SeekOrigin.Begin);

                    PropertyInfo[] info = t.GetType().GetProperties();
                    foreach (PropertyInfo pinfo in info)
                    {
                        Type type = pinfo.PropertyType;
                        object obj = pinfo.GetValue(t, null);

                        if (type.IsGenericType)
                        {
                            continue;
                        }
                        if (type == typeof(int))
                        {
                            bwData.Write((int)obj);
                        }
                        else if (type == typeof(long))
                        {
                            bwData.Write((long)obj);
                        }
                        else if (type == typeof(float))
                        {
                            bwData.Write((float)obj);
                        }
                        else if (type == typeof(double))
                        {
                            bwData.Write((double)obj);
                        }
                        else if (type == typeof(decimal))
                        {
                            bwData.Write((decimal)obj);
                        }
                        else if (type == typeof(char))
                        {
                            bwData.Write((char)obj);
                        }
                        else if (type == typeof(bool))
                        {
                            bwData.Write((bool)obj);
                        }
                        else if (type == typeof(string))
                        {
                            bwData.Write((string)obj);
                        }
                    }
                }
                return id;
            }
            catch (Exception)
            {
                throw new ArgumentException($"El objeto {t.GetType().Name} no contiene la propiedad Id");
            }
        }

        public void Delete<T>(T t)
        {
            try
            {
                int Id = (int)t.GetType().GetProperty("Id").GetValue(t);
                int n = 0;
                int pos = 8 + (Id - 1) * 4;
                using (BinaryWriter brHeader = new BinaryWriter(HeaderStream))
                {
                    if (brHeader.BaseStream.Length > 0)
                    {
                        brHeader.BaseStream.Seek(pos, SeekOrigin.Begin);
                        brHeader.Write(0);
                    }

                }
            }
            catch (IOException)
            {
                throw;
            }

        }
        private int BinarySearch(int Buscardato)
        {
            using (BinaryReader brHeader = new BinaryReader(HeaderStream))
            {
                brHeader.BaseStream.Seek(0, SeekOrigin.Begin);
                int fin = brHeader.ReadInt32() - 1;
                int inicio = 0;
                while (inicio <= fin)
                {
                    int indiceCentral = Convert.ToInt32(Math.Floor(Convert.ToDouble(inicio + fin) / 2));
                    brHeader.BaseStream.Seek(8 + 4 * indiceCentral, SeekOrigin.Begin);
                    int valorCentral = brHeader.ReadInt32();
                    if (valorCentral == Buscardato)
                    {
                        return indiceCentral;
                    }
                    if (Buscardato < valorCentral)
                    {
                        fin = indiceCentral - 1;
                    }
                    else
                    {
                        inicio = indiceCentral + 1;
                    }
                }
                return -1;
            }
        }
        private void WriteObject(object t, BinaryWriter bwData)
        {
            if (t == null)
            {
                return;
            }
            PropertyInfo[] infoClass = t.GetType().GetProperties();
            foreach (PropertyInfo pinfoclass in infoClass)
            {
                Type typeOne = pinfoclass.PropertyType;
                object obj = pinfoclass.GetValue(t, null);
                if (!typeOne.IsPrimitive && typeOne.IsClass && typeOne != Type.GetType("System.String"))
                {
                    WriteObject(obj, bwData);
                }
                if (typeOne.IsGenericType)
                {
                    continue;
                }
                if (typeOne == typeof(int))
                {
                    bwData.Write((int)obj);
                    continue;
                }
                else if (typeOne == typeof(long))
                {
                    bwData.Write((long)obj);
                    continue;
                }
                else if (typeOne == typeof(float))
                {
                    bwData.Write((float)obj);
                    continue;
                }
                else if (typeOne == typeof(double))
                {
                    bwData.Write((double)obj);
                    continue;
                }
                else if (typeOne == typeof(decimal))
                {
                    bwData.Write((decimal)obj);
                    continue;
                }
                else if (typeOne == typeof(char))
                {
                    bwData.Write((char)obj);
                    continue;
                }
                else if (typeOne == typeof(bool))
                {
                    bwData.Write((bool)obj);
                    continue;
                }
                else if (typeOne == typeof(string))
                {
                    bwData.Write((string)obj);
                    continue;
                }
                if (typeOne.IsEnum)
                {
                    bwData.Write((int)obj);
                    continue;
                }
            }
        }
    }
}

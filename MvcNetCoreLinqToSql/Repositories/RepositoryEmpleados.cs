using MvcNetCoreLinqToSql.Models;
using System.Data;
using System.Data.SqlClient;

namespace MvcNetCoreLinqToSql.Repositories
{
    public class RepositoryEmpleados
    {

        private DataTable tablaEmpleados;
        public RepositoryEmpleados() 
        {
            string connectionString = "Data Source=LOCALHOST\\SQLEXPRESS;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=sa;Password=MCSD2023";
            string sql = "select * from EMP";
            SqlDataAdapter adEmp = new SqlDataAdapter(sql,connectionString);
            //Instanciamos nuestro datatable
            this.tablaEmpleados = new DataTable();
            //Traemos los datos
            adEmp.Fill(tablaEmpleados);
        }
        //METODO PARA FILTRAR EMPLEADOS POR SU DEPARTAMENTO
        public ResumenEmpleados GetEmpleadosDepartamento(int departamento)
        {
            var consulta = from datos in this.tablaEmpleados.AsEnumerable()
                           where datos.Field<int>("DEPT_NO") == departamento
                           select datos;
            //Me gustaria que los datos esten ordenados por salario
            consulta = consulta.OrderBy(x => x.Field<int>("SALARIO"));
            int personas = consulta.Count();
            int maximo = consulta.Max(z => z.Field<int>("SALARIO"));
            double media = consulta.Average(x => x.Field<int>("SALARIO"));
            List<Empleado> empleados = new List<Empleado>();
            foreach (var row in consulta)
            {
                Empleado emp = new Empleado
                {
                    IdEmpleado = row.Field<int>("EMP_NO"),
                    Apellido = row.Field<string>("APELLIDO"),
                    Oficio = row.Field<string>("OFICIO"),
                    Salario = row.Field<int>("SALARIO"),
                    IdDepartamento = row.Field<int>("DEPT_NO")
                };
                empleados.Add(emp);
            }
            ResumenEmpleados resumen = new ResumenEmpleados
            {
                Personas = personas,
                MaximoSalario = maximo,
                MediaSalarial = media,
                Empleados = empleados
            };
            return resumen;
        }

        //FILTRAR POR DEPARTAMENTO
        public List<int> GetDepartamentos()
        {
            var consulta = (from datos in this.tablaEmpleados.AsEnumerable()
                            select datos.Field<int>("DEPT_NO")).Distinct();
            List<int> departamentos = new List<int>();
            foreach (int dept in consulta)
            {
                departamentos.Add(dept);
            }
            return departamentos;
        }


        //METODO PARA GILTRAR EMPLEADOS POR SU OFICIO
        public ResumenEmpleados GetEmpleadosOficio(string oficio)
        {
            var consulta = from datos in this.tablaEmpleados.AsEnumerable() where datos.Field<string>("OFICIO") == oficio
                           select datos;
            //Me gustaria que los datos esten ordenados por salario
            consulta = consulta.OrderBy(x => x.Field<int>("SALARIO"));
            int personas = consulta.Count();
            int maximo = consulta.Max(z => z.Field<int>("SALARIO"));
            double media = consulta.Average(x => x.Field<int>("SALARIO"));
            List<Empleado> empleados = new List<Empleado>();
            foreach(var row in consulta)
            {
                Empleado emp = new Empleado
                {
                    IdEmpleado = row.Field<int>("EMP_NO"),
                    Apellido = row.Field<string>("APELLIDO"),
                    Oficio = row.Field<string>("OFICIO"),
                    Salario = row.Field<int>("SALARIO"),
                    IdDepartamento = row.Field<int>("DEPT_NO")
                };
                empleados.Add(emp);
            }
            ResumenEmpleados resumen = new ResumenEmpleados
            {
                Personas = personas,
                MaximoSalario = maximo,
                MediaSalarial = media,
                Empleados = empleados
            };
            return resumen;
        }

        public List<string> GetOficios()
        {
            var consulta = (from datos in this.tablaEmpleados.AsEnumerable() 
                            select datos.Field<string>("OFICIO")).Distinct();
            List<string> oficios = new List<string>();
            foreach(string ofi in consulta)
            {
                oficios.Add(ofi);
            }
            return oficios;
            
        }

        //Metodo para recuperar todos los empleados
        public List<Empleado> GetEmpleados()
        {
            //La consulta linq se almacena en variables de tipo var
            var consulta = from datos in this.tablaEmpleados.AsEnumerable() select datos;
            //LO QUE TENEMOS ALMACENADO EN CONSULTA ES UN CONJUNTO DE OBJETOS DataRow que son los objetos que
            //contiene la clase DataTable, debemos convertir dichos objetos DataRow en empleados 
            List<Empleado> empleados = new List<Empleado>();
            //recorremos cada fila de la consulta
            foreach (var row in consulta)
            {
                //Para extraer los datos de una fila DataRow fila.Field<Typo>("COLUMNA")
                Empleado emp = new Empleado();
                emp.IdEmpleado = row.Field<int>("EMP_NO");
                emp.Apellido = row.Field<string>("APELLIDO");
                emp.Oficio = row.Field<string>("OFICIO");
                emp.Salario = row.Field<int>("SALARIO");
                emp.IdDepartamento = row.Field<int>("DEPT_NO");
                empleados.Add(emp);
            }
            return empleados;
        }
        //METODO PARA BUSCAR UN EMPLEADO POR SU ID
        public Empleado FindEmpleado(int idEmpleado)
        {
            //El alias datos representa cada objeto dentro del conjunto
            var consulta = from datos in this.tablaEmpleados.AsEnumerable() where datos.Field<int>("EMP_NO") == idEmpleado select datos;
            var row = consulta.First();
            Empleado empleado = new Empleado();
            empleado.IdEmpleado = row.Field<int>("EMP_NO");
            empleado.Apellido = row.Field<string>("APELLIDO");
            empleado.Oficio = row.Field<string>("OFICIO");
            empleado.Salario = row.Field<int>("SALARIO");
            empleado.IdDepartamento = row.Field<int>("DEPT_NO");
            return empleado;

        }

        //Filtrar empleados por oficio y salario
        public List<Empleado> GetEmpleadosOficioSalario(string oficio, int salario)
        {
            var consulta = from datos in this.tablaEmpleados.AsEnumerable()
                           where datos.Field<string>("OFICIO") == oficio
                           && datos.Field<int>("SALARIO") >= salario
                           select datos;

            //Con este if si no hay datos que mostrar devuelve null y si si los hay hace la consulta
            if (consulta.Count() == 0)
            {
                return null;
            }
            else
            {
                List<Empleado> empleados = new List<Empleado>();
                foreach (var row in consulta)
                {
                    //SINTAXIS PARA INSTANCIAR UN OBJETO Y RELLENAR SUS PROPIEDADES A LA VEZ
                    Empleado empleado = new Empleado
                    {
                        IdEmpleado = row.Field<int>("EMP_NO"),
                        Apellido = row.Field<string>("APELLIDO"),
                        Oficio = row.Field<string>("OFICIO"),
                        Salario = row.Field<int>("SALARIO"),
                        IdDepartamento = row.Field<int>("DEPT_NO")
                    };
                    empleados.Add(empleado);
                }
                return empleados;
            }       
        }
    }
}

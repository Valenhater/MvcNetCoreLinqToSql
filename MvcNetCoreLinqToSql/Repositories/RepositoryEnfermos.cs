using MvcNetCoreLinqToSql.Models;
using System.Data;
using System.Data.SqlClient;

namespace MvcNetCoreLinqToSql.Repositories
{
    public class RepositoryEnfermos
    {
        private DataTable tablaEnfermos;
        SqlConnection cn;
        SqlCommand com;
        SqlDataReader reader;
        public RepositoryEnfermos() 
        {
            string connectionString = "Data Source=LOCALHOST\\SQLEXPRESS;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=sa;Password=MCSD2023";
            string sql = "select * from ENFERMO";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
            SqlDataAdapter adEnf = new SqlDataAdapter(sql, connectionString);
            this.tablaEnfermos = new DataTable();
            adEnf.Fill(tablaEnfermos);
        }

        public List<Enfermo> GetEnfermos()
        {
            var consulta = from datos in this.tablaEnfermos.AsEnumerable() select datos;
            List<Enfermo> enfermos = new List<Enfermo>();
            foreach(var row in consulta)
            {
                Enfermo enfermo = new Enfermo();
                enfermo.Inscripcion = row.Field<string>("INSCRIPCION");
                enfermo.Apellido = row.Field<string>("APELLIDO");
                enfermo.Direccion = row.Field<string>("DIRECCION");
                enfermo.FechaNacimiento = row.Field<DateTime>("FECHA_NAC");
                enfermo.Sexo = row.Field<string>("S");
                enfermo.NumSeguridadSocial = row.Field<string>("NSS");
                enfermos.Add(enfermo);
            }
            return enfermos;
        }
        public Enfermo FindEnfermo(string inscripcion)
        {
            var consulta = from datos in this.tablaEnfermos.AsEnumerable() where datos.Field<string>("INSCRIPCION") == inscripcion select datos;
            var row = consulta.First();
            Enfermo enfermo = new Enfermo();
            enfermo.Inscripcion = row.Field<string>("INSCRIPCION");
            enfermo.Apellido = row.Field<string>("APELLIDO");
            enfermo.Direccion = row.Field<string>("DIRECCION");
            enfermo.FechaNacimiento = row.Field<DateTime>("FECHA_NAC");
            enfermo.Sexo = row.Field<string>("S");
            enfermo.NumSeguridadSocial = row.Field<string>("NSS");
            return enfermo;
        }
        public void DeleteEnfermos(string inscripcion)
        {
            string sql = "delete from ENFERMO where INSCRIPCION = @inscripcion";
            this.com.Parameters.AddWithValue("@inscripcion", inscripcion);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            int af =  this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }
    }
}

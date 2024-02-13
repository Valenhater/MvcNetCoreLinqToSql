using Microsoft.AspNetCore.Mvc;
using MvcNetCoreLinqToSql.Models;
using MvcNetCoreLinqToSql.Repositories;

namespace MvcNetCoreLinqToSql.Controllers.Empleados
{
    public class EmpleadosController : Controller
    {
        RepositoryEmpleados repo;
        public EmpleadosController()
        {
            this.repo = new RepositoryEmpleados();
        }
        public IActionResult DatosEmpleadosOficios()
        {
            ViewData["DEPARTAMENTOS"] = this.repo.GetDepartamentos();
            return View();
        }
        [HttpPost]
        public IActionResult DatosEmpleadosOficios(int departamento)
        {
            ViewData["DEPARTAMENTOS"] = this.repo.GetDepartamentos();
            ResumenEmpleados model = this.repo.GetEmpleadosDepartamento(departamento);
            if (model == null)
            {
                ViewData["MENSAJEERROR"] = "No existe el departamento" + model;
                return View();
            }
            else
            {
                return View(model);
            }      
        }
        public IActionResult DatosEmpleados()
        {
            ViewData["OFICIOS"] = this.repo.GetOficios();
            return View();
        }
        [HttpPost]
        public IActionResult DatosEmpleados(string oficio)
        {
            ViewData["OFICIOS"] = this.repo.GetOficios();
            ResumenEmpleados model = this.repo.GetEmpleadosOficio(oficio);
            return View(model);
        }

        public IActionResult Index()
        {
            List<Empleado> empleados = this.repo.GetEmpleados();
            return View(empleados);
        }
        public IActionResult Details(int id)
        {
            Empleado empleado = this.repo.FindEmpleado(id);
            return View(empleado);
        }
        public IActionResult BuscadorEmpleados()
        {
            List<Empleado> empleados = this.repo.GetEmpleados();
            return View(empleados);
        }
        [HttpPost]
        public IActionResult BuscadorEmpleados(string oficio, int salario)
        {
            List<Empleado> empleados = this.repo.GetEmpleadosOficioSalario(oficio, salario);
            if (empleados == null)
            {
                ViewData["MENSAJE"] = "No existen registros con oficio: " + oficio + " ni salario mayor a: " + salario;
                return View();//Si es null devolvemos vista vacia con mensaje
            }
            else
            {   
                //Si existen los registros mostramos el dibujo con los empleados
                return View(empleados);
            }        
        }

    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RESERVAS.API.Controllers;
using RESERVAS.API.Data;
using RESERVAS.API.Entities;
using RESERVAS.TEST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESERVAS.TEST.UnitTesting
{
    [TestClass]
    public class ReservasControllerTest : BasePruebas
    {
        private string CrearDataPrueba()
        {
            string NombreDB = Guid.NewGuid().ToString();
            DataContext ContextDB = ConstruirContext(NombreDB);
            Usuario _usuario = new Usuario()
            {
                ID = 0,
                NOMBRE = "DANIEL",
                APELLIDOS = "OICATA",
                EMAIL = "danielandres011@hotmail.com",
                DIRECCION = "CALLE FALSA 123",
                CREATED = DateTime.Parse("2022-01-01"),
                UPDATED = DateTime.Parse("2022-01-01"),
                DELETED = null
            };

            ContextDB.Add(_usuario);
            ContextDB.SaveChanges();

            Hotel _hotel = new Hotel()
            {
                ID = 0,
                NOMBRE = "HOTEL PRUEBA",
                PAIS = "COLOMBIA",
                HABITACIONES = 10,
                ACTIVO = true,
                LONGITUD = 0,
                LATITUD = 0,
                DESCRIPCION = "Hotel de pruebas unitarias.",
                CREATED = DateTime.Parse("2022-01-01"),
                UPDATED = DateTime.Parse("2022-01-01"),
                DELETED = null
            };
            ContextDB.Add(_hotel);

            List<Reserva> _reservas = new List<Reserva>()
            {
                new Reserva(){
                 ID = 0,
                 ID_USUARIO = _usuario,
                 ID_HOTEL = _hotel,
                 ID_HABITACION = 1,
                 CHECKIN = DateTime.Parse("2022-02-05"),
                 CHECKOUT = DateTime.Parse("2022-02-06"),
                 ESTADO = true},
                new Reserva(){
                 ID = 0,
                 ID_USUARIO = _usuario,
                 ID_HOTEL = _hotel,
                 ID_HABITACION = 2,
                 CHECKIN = DateTime.Parse("2022-02-08"),
                 CHECKOUT = DateTime.Parse("2022-02-10"),
                 ESTADO = true},
                new Reserva(){
                 ID = 0,
                 ID_USUARIO = _usuario,
                 ID_HOTEL = _hotel,
                 ID_HABITACION = 3,
                 CHECKIN = DateTime.Parse("2022-02-12"),
                 CHECKOUT = DateTime.Parse("2022-02-13"),
                 ESTADO = true}
            };
            ContextDB.AddRange(_reservas);
            ContextDB.SaveChanges();
            return NombreDB;
        }

        [TestMethod]
        public async Task ObtenerTodasLasReservas()
        {
            //Preparacion
            string NombreDB = CrearDataPrueba();
            DataContext contexto = ConstruirContext(NombreDB);
            ReservasController controller = new ReservasController(contexto);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            //Prueba
            var respuesta = await controller.GetReservas();
            //Verificar
            Assert.AreEqual(3, respuesta.Value.Count());
        }

        [TestMethod]
        public async Task ObtenerDisponibilidadPorFecha()
        {
            //Preparacion
            string NombreDB = CrearDataPrueba();
            DataContext contexto = ConstruirContext(NombreDB);
            ReservasController controller = new ReservasController(contexto);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            //Prueba
            var respuesta = await controller.GetDisponibilidadByFecha(1, DateTime.Parse("2022-02-05"));
            //Verificar
            Assert.AreEqual(true, respuesta.Value);
        }
        
        [TestMethod]
        public async Task ObtenerReservaPorId()
        {
            //Preparacion
            string NombreDB = CrearDataPrueba();
            DataContext contexto = ConstruirContext(NombreDB);
            ReservasController controller = new ReservasController(contexto);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            //Prueba
            var respuesta = await controller.GetReserva(3);
            //Verificar
            Assert.AreEqual(3, respuesta.Value.ID_HABITACION);
        }

        [TestMethod]
        public async Task ObtenerReservasPorFechas()
        {
            //Preparacion
            string NombreDB = CrearDataPrueba();
            DataContext contexto = ConstruirContext(NombreDB);
            ReservasController controller = new ReservasController(contexto);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            //Prueba
            var respuesta = await controller.GetReservasByFechas(1, DateTime.Parse("2022-02-05"), DateTime.Parse("2022-02-15"));
            //Verificar
            Assert.AreEqual(3, respuesta.Value.Count());
        }
        [TestMethod]
        public async Task ModificarReserva()
        {
            //Preparacion
            string NombreDB = CrearDataPrueba();
            DataContext contexto = ConstruirContext(NombreDB);
            ReservasController controller = new ReservasController(contexto);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            //Prueba
            Usuario _usuario = new Usuario()
            {
                ID = 0,
                NOMBRE = "DANIEL",
                APELLIDOS = "OICATA",
                EMAIL = "danielandres011@hotmail.com",
                DIRECCION = "CALLE FALSA 123",
                CREATED = DateTime.Now,
                UPDATED = DateTime.Now,
                DELETED = null
            };
            Hotel _hotel = new Hotel()
            {
                ID = 0,
                NOMBRE = "HOTEL PRUEBA",
                PAIS = "COLOMBIA",
                HABITACIONES = 10,
                ACTIVO = true,
                LONGITUD = 0,
                LATITUD = 0,
                DESCRIPCION = "Hotel de pruebas unitarias.",
                CREATED = DateTime.Now,
                UPDATED = DateTime.Now,
                DELETED = null
            };
            Reserva _reservaNew = new Reserva()
            {
                ID = 1,
                ID_USUARIO = _usuario,
                ID_HOTEL = _hotel,
                ID_HABITACION = 1,
                CHECKIN = DateTime.Parse("2022-02-20"),
                CHECKOUT = DateTime.Parse("2022-02-21"),
                ESTADO = true
            };
            var respuesta = await controller.PutReserva(1, _reservaNew);
            //Verificar
            Assert.AreEqual(true, respuesta.Value);
        }
        [TestMethod]
        public async Task CancelarReservaPorId()
        {
            //Preparacion
            string NombreDB = CrearDataPrueba();
            DataContext contexto = ConstruirContext(NombreDB);
            ReservasController controller = new ReservasController(contexto);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            //Prueba
            var respuesta = await controller.PutReservaCancelar(1);
            //Verificar
            Assert.AreEqual(true, respuesta.Value);
        }
        [TestMethod]
        public async Task CrearReserva()
        {
            //Preparacion
            string NombreDB = CrearDataPrueba();
            DataContext contexto = ConstruirContext(NombreDB);
            ReservasController controller = new ReservasController(contexto);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            //Prueba
            Usuario _usuario = new Usuario()
            {
                ID = 1,
                NOMBRE = "DANIEL",
                APELLIDOS = "OICATA",
                EMAIL = "danielandres011@hotmail.com",
                DIRECCION = "CALLE FALSA 123",
                CREATED = DateTime.Parse("2022-01-01"),
                UPDATED = DateTime.Parse("2022-01-01"),
                DELETED = null
            };
            Hotel _hotel = new Hotel()
            {
                ID = 1,
                NOMBRE = "HOTEL PRUEBA",
                PAIS = "COLOMBIA",
                HABITACIONES = 10,
                ACTIVO = true,
                LONGITUD = 0,
                LATITUD = 0,
                DESCRIPCION = "Hotel de pruebas unitarias.",
                CREATED = DateTime.Parse("2022-01-01"),
                UPDATED = DateTime.Parse("2022-01-01"),
                DELETED = null
            };
            Reserva _reservaNew = new Reserva()
            {
                ID = 0,
                ID_USUARIO = _usuario,
                ID_HOTEL = _hotel,
                ID_HABITACION = 1,
                CHECKIN = DateTime.Parse("2022-02-07"),
                CHECKOUT = DateTime.Parse("2022-02-08"),
                ESTADO = true,
                CREATED = DateTime.Parse("2022-01-01"),
                UPDATED = DateTime.Parse("2022-01-01"),
                DELETED = null
            };
            var respuesta = await controller.PostReserva(_reservaNew);
            //Verificar
            Assert.AreEqual(true, respuesta.Value);
        }
        [TestMethod]
        public async Task EliminarReservaPorId()
        {
            //Preparacion
            string NombreDB = CrearDataPrueba();
            DataContext contexto = ConstruirContext(NombreDB);
            ReservasController controller = new ReservasController(contexto);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            //Prueba
            var respuesta = await controller.DeleteReserva(1);
            //Verificar
            Assert.AreEqual(true, respuesta.Value);
        }
    }
}
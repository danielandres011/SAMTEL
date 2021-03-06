#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESERVAS.API.Data;
using RESERVAS.API.Entities;


namespace RESERVAS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservasController : ControllerBase
    {
        private readonly DataContext _context;
        public ReservasController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Reservas
        /// <summary>
        /// Función que retorna todas las reservas del sistema, menos las eliminadas.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reserva>>> GetReservas()
        {
            return await _context.RESERVAS.Where(l => l.DELETED == null).ToListAsync();
        }

        // GET: api/Reservas/ByHotelByFechas/5/2022-02-05/2022-02-06
        /// <summary>
        /// Para 1 día concreto no puede haber más reservas que habitaciones disponibles (no overbooking).
        /// </summary>
        /// <returns></returns>
        [HttpGet("Disponibilidad/{idhotel}/{fechaIni}")]
        public async Task<ActionResult<bool>> GetDisponibilidadByFecha(int idhotel, DateTime fecha)
        {
            try
            {
                var hotel = await _context.HOTELES.FindAsync(idhotel);
                if (hotel == null)
                {
                    return NotFound("Hotel no encontrado.");
                }
                var reservas = await _context.RESERVAS
                    .Where(l => l.DELETED == null)
                    .Where(l => l.ESTADO == true)
                    .Where(l => l.ID_HOTEL == hotel)
                    .Where(l => l.CHECKIN <= fecha && l.CHECKOUT > fecha)
                    .ToListAsync();
                if (reservas.Count() < hotel.HABITACIONES)
                {
                    //"No hay habitaciones disponibles."
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.Now + " - ERROR: " + ex.Message);
            }
            return false;
        }

        // GET: api/Reservas/ByHotelByFechas/5/2022-02-05/2022-02-06
        /// <summary>
        /// Consultar todas las reservas activas de un hotel que estén dentro de un rango de fechas (al menos 1 noche de la reserva debe 
        /// estar comprendida entre las fechas solicitadas). La respuesta debe contener, además de la información de cada reserva, el 
        /// nombre del hotel y el mail del usuario asociado a la reserva.
        /// </summary>
        /// <returns></returns>
        [HttpGet("ByHotelByFechas/{idhotel}/{fechaIni}/{fechaFin}")]
        public async Task<ActionResult<IEnumerable<Reserva>>> GetReservasByFechas(int idhotel, DateTime fechaIni, DateTime fechaFin)
        {
            var hotel = await _context.HOTELES.FindAsync(idhotel);
            if (hotel == null)
            {
                return NotFound("Hotel no encontrado.");
            }
            var reservas = await _context.RESERVAS
                .Where(l => l.DELETED == null)
                .Where(l => l.ESTADO == true)
                .Where(l => l.ID_HOTEL == hotel)
                .Where(l => l.CHECKIN <= fechaIni || l.CHECKIN <= fechaFin)
                .ToListAsync();
            if (reservas == null)
            {
                return BadRequest("No se encontraron reservas.");
            }
            return reservas;
        }

        // GET: api/Reservas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reserva>> GetReserva(int id)
        {
            var reserva = await _context.RESERVAS.FindAsync(id);

            if (reserva == null)
            {
                return NotFound();
            }

            return reserva;
        }

        // PUT: api/Reservas/5
        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> PutReserva(int id, Reserva reserva)
        {
            if (id != reserva.ID)
            {
                return BadRequest();
            }
            _context.Entry(reserva).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // PUT: api/Reservas/Cancelar/5
        /// <summary>
        /// Cancelar una reserva dado su identificador (no se eliminará, se marcará como cancelada y dejará de "ocupar" la habitacion del
        /// hotel para los días de la reserva).
        /// </summary>
        /// <returns></returns>
        [HttpPost("Cancelar/{id}")]
        public async Task<ActionResult<bool>> PutReservaCancelar(int id)
        {
            var reserva = await _context.RESERVAS.FindAsync(id);
            if (reserva == null)
            {
                return BadRequest();
            }
            reserva.ESTADO = false;
            _context.Entry(reserva).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // POST: api/Reservas
        /// <summary>
        /// Registrar una reserva dados un usuario, un hotel y un rango de fechas (checkin y checkout). 
        /// La fecha de entrada (checkin) siempre será menor que la fecha de salida (checkout)
        /// La fecha de checkout NO cuenta como día de reserva, ya que el usuario sale del hotel por la mañana.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<bool>> PostReserva(Reserva reserva)
        {
            if (reserva.CHECKIN >= reserva.CHECKOUT)
            {
                return BadRequest("Las fechas no son validas.");
            }
            try
            {
                var hotel = await _context.HOTELES.FindAsync(reserva.ID_HOTEL.ID);
                if (hotel == null)
                {
                    return NotFound("Hotel no encontrado.");
                }
                var user = await _context.USUARIOS.FindAsync(reserva.ID_USUARIO.ID);
                if (user == null)
                {
                    return NotFound("User no encontrado.");
                }
                var reservas = await _context.RESERVAS
                    .Where(l => l.DELETED == null)
                    .Where(l => l.ESTADO == true)
                    .Where(l => l.ID_HOTEL == hotel)
                    .Where(l => l.ID_HABITACION == reserva.ID_HABITACION)
                    .Where(l => l.CHECKIN <= reserva.CHECKIN && l.CHECKOUT > reserva.CHECKIN)
                    .Where(l => l.CHECKIN <= reserva.CHECKOUT && l.CHECKOUT > reserva.CHECKOUT)
                    .ToListAsync();
                if (reservas.Count() > 0)
                {
                    return BadRequest("Hay reservas registradas");
                }
                reserva.ID_HOTEL = hotel;
                reserva.ID_USUARIO = user;
                _context.RESERVAS.Add(reserva);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.Now + " - ERROR: " + ex.Message);
            }
            return false;
        }

        // DELETE: api/Reservas/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteReserva(int id)
        {
            var reserva = await _context.RESERVAS.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }
            _context.RESERVAS.Remove(reserva);
            await _context.SaveChangesAsync();
            return true;
        }

        private bool ReservaExists(int id)
        {
            return _context.RESERVAS.Any(e => e.ID == id);
        }
    }
}

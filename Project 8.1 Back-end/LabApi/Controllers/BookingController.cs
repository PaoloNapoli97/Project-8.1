using BookingModel;
using BookingServices;
using Microsoft.AspNetCore.Mvc;
using LabServices;
using ComputerDTO;

namespace BookingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class BookingController : ControllerBase
    {
        BookingService bookingService = new();
        LabService labService = new();

        [HttpGet]
        [Route("")]
        public ActionResult<List<Booking>> GetBooking()
        {
            var bookings = bookingService.ReadBookings();
            return Ok(bookings);
        }

        [HttpPost]
        [Route("Add/{LabId}/Resource/{ResourceName}")]
        public ActionResult ResourcesBooking(string LabId, string ResourceName, [FromBody] BookingAddDTO bookingAddDTO)
        {
            var labs = labService.ReadLabs();
            var lab = labs.FirstOrDefault(x => x.Id == LabId);
            if (lab == null)
            {
                throw new Exception("Lab not found");
            }
            else
            {
                var resource = lab.Resource.FirstOrDefault(x => x.Name == ResourceName);
                if (resource == null)
                {
                    return NotFound();
                }
                else
                {
                    resource.AddBooking(bookingAddDTO.UserId, bookingAddDTO.Day, bookingAddDTO.Hour);
                    Booking booking = new(LabId, bookingAddDTO.UserId, ResourceName);
                    booking.AddBooking(booking);
                    labService.WriteLabs(labs);
                    var pathToUrl = Request.Path.ToString() + '/' + lab.Id;
                    return Created(pathToUrl, resource);
                }
            }
        }

        [HttpPost]
        [Route("Add/{LabId}/Computer/{ComputerId}")]
        public ActionResult ComputerBooking(string LabId, string ComputerId, [FromBody] BookingAddDTO bookingAddDTO)
        {
            var labs = labService.ReadLabs();
            var lab = labs.FirstOrDefault(x => x.Id == LabId);
            if (lab == null)
            {
                return NotFound();
            }
            else
            {
                var computer = lab.Computers.FirstOrDefault(x => x.Id == ComputerId && x.Status == 0);
                if (computer == null)
                {
                    return NotFound();
                }
                else
                {
                    computer.AddBooking(bookingAddDTO.UserId, bookingAddDTO.Day, bookingAddDTO.Hour);
                    Booking booking = new(LabId, bookingAddDTO.UserId, ComputerId);
                    labService.WriteLabs(labs);
                    var pathToUrl = Request.Path.ToString() + '/' + lab.Id;
                    return Created(pathToUrl, computer);
                }
            }
        }

        [HttpDelete]
        [Route("Delete/{LabId}/Computer/{ComputerId}")]
        public ActionResult DeleteComputerBooking(string LabId, string ComputerId, [FromBody] BookingAddDTO bookingAddDTO){
            var labs = labService.ReadLabs();
            var lab = labs.FirstOrDefault(x => x.Id == LabId);
            if (lab == null)
            {
                return NotFound();
            }
            else
            {
                var computer = lab.Computers.FirstOrDefault(x => x.Id == ComputerId);
                if (computer == null)
                {
                    return NotFound();
                }
                else
                {
                    computer.DeleteBooking(bookingAddDTO.UserId, bookingAddDTO.Day, bookingAddDTO.Hour);
                    labService.WriteLabs(labs);
                    return Ok(computer);
                }
            }
        }

        [HttpDelete]
        [Route("Delete/{LabId}/Resource/{ResourceName}")]
        public ActionResult DeleteResourceBooking(string LabId, string ResourceName, [FromBody] BookingAddDTO bookingAddDTO){
            var labs = labService.ReadLabs();
            var lab = labs.FirstOrDefault(x => x.Id == LabId);
            if (lab == null)
            {
                return NotFound();
            }
            else
            {
                var computer = lab.Computers.FirstOrDefault(x => x.Id == ResourceName);
                if (computer == null)
                {
                    return NotFound();
                }
                else
                {
                    computer.DeleteBooking(bookingAddDTO.UserId, bookingAddDTO.Day, bookingAddDTO.Hour);
                    labService.WriteLabs(labs);
                    return Ok(computer);
                }
            }
        }
    }
}